using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sword : Enemy {
    public float speed = 5f; // 移速
    public float maxLeave = 15f; // 离开巡逻区域最远距离
    private float leave; // 离开巡逻区域距离
    private bool moving = false; // 是否正在移动
    private bool readyToAttack = false; // 玩家是否在攻击范围内
    public float moveDealyTime; // 巡逻延迟
    public float attackDealyTime; // 攻击延迟
    [Space]
    private float destination; // 巡逻区域内一点
    [Space]
    public GameObject sword; // 剑
    private Animator animSword;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        animSword = sword.GetComponent<Animator>();
        moveDealyTime = Random.Range(10f, 20f); // 10s到20s延迟触发一次移动
        attackDealyTime = Random.Range(1f, 5f); // 设置1s到5s的攻击延迟

        SetATK(); // 为剑赋予攻击力
    }

    // Update is called once per frame
    void Update() {
        // 准备好攻击，等待攻击延迟（随机性）
        if (readyToAttack) {
            // 延迟时间到了
            if (attackDealyTime <= 0) {
                attackDealyTime = Random.Range(1f, 5f); // 设置1s到5s的攻击延迟
                anim.SetTrigger("attack");
                anim.SetBool("attacking", true);
                animSword.SetTrigger("attack");
            }
            // 延迟时间没到
            else attackDealyTime -= Time.deltaTime;
        }
        else {
            // 在不追踪玩家的时候等待延迟时间触发移动
            if (!follow) {
                // 延迟时间到了
                if (moveDealyTime <= 0) {
                    if (!moving) destination = Random.Range(left_x, right_x); // 设置目的地
                    moving = true;
                    MoveToDestination();
                }
                // 延迟时间没到
                else {
                    moveDealyTime -= Time.deltaTime;
                }
            }
            // 追踪玩家时判定是否离开太远
            else {
                LeaveFarOrNot();
            }
        }

        if (found) FollowPlayer();
        // 玩家不在索敌范围内，取消追踪
        else if (follow) {
            follow = false; // 不再追踪
            moving = false; // 不再移动
            // 移动速度置零
            SetVelocity(0);
            // 如果离开了巡逻区域，返回
            if (transform.position.x > right_x || transform.position.x < left_x) {
                moveDealyTime = 0;
                destination = Random.Range(left_x, right_x); // 设置目的地
                MoveToDestination();
            }
            // 没离开，原地待命
            else
                anim.SetBool("running", false);
        }
    }

    // 赋予攻击力，重写父类方法
    public override void SetATK() {
        base.SetATK();
        Sword sw = sword.GetComponent<Sword>();
        sw.SetAttack(atk);
    }

    // 攻击完了
    public void AttackIsOver() {
        anim.SetBool("attacking", false);
    }

    // 移动到目的地
    void MoveToDestination() {
        if (!moving) {
            // 切换动画
            anim.SetTrigger("run");
            anim.SetBool("running", true);
        }
        // 设置朝向
        int tag;
        if (destination < transform.position.x) tag = -1;
        else tag = 1;
        transform.localScale = new Vector3(tag * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        // 向着目的地前进
        if (Mathf.Abs(transform.position.x - destination) > 0.2f) { // 0.2的误差
            SetVelocity(tag * speed);
        }
        // 到达目的地，取消正在移动状态，设置延迟
        else {
            moving = false;
            SetVelocity(0);
            moveDealyTime = Random.Range(10f, 20f); // 10s到20s延迟触发一次移动
            anim.SetBool("running", false);
        }
    }

    // 判断敌人是否超出巡逻区域一定距离，超出则返回
    void LeaveFarOrNot() {
        if (transform.position.x > right_x) {
            leave = transform.position.x - right_x;
        }
        else if (transform.position.x < left_x) {
            leave = left_x - transform.position.x;
        }
        else leave = 0;

        if (leave > maxLeave) {
            found = false; // 视而不见
            moving = true; // 正在移动
            destination = Random.Range(left_x, right_x); // 设置目的地
            MoveToDestination();
        }
    }

    // 索敌
    void FollowPlayer() {
        // 追踪玩家直至到攻击范围内随机一点时
        if (follow && Mathf.Abs(player.transform.position.x - this.transform.position.x) > finalDistance) {
            if (!moving) {
                // 切换动画
                anim.SetTrigger("run");
                anim.SetBool("running", true);
                moving = true;
            }

            // 设置朝向
            int tag;
            if (player.transform.position.x > this.transform.position.x) tag = 1;
            else tag = -1;
            this.transform.localScale = new Vector3(tag * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

            SetVelocity(tag * speed);
        }
        // 超出了攻击范围，追踪
        else if (Mathf.Abs(player.transform.position.x - this.transform.position.x) > maxDistance) {
            follow = true; // 继续追踪
            readyToAttack = false; // 取消攻击准备
            anim.SetTrigger("run");
            anim.SetBool("running", true);
        }
        // 在攻击范围内，准备攻击
        else if (Mathf.Abs(player.transform.position.x - this.transform.position.x) <= maxDistance) {
            follow = false;
            readyToAttack = true;
            SetVelocity(0);
            anim.SetBool("running", false);
        }
    }

    // 受伤反击
    public void CounterAttack() {
        if (Random.Range(0, 2) == 0) {
            readyToAttack = true;
            attackDealyTime = 0;
        }
    }

    public override void Hurt(float hurtBlood) {
        if (blood == 0) return;

        // 死亡时取消剑的触发器动作
        if (hurtBlood >= blood) animSword.SetBool("death", true);
        base.Hurt(hurtBlood);
    }
}

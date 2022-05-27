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
    public GameObject leftPosition; // 巡逻区域左端点
    public GameObject rightPosition; // 巡逻区域右端点
    private float left_x; // 巡逻区域左端点值
    private float right_x; // 巡逻区域右端点值
    private float destination; // 巡逻区域内一点
    [Space]
    private Rigidbody2D rb;
    private Rigidbody2D wallRb;
    public Animator anim;

    // Use this for initialization
    void Start() {
        left_x = leftPosition.transform.position.x;
        right_x = rightPosition.transform.position.x;
        rb = this.transform.GetComponent<Rigidbody2D>();
        wallRb = this.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        moveDealyTime = Random.Range(10f, 20f); // 10s到20s延迟触发一次移动
        attackDealyTime = Random.Range(1f, 5f); // 设置1s到5s的攻击延迟
    }

    // Update is called once per frame
    void Update() {
        // 准备好攻击，等待攻击延迟（随机性）
        if (readyToAttack) {
            if (attackDealyTime <= 0) {
                attackDealyTime = Random.Range(1f, 5f); // 设置1s到5s的攻击延迟
                anim.SetTrigger("attack");
            }
            else attackDealyTime -= Time.deltaTime;
        }
        else {
            // 在不追踪玩家的时候等待延迟时间触发移动
            if (!follow) {
                if (moveDealyTime <= 0) {
                    if (!moving) destination = Random.Range(left_x, right_x); // 设置目的地
                    moving = true;
                    MoveToDestination();
                }
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
            rb.velocity = new Vector2(0, rb.velocity.y);
            wallRb.velocity = new Vector2(0, wallRb.velocity.y);
            if (transform.position.x > right_x || transform.position.x < left_x) { // 如果离开了巡逻区域，返回
                moveDealyTime = 0;
                destination = Random.Range(left_x, right_x); // 设置目的地
                MoveToDestination();
            }
            else
                anim.SetBool("running", false);
        }
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
            rb.velocity = new Vector2(tag * speed, rb.velocity.y);
            wallRb.velocity = new Vector2(tag * speed, wallRb.velocity.y);
        }
        // 到达目的地，取消正在移动状态，设置延迟
        else {
            moving = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            wallRb.velocity = new Vector2(0, wallRb.velocity.y);
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
            //follow = false; // 取消追踪
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

            // 带着墙体移动
            rb.velocity = new Vector2(tag * speed, rb.velocity.y);
            wallRb.velocity = new Vector2(tag * speed, wallRb.velocity.y);
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
            rb.velocity = new Vector2(0, rb.velocity.y);
            wallRb.velocity = new Vector2(0, wallRb.velocity.y);
            anim.SetBool("running", false);
        }
    }

    // 受伤，重写父类方法
    public override void Hurt(float hurtBlood) {
        if (blood == 0) return;

        anim.SetTrigger("hurt");
        if (hurtBlood > blood) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有轴，防止取消碰撞体后物体下坠
            rb.Sleep();
            blood = 0;
            Death();
        }
        else {
            blood -= hurtBlood;
        }
    }

    // 死亡
    void Death() {
        anim.SetBool("death", true);
        Debug.Log(this.name + " Death");
    }

    // 摧毁该敌人
    void Distory() {
        Destroy(this.gameObject);
    }
}

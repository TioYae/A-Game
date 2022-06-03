using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WandAndCup : Enemy {
    public GameObject prefab;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        moveDealyTime = Random.Range(10f, 20f); // 10s到20s延迟触发一次移动
        attackDealyTime = Random.Range(1f, 5f); // 设置1s到5s的攻击延迟
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        // 准备好攻击，等待攻击延迟（随机性）
        if (readyToAttack) {
            // 设置朝向
            int tag;
            if (player.transform.position.x < transform.position.x) tag = -1;
            else tag = 1;
            transform.localScale = new Vector3(tag * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            // 延迟时间到了
            if (attackDealyTime <= 0) {
                attackDealyTime = Random.Range(1f, 5f); // 设置1s到5s的攻击延迟
                anim.SetTrigger("attack");
                //anim.SetBool("attacking", true);
                // todo 
                // 根据prefab创建新子弹
                SetATK();
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
        GameObject obj = Instantiate(prefab);
        obj.SetActive(true);
        obj.GetComponent<Bullet>().SetAttack(atk);
        obj.transform.parent = this.transform;
        obj.transform.position = this.transform.position;
        Rigidbody2D bulletRb = obj.GetComponent<Rigidbody2D>();
        // 调整方向
        if (transform.localScale.x < 0) {
            obj.transform.localScale = new Vector3(-obj.transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
            bulletRb.velocity = new Vector2(-4, 0);
        }
        else {
            bulletRb.velocity = new Vector2(4, 0);
        }

    }
}

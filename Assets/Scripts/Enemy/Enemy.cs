using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    public float blood; // 血量
    public float atk; // 攻击力
    public int exp; // 经验值
    public float moveSpeed = 3.3f; // 移速
    public float maxLeave = 15f; // 离开巡逻区域最远距离
    public float minDistance; // 攻击范围最小值
    public float maxDistance; // 攻击范围最大值
    protected float finalDistance; // 锁定玩家时与玩家的间隔
    protected float bloodMax; // 最大血量
    protected bool found = false; // 是否找到玩家
    protected bool follow = false; // 是否跟随玩家
    protected bool moving = false; // 是否正在移动
    protected float leave; // 离开巡逻区域距离
    public float moveDealyTime; // 巡逻延迟
    public float attackDealyTime; // 攻击延迟
    protected bool readyToAttack = false; // 玩家是否在攻击范围内
    public string enemyName; // 怪物名字
    public string enemyLevel; // 怪物等级
    [Space]
    public bool isAnim; // 是否播放初见动画
    public bool isBoss; // 是否BOSS，是BOSS要打完才开门
    public Image picture; // 初见动画
    public GameObject door; // 真通关门
    public GameObject doorClose; // 关着的通关门
    [Space]
    public GameObject leftPosition; // 巡逻区域左端点
    public GameObject rightPosition; // 巡逻区域右端点
    protected float left_x; // 巡逻区域左端点值
    protected float right_x; // 巡逻区域右端点值
    protected float destination; // 巡逻区域内一点
    [Space]
    public GameObject player;
    public GameObject enemyStatus; // 名字、等级、血条
    public GameObject popupDamage; // 回复数字
    protected Rigidbody2D rb;
    protected Rigidbody2D wallRb;
    protected Animator anim;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip death1;
    protected AudioSource audioSource;

    protected virtual void Start() {
        bloodMax = blood;
        left_x = leftPosition.transform.position.x;
        right_x = rightPosition.transform.position.x;
        rb = this.transform.GetComponent<Rigidbody2D>();
        wallRb = this.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        // 设置名字与等级
        enemyStatus.transform.GetChild(0).GetComponent<Text>().text = enemyName;
        enemyStatus.transform.GetChild(1).GetComponent<Text>().text = enemyLevel;
    }

    protected virtual void Update() {
        // 更新血量
        enemyStatus.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().fillAmount = blood / bloodMax;    
    }

    // 赋予攻击力的空方法，待子类重写
    public virtual void SetATK() {

    }

    // 设置移速
    protected void SetVelocity(float speed) {
        // 带着墙体移动
        rb.velocity = new Vector2(speed, rb.velocity.y);
        wallRb.velocity = new Vector2(speed, wallRb.velocity.y);
    }

    // 攻击完了
    public void AttackIsOver() {
        anim.SetBool("attacking", false);
    }

    // 移动到目的地
    protected void MoveToDestination() {
        if (!moving) {
            // 切换动画
            anim.SetTrigger("run");
            anim.SetBool("running", true);
            moving = true;
        }
        // 设置朝向
        int tag;
        if (destination < transform.position.x) tag = -1;
        else tag = 1;
        transform.localScale = new Vector3(tag * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        // 向着目的地前进
        if (Mathf.Abs(transform.position.x - destination) > 0.2f) { // 0.2的误差
            SetVelocity(tag * moveSpeed);
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
    protected void LeaveFarOrNot() {
        if (transform.position.x > right_x) {
            leave = transform.position.x - right_x;
        }
        else if (transform.position.x < left_x) {
            leave = left_x - transform.position.x;
        }
        else leave = 0;

        if (leave > maxLeave) {
            found = false; // 视而不见
            destination = Random.Range(left_x, right_x); // 设置目的地
            MoveToDestination();
        }
    }

    // 索敌
    protected void FollowPlayer() {
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

            SetVelocity(tag * moveSpeed);
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

    // 受伤，子类可重写
    public virtual void Hurt(float hurtBlood) {
        if (blood == 0) return;

        anim.SetTrigger("hurt");
        if (hurtBlood >= blood) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有轴，防止取消碰撞体后物体下坠
            rb.Sleep();
            blood = 0;
            anim.SetBool("death", true);
            audioSource.clip = death1;
            audioSource.Play();
            Debug.Log(this.name + " Death");
        }
        else {
            blood -= hurtBlood;
            if (Random.Range(0, 2) == 0)
                audioSource.clip = hurt1;
            else
                audioSource.clip = hurt2;
            audioSource.Play();
        }
    }

    // 找到玩家
    public void FoundPlayer() {
        finalDistance = Random.Range(minDistance, maxDistance);
        found = true;
        follow = true;
        if (isAnim) PlayAnimation();
    }

    // 跟丢玩家
    public void LostPlayer() {
        finalDistance = 0f;
        found = false;
    }

    // 给星币返回血量，非战斗状态时返回-1
    public float GetBlood() {
        if (!found) return -1;
        else return blood;
    }

    // 回血
    public void BloodUp(float present) {
        blood += bloodMax * present;
        // 回复数字
        GameObject obj = Instantiate(popupDamage, this.transform.position, Quaternion.identity);
        obj.GetComponent<DamagePopup>().value = -bloodMax * present;
    }

    // 死亡
    void Death() {
        player.GetComponent<PlayerController>().ExpUp(exp);
        if (isBoss) {
            door.SetActive(true);
            doorClose.SetActive(false);
        }
        Destroy(enemyStatus);
        Destroy(this.gameObject);
    }

    void PlayAnimation() {
        picture.gameObject.SetActive(true);
        isBoss = false;
    }
}

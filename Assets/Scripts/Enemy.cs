using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    public float blood; // 血量
    public float atk; // 攻击力
    public float minDistance = 5f; // 攻击范围最小值
    public float maxDistance = 10f; // 攻击范围最大值
    protected float finalDistance; // 锁定玩家时与玩家的间隔
    protected bool found = false; // 是否找到玩家
    protected bool follow = false; // 是否跟随玩家
    [Space]
    public GameObject leftPosition; // 巡逻区域左端点
    public GameObject rightPosition; // 巡逻区域右端点
    protected float left_x; // 巡逻区域左端点值
    protected float right_x; // 巡逻区域右端点值
    [Space]
    public GameObject player;
    protected Rigidbody2D rb;
    protected Rigidbody2D wallRb;
    protected Animator anim;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip death1;
    protected AudioSource audioSource;

    protected virtual void Start() {
        left_x = leftPosition.transform.position.x;
        right_x = rightPosition.transform.position.x;
        rb = this.transform.GetComponent<Rigidbody2D>();
        wallRb = this.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
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
        finalDistance = UnityEngine.Random.Range(minDistance, maxDistance);
        found = true;
        follow = true;
    }

    // 跟丢玩家
    public void LostPlayer() {
        finalDistance = 0f;
        found = false;
    }

    // 死亡
    void Death() {
        Destroy(this.gameObject);
    }
}

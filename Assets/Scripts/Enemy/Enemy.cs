using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    public float blood; // Ѫ��
    public float atk; // ������
    public float minDistance = 5f; // ������Χ��Сֵ
    public float maxDistance = 10f; // ������Χ���ֵ
    protected float bloodMax; // ���Ѫ��
    protected float finalDistance; // �������ʱ����ҵļ��
    protected bool found = false; // �Ƿ��ҵ����
    protected bool follow = false; // �Ƿ�������
    public string enemyName; // ��������
    [Space]
    public GameObject leftPosition; // Ѳ��������˵�
    public GameObject rightPosition; // Ѳ�������Ҷ˵�
    protected float left_x; // Ѳ��������˵�ֵ
    protected float right_x; // Ѳ�������Ҷ˵�ֵ
    [Space]
    public GameObject player;
    public GameObject enemyStatus;
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
        // ��������
        enemyStatus.transform.GetChild(0).GetComponent<Text>().text = enemyName;
    }

    protected virtual void Update() {
        // ����Ѫ��
        enemyStatus.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = blood / bloodMax;
    }

    // ���蹥�����Ŀշ�������������д
    public virtual void SetATK() {

    }

    // ��������
    protected void SetVelocity(float speed) {
        // ����ǽ���ƶ�
        rb.velocity = new Vector2(speed, rb.velocity.y);
        wallRb.velocity = new Vector2(speed, wallRb.velocity.y);
    }

    // ���ˣ��������д
    public virtual void Hurt(float hurtBlood) {
        if (blood == 0) return;

        anim.SetTrigger("hurt");
        if (hurtBlood >= blood) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // ���������ᣬ��ֹȡ����ײ���������׹
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

    // �ҵ����
    public void FoundPlayer() {
        finalDistance = UnityEngine.Random.Range(minDistance, maxDistance);
        found = true;
        follow = true;
    }

    // �������
    public void LostPlayer() {
        finalDistance = 0f;
        found = false;
    }

    // ����
    void Death() {
        Destroy(enemyStatus);
        Destroy(this.gameObject);
    }
}

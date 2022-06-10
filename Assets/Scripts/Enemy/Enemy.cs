using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    public float blood; // Ѫ��
    public float atk; // ������
    public int exp; // ����ֵ
    public int level; // �ȼ�
    public List<float> atkLevel; // ÿһ���Ĺ�����
    public List<float> bloodLevel; // ÿһ����Ѫ������
    public List<int> expLevel; // ������Ҫ�ľ���ֵ
    public float moveSpeed = 3.3f; // ����
    public float maxLeave = 15f; // �뿪Ѳ��������Զ����
    public float minDistance; // ������Χ��Сֵ
    public float maxDistance; // ������Χ���ֵ
    protected float finalDistance; // �������ʱ����ҵļ��
    protected float bloodMax; // ���Ѫ��
    protected bool found = false; // �Ƿ��ҵ����
    protected bool follow = false; // �Ƿ�������
    protected bool moving = false; // �Ƿ������ƶ�
    protected float leave; // �뿪Ѳ���������
    public float moveDealyTime; // Ѳ���ӳ�
    public float attackDealyTime; // �����ӳ�
    public bool readyToAttack = false; // ����Ƿ��ڹ�����Χ��
    public string enemyName; // ��������
    [Space]
    public bool isAnim; // �Ƿ񲥷ų�������
    public Image picture; // ��������
    [Space]
    public GameObject leftPosition; // Ѳ��������˵�
    public GameObject rightPosition; // Ѳ�������Ҷ˵�
    protected float left_x; // Ѳ��������˵�ֵ
    protected float right_x; // Ѳ�������Ҷ˵�ֵ
    protected float destination; // Ѳ��������һ��
    [Space]
    public GameObject player;
    public GameObject enemyStatus; // ���֡��ȼ���Ѫ��
    public GameObject popupDamage; // �ظ�����
    protected Rigidbody2D rb;
    protected Rigidbody2D wallRb;
    protected Animator anim;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip death1;
    protected AudioSource audioSource;

    protected virtual void Start() {
        atk = atkLevel[level - 1];
        blood = bloodLevel[level - 1];
        exp = expLevel[level - 1];
        bloodMax = blood;
        left_x = leftPosition.transform.position.x;
        right_x = rightPosition.transform.position.x;
        rb = this.transform.GetComponent<Rigidbody2D>();
        wallRb = this.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        // ����������ȼ�
        enemyStatus.transform.GetChild(0).GetComponent<Text>().text = enemyName;
        enemyStatus.transform.GetChild(1).GetComponent<Text>().text = "Lv. " + level;
    }

    protected virtual void Update() {
        // ����Ѫ��
        enemyStatus.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().fillAmount = blood / bloodMax;
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

    // ��������
    public void AttackIsOver() {
        anim.SetBool("attacking", false);
    }

    // �ƶ���Ŀ�ĵ�
    protected void MoveToDestination() {
        if (!moving) {
            // �л�����
            anim.SetTrigger("run");
            anim.SetBool("running", true);
            moving = true;
        }
        // ���ó���
        int tag;
        if (destination < transform.position.x) tag = -1;
        else tag = 1;
        transform.localScale = new Vector3(tag * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        // ����Ŀ�ĵ�ǰ��
        if (Mathf.Abs(transform.position.x - destination) > 0.2f) { // 0.2�����
            SetVelocity(tag * moveSpeed);
        }
        // ����Ŀ�ĵأ�ȡ�������ƶ�״̬�������ӳ�
        else {
            moving = false;
            SetVelocity(0);
            moveDealyTime = Random.Range(10f, 20f); // 10s��20s�ӳٴ���һ���ƶ�
            anim.SetBool("running", false);
        }
    }

    // �жϵ����Ƿ񳬳�Ѳ������һ�����룬�����򷵻�
    protected void LeaveFarOrNot() {
        if (transform.position.x > right_x) {
            leave = transform.position.x - right_x;
        }
        else if (transform.position.x < left_x) {
            leave = left_x - transform.position.x;
        }
        else leave = 0;

        if (leave > maxLeave) {
            found = false; // �Ӷ�����
            destination = Random.Range(left_x, right_x); // ����Ŀ�ĵ�
            MoveToDestination();
        }
    }

    // ����
    protected void FollowPlayer() {
        // ׷�����ֱ����������Χ�����һ��ʱ
        if (follow && Mathf.Abs(player.transform.position.x - this.transform.position.x) > finalDistance) {
            if (!moving) {
                // �л�����
                anim.SetTrigger("run");
                anim.SetBool("running", true);
                moving = true;
            }

            // ���ó���
            int tag;
            if (player.transform.position.x > this.transform.position.x) tag = 1;
            else tag = -1;
            this.transform.localScale = new Vector3(tag * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);

            SetVelocity(tag * moveSpeed);
        }
        // �����˹�����Χ��׷��
        else if (Mathf.Abs(player.transform.position.x - this.transform.position.x) > maxDistance) {
            follow = true; // ����׷��
            readyToAttack = false; // ȡ������׼��
            anim.SetTrigger("run");
            anim.SetBool("running", true);
        }
        // �ڹ�����Χ�ڣ�׼������
        else if (Mathf.Abs(player.transform.position.x - this.transform.position.x) <= maxDistance) {
            follow = false;
            if (blood > 0) readyToAttack = true;
            SetVelocity(0);
            anim.SetBool("running", false);
        }
    }

    // ���˷���
    public void CounterAttack() {
        if (Random.Range(0, 2) == 0 && blood > 0) {
            readyToAttack = true;
            attackDealyTime = 0;
        }
    }

    // ���ˣ��������д
    public virtual void Hurt(float hurtBlood) {
        if (blood == 0) return;

        anim.SetTrigger("hurt");
        if (hurtBlood >= blood) {
            blood = 0;
            readyToAttack = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // ���������ᣬ��ֹȡ����ײ���������׹
            rb.Sleep();
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
        finalDistance = Random.Range(minDistance, maxDistance);
        found = true;
        follow = true;
        if (isAnim) PlayAnimation();
    }

    // �������
    public void LostPlayer() {
        finalDistance = 0f;
        found = false;
        readyToAttack = false;
    }

    // ���Ǳҷ���Ѫ������ս��״̬ʱ����-1
    public float GetBlood() {
        if (!found) return -1;
        else return blood;
    }

    // ��Ѫ
    public void BloodUp(float present) {
        blood += bloodMax * present;
        // �ظ�����
        GameObject obj = Instantiate(popupDamage, this.transform.position, Quaternion.identity);
        obj.GetComponent<DamagePopup>().value = -bloodMax * present;
    }

    // ����
    void Death() {
        player.GetComponent<PlayerController>().ExpUp(exp);
        Destroy(enemyStatus);
        Destroy(this.gameObject);
    }

    void PlayAnimation() {
        picture.gameObject.SetActive(true);
        isAnim = false;
    }
}

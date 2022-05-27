using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sword : Enemy {
    public float speed = 5f; // ����
    public float maxLeave = 15f; // �뿪Ѳ��������Զ����
    private float leave; // �뿪Ѳ���������
    private bool moving = false; // �Ƿ������ƶ�
    private bool readyToAttack = false; // ����Ƿ��ڹ�����Χ��
    public float moveDealyTime; // Ѳ���ӳ�
    public float attackDealyTime; // �����ӳ�
    [Space]
    public GameObject leftPosition; // Ѳ��������˵�
    public GameObject rightPosition; // Ѳ�������Ҷ˵�
    private float left_x; // Ѳ��������˵�ֵ
    private float right_x; // Ѳ�������Ҷ˵�ֵ
    private float destination; // Ѳ��������һ��
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
        moveDealyTime = Random.Range(10f, 20f); // 10s��20s�ӳٴ���һ���ƶ�
        attackDealyTime = Random.Range(1f, 5f); // ����1s��5s�Ĺ����ӳ�
    }

    // Update is called once per frame
    void Update() {
        // ׼���ù������ȴ������ӳ٣�����ԣ�
        if (readyToAttack) {
            if (attackDealyTime <= 0) {
                attackDealyTime = Random.Range(1f, 5f); // ����1s��5s�Ĺ����ӳ�
                anim.SetTrigger("attack");
            }
            else attackDealyTime -= Time.deltaTime;
        }
        else {
            // �ڲ�׷����ҵ�ʱ��ȴ��ӳ�ʱ�䴥���ƶ�
            if (!follow) {
                if (moveDealyTime <= 0) {
                    if (!moving) destination = Random.Range(left_x, right_x); // ����Ŀ�ĵ�
                    moving = true;
                    MoveToDestination();
                }
                else {
                    moveDealyTime -= Time.deltaTime;
                }
            }
            // ׷�����ʱ�ж��Ƿ��뿪̫Զ
            else {
                LeaveFarOrNot();
            }
        }

        if (found) FollowPlayer();
        // ��Ҳ������з�Χ�ڣ�ȡ��׷��
        else if (follow) {
            follow = false; // ����׷��
            moving = false; // �����ƶ�
            rb.velocity = new Vector2(0, rb.velocity.y);
            wallRb.velocity = new Vector2(0, wallRb.velocity.y);
            if (transform.position.x > right_x || transform.position.x < left_x) { // ����뿪��Ѳ�����򣬷���
                moveDealyTime = 0;
                destination = Random.Range(left_x, right_x); // ����Ŀ�ĵ�
                MoveToDestination();
            }
            else
                anim.SetBool("running", false);
        }
    }

    // �ƶ���Ŀ�ĵ�
    void MoveToDestination() {
        if (!moving) {
            // �л�����
            anim.SetTrigger("run");
            anim.SetBool("running", true);
        }
        // ���ó���
        int tag;
        if (destination < transform.position.x) tag = -1;
        else tag = 1;
        transform.localScale = new Vector3(tag * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        // ����Ŀ�ĵ�ǰ��
        if (Mathf.Abs(transform.position.x - destination) > 0.2f) { // 0.2�����
            rb.velocity = new Vector2(tag * speed, rb.velocity.y);
            wallRb.velocity = new Vector2(tag * speed, wallRb.velocity.y);
        }
        // ����Ŀ�ĵأ�ȡ�������ƶ�״̬�������ӳ�
        else {
            moving = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            wallRb.velocity = new Vector2(0, wallRb.velocity.y);
            moveDealyTime = Random.Range(10f, 20f); // 10s��20s�ӳٴ���һ���ƶ�
            anim.SetBool("running", false);
        }
    }

    // �жϵ����Ƿ񳬳�Ѳ������һ�����룬�����򷵻�
    void LeaveFarOrNot() {
        if (transform.position.x > right_x) {
            leave = transform.position.x - right_x;
        }
        else if (transform.position.x < left_x) {
            leave = left_x - transform.position.x;
        }
        else leave = 0;

        if (leave > maxLeave) {
            found = false; // �Ӷ�����
            //follow = false; // ȡ��׷��
            moving = true; // �����ƶ�
            destination = Random.Range(left_x, right_x); // ����Ŀ�ĵ�
            MoveToDestination();
        }
    }

    // ����
    void FollowPlayer() {
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

            // ����ǽ���ƶ�
            rb.velocity = new Vector2(tag * speed, rb.velocity.y);
            wallRb.velocity = new Vector2(tag * speed, wallRb.velocity.y);
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
            readyToAttack = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
            wallRb.velocity = new Vector2(0, wallRb.velocity.y);
            anim.SetBool("running", false);
        }
    }

    // ���ˣ���д���෽��
    public override void Hurt(float hurtBlood) {
        if (blood == 0) return;

        anim.SetTrigger("hurt");
        if (hurtBlood > blood) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // ���������ᣬ��ֹȡ����ײ���������׹
            rb.Sleep();
            blood = 0;
            Death();
        }
        else {
            blood -= hurtBlood;
        }
    }

    // ����
    void Death() {
        anim.SetBool("death", true);
        Debug.Log(this.name + " Death");
    }

    // �ݻٸõ���
    void Distory() {
        Destroy(this.gameObject);
    }
}

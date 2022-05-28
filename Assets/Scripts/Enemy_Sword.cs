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
    private float destination; // Ѳ��������һ��
    [Space]
    public GameObject sword; // ��
    private Animator animSword;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        animSword = sword.GetComponent<Animator>();
        moveDealyTime = Random.Range(10f, 20f); // 10s��20s�ӳٴ���һ���ƶ�
        attackDealyTime = Random.Range(1f, 5f); // ����1s��5s�Ĺ����ӳ�

        SetATK(); // Ϊ�����蹥����
    }

    // Update is called once per frame
    void Update() {
        // ׼���ù������ȴ������ӳ٣�����ԣ�
        if (readyToAttack) {
            // �ӳ�ʱ�䵽��
            if (attackDealyTime <= 0) {
                attackDealyTime = Random.Range(1f, 5f); // ����1s��5s�Ĺ����ӳ�
                anim.SetTrigger("attack");
                anim.SetBool("attacking", true);
                animSword.SetTrigger("attack");
            }
            // �ӳ�ʱ��û��
            else attackDealyTime -= Time.deltaTime;
        }
        else {
            // �ڲ�׷����ҵ�ʱ��ȴ��ӳ�ʱ�䴥���ƶ�
            if (!follow) {
                // �ӳ�ʱ�䵽��
                if (moveDealyTime <= 0) {
                    if (!moving) destination = Random.Range(left_x, right_x); // ����Ŀ�ĵ�
                    moving = true;
                    MoveToDestination();
                }
                // �ӳ�ʱ��û��
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
            // �ƶ��ٶ�����
            SetVelocity(0);
            // ����뿪��Ѳ�����򣬷���
            if (transform.position.x > right_x || transform.position.x < left_x) {
                moveDealyTime = 0;
                destination = Random.Range(left_x, right_x); // ����Ŀ�ĵ�
                MoveToDestination();
            }
            // û�뿪��ԭ�ش���
            else
                anim.SetBool("running", false);
        }
    }

    // ���蹥��������д���෽��
    public override void SetATK() {
        base.SetATK();
        Sword sw = sword.GetComponent<Sword>();
        sw.SetAttack(atk);
    }

    // ��������
    public void AttackIsOver() {
        anim.SetBool("attacking", false);
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
            SetVelocity(tag * speed);
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

            SetVelocity(tag * speed);
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
            SetVelocity(0);
            anim.SetBool("running", false);
        }
    }

    // ���˷���
    public void CounterAttack() {
        if (Random.Range(0, 2) == 0) {
            readyToAttack = true;
            attackDealyTime = 0;
        }
    }

    public override void Hurt(float hurtBlood) {
        if (blood == 0) return;

        // ����ʱȡ�����Ĵ���������
        if (hurtBlood >= blood) animSword.SetBool("death", true);
        base.Hurt(hurtBlood);
    }
}

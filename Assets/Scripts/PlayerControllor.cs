using UnityEngine;
using System.Collections;

public class PlayerControllor : MonoBehaviour {
    [SerializeField] float speed = 4.0f; // �����ٶ�
    [SerializeField] float jumpForce = 7.5f; // ��Ծ��
    private SaveData saveData; // ����״̬
    [Space]
    public GameObject sword; // ���Ĵ�����
    public GameObject shield; // ���ƵĴ�����

    private Animator animator;
    private Animator animatorSword;
    private Rigidbody2D body2d;
    private GroundSensor groundSensor;
    private bool grounded = false;
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        animatorSword = sword.GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
    }

    // Update is called once per frame
    void Update() {

        // �������¹������ʱ��
        timeSinceAttack += Time.deltaTime;

        // �����ŵ�״̬
        if (!grounded && groundSensor.State()) {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        // �������״̬
        if (grounded && !groundSensor.State()) {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }

        // ��ȡHorizontal��Ӧ��λ�����ֵ
        float inputX = Input.GetAxis("Horizontal");

        // ���ý�ɫ����
        if (inputX > 0) {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (inputX < 0) {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        // ���ÿ��д�ֱ�ٶ�
        animator.SetFloat("AirSpeedY", body2d.velocity.y);

        /*if (Input.GetMouseButtonUp(0)) {
            Invoke(nameof(SetSwordAndShieldFalse), 0.5f);
        }*/

        // ����������������
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
            currentAttack++;

            // ѭ��
            if (currentAttack > 3)
                currentAttack = 1;

            // �������̫����Ϊ��һ�¹���
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // ���ô�����
            /*if (currentAttack == 1) {
                sword1.gameObject.SetActive(true);
                sword2.gameObject.SetActive(false);
                sword3.gameObject.SetActive(false);
                shield.gameObject.SetActive(false);
            }
            else if (currentAttack == 2) {
                sword1.gameObject.SetActive(false);
                sword2.gameObject.SetActive(true);
                sword3.gameObject.SetActive(false);
                shield.gameObject.SetActive(false);
            }
            else if (currentAttack == 3) {
                sword1.gameObject.SetActive(false);
                sword2.gameObject.SetActive(false);
                sword3.gameObject.SetActive(true);
                shield.gameObject.SetActive(false);
            }*/

            // ����n�¹�����Triggerѡ��
            animator.SetTrigger("Attack" + currentAttack);
            animatorSword.SetTrigger("Attack" + currentAttack);

            // ���ù������
            timeSinceAttack = 0.0f;
        }
        // ��������������Ҽ�
        else if (Input.GetMouseButtonDown(1)) {
            /*sword1.gameObject.SetActive(false);
            sword2.gameObject.SetActive(false);
            sword3.gameObject.SetActive(false);
            shield.gameObject.SetActive(true);*/

            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }
        // ȡ���������ɿ�����Ҽ�
        else if (Input.GetMouseButtonUp(1)) {
            //SetSwordAndShieldFalse();

            animator.SetBool("IdleBlock", false);
        }
        // ��Ծ
        else if (Input.GetKeyDown("space") && grounded) {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
        // ����
        else if (Mathf.Abs(inputX) > Mathf.Epsilon) {
            // ����վ���ӳ�
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }
        // վ��
        else {
            // �ȴ�վ���ӳ�
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0) {
                animator.SetInteger("AnimState", 0);
            }
        }
    }

    /*void SetSwordAndShieldFalse() {
        sword1.gameObject.SetActive(false);
        sword2.gameObject.SetActive(false);
        sword3.gameObject.SetActive(false);
        shield.gameObject.SetActive(false);
    }*/
}
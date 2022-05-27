using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    [SerializeField] float speed = 4.0f; // �����ٶ�
    [SerializeField] float jumpForce = 7.5f; // ��Ծ��
    [SerializeField] float blood = 100f; // Ѫ��
    private float normalSpeed; // Ĭ���ٶ�
    private SaveData saveData; // ����״̬
    [Space]
    public GameObject sword; // ���Ĵ�����
    public GameObject shield; // ���ƵĴ�����
    [Space]
    private Animator animator;
    private Animator animatorSword;
    private Animator animatorShield;
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
        animatorShield = shield.GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
        normalSpeed = speed;
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

        // ��סShift����
        if (Input.GetKey(KeyCode.LeftShift)) speed = normalSpeed * 3 / 2;
        else speed = normalSpeed;
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        // ���ÿ��д�ֱ�ٶ�
        animator.SetFloat("AirSpeedY", body2d.velocity.y);

        // ����������������
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
            currentAttack++;

            // ѭ��
            if (currentAttack > 3)
                currentAttack = 1;

            // �������̫����Ϊ��һ�¹���
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // ����n�¹�����Triggerѡ��
            animator.SetTrigger("Attack" + currentAttack);
            animatorSword.SetTrigger("Attack" + currentAttack);
            animatorShield.SetBool("Defensing", false);

            // ���ù������
            timeSinceAttack = 0.0f;
        }
        // ��������������Ҽ�
        else if (Input.GetMouseButtonDown(1)) {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
            animatorShield.SetTrigger("Defense");
            animatorShield.SetBool("Defensing", true);
        }
        // ȡ���������ɿ�����Ҽ�
        else if (Input.GetMouseButtonUp(1)) {
            animator.SetBool("IdleBlock", false);
            animatorShield.SetBool("Defensing", false);
        }
        // ��Ծ
        else if (Input.GetKeyDown("space") && grounded) {
            animator.SetTrigger("Jump");
            animatorShield.SetBool("Defensing", false);
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

    public void Hurt(float hurtBlood) {
        if (hurtBlood > blood) {
            blood = 0;
            Dead();
        }
        else {
            blood -= hurtBlood;
        }
    }

    private void Dead() {
        // todo
        Debug.Log(this.name + "Dead");
    }
}
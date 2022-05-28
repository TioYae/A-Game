using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    [SerializeField] float speed = 2.0f; // �����ٶ�
    [SerializeField] float jumpForce = 5f; // ��Ծ��
    [SerializeField] float atk = 10f; // ����������
    [SerializeField] float blood = 100f; // Ѫ��
    private float normalSpeed; // Ĭ���ٶ�
    [Space]
    public GameObject sword; // ���Ĵ�����
    public GameObject shield; // ���ƵĴ�����
    //public GameObject deathMenu; // �����˵�
    [Space]
    private Animator animator;
    private Animator animatorSword;
    private Animator animatorShield;
    private Rigidbody2D rb;
    private GroundSensor groundSensor; // ���洫����
    private Sword sw; // ������
    [Space]
    private bool grounded = false; // �Ƿ��ڵ���
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;
    private bool attacking = false; // �Ƿ����ڹ���
    private bool defining = false; // �Ƿ����ڷ���
    [Space]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip death1;
    private AudioSource audioSource;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        animatorSword = sword.GetComponent<Animator>();
        animatorShield = shield.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
        normalSpeed = speed;
        audioSource = this.GetComponent<AudioSource>();
        sw = sword.GetComponent<Sword>();
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

        // ���ÿ��д�ֱ�ٶ�
        animator.SetFloat("AirSpeedY", rb.velocity.y);

        GetInput();
    }

    void GetInput() {
        // ��ȡHorizontal��Ӧ��λ(A/D)�����ֵ
        float inputX = Input.GetAxis("Horizontal");

        // ���ý�ɫ����
        if (inputX > 0) {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (inputX < 0) {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        // ��סShift����
        if (Input.GetKey(KeyCode.LeftShift)) speed = normalSpeed * 1.5f;
        else speed = normalSpeed;
        // ���ڹ����������ڷ��������ƶ�
        if (!attacking && !defining)
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
        // ����������������
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
            attacking = true;
            currentAttack++;

            // ѭ��
            if (currentAttack > 3)
                currentAttack = 1;

            // �������̫����Ϊ��һ�¹���
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            switch (currentAttack) {
                case 1:
                    // Ϊ�����蹥����
                    sw.SetAttack(atk);

                    audioSource.clip = attack1;
                    audioSource.Play();
                    break;
                case 2:
                    // ��2���˺�Ϊ1.1������������
                    sw.SetAttack(atk * 1.1f);

                    audioSource.clip = attack1;
                    audioSource.Play();
                    break;
                case 3:
                    // ��3���˺�Ϊ1.25������������
                    sw.SetAttack(atk * 1.25f);

                    audioSource.clip = attack2;
                    audioSource.Play();
                    break;
                default:
                    break;
            }

            // ����n�¹�����Triggerѡ��
            animator.SetTrigger("Attack" + currentAttack);
            animatorSword.SetTrigger("Attack" + currentAttack);
            animatorShield.SetBool("Defensing", false);

            // ���ù������
            timeSinceAttack = 0.0f;
        }
        // ��������������Ҽ�
        else if (Input.GetMouseButtonDown(1)) {
            defining = true;
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
            animatorShield.SetTrigger("Defense");
            animatorShield.SetBool("Defensing", true);
        }
        // ȡ���������ɿ�����Ҽ�
        else if (Input.GetMouseButtonUp(1)) {
            defining = false;
            animator.SetBool("IdleBlock", false);
            animatorShield.SetBool("Defensing", false);
        }
        // ��Ծ
        else if (Input.GetKeyDown("space") && grounded) {
            animator.SetTrigger("Jump");
            animatorShield.SetBool("Defensing", false);
            grounded = false;
            animator.SetBool("Grounded", grounded);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

    // ��������
    public void AttackFinished() {
        attacking = false;
    }

    // ����
    public void Hurt(float hurtBlood) {
        if (blood == 0) return;

        animator.SetTrigger("Hurt");
        animatorSword.SetTrigger("hurt");
        if (hurtBlood >= blood) {
            blood = 0;
            // �л���������
            animator.SetBool("Death", true);
            audioSource.clip = death1;
            audioSource.Play();
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

    public void Death() {
        // ��ͣ��Ϸ
        Time.timeScale = 0f;
        // �������˵�
        //deathMenu.SetActive(true);
        Debug.Log(this.name + " Dead");
    }
}
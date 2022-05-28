using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    [SerializeField] float speed = 2.0f; // 主角速度
    [SerializeField] float jumpForce = 5f; // 跳跃力
    [SerializeField] float atk = 10f; // 基础攻击力
    [SerializeField] float blood = 100f; // 血量
    private float normalSpeed; // 默认速度
    [Space]
    public GameObject sword; // 剑的触发器
    public GameObject shield; // 盾牌的触发器
    //public GameObject deathMenu; // 死亡菜单
    [Space]
    private Animator animator;
    private Animator animatorSword;
    private Animator animatorShield;
    private Rigidbody2D rb;
    private GroundSensor groundSensor; // 地面传感器
    private Sword sw; // 剑的类
    [Space]
    private bool grounded = false; // 是否在地面
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;
    private bool attacking = false; // 是否正在攻击
    private bool defining = false; // 是否正在防御
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
        // 持续更新攻击间隔时间
        timeSinceAttack += Time.deltaTime;

        // 设置着地状态
        if (!grounded && groundSensor.State()) {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        // 设置离地状态
        if (grounded && !groundSensor.State()) {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }

        // 设置空中垂直速度
        animator.SetFloat("AirSpeedY", rb.velocity.y);

        GetInput();
    }

    void GetInput() {
        // 获取Horizontal对应键位(A/D)输入的值
        float inputX = Input.GetAxis("Horizontal");

        // 设置角色朝向
        if (inputX > 0) {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (inputX < 0) {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        // 按住Shift加速
        if (Input.GetKey(KeyCode.LeftShift)) speed = normalSpeed * 1.5f;
        else speed = normalSpeed;
        // 正在攻击或者正在防御不能移动
        if (!attacking && !defining)
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
        // 攻击，输入鼠标左键
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
            attacking = true;
            currentAttack++;

            // 循环
            if (currentAttack > 3)
                currentAttack = 1;

            // 攻击间隔太大，切为第一下攻击
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            switch (currentAttack) {
                case 1:
                    // 为剑赋予攻击力
                    sw.SetAttack(atk);

                    audioSource.clip = attack1;
                    audioSource.Play();
                    break;
                case 2:
                    // 第2击伤害为1.1倍基础攻击力
                    sw.SetAttack(atk * 1.1f);

                    audioSource.clip = attack1;
                    audioSource.Play();
                    break;
                case 3:
                    // 第3击伤害为1.25倍基础攻击力
                    sw.SetAttack(atk * 1.25f);

                    audioSource.clip = attack2;
                    audioSource.Play();
                    break;
                default:
                    break;
            }

            // 将第n下攻击的Trigger选中
            animator.SetTrigger("Attack" + currentAttack);
            animatorSword.SetTrigger("Attack" + currentAttack);
            animatorShield.SetBool("Defensing", false);

            // 重置攻击间隔
            timeSinceAttack = 0.0f;
        }
        // 防御，输入鼠标右键
        else if (Input.GetMouseButtonDown(1)) {
            defining = true;
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
            animatorShield.SetTrigger("Defense");
            animatorShield.SetBool("Defensing", true);
        }
        // 取消防御，松开鼠标右键
        else if (Input.GetMouseButtonUp(1)) {
            defining = false;
            animator.SetBool("IdleBlock", false);
            animatorShield.SetBool("Defensing", false);
        }
        // 跳跃
        else if (Input.GetKeyDown("space") && grounded) {
            animator.SetTrigger("Jump");
            animatorShield.SetBool("Defensing", false);
            grounded = false;
            animator.SetBool("Grounded", grounded);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
        // 奔跑
        else if (Mathf.Abs(inputX) > Mathf.Epsilon) {
            // 重置站立延迟
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }
        // 站立
        else {
            // 等待站立延迟
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0) {
                animator.SetInteger("AnimState", 0);
            }
        }
    }

    // 攻击完了
    public void AttackFinished() {
        attacking = false;
    }

    // 受伤
    public void Hurt(float hurtBlood) {
        if (blood == 0) return;

        animator.SetTrigger("Hurt");
        animatorSword.SetTrigger("hurt");
        if (hurtBlood >= blood) {
            blood = 0;
            // 切换死亡动画
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
        // 暂停游戏
        Time.timeScale = 0f;
        // 打开死亡菜单
        //deathMenu.SetActive(true);
        Debug.Log(this.name + " Dead");
    }
}
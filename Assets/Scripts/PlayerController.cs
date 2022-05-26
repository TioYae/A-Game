using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    [SerializeField] float speed = 4.0f; // 主角速度
    [SerializeField] float jumpForce = 7.5f; // 跳跃力
    [SerializeField] float blood = 100f; // 血量
    private float normalSpeed; // 默认速度
    private SaveData saveData; // 主角状态
    [Space]
    public GameObject sword; // 剑的触发器
    public GameObject shield; // 盾牌的触发器
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

        // 获取Horizontal对应键位输入的值
        float inputX = Input.GetAxis("Horizontal");

        // 设置角色朝向
        if (inputX > 0) {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (inputX < 0) {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }

        // 按住Shift加速
        if (Input.GetKey(KeyCode.LeftShift)) speed = normalSpeed * 3 / 2;
        else speed = normalSpeed;
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        // 设置空中垂直速度
        animator.SetFloat("AirSpeedY", body2d.velocity.y);

        // 攻击，输入鼠标左键
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
            currentAttack++;

            // 循环
            if (currentAttack > 3)
                currentAttack = 1;

            // 攻击间隔太大，切为第一下攻击
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // 将第n下攻击的Trigger选中
            animator.SetTrigger("Attack" + currentAttack);
            animatorSword.SetTrigger("Attack" + currentAttack);

            // 重置攻击间隔
            timeSinceAttack = 0.0f;
        }
        // 防御，输入鼠标右键
        else if (Input.GetMouseButtonDown(1)) {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
            animatorShield.SetTrigger("Defense");
            animatorShield.SetBool("Defensing", true);
        }
        // 取消防御，松开鼠标右键
        else if (Input.GetMouseButtonUp(1)) {
            animator.SetBool("IdleBlock", false);
            animatorShield.SetBool("Defensing", false);
        }
        // 跳跃
        else if (Input.GetKeyDown("space") && grounded) {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
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
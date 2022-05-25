using UnityEngine;
using System.Collections;

public class PlayerControllor : MonoBehaviour {
    [SerializeField] float speed = 4.0f; // 主角速度
    [SerializeField] float jumpForce = 7.5f; // 跳跃力
    private SaveData saveData; // 主角状态
    [Space]
    public GameObject sword; // 剑的触发器
    public GameObject shield; // 盾牌的触发器

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

        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        // 设置空中垂直速度
        animator.SetFloat("AirSpeedY", body2d.velocity.y);

        /*if (Input.GetMouseButtonUp(0)) {
            Invoke(nameof(SetSwordAndShieldFalse), 0.5f);
        }*/

        // 攻击，输入鼠标左键
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
            currentAttack++;

            // 循环
            if (currentAttack > 3)
                currentAttack = 1;

            // 攻击间隔太大，切为第一下攻击
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // 启用触发器
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

            // 将第n下攻击的Trigger选中
            animator.SetTrigger("Attack" + currentAttack);
            animatorSword.SetTrigger("Attack" + currentAttack);

            // 重置攻击间隔
            timeSinceAttack = 0.0f;
        }
        // 防御，输入鼠标右键
        else if (Input.GetMouseButtonDown(1)) {
            /*sword1.gameObject.SetActive(false);
            sword2.gameObject.SetActive(false);
            sword3.gameObject.SetActive(false);
            shield.gameObject.SetActive(true);*/

            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }
        // 取消防御，松开鼠标右键
        else if (Input.GetMouseButtonUp(1)) {
            //SetSwordAndShieldFalse();

            animator.SetBool("IdleBlock", false);
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

    /*void SetSwordAndShieldFalse() {
        sword1.gameObject.SetActive(false);
        sword2.gameObject.SetActive(false);
        sword3.gameObject.SetActive(false);
        shield.gameObject.SetActive(false);
    }*/
}
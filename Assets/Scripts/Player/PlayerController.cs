using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class PlayerController : MonoBehaviour {
    [SerializeField] float speed = 3.0f; // 当前速度
    [SerializeField] float jumpForce = 5f; // 跳跃力
    [SerializeField] float atk = 10f; // 基础攻击力
    [SerializeField] float blood = 100f; // 血量
    [SerializeField] int exp; // 经验值
    [SerializeField] int level; // 当前等级
    [SerializeField] List<float> atkLevel; // 每一级的攻击力
    [SerializeField] List<float> bloodLevel; // 每一级的血量上限
    [SerializeField] List<int> expLevel; // 升级需要的经验值
    private float bloodMax; // 最大血量
    private float normalSpeed; // 默认速度
    private float speedRemember; // 记录初始速度
    [Space]
    public GameObject sword; // 剑的触发器
    public GameObject popupDamage; // 伤害数字
    //public GameObject shield; // 盾牌的触发器
    //public GameObject deathMenu; // 死亡菜单
    [Space]
    private Animator animator;
    private Animator animatorSword;
    private Rigidbody2D rb;
    private GroundSensor groundSensor; // 地面传感器
    private Sword sw; // 剑的类
    public Image bloodImage; // 血条
    [Space]
    private bool grounded = false; // 是否在地面
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;
    private bool attacking = false; // 是否正在攻击
    private bool defining = false; // 是否正在防御
    private bool fire = false; // 是否烧伤
    private bool water = false; // 是否迟滞
    private float fireHurt; // 烧伤伤害
    public float fireTime; // 烧伤持续时间
    public float waterTime; // 迟滞持续时间
    private float fireStatusTime; // 已持续烧伤时间
    private float waterStatusTime; // 已持续迟滞时间
    public GameObject fireImage; // 烧伤标志
    public GameObject waterImage; // 迟缓标志
    [Space]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip death1;
    private AudioSource audioSource;
    //反弹机制
    public static bool isNearBird = false;
    public static int reBoundCount = 0;
    public GameObject theBird;
    private Vector2 playReBoundDirect;
    public float reBoundForce;

    // Use this for initialization
    void Start() {
        Load();
        atk = atkLevel[level - 1];
        blood = bloodLevel[level - 1];
        bloodMax = blood;
        animator = GetComponent<Animator>();
        animatorSword = sword.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
        normalSpeed = speed;
        speedRemember = speed;
        audioSource = this.GetComponent<AudioSource>();
        sw = sword.GetComponent<Sword>();
    }

    // Update is called once per frame
    void Update() {
        // 烧伤计时
        if(fire)
            fireStatusTime += Time.deltaTime;

        // 迟滞
        if (water) {
            // 默认速度设为初始速度的1/2，速度更新在判断是否按下左Shift处，此处无需更新
            normalSpeed = speedRemember / 2;
            waterStatusTime += Time.deltaTime;
            // 持续时间已到，取消迟滞状态
            if (waterStatusTime >= waterTime) {
                water = false;
                waterStatusTime = 0;
                normalSpeed = speedRemember;
                waterImage.SetActive(false);
            }
        }

        // 更新血量
        bloodImage.transform.GetChild(0).GetComponent<Image>().fillAmount = blood / bloodMax;

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
            this.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (inputX < 0) {
            this.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // 按住Shift加速
        if (Input.GetKey(KeyCode.LeftShift)) speed = normalSpeed * 1.5f;
        else speed = normalSpeed;
        // 正在地面攻击或者正在防御不能移动
        if ((!attacking && !defining) || (attacking && !grounded))
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
        // 攻击，输入鼠标左键
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
            this.tag = "Player";
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
                    break;
                case 2:
                    // 第2击伤害为1.1倍基础攻击力
                    sw.SetAttack(atk * 1.1f);

                    audioSource.clip = attack1;
                    break;
                case 3:
                    // 第3击伤害为1.25倍基础攻击力
                    sw.SetAttack(atk * 1.25f);

                    audioSource.clip = attack2;
                    break;
                default:
                    break;
            }

            // 将第n下攻击的Trigger选中
            animator.SetTrigger("Attack" + currentAttack);
            animatorSword.SetTrigger("Attack" + currentAttack);

            // 重置攻击间隔
            timeSinceAttack = 0.0f;
        }
        // 防御，输入鼠标右键
        else if (Input.GetMouseButtonDown(1)) {
            this.tag = "Shield";
            attacking = false;
            defining = true;
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
            animatorSword.SetTrigger("cancel");
        }
        // 取消防御，松开鼠标右键
        else if (Input.GetMouseButtonUp(1)) {
            this.tag = "Player";
            defining = false;
            animator.SetBool("IdleBlock", false);
        }
        // 跳跃
        else if (Input.GetKeyDown("space") && grounded) {
            this.tag = "Player";
            attacking = false;
            animator.SetTrigger("Jump");
            animatorSword.SetTrigger("cancel");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
        //脚下是可踩下落物，暂命名为“雷鸟”
        else if (Input.GetKeyDown("space") && isNearBird && reBoundCount > 0 && transform.position.y > theBird.transform.position.y) {
            reBoundCount--;
            //计算角度
            playReBoundDirect = theBird.GetComponent<Lightning_Bird>().playerReboundDir;
            //Debug.Log(-playReBoundDirect);
            //对雷鸟添加一个力
            theBird.GetComponent<Rigidbody2D>().AddForce(-playReBoundDirect * reBoundForce);
            this.tag = "Player";
            attacking = false;
            animator.SetTrigger("Jump");
            animatorSword.SetTrigger("cancel");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            //Todo:给玩家垂直向上力还是斜方向力？
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.2f);
            //rb.AddForce(playReBoundDirect * reBoundForce);
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

    // 播放声音
    public void AudioPlay() {
        audioSource.Play();
    }

    // 增加经验值
    public void ExpUp(int exp) {
        this.exp += exp;
        if (level < expLevel.Count) {
            if (this.exp >= expLevel[level]) LevelUp();
        }
    }

    // 角色升级
    private void LevelUp() {
        level++;
        atk = atkLevel[level - 1];
        blood = bloodLevel[level - 1];
        bloodMax = blood;
    }

    // 攻击完了
    public void AttackFinished() {
        attacking = false;
    }

    // 受伤
    public void Hurt(float hurtBlood) {
        if (blood == 0) return;

        animator.SetTrigger("Hurt");
        animator.SetBool("Hurting", true);
        animatorSword.SetTrigger("hurt");
        if (hurtBlood >= blood) {
            blood = 0;
            // 切换死亡动画
            animator.SetBool("IsDeath", true);
            animator.SetTrigger("Death");
            audioSource.clip = death1;
            audioSource.Play();
        }
        else {
            blood -= hurtBlood;
            if (UnityEngine.Random.Range(0, 2) == 0)
                audioSource.clip = hurt1;
            else
                audioSource.clip = hurt2;
            audioSource.Play();
        }
    }

    // 结束受伤动画
    public void SetHurtingFalse() {
        animator.SetBool("Hurting", false);
    }

    // 设置异常状态
    public void SetStatus(string status, float hurt) {
        if (status.Equals("Fire")) {
            // 重复触发刷新持续时间
            if (fire) {
                fireStatusTime = 0;
            }
            else {
                fireHurt = hurt;
                fire = true;
                fireImage.SetActive(true);
                // 每秒扣除一定血量
                Invoke(nameof(HurtByFire), 1f);
            }
        }
        else if (status.Equals("Water")) {
            // 重复触发刷新持续时间
            if (water) {
                waterStatusTime = 0;
            }
            else {
                water = true;
                waterImage.SetActive(true);
            }
        }
    }

    // 烧伤
    void HurtByFire() {
        blood -= fireHurt;
        // 玩家伤害数字位置补偿
        Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
        // 伤害数字
        GameObject obj = Instantiate(popupDamage, position, Quaternion.identity);
        obj.GetComponent<DamagePopup>().value = fireHurt;
        // 持续时间已到，取消烧伤状态
        if (fireStatusTime >= fireTime) {
            fire = false;
            fireStatusTime = 0;
            fireImage.SetActive(false);
        }
        if (fire) {
            Invoke(nameof(HurtByFire), 1f);
        }
    }

    // 死亡
    public void Death() {
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结所有轴，防止取消碰撞体后物体下坠
        rb.Sleep();
        // 暂停游戏
        Time.timeScale = 0f;
        // 打开死亡菜单
        //deathMenu.SetActive(true);
        Debug.Log(this.name + " Dead");
    }

    //Todo:检查问题
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Lightning_Bird>()) {
            theBird = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Lightning_Bird>()) {
            theBird = null;
        }
    }

    // 存档
    public void Save() {
        SaveData saveData = new SaveData(exp, level, SceneManager.GetActiveScene().buildIndex + 1);

        var path = Path.Combine(Application.dataPath, "Savedata");
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists) dir.Create();

        path = Path.Combine(path, "data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists) fileInfo.Delete();

        StreamWriter writer = fileInfo.CreateText();
        writer.Write(JsonUtility.ToJson(saveData));
        writer.Flush();
        writer.Dispose();
        writer.Close();

        if (File.Exists(path)) Debug.Log("save: " + true);
        else Debug.Log("save: " + false);
    }

    // 读档
    void Load() {
        var path = Path.Combine(Application.dataPath, "savedata");
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists) {
            level = 1;
            exp = 0;
            return;
        }

        path = Path.Combine(path, "data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) {
            level = 1;
            exp = 0;
            return;
        }

        var str = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(str);
        level = saveData.GetLevel();
        exp = saveData.GetExp();
    }
}
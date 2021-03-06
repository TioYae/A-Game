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
    [SerializeField] float energy = 100f; // 瞬移需要的能量
    [SerializeField] int exp; // 经验值
    [SerializeField] int level; // 当前等级
    [SerializeField] List<float> atkLevel; // 每一级的攻击力
    [SerializeField] List<float> bloodLevel; // 每一级的血量上限
    [SerializeField] List<int> expLevel; // 升级需要的经验值

    //因道具使用需要把最大血量/能量改成public
    public float bloodMax; // 最大血量
    public float energyMax; // 最大能量
    private float normalSpeed; // 默认速度
    private Vector2 moveDirection; // 角色朝向
    [Space]
    private bool isShift;
    private bool isShiftFinish = true; // 瞬移是否完成
    private float startShiftTime;
    public float shiftCD;
    [Space]
    public GameObject sword; // 剑的触发器
    public GameObject popupDamage; // 伤害数字
    //public GameObject shield; // 盾牌的触发器
    public GameObject deathMenu; // 死亡菜单
    public GameObject pauseMenu; // 暂停菜单
    public GameObject passMenu; // 过关菜单
    public GameObject DiaLogUI; // 剧情UI
    public GameObject reburnUI; // 复活UI
    [Space]
    private Animator animator;
    private Animator animatorSword;
    private Rigidbody2D rb;
    private GroundSensor groundSensor; // 地面传感器
    private Sword sw; // 剑的类
    public Image bloodImage; // 血条
    public Image energyImage; // 能量条
    public Image expImage; // 经验条
    public Text levelText; // 等级
    [Space]
    private bool grounded = false; // 是否在地面
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;
    private bool attacking = false; // 是否正在攻击
    private bool defining = false; // 是否正在防御
    private bool controllable = true; // 是否允许玩家操控
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
    public AudioSource audioSource;
    public AudioSource bgm;
    //反弹机制
    public static bool isNearBird = false;
    public static int reBoundCount = 0;
    public GameObject theBird;
    private Vector2 playReBoundDirect;
    public float reBoundForce;
    public GameObject inventorySys;
    public bool secendaryJump = false;
    public GameObject bag;

    public Inventory Mybag;
    public Item Shoe;
    public bool haveShoe;//是否有二段跳的鞋

    // Use this for initialization
    void Start() {
        inventorySys = GameObject.Find("InventorySys");
        Load();
        haveShoe = Mybag.itemList.Contains(Shoe);
        atk = atkLevel[level - 1];
        blood = bloodLevel[level - 1];
        bloodMax = blood;
        energyMax = energy;
        animator = GetComponent<Animator>();
        animatorSword = sword.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();
        normalSpeed = speed;
        sw = sword.GetComponent<Sword>();
    }

    // Update is called once per frame
    void Update() {
        // 进剧情或开背包时禁止玩家操控
        if (DiaLogUI.activeSelf || bag.activeSelf) {
            controllable = false;
            // 取消速度
            rb.velocity = new Vector2(0, 0);
            // 还原站立
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0) {
                animator.SetInteger("AnimState", 0);
            }
        }
        else {
            controllable = true;
        }

        // 烧伤计时
        if (fire)
            fireStatusTime += Time.deltaTime;

        // 迟滞
        if (water) {
            waterStatusTime += Time.deltaTime;
            // 持续时间已到，取消迟滞状态
            if (waterStatusTime >= waterTime) {
                speed = normalSpeed;
                water = false;
                waterStatusTime = 0;
                waterImage.SetActive(false);
            }
        }

        // 更新血量、能量条、经验条、等级
        bloodImage.transform.GetChild(0).GetComponent<Image>().fillAmount = blood / bloodMax;
        energyImage.transform.GetChild(0).GetComponent<Image>().fillAmount = energy / energyMax;
        if (level < expLevel.Count) {
            expImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f * exp / expLevel[level];
            expImage.transform.GetChild(1).GetComponent<Text>().text = exp + "/" + expLevel[level];
        }
        else {
            if (exp >= expLevel[expLevel.Count - 1]) {
                expImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
                expImage.transform.GetChild(1).GetComponent<Text>().text = expLevel[expLevel.Count - 1] + "/" + expLevel[expLevel.Count - 1];
            }
            else {
                expImage.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f * exp / expLevel[expLevel.Count - 1];
                expImage.transform.GetChild(1).GetComponent<Text>().text = exp + "/" + expLevel[expLevel.Count - 1];
            }
        }
        levelText.text = "Lv." + level;

        // 持续更新攻击间隔时间
        timeSinceAttack += Time.deltaTime;

        // 设置着地状态
        if (!grounded && groundSensor.State()) {
            grounded = true;
            secendaryJump = true;
            animator.SetBool("Grounded", grounded);
        }

        // 设置离地状态
        if (grounded && !groundSensor.State()) {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }

        // 设置空中垂直速度
        animator.SetFloat("AirSpeedY", rb.velocity.y);

        // 允许操作时读取输入键位
        if (controllable)
            GetInput();

        // 自动回复能量
        if (energy < 100) {
            energy += Time.timeScale * 0.02f;
        }
    }

    private void FixedUpdate() {
        if (isShift) {
            float movePositionX = transform.position.x + moveDirection.x * 3;
            float movePositionY = transform.position.y + moveDirection.y;
            Vector2 desPos = new Vector2(movePositionX, movePositionY);
            Debug.Log(desPos);
            rb.MovePosition(desPos);
            energy -= 30f;
            //Debug.Log("energy: " + energy);
            isShift = false;
        }
    }


    void GetInput() {
        // 获取Horizontal对应键位(A/D)输入的值
        float inputX = Input.GetAxis("Horizontal");


        if (!animator.GetBool("IsDeath")) {

            // 设置角色朝向
            if (inputX > 0) {
                this.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (inputX < 0) {
                this.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            moveDirection = new Vector2(transform.localScale.x, 0).normalized;
            // 按下Shift瞬移
            if (Input.GetKeyDown(KeyCode.LeftShift) && energy >= 30) {
                isShiftFinish = false;
                startShiftTime = shiftCD;
                isShift = true;
            }
            else {
                startShiftTime -= Time.deltaTime;
                if (startShiftTime <= 0) {
                    isShiftFinish = true;
                }
            }
            // 正在地面攻击或者正在防御不能移动
            if ((!attacking && !defining) || (attacking && !grounded))
                rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
            else if (defining)
                rb.velocity = new Vector2(0, rb.velocity.y);

            // 攻击，输入鼠标左键
            if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
                if (grounded) rb.velocity = new Vector2(0, rb.velocity.y);
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
                //animatorSword.SetTrigger("Attack" + currentAttack);

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

            //二段跳
            else if (Input.GetKeyDown("space") && secendaryJump && !grounded && haveShoe) {
                this.tag = "Player";
                attacking = false;
                animator.SetTrigger("Jump");
                animatorSword.SetTrigger("cancel");
                grounded = false;
                animator.SetBool("Grounded", grounded);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                groundSensor.Disable(0.2f);
                energy -= 10f;
                secendaryJump = false;
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
    }

    // 播放剑的触发器动画并播放攻击音效
    public void SetSwordPlay(int i) {
        animatorSword.SetTrigger("Attack" + i);
        audioSource.Play();
    }

    // 增加经验值
    public void ExpUp(int exp) {
        this.exp += exp;
        //Debug.Log(this.exp);
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

    // 攻击完了，取消防御状态
    public void AttackFinished() {
        attacking = false;
        defining = false;
    }

    // 受伤
    public void Hurt(float hurtBlood, bool animPlay) {
        if (blood == 0) return;

        if (animPlay) {
            animator.SetTrigger("Hurt");
            animator.SetBool("Hurting", true);
            animatorSword.SetTrigger("hurt");
        }
        // 玩家伤害数字位置补偿
        Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
        // 伤害数字
        GameObject obj = Instantiate(popupDamage, position, Quaternion.identity);
        obj.GetComponent<DamagePopup>().value = hurtBlood;
        if (hurtBlood >= blood) {
            blood = 0;
            rb.velocity = new Vector2(0, rb.velocity.y);
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
            if (animPlay) audioSource.Play();
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
                speed /= 2;
                water = true;
                waterImage.SetActive(true);
            }
        }
    }

    // 烧伤
    void HurtByFire() {
        Hurt(fireHurt, false);
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

    // 显示死亡菜单
    public void DeathOrReburn() {
        // 取消Death的trigger
        animator.SetTrigger("Useless");

        if (inventorySys.GetComponent<InventorySys>().findRevivePotion()) {
            reburnUI.SetActive(true);
        }
        else {
            Death();
        }
    }

    // 回血
    public void BloodUp(float value) {
        if (blood > 0 && blood < bloodMax) blood += value;
        if (blood > bloodMax) blood = bloodMax;
    }

    public void EnergyUp(float value) {
        if (energy > 0 && energy < energyMax) energy += value;
        if (energy > energyMax) energy = energyMax;
    }


    // 复活
    public void Reburn() {
        blood = bloodMax;
        animator.SetTrigger("Reburn");
        animator.SetBool("IsDeath", false);
        Time.timeScale = 1f;
    }

    // 死亡
    public void Death() {
        // 暂停游戏
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
        Debug.Log(this.name + " Dead");
    }

    //Todo:检查问题
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Lightning_Bird>()) {
            theBird = collision.gameObject;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision) {
    //    if (collision.gameObject.GetComponent<Lightning_Bird>()) {
    //        theBird = null;
    //    }
    //}

    // 存档
    public void Save() {
        inventorySys.GetComponent<InventorySys>().GetAmountofAllItems();
        SaveData saveData = new SaveData(exp, level, SceneManager.GetActiveScene().buildIndex + 1,
            inventorySys.GetComponent<InventorySys>().GetPackages(), inventorySys.GetComponent<InventorySys>().GetAmount());

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
            Debug.Log("!dir.Exists");
            level = 1;
            exp = 0;
            inventorySys.GetComponent<InventorySys>().SetPackage(new List<Item>(new Item[18]));
            inventorySys.GetComponent<InventorySys>().SetAmount(new List<int>(new int[18]));
            InventoryManager.RefreshItem();

            return;
        }

        path = Path.Combine(path, "data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) {
            Debug.Log("!fileInfo.Exists");
            level = 1;
            exp = 0;
            inventorySys.GetComponent<InventorySys>().SetPackage(new List<Item>(new Item[18]));
            inventorySys.GetComponent<InventorySys>().SetAmount(new List<int>(new int[18]));
            InventoryManager.RefreshItem();

            return;
        }

        var str = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(str);
        level = saveData.GetLevel();
        exp = saveData.GetExp();
        inventorySys.GetComponent<InventorySys>().SetPackage(saveData.GetPackage());
        inventorySys.GetComponent<InventorySys>().SetAmountofAllItems(saveData.GetPackage(), saveData.GetItemsAmount());
        InventoryManager.RefreshItem();

    }

    // 播放BGM
    public void BGMPlay() {
        bgm.Play();

    }


}
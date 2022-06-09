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
    [SerializeField] float speed = 3.0f; // ��ǰ�ٶ�
    [SerializeField] float jumpForce = 5f; // ��Ծ��
    [SerializeField] float atk = 10f; // ����������
    [SerializeField] float blood = 100f; // Ѫ��
    [SerializeField] float energy = 100f; // ˲����Ҫ������
    [SerializeField] int exp; // ����ֵ
    [SerializeField] int level; // ��ǰ�ȼ�
    [SerializeField] List<float> atkLevel; // ÿһ���Ĺ�����
    [SerializeField] List<float> bloodLevel; // ÿһ����Ѫ������
    [SerializeField] List<int> expLevel; // ������Ҫ�ľ���ֵ
    private float bloodMax; // ���Ѫ��
    private float energyMax; // �������
    private float normalSpeed; // Ĭ���ٶ�
    private Vector2 moveDirection; // ��ɫ����
    [Space]
    private bool isShift;
    private bool isShiftFinish = true; // ˲���Ƿ����
    private float startShiftTime;
    public float shiftCD;
    [Space]
    public GameObject sword; // ���Ĵ�����
    public GameObject popupDamage; // �˺�����
    //public GameObject shield; // ���ƵĴ�����
    public GameObject deathMenu; // �����˵�
    public GameObject pauseMenu; // ��ͣ�˵�
    public GameObject passMenu; // ���ز˵�
    public GameObject DiaLogUI; // ����UI
    public GameObject reburnUI; // ����UI
    [Space]
    private Animator animator;
    private Animator animatorSword;
    private Rigidbody2D rb;
    private GroundSensor groundSensor; // ���洫����
    private Sword sw; // ������
    public Image bloodImage; // Ѫ��
    public Image energyImage; // ������
    public Image expImage; // ������
    public Text levelText; // �ȼ�
    [Space]
    private bool grounded = false; // �Ƿ��ڵ���
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;
    private bool attacking = false; // �Ƿ����ڹ���
    private bool defining = false; // �Ƿ����ڷ���
    private bool controllable = true; // �Ƿ�������Ҳٿ�
    private bool fire = false; // �Ƿ�����
    private bool water = false; // �Ƿ����
    private float fireHurt; // �����˺�
    public float fireTime; // ���˳���ʱ��
    public float waterTime; // ���ͳ���ʱ��
    private float fireStatusTime; // �ѳ�������ʱ��
    private float waterStatusTime; // �ѳ�������ʱ��
    public GameObject fireImage; // ���˱�־
    public GameObject waterImage; // �ٻ���־
    [Space]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip death1;
    public AudioSource audioSource;
    public AudioSource bgm;
    //��������
    public static bool isNearBird = false;
    public static int reBoundCount = 0;
    public GameObject theBird;
    private Vector2 playReBoundDirect;
    public float reBoundForce;
    public GameObject inventorySys;

    public GameObject bag;

    // Use this for initialization
    void Start() {
        Load();
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
        inventorySys = GameObject.Find("InventorySys");


    }

    // Update is called once per frame
    void Update() {
        // ������򿪱���ʱ��ֹ��Ҳٿ�
        if (DiaLogUI.activeSelf || bag.activeSelf) {
            controllable = false;
            // ȡ���ٶ�
            rb.velocity = new Vector2(0, 0);
            // ��ԭվ��
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0) {
                animator.SetInteger("AnimState", 0);
            }
        }
        else {
            controllable = true;
        }

        // ���˼�ʱ
        if (fire)
            fireStatusTime += Time.deltaTime;

        // ����
        if (water) {
            waterStatusTime += Time.deltaTime;
            // ����ʱ���ѵ���ȡ������״̬
            if (waterStatusTime >= waterTime) {
                speed = normalSpeed;
                water = false;
                waterStatusTime = 0;
                waterImage.SetActive(false);
            }
        }

        // ����Ѫ���������������������ȼ�
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

        // �������ʱ��ȡ�����λ
        if (controllable)
            GetInput();

        // �Զ��ظ�����
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
        // ��ȡHorizontal��Ӧ��λ(A/D)�����ֵ
        float inputX = Input.GetAxis("Horizontal");

        // ���ý�ɫ����
        if (inputX > 0) {
            this.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (inputX < 0) {
            this.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        moveDirection = new Vector2(transform.localScale.x, 0).normalized;
        // ����Shift˲��
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
        // ���ڵ��湥���������ڷ��������ƶ�
        if ((!attacking && !defining) || (attacking && !grounded))
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        else if (defining)
            rb.velocity = new Vector2(0, rb.velocity.y);

        // ����������������
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
            if (grounded) rb.velocity = new Vector2(0, rb.velocity.y);
            this.tag = "Player";
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
                    break;
                case 2:
                    // ��2���˺�Ϊ1.1������������
                    sw.SetAttack(atk * 1.1f);

                    audioSource.clip = attack1;
                    break;
                case 3:
                    // ��3���˺�Ϊ1.25������������
                    sw.SetAttack(atk * 1.25f);

                    audioSource.clip = attack2;
                    break;
                default:
                    break;
            }

            // ����n�¹�����Triggerѡ��
            animator.SetTrigger("Attack" + currentAttack);
            //animatorSword.SetTrigger("Attack" + currentAttack);

            // ���ù������
            timeSinceAttack = 0.0f;
        }
        // ��������������Ҽ�
        else if (Input.GetMouseButtonDown(1)) {
            this.tag = "Shield";
            attacking = false;
            defining = true;
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
            animatorSword.SetTrigger("cancel");
        }
        // ȡ���������ɿ�����Ҽ�
        else if (Input.GetMouseButtonUp(1)) {
            this.tag = "Player";
            defining = false;
            animator.SetBool("IdleBlock", false);
        }
        // ��Ծ
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
        //�����ǿɲ������������Ϊ������
        else if (Input.GetKeyDown("space") && isNearBird && reBoundCount > 0 && transform.position.y > theBird.transform.position.y) {
            reBoundCount--;
            //����Ƕ�
            playReBoundDirect = theBird.GetComponent<Lightning_Bird>().playerReboundDir;
            //Debug.Log(-playReBoundDirect);
            //���������һ����
            theBird.GetComponent<Rigidbody2D>().AddForce(-playReBoundDirect * reBoundForce);
            this.tag = "Player";
            attacking = false;
            animator.SetTrigger("Jump");
            animatorSword.SetTrigger("cancel");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            //Todo:����Ҵ�ֱ����������б��������
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.2f);
            //rb.AddForce(playReBoundDirect * reBoundForce);
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

    // ���Ž��Ĵ��������������Ź�����Ч
    public void SetSwordPlay(int i) {
        animatorSword.SetTrigger("Attack" + i);
        audioSource.Play();
    }

    // ���Ӿ���ֵ
    public void ExpUp(int exp) {
        this.exp += exp;
        //Debug.Log(this.exp);
        if (level < expLevel.Count) {
            if (this.exp >= expLevel[level]) LevelUp();
        }
    }

    // ��ɫ����
    private void LevelUp() {
        level++;
        atk = atkLevel[level - 1];
        blood = bloodLevel[level - 1];
        bloodMax = blood;
    }

    // �������ˣ�ȡ������״̬
    public void AttackFinished() {
        attacking = false;
        defining = false;
    }

    // ����
    public void Hurt(float hurtBlood, bool animPlay) {
        if (blood == 0) return;

        if (animPlay) {
            animator.SetTrigger("Hurt");
            animator.SetBool("Hurting", true);
            animatorSword.SetTrigger("hurt");
        }
        // ����˺�����λ�ò���
        Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
        // �˺�����
        GameObject obj = Instantiate(popupDamage, position, Quaternion.identity);
        obj.GetComponent<DamagePopup>().value = hurtBlood;
        if (hurtBlood >= blood) {
            blood = 0;
            // �л���������
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

    // �������˶���
    public void SetHurtingFalse() {
        animator.SetBool("Hurting", false);
    }

    // �����쳣״̬
    public void SetStatus(string status, float hurt) {
        if (status.Equals("Fire")) {
            // �ظ�����ˢ�³���ʱ��
            if (fire) {
                fireStatusTime = 0;
            }
            else {
                fireHurt = hurt;
                fire = true;
                fireImage.SetActive(true);
                // ÿ��۳�һ��Ѫ��
                Invoke(nameof(HurtByFire), 1f);
            }
        }
        else if (status.Equals("Water")) {
            // �ظ�����ˢ�³���ʱ��
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

    // ����
    void HurtByFire() {
        Hurt(fireHurt, false);
        // ����ʱ���ѵ���ȡ������״̬
        if (fireStatusTime >= fireTime) {
            fire = false;
            fireStatusTime = 0;
            fireImage.SetActive(false);
        }
        if (fire) {
            Invoke(nameof(HurtByFire), 1f);
        }
    }

    // ��ʾ�����˵�
    public void DeathOrReburn() {
        // ȡ��Death��trigger
        animator.SetTrigger("Useless");
        if (inventorySys.GetComponent<InventorySys>().findRevivePotion()) {
            reburnUI.SetActive(true);
        }
        else {
            Death();
        }
    }

    // ��Ѫ
    public void BloodUp(float value) {
        if (blood > 0 && blood < bloodMax) blood += value;
        if (blood > bloodMax) blood = bloodMax;
    }

    // ����
    public void Reburn() {
        blood = bloodMax;
        animator.SetTrigger("Reburn");
        Time.timeScale = 1f;
    }

    // ����
    public void Death() {
        // ��ͣ��Ϸ
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
        Debug.Log(this.name + " Dead");
    }

    //Todo:�������
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

    // �浵
    public void Save() {
        SaveData saveData = new SaveData(exp, level, SceneManager.GetActiveScene().buildIndex + 1, inventorySys.GetComponent<InventorySys>().GetPackages());

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

    // ����
    void Load() {
        var path = Path.Combine(Application.dataPath, "savedata");
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists) {
            level = 1;
            exp = 0;
            inventorySys.GetComponent<InventorySys>().SetPackage(new List<Item> { });
            return;
        }

        path = Path.Combine(path, "data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) {
            level = 1;
            exp = 0;
            inventorySys.GetComponent<InventorySys>().SetPackage(new List<Item> { });
            return;
        }

        var str = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(str);
        level = saveData.GetLevel();
        exp = saveData.GetExp();
        inventorySys.GetComponent<InventorySys>().SetPackage(saveData.GetPackage());
    }

    // ����BGM
    public void BGMPlay() {
        bgm.Play();

    }

    //��ҩ by����
    public void usePotion() {
        blood += 10;
        if (blood >= 100)
            blood = 100;
    }

    public void useBigPotion() {
        blood += 50;
        if (blood >= 100)
            blood = 100;
    }
}
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
    [SerializeField] int exp; // ����ֵ
    [SerializeField] int level; // ��ǰ�ȼ�
    [SerializeField] List<float> atkLevel; // ÿһ���Ĺ�����
    [SerializeField] List<float> bloodLevel; // ÿһ����Ѫ������
    [SerializeField] List<int> expLevel; // ������Ҫ�ľ���ֵ
    private float bloodMax; // ���Ѫ��
    private float normalSpeed; // Ĭ���ٶ�
    private float speedRemember; // ��¼��ʼ�ٶ�
    [Space]
    public GameObject sword; // ���Ĵ�����
    public GameObject shield; // ���ƵĴ�����
    //public GameObject deathMenu; // �����˵�
    [Space]
    private Animator animator;
    private Animator animatorSword;
    private Rigidbody2D rb;
    private GroundSensor groundSensor; // ���洫����
    private Sword sw; // ������
    public Image bloodImage; // Ѫ��
    [Space]
    private bool grounded = false; // �Ƿ��ڵ���
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;
    private bool attacking = false; // �Ƿ����ڹ���
    private bool defining = false; // �Ƿ����ڷ���
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
    private AudioSource audioSource;
    //��������
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
        // ���˼�ʱ
        if(fire)
            fireStatusTime += Time.deltaTime;

        // ����
        if (water) {
            // Ĭ���ٶ���Ϊ��ʼ�ٶȵ�1/2���ٶȸ������ж��Ƿ�����Shift�����˴��������
            normalSpeed = speedRemember / 2;
            waterStatusTime += Time.deltaTime;
            // ����ʱ���ѵ���ȡ������״̬
            if (waterStatusTime >= waterTime) {
                water = false;
                waterStatusTime = 0;
                normalSpeed = speedRemember;
                waterImage.SetActive(false);
            }
        }

        // ����Ѫ��
        bloodImage.transform.GetChild(0).GetComponent<Image>().fillAmount = blood / bloodMax;

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
            this.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (inputX < 0) {
            this.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // ��סShift����
        if (Input.GetKey(KeyCode.LeftShift)) speed = normalSpeed * 1.5f;
        else speed = normalSpeed;
        // ���ڵ��湥���������ڷ��������ƶ�
        if ((!attacking && !defining) || (attacking && !grounded))
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
        // ����������������
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f) {
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
            animatorSword.SetTrigger("Attack" + currentAttack);

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

    // ��������
    public void AudioPlay() {
        audioSource.Play();
    }

    // ���Ӿ���ֵ
    public void ExpUp(int exp) {
        this.exp += exp;
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
            if (UnityEngine.Random.Range(0, 2) == 0)
                audioSource.clip = hurt1;
            else
                audioSource.clip = hurt2;
            audioSource.Play();
        }
    }

    // �����쳣״̬
    public void SetStatus(string status, float atk) {
        if (status.Equals("Fire")) {
            fireHurt = atk;
            fire = true;
            fireImage.SetActive(true);
            // ÿ��۳�һ��Ѫ��
            Invoke(nameof(HurtByFire), 1f);
        }
        else if (status.Equals("Water")) {
            water = true;
            waterImage.SetActive(true);
        }
    }

    // ����
    void HurtByFire() {
        blood -= fireHurt;
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

    // ����
    public void Death() {
        // ��ͣ��Ϸ
        Time.timeScale = 0f;
        // �������˵�
        //deathMenu.SetActive(true);
        Debug.Log(this.name + " Dead");
    }

    //Todo:�������
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

    // �浵
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

    // ����
    void Load() {
        var path = Path.Combine(Application.dataPath, "savedata");
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists) level = 1;

        path = Path.Combine(path, "data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) level = 1;

        var str = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(str);
        level = saveData.GetLevel();
        exp = saveData.GetExp();
    }
}
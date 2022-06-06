using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tri_of_Platform : MonoBehaviour
{
    public bool canMove = false;

    public GameObject Platform;
    public float movingSpeed;
    public float stopTime;

    public Transform[] movePosition;
    private int i;
    [Space]
    //���ص�����״̬
    public GameObject Tri_close;
    public GameObject Tri_open;
    private bool playerStay = false; // �ж�player�Ƿ��ڻ��ظ���

    void Start()
    {
        i = 1;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
        //�������޴򿪻��أ��Ի��ص�״̬������ʾ
        Tri_close.SetActive(!canMove);
        Tri_open.SetActive(canMove);
        GetInput();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerStay = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerStay = false;
        }
    }

    void GetInput()
    {
        if (playerStay && Input.GetKeyDown(KeyCode.F))
        {
            canMove = !canMove;
        }
    }

    void MovePlatform()
    {
        if (canMove)
        {
            Platform.transform.position = Vector2.MoveTowards(Platform.transform.position, movePosition[i].position, movingSpeed * Time.deltaTime);
            if (Mathf.Abs(Platform.transform.position.y - movePosition[i].position.y) < 0.1f)
            {
                if (stopTime < 0.0f)
                {
                    if (i == 0)
                    {
                        i = 1;
                    }
                    else
                    {
                        i = 0;
                    }

                    stopTime = 0.5f;
                }
                else
                {
                    stopTime -= Time.deltaTime;
                }
            }
        }
    }
}

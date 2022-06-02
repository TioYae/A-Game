using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Bird : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject player; //��ȡ���gameObject
    public Vector2 playerReboundDir; //��ұ�������
    public float moveSpeed; //�����ʼ�ٶ�
    public float atkBird; //�����������˺�

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>().gameObject;
        }
        playerReboundDir = (player.transform.position - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.isNearBird = true;
            PlayerController.reBoundCount++;
            //Debug.Log("isNearBird = " + PlayerController.isNearBird);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController.isNearBird = false;
            if (PlayerController.reBoundCount > 0)
            {
                PlayerController.reBoundCount--;
            }
            //Debug.Log("isNearBird = " + PlayerController.isNearBird);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ҵ�ǽ��������
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }else if (collision.gameObject.tag == "Player")
        {
            //��ȡ����ҽű�
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Hurt(atkBird);
            Destroy(gameObject);
        }
    }

}
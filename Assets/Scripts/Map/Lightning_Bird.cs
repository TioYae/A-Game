using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Bird : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject player; //获取玩家gameObject
    public Vector2 playerReboundDir; //玩家被弹方向
    public float moveSpeed; //雷鸟初始速度
    public float atkBird; //雷鸟击中玩家伤害
    private Animator anim;
    //private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //boxCollider = GetComponent<BoxCollider2D>();
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
        //砸到墙体则销毁
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "OneWayPlatform")
        {
            anim.SetTrigger("boom");
        }
        else if (collision.gameObject.tag == "Player")
        {
            //获取到玩家脚本
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Hurt(atkBird);
            Destroy(gameObject);
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

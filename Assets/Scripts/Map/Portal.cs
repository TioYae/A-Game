using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform targetDoor;

    private bool isDoor;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        EnterDoor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isDoor = false;
        }
    }

    void EnterDoor()
    {
        if (Input.GetKeyDown(KeyCode.F) && isDoor)      //°´ÏÂF¼ü´«ËÍ
        {
            playerTransform.position = targetDoor.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Transform playerDefTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerDefTransform = GameObject.FindGameObjectWithTag("Player").transform.parent;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = gameObject.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = playerDefTransform;
        }
    }

}

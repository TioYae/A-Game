using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashableWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bird")
        {
            Debug.Log("touch");
            Destroy(gameObject);
        }
    }
}

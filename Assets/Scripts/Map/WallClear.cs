using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClear : MonoBehaviour
{
    public GameObject Tri_close;
    public GameObject Tri_open;
    public GameObject wall;
    private bool isWallExist = true; // 墙是否存在
    private bool playerStay = false; // 判别玩家是否在机关附近


    // Update is called once per frame
    void Update()
    {
        GetInput();
        Tri_close.SetActive(isWallExist);
        Tri_open.SetActive(!isWallExist);
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
            isWallExist = false;
            wall.SetActive(false);
        }
    }
}

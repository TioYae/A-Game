using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaLogTrigger : MonoBehaviour
{
    public GameObject DiaLogUI;
    public bool isTalk = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DiaLogUI.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTalk)
        {
            if (collision.collider.tag == "Player")
            {
                DiaLogUI.SetActive(true);
                isTalk = true;
            }
        }
    }
}

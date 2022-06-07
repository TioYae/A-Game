using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSence : MonoBehaviour {
    public GameObject PassMenu;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    //��һ��
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag.Equals("Player") && Input.GetKey(KeyCode.F)) {
            collision.gameObject.GetComponent<PlayerController>().Save();
            Time.timeScale = 0f;
            PassMenu.SetActive(true);
        }
    }
}

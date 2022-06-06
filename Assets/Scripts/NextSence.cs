using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSence : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    //обр╩╧ь
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag.Equals("Player") && Input.GetKey(KeyCode.F)) {
            collision.gameObject.GetComponent<PlayerController>().Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

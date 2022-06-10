using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaLogTrigger : MonoBehaviour {
    public GameObject DiaLogUI;
    public GameObject wall;
    public bool isTalk = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!isTalk && Input.GetKey(KeyCode.F)) {
            if (collision.tag == "Player") {
                DiaLogUI.SetActive(true);
                Destroy(wall.gameObject);
                isTalk = true;
            }
        }
    }
}

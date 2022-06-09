using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeDoor : MonoBehaviour {
    public List<GameObject> boss;
    public GameObject DialogUI;
    public GameObject door;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Check();
    }

    // 检查Boss是否已全部狗带
    void Check() {
        foreach (GameObject i in boss) {
            if (i != null) return;
        }
        if (DialogUI != null)
            DialogUI.SetActive(true);//boss死了触发剧情对话
        door.SetActive(true);
        this.gameObject.SetActive(false);

    }
}

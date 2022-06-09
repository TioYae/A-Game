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

    // ���Boss�Ƿ���ȫ������
    void Check() {
        foreach (GameObject i in boss) {
            if (i != null) return;
        }
        if (DialogUI != null)
            DialogUI.SetActive(true);//boss���˴�������Ի�
        door.SetActive(true);
        this.gameObject.SetActive(false);

    }
}

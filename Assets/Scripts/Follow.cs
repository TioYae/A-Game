using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    public Transform Target;
    private Camera maincamera; // �������

    void Start() {
        maincamera = Camera.main;
    }

    void Update() {
        if (Target != null) {
            //��Ŀ������ת��Ϊ��Ļ����
            Vector3 pos = maincamera.WorldToScreenPoint(Target.position);
            transform.position = pos;
        }

    }
}

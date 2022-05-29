using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    public Transform Target;
    private Camera maincamera; // 主摄像机

    void Start() {
        maincamera = Camera.main;
    }

    void Update() {
        if (Target != null) {
            //把目标坐标转换为屏幕坐标
            Vector3 pos = maincamera.WorldToScreenPoint(Target.position);
            transform.position = pos;
        }

    }
}

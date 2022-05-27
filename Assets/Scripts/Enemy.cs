using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float blood;
    public float minDistance = 5f;// ������Χ��Сֵ
    public float maxDistance = 10f; // ������Χ���ֵ
    public float finalDistance; // �������ʱ����ҵļ��
    public bool found = false; // �Ƿ��ҵ����
    public bool follow = false; // �Ƿ�������
    public GameObject player;

    public virtual void Hurt(float hurtBlood) {

    }

    // �ҵ����
    public void FoundPlayer() {
        finalDistance = UnityEngine.Random.Range(minDistance, maxDistance);
        found = true;
        follow = true;
    }

    // �������
    public void LostPlayer() {
        finalDistance = 0f;
        found = false;
    }
}

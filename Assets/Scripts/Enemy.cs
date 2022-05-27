using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float blood;
    public float minDistance = 5f;// 攻击范围最小值
    public float maxDistance = 10f; // 攻击范围最大值
    public float finalDistance; // 锁定玩家时与玩家的间隔
    public bool found = false; // 是否找到玩家
    public bool follow = false; // 是否跟随玩家
    public GameObject player;

    public virtual void Hurt(float hurtBlood) {

    }

    // 找到玩家
    public void FoundPlayer() {
        finalDistance = UnityEngine.Random.Range(minDistance, maxDistance);
        found = true;
        follow = true;
    }

    // 跟丢玩家
    public void LostPlayer() {
        finalDistance = 0f;
        found = false;
    }
}

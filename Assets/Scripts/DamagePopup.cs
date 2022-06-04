using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour {
    //目标位置  
    private Vector3 mTarget;
    //屏幕坐标  
    private Vector3 mScreen;
    //伤害数值  
    public float value;

    //文本宽度  
    public float ContentWidth = 100;
    //文本高度  
    public float ContentHeight = 50;

    //GUI坐标  
    private Vector2 mPoint;

    //销毁时间  
    public float FreeTime = 1.5F;

    void Start() {
        //获取目标位置  
        mTarget = transform.position;
        //获取屏幕坐标  
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //将屏幕坐标转化为GUI坐标  
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        //开启自动销毁线程  
        StartCoroutine("Free");
    }

    void Update() {
        //使文本在垂直方向上产生一个偏移  
        transform.Translate(Vector3.up * 0.5F * Time.deltaTime);
        //重新计算坐标  
        mTarget = transform.position;
        //获取屏幕坐标  
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //将屏幕坐标转化为GUI坐标  
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
    }

    void OnGUI() {
        //定义一个GUIStyle的对象
        GUIStyle labelFont = new GUIStyle();
        //设置文本颜色
        if (value > 0)
            labelFont.normal.textColor = new Color(255, 0, 0);
        else if (value < 0)
            labelFont.normal.textColor = new Color(0, 255, 0);
        else
            labelFont.normal.textColor = new Color(255, 255, 255);
        //设置字体大小
        labelFont.fontSize = 20;
        //保证目标在摄像机前方  
        if (mScreen.z > 0) {
            //内部使用GUI坐标进行绘制  
            GUI.Label(new Rect(mPoint.x, mPoint.y, ContentWidth, ContentHeight), (-value).ToString(), labelFont);
        }
    }

    IEnumerator Free() {
        yield return new WaitForSeconds(FreeTime);
        Destroy(this.gameObject);
    }
}

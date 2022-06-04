using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour {
    //Ŀ��λ��  
    private Vector3 mTarget;
    //��Ļ����  
    private Vector3 mScreen;
    //�˺���ֵ  
    public float value;

    //�ı����  
    public float ContentWidth = 100;
    //�ı��߶�  
    public float ContentHeight = 50;

    //GUI����  
    private Vector2 mPoint;

    //����ʱ��  
    public float FreeTime = 1.5F;

    void Start() {
        //��ȡĿ��λ��  
        mTarget = transform.position;
        //��ȡ��Ļ����  
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //����Ļ����ת��ΪGUI����  
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        //�����Զ������߳�  
        StartCoroutine("Free");
    }

    void Update() {
        //ʹ�ı��ڴ�ֱ�����ϲ���һ��ƫ��  
        transform.Translate(Vector3.up * 0.5F * Time.deltaTime);
        //���¼�������  
        mTarget = transform.position;
        //��ȡ��Ļ����  
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //����Ļ����ת��ΪGUI����  
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
    }

    void OnGUI() {
        //����һ��GUIStyle�Ķ���
        GUIStyle labelFont = new GUIStyle();
        //�����ı���ɫ
        if (value > 0)
            labelFont.normal.textColor = new Color(255, 0, 0);
        else if (value < 0)
            labelFont.normal.textColor = new Color(0, 255, 0);
        else
            labelFont.normal.textColor = new Color(255, 255, 255);
        //���������С
        labelFont.fontSize = 20;
        //��֤Ŀ���������ǰ��  
        if (mScreen.z > 0) {
            //�ڲ�ʹ��GUI������л���  
            GUI.Label(new Rect(mPoint.x, mPoint.y, ContentWidth, ContentHeight), (-value).ToString(), labelFont);
        }
    }

    IEnumerator Free() {
        yield return new WaitForSeconds(FreeTime);
        Destroy(this.gameObject);
    }
}

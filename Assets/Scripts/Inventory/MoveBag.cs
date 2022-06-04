using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//������ڿ�������ƶ�
public class MoveBag : MonoBehaviour, IDragHandler
{
    RectTransform currentRect;

    public void OnDrag(PointerEventData eventData)
    {
        currentRect.anchoredPosition += eventData.delta;//��װ�������ƶ�
    }

    private void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

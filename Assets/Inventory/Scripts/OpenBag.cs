using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��I���ر���

public class OpenBag : MonoBehaviour
{
    public GameObject Bag;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OpenMyBag();
    }
    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {

            Bag.SetActive(!Bag.activeSelf);
        }

    }
}

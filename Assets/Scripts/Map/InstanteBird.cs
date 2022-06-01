using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanteBird : MonoBehaviour
{
    public GameObject birdPrefab;
    [SerializeField] float countDown = 3.0f;
    float tempCountDown;
    // Start is called before the first frame update
    void Start()
    {
        tempCountDown = countDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown <= 0)
        {
            Instantiate(birdPrefab,this.transform);
            countDown = tempCountDown;
        }
        else
        {
            countDown -= Time.deltaTime;
        }
    }
}

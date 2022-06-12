using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public int clickCount = 0;
    public GameObject shoeOnWorld;
    public GameObject Player;
    public void MyBagClicked()
    {
        clickCount++;
        if (clickCount == 10)
        { 
            shoeOnWorld.GetComponent<ItemOnWorld>().AddNewItem();
        Player.GetComponent<PlayerController>().haveShoe = true;
        }

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

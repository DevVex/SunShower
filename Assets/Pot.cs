using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    MechanicsManager mechanicsManager;

    void Start()
    {
        mechanicsManager = GameObject.Find("GameManager").GetComponent<MechanicsManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Rain")
        {
            mechanicsManager.GotWater();
            Destroy(col.gameObject);
        }

        if(col.tag == "Sun")
        {
            mechanicsManager.GotSun();
            Destroy(col.gameObject);
        }
    }
}

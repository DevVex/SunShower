using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    public MechanicsManager mechanicsManager;


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Rain" || col.tag == "Sun")
        {
            Destroy(col.gameObject);
        }

        if (col.tag == "Plant")
        {
            Debug.Log("Game Over");
            col.GetComponent<Rigidbody2D>().simulated = false;
            StartCoroutine(mechanicsManager.RoundLost(0.1f));
        }
    }
}   

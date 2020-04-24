using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{

    Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        position = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = position;
    }
}

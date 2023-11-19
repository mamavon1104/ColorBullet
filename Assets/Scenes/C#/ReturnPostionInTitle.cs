using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPostionInTitle : MonoBehaviour
{
    Vector3 mypos;
    void Start()
    {
        mypos = transform.position;
    }
    private void OnDisable()
    {
        transform.position = mypos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteChildBulletInTitle : MonoBehaviour
{
    private void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setChildTag : MonoBehaviour
{
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameMaster.AddTeamID(transform.GetChild(i).tag);
        }
    }
}
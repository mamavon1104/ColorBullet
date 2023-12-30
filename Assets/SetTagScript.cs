using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTagScript : MonoBehaviour
{
    [SerializeField] Transform[] players;
    void Awake()
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameMaster.AddTeamID(players[i].tag);
        }
    }
}

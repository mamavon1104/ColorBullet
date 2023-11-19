using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonParentScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject == null)
            {
                Debug.Log("null‚Å‚·B");
            }
            else
            {
                GameMaster.AddGameObjectID(transform.GetChild(i).gameObject, 2);
            }
        }
    }
}

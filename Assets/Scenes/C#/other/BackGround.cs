using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] float scrollsMin = 0.05f;
    [SerializeField] float scrollsMax = 0.2f;
    void Update()
    {
        float scroll = Mathf.Repeat(Time.time * scrollsMin, scrollsMax);
        Vector2 offset = new Vector2(scroll, 0);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIColorChanger : MonoBehaviour
{
    [SerializeField] Color[] colors;
    TextMeshProUGUI text;
    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        ChangeColorUI();
    }
    public void ChangeColorUI()
    {
        text.color = colors[Random.Range(0, colors.Length)];
    }
}

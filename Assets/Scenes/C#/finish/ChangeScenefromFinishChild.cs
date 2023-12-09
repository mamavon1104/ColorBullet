using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenefromFinishChild : MonoBehaviour
{
    bool isthisTrue = false;
    ChangeScenefromFinishParent parent;
    string beforeString;
    TextMeshProUGUI parentText;
    private void Awake()
    {
        parentText = transform.parent.GetComponent<TextMeshProUGUI>();
        beforeString = parentText.text;
        parent = GameObject.FindGameObjectWithTag("ParentUI").GetComponent<ChangeScenefromFinishParent>();
    }
    private void OnDisable()
    {
        BeforeSetting();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameMaster.GetGameObjectID(other.gameObject) != 1 || isthisTrue)
            return;
        
        isthisTrue = true;
        parentText.text = "OK";
        transform.parent.GetChild(0).gameObject.SetActive(false);
        parent.ChangeTrue(transform.parent.parent);
        other.GetComponent<BulletController>().CreateBulletEffect(transform);
        GameMaster.audioManagerMaster.CheckAudio();
    }
    void BeforeSetting()
    {
        isthisTrue = false;
        transform.parent.GetChild(0).gameObject.SetActive(true);
        parentText.text = beforeString;
        parent.ChangeFalse(transform.parent.parent);
    }
}

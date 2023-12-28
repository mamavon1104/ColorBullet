using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIPointEnter : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] trueObj;
    [SerializeField] GameObject[] falseObj;
    [SerializeField] GameObject selectObj;
    public EventSystem m_eventSystem; // EventSystem�ɑ΂��Ă̎Q��
    [SerializeField] bool selected = false;
    OperationExplanationParent parentCS;
    UIAudioManager uIAudioManager;
    InputAction Submit, Cancel;

    private void Start()
    {
        var director = GameObject.FindGameObjectWithTag("GameDirector");
        Submit = director.GetComponent<PlayerInput>().actions["Submit"]; // �� "Move" Action�𗘗p����B
        Cancel = director.GetComponent<PlayerInput>().actions["Cancel"]; // �� "ButtonA" Action�𗘗p
        uIAudioManager = director.GetComponent<UIAudioManager>();
        parentCS = transform.parent.GetComponent<OperationExplanationParent>();
    }
    private void OnDisable()
    {
        selected�@= false;
    }
    private void OnEnable()
    {
        m_eventSystem.gameObject.SetActive(true);
    }
    private void Update()
    {
        // ���̃Q�[���I�u�W�F�N�g���J�[�\���ɂ���đI������Ă��邩�H
        bool hoverOver = m_eventSystem.currentSelectedGameObject == this.gameObject;
        
        if (selected != hoverOver)  // �O�̏�ԂƂ̔�r
        {
            selected = hoverOver;
            if (selected)
            {
                if (player != null)
                {
                    var playerTrans = player.transform;
                    Vector2 playerPos = new Vector2(playerTrans.position.x, playerTrans.position.y);
                    playerPos.y = gameObject.transform.position.y;
                    playerTrans.position = playerPos;
                }
                if (transform.tag == "FinalUI")
                {
                    m_eventSystem.gameObject.SetActive(false);
                    ChangeSelectObj();
                }
                uIAudioManager.SelectAudio();
            }
            StartCoroutine(SettingActive());
        }

        if (selected && hoverOver)
        {
            if (Submit.WasPressedThisFrame())
                parentCS?.ChangeSelectUI(gameObject, true);
            if (Cancel.WasPressedThisFrame())
                parentCS?.ChangeSelectUI(gameObject, false);
        }
    }
    IEnumerator SettingActive()
    {
        var awakebool = selected;
        yield return new WaitForSeconds(0.1f);

        if (transform.CompareTag("FinalUI"))
        {
            yield return new WaitForSeconds(0.1f);
            SetTrueObjects();
            SetFalseObjects();
            yield break;
        }

        if (trueObj.Length > 0 && awakebool == true)
            SetTrueObjects();
        if (falseObj.Length > 0 && awakebool == false)
            SetFalseObjects();
    }
    public void ChangeSelectObj()
    {
        m_eventSystem.SetSelectedGameObject(selectObj);
    }
    private void SetTrueObjects()
    {
        for (int i = 0; i < trueObj.Length; i++)
        {
            trueObj[i].SetActive(true);
        }
    }
    private void SetFalseObjects()
    {
        for (int i = 0; i < falseObj.Length; i++)
        {
            falseObj[i].SetActive(false);
        }
    }
}

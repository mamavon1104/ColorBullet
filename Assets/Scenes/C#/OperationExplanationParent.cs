using UnityEngine;
using UnityEngine.EventSystems;

public class OperationExplanationParent : MonoBehaviour
{
    public EventSystem _eventSystem;
    GameObject[] texts;
    private void Start()
    {
        texts = new GameObject[transform.childCount];
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i] = transform.GetChild(i).gameObject;
        }
    }
    public void ChangeSelectUI(GameObject getText, bool submitorcancel) //cancel = false,submit = true;
    {
        GameObject nextTexts = null;
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] != getText)
                continue;

            if (submitorcancel)
                nextTexts = texts[i+1];
            else
                nextTexts = texts[i-1];

            break;
        }

        if (nextTexts != null)
            _eventSystem.SetSelectedGameObject(nextTexts);
    }
}

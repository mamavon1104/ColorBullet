using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    bool[] countDownObjectBool = {false,false,false,false };
    [SerializeField] GameObject[] countDownObject;//[3][2][1][Go]�̎l��
    [SerializeField] GameObject finishTimeObject;
    [SerializeField] TextMeshProUGUI gameTimerText;
    [SerializeField] GameObject[] textsObject;
    GameObject[] childcrowns;
    TextMeshProUGUI[] playersScoreText;
    GameObject[,] childsBullet;
    private void Awake()
    {
        childcrowns= new GameObject[GameMaster.setPlayersNumMaster];
        childsBullet = new GameObject[GameMaster.setPlayersNumMaster,GameMaster.setCanShotBulletMaster];
        playersScoreText = new TextMeshProUGUI[GameMaster.setPlayersNumMaster];
        if (GameMaster.setPlayersNumMaster < textsObject.Length)//�܂�S�ȉ��Ȃ�
        {
            for (int i = GameMaster.setPlayersNumMaster ; i <textsObject.Length; i++)
            {
                textsObject[i].SetActive(false);
            }
        }
        for (int i = 0; i < GameMaster.setPlayersNumMaster; i++)
        {
            int bulletNum = 0;
            for(int j = 0;j < textsObject[i].transform.GetChild(0).childCount; j++) 
            {
                if (textsObject[i].transform.GetChild(0).GetChild(j).tag == "BulletUI")
                {
                    childsBullet[i, bulletNum] = textsObject[i].transform.GetChild(0).GetChild(j).gameObject;
                    childsBullet[i, bulletNum].SetActive(true);
                    bulletNum++;
                    if (bulletNum == GameMaster.setCanShotBulletMaster)
                        break;
                }
            }
            childcrowns[i] = FindMyCrown(textsObject[i]);
            playersScoreText[i] = textsObject[i].GetComponent<TextMeshProUGUI>();
            playersScoreText[i].text = 0.ToString();
            WritePlayersScore(i,0);
        }
    }
    GameObject FindMyCrown(GameObject myText)
    {
        var playerObj = myText.transform.GetChild(0);
        GameObject returnCrown = null;
        for (int i = 0; i < playerObj.childCount; i++)
        {
            if(playerObj.GetChild(i).tag == "Crown")
            {
                returnCrown = playerObj.GetChild(i).gameObject;
                break;
            }
        }
        Debug.Log(returnCrown.name);
        return returnCrown;
    }
    public void WritePlayersScore(int playerNum,int nowScore)
    {
        playersScoreText[playerNum].text = nowScore.ToString() + " point";
    }
    public void WriteGameTimer(float getTime)
    {
        gameTimerText.text = "Time : " + getTime.ToString("f2");
    }
    public void WriteCountDown(int setActiveNum)
    {
        countDownObject[setActiveNum].SetActive(!countDownObjectBool[setActiveNum]);
        countDownObjectBool[setActiveNum] = !countDownObjectBool[setActiveNum];
    }
    public IEnumerator WriteFinishText()
    {
        finishTimeObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        GameMaster.SceneManagerMaster.GoGameResult();
    }
    public void SetActiveCrown(bool[] setActiveCrowns)
    {
        for (int i = 0; i < setActiveCrowns.Length; i++)
        {
            childcrowns[i].gameObject.SetActive(setActiveCrowns[i]);
        }
    }

    public void WritePlayerBullet(int playerNum,int BulletNum,bool getbool)
    {
        if(childsBullet[playerNum, BulletNum] != null)
           childsBullet[playerNum, BulletNum].gameObject.SetActive(getbool);
    }
}

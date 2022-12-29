using TMPro;
using UnityEngine;

public class TotalScoreDialog : MonoBehaviour
{
    [Header("Dialog box")] 
    [SerializeField] private TextMeshProUGUI getScoreInfo;
    [SerializeField] private TextMeshProUGUI totalScoreInfo;

    [Header("Game system")] 
    [SerializeField] private GameScore gameScoreCounter;


    public void UpdateGameTotalScore()
    {
        string getScore = gameScoreCounter.GetGameScore().ToString();
        getScoreInfo.text = getScore;
        
        string totalScore = gameScoreCounter.GetTotalScore().ToString();
        totalScoreInfo.text = totalScore;
    }
}

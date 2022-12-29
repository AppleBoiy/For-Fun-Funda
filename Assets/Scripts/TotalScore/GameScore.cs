using TMPro;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    private int _gameScore;

    private int _totalScore;

    public void GetGameScore(int score)
    {
        _totalScore += score;
        _gameScore += score;
    }

    public void NotGetGameScore(int score)
    {
        _totalScore += score;
    }


    public int GetTotalScore()
    {
        Debug.Log(_totalScore);
        return _totalScore;
    }
        
    public int GetGameScore()
    {
        Debug.Log(_gameScore);
        return _gameScore;
    }

    
    
    public void ResetScore()
    {
        _gameScore = 0;
        _totalScore = 0;
    }
}

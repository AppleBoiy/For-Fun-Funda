using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Variables

    private             Question[]          _questions              = null;
    public              Question[]          Questions               { get { return _questions; } }

    [SerializeField]    GameEvents          events                  = null;

    [SerializeField]    Animator            timerAnimtor            = null;
    [SerializeField]    TextMeshProUGUI     timerText               = null;
    [SerializeField]    Color               timerHalfWayOutColor    = Color.yellow;
    [SerializeField]    Color               timerAlmostOutColor     = Color.red;
    private             Color               _timerDefaultColor       = Color.white;

    private             List<AnswerData>    _pickedAnswers           = new List<AnswerData>();
    private readonly List<int>           _finishedQuestions       = new List<int>();
    private             int                 _currentQuestion         = 0;

    private             int                 _timerStateParaHash      = 0;

    private             IEnumerator         _ieWaitTillNextRound    = null;
    private             IEnumerator         _ieStartTimer           = null;

    private             bool                IsFinished
    {
        get
        {
            return (_finishedQuestions.Count >= Questions.Length);
        }
    }

    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called when the object becomes enabled and active
    /// </summary>
    void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }
    /// <summary>
    /// Function that is called when the behaviour becomes disabled
    /// </summary>
    void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    /// <summary>
    /// Function that is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// </summary>
    void Awake()
    {
        events.CurrentFinalScore = 0;
    }
    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        _timerDefaultColor = timerText.color;
        LoadQuestions();

        _timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }

    #endregion

    /// <summary>
    /// Function that is called to update new selected answer.
    /// </summary>
    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (Questions[_currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in _pickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            _pickedAnswers.Clear();
            _pickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = _pickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                _pickedAnswers.Remove(newAnswer);
            }
            else
            {
                _pickedAnswers.Add(newAnswer);
            }
        }
    }

    /// <summary>
    /// Function that is called to clear PickedAnswers list.
    /// </summary>
    public void EraseAnswers()
    {
        _pickedAnswers = new List<AnswerData>();
    }

    /// <summary>
    /// Function that is called to display new question.
    /// </summary>
    void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        } else { Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method."); }

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    /// <summary>
    /// Function that is called to accept picked answers and check/display the result.
    /// </summary>
    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        _finishedQuestions.Add(_currentQuestion);

        UpdateScore((isCorrect) ? Questions[_currentQuestion].AddScore : 0);

        if (IsFinished)
        {
            SetHighscore();
        }

        var type 
            = (IsFinished) 
            ? UIManager.ResolutionScreenType.Finish 
            : (isCorrect) ? UIManager.ResolutionScreenType.Correct 
            : UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen(type, Questions[_currentQuestion].AddScore);
        }

        AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if (type != UIManager.ResolutionScreenType.Finish)
        {
            if (_ieWaitTillNextRound != null)
            {
                StopCoroutine(_ieWaitTillNextRound);
            }
            _ieWaitTillNextRound = WaitTillNextRound();
            StartCoroutine(_ieWaitTillNextRound);
        }
    }

    #region Timer Methods

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                _ieStartTimer = StartTimer();
                StartCoroutine(_ieStartTimer);

                timerAnimtor.SetInteger(_timerStateParaHash, 2);
                break;
            case false:
                if (_ieStartTimer != null)
                {
                    StopCoroutine(_ieStartTimer);
                }

                timerAnimtor.SetInteger(_timerStateParaHash, 1);
                break;
        }
    }
    IEnumerator StartTimer()
    {
        var totalTime = Questions[_currentQuestion].Timer;
        var timeLeft = totalTime;

        timerText.color = _timerDefaultColor;
        while (timeLeft > 0)
        {
            timeLeft--;

            AudioManager.Instance.PlaySound("CountdownSFX");

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }

            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }
    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    #endregion

    /// <summary>
    /// Function that is called to check currently picked answers and return the result.
    /// </summary>
    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Function that is called to compare picked answers with question correct answers.
    /// </summary>
    bool CompareAnswers()
    {
        if (_pickedAnswers.Count > 0)
        {
            List<int> c = Questions[_currentQuestion].GetCorrectAnswers();
            List<int> p = _pickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    /// <summary>
    /// Function that is called to load all questions from the Resource folder.
    /// </summary>
    void LoadQuestions()
    {
        Object[] objs = Resources.LoadAll($"Questions", typeof(Question));
        _questions = new Question[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Question)objs[i];
        }
    }

    /// <summary>
    /// Function that is called restart the game.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Function that is called to quit the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Function that is called to set new highscore if game score is higher.
    /// </summary>
    private void SetHighscore()
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if (highscore < events.CurrentFinalScore)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }
    /// <summary>
    /// Function that is called update the score and update the UI.
    /// </summary>
    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }

    #region Getters

    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        _currentQuestion = randomIndex;

        return Questions[_currentQuestion];
    }
    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (_finishedQuestions.Count < Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Questions.Length);
            } while (_finishedQuestions.Contains(random) || random == _currentQuestion);
        }
        return random;
    }

    #endregion
}
using System.Collections;
using UnityEngine;

namespace DraggableObj
{
    public class Counter : MonoBehaviour
    {
        #region Counter Atrributes
        private int _counter = 0;

        [Header("Count to finish")] 
        [SerializeField] public int totalCount;

        [Header("Show when finish")] 
        [SerializeField] private GameObject showWhenFinish;
        
        [Header("Deactivate when finish")]
        [SerializeField] private bool deactivateWhenCount;
        [SerializeField] private GameObject deactivateObject;

        [Header("Answer status box")] 
        [SerializeField] private bool showAnswerStatus = true;
        [SerializeField] private GameObject correctFrame;
        [SerializeField] private GameObject wrongFrame;

        [Header("Scene score")] 
        [SerializeField] private int totalSceneScore;
        private int _sceneScore = 0;
        
        #endregion
        
        public void WrongAnswer()
        {
            if (showAnswerStatus)
            {
                StartCoroutine(ShowStatus(false));
            }
        }

        public void CorrectAnswer()
        {
            _counter++;
            if (showAnswerStatus)
            {
                StartCoroutine(ShowStatus(true));    
            }
            if (_counter >= totalCount)
            {
                if (deactivateWhenCount)
                    deactivateObject.SetActive(false);
                
                showWhenFinish.SetActive(true);
            }
            Debug.Log(_counter);
        }

        public void UpdateSceneScore()
        {
            _sceneScore++;
            Debug.Log(_sceneScore);
            Debug.Log(totalSceneScore);
            if (_sceneScore >= totalSceneScore)
            {
                if (deactivateWhenCount)
                    deactivateObject.SetActive(false);
                
                showWhenFinish.SetActive(true);
            }
        }
        private IEnumerator ShowStatus(bool status)
        {
            if (status)
            {
                correctFrame.SetActive(true);
                yield return new WaitForSeconds(1);
                correctFrame.SetActive(false);
            }
            else
            {
                wrongFrame.SetActive(true);
                yield return new WaitForSeconds(1);
                wrongFrame.SetActive(false);
            }
        }
    }
}
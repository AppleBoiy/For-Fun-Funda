using System.Collections;
using UnityEngine;

namespace DraggableObj
{
    public class Counter : MonoBehaviour
    {
        private int _counter;

        [Header("Count to finish")] 
        [SerializeField] private int totalCount;

        [Header("Show when finish")] 
        [SerializeField] private GameObject showWhenFinish;

        [Header("Answer status box")] 
        [SerializeField] private bool showAnswerStatus = true;
        [SerializeField] private GameObject correctFrame;
        [SerializeField] private GameObject wrongFrame;

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
                showWhenFinish.SetActive(true);
            }
            Debug.Log(_counter);
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
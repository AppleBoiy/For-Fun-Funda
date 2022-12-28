using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
   
   [Header("Score Collect item scene")]
   [SerializeField] private int collectCount;
   private int _totalCount;
   private int _totalScore;
   
   [Header("Counter scene")]
   [SerializeField] public GameObject nextGameDialogBox;

   [Header("Object to show")]
   [SerializeField] private GameObject wrongItemGet;
   [SerializeField] private GameObject correctItemGet;
   [SerializeField] private GameObject showObj1;
   [SerializeField] private GameObject showObj2;
   
   private WaitForSeconds _waitForSeconds;

   public void OnDrop(PointerEventData eventData)
   {
      Debug.Log("OnDrop");
      if (eventData.pointerDrag != null)
      {
         
         //ID 0 = Show more object ondrop
         //ID 1 = Correct
         //ID 2 = Collect Item
         //ID ELSE = False
         
         
         //Show more object ondrop
         if (eventData.pointerDrag.GetComponent<DragAndDrop>().id == 0)
         {
            Vector2 outSidePosition = new Vector2(10000, 10000);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
            _totalCount++;
            if (_totalCount >= collectCount)
            {
               showObj1.SetActive(true);
               showObj2.SetActive(true);
            }

         }
         
         //Drop to collect item
         else if (eventData.pointerDrag.GetComponent<DragAndDrop>().id == 1)
         {
            Debug.Log("Correct!!");
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
         }
         
         //Drop in score count
         else if (eventData.pointerDrag.GetComponent<DragAndDrop>().id == 2)
         {
            Vector2 outSidePosition = new Vector2(10000, 10000);
            Debug.Log("Collect!!");
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
            StartCoroutine(Wait(2));
            _totalCount++;
            if (_totalCount >= collectCount)
            {
               nextGameDialogBox.SetActive(true);
               correctItemGet.SetActive(false);
            }
            
         }
         else if (eventData.pointerDrag.GetComponent<DragAndDrop>().id == 3)
         {
            eventData.pointerDrag.GetComponent<DragAndDrop>().ResetPosition();
            StartCoroutine(Wait(3));
         }
         
         //Drop wrong position
         else
         {
            Debug.Log("False!!");
            eventData.pointerDrag.GetComponent<DragAndDrop>().ResetPosition();
         }
      }
   }
   
   // ReSharper disable Unity.PerformanceAnalysis
   private IEnumerator Wait(int id)
   {
      Debug.Log("Wait");
      switch (id)
      {
         case 3:
            wrongItemGet.SetActive(true);
            break;
         case 2:
            correctItemGet.SetActive(true);
            break;
      }
      
      yield return new WaitForSeconds(1);
      
      switch (id)
      {
         case 3:
            wrongItemGet.SetActive(false);
            break;
         case 2:
            correctItemGet.SetActive(false);
            break;
      }
   }

}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DragableObj
{
   public class SlotScript : MonoBehaviour, IDropHandler
   {
   
      [Header("Score Collect item scene")]
      [SerializeField] private int collectCount;
      private int _totalCount;
      private int _totalScore;
   
      [Header("Counter scene")]
      [SerializeField] public GameObject nextGameDialogBox;
      [SerializeField] public Counter gameCounter;
      
      [Header("Status")]
      [SerializeField] private GameObject wrongItemGet;
      [SerializeField] private GameObject correctItemGet;
      
      [Header("Object to show and hide")]
      [SerializeField] private GameObject showObj1;
      [SerializeField] private GameObject showObj2;
      [SerializeField] private GameObject objectToShow;
      [SerializeField] private GameObject objectToHide;
      [SerializeField] private GameObject hideObj1;
      [SerializeField] private GameObject hideObj2;

      [FormerlySerializedAs("slotID")]
      [Header("Slot attribute")] 
      [SerializeField] private string slotType;
      
      private WaitForSeconds _waitForSeconds;

      public void OnDrop(PointerEventData eventData)
      {
         Debug.Log("OnDrop");
         if (eventData.pointerDrag != null)
         {
            int id = eventData.pointerDrag.GetComponent<DragAndDrop>().id;
            string type = eventData.pointerDrag.GetComponent<DragAndDrop>().type;
            Vector2 outSidePosition = new Vector2(10000, 10000);

            if (type == "S" || type == "T")
            {
               if (type == slotType)
               {
                  //Send to Hide and Show status 
                  //When True
                  OnDropAction(6, outSidePosition, eventData);
                  gameCounter.CorrectAnswer();
               }
               else
               {
                  //When False
                  //Send to Hide and Show status
                  OnDropAction(999, outSidePosition, eventData);
                  gameCounter.WrongAnswer();
               }
               return;
            }
            
            
            if (slotType != "except")
            {
               if (slotType == "false")
               {
                  StartCoroutine(Wait(3));
               }
               else
               {
                  StartCoroutine(Wait(2));
               }
            }
            
            
            //OnDrop to skip scene
            if (id < 0){
               OndropSkipScene(id);
            }
            //OnDrop action case
            else
            {  
               OnDropAction(id, outSidePosition, eventData);
            }
         }
      }

      private void OndropSkipScene(int id)
      {
         switch (id)
         {
            //Swap from scene 4 part 2 to 3
            case -1:
               SceneManager.LoadScene("R4 part 3");
               break;
                  
            //Swap from scene 4 part 3 to 4
            case -2:
               SceneManager.LoadScene("R4 part 4");
               break;
                  
            //Swap from scene 4 part 4 to 5
            case -3:
               SceneManager.LoadScene("R4 part 5");
               break;
         }
      }
      
      private void OnDropAction(int id, Vector2 outSidePosition, PointerEventData eventData)
      {
         switch (id) {
                     //Show more object ondrop
                  case 0:
                     eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
                     _totalCount++;
                     if (_totalCount >= collectCount)
                     {
                        showObj1.SetActive(true);
                        showObj2.SetActive(true);
                     }
                     break;
                  
                  //Drop to Hide and show object
                  case 1:
                     eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
                     objectToHide.SetActive(false);
                     objectToShow.SetActive(true);
                     break;
                  
                  //Drop in score count
                  case 2:
                     Debug.Log("Collect!!");
                     eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
                     StartCoroutine(Wait(2));
                     _totalCount++;
                     if (_totalCount >= collectCount)
                     {
                        nextGameDialogBox.SetActive(true);
                        correctItemGet.SetActive(false);
                     }
                     break;
                  
                  //Reset and check type
                  case 3:
                     eventData.pointerDrag.GetComponent<DragAndDrop>().ResetPosition();
                     StartCoroutine(Wait(3));
                     break;
                  
                  //Collect object
                  case 4:
                     eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =  GetComponent<RectTransform>().anchoredPosition;
                     objectToHide.SetActive(false);
                     objectToShow.SetActive(true);
                     break;
                  
                  //Hide multiObject
                  case 5:
                     eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
                     hideObj1.SetActive(false);
                     hideObj2.SetActive(false);
                     objectToShow.SetActive(true);
                     break;
                  
                  //Get out from there
                  case 6:
                     eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
                     break;

                     //Default return to main
                  default:
                     Debug.Log("False!!");
                     eventData.pointerDrag.GetComponent<DragAndDrop>().ResetPosition();
                     break;
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
               yield return new WaitForSeconds(1);
               wrongItemGet.SetActive(false);
               break;
            case 2:
               correctItemGet.SetActive(true);
               yield return new WaitForSeconds(1);
               correctItemGet.SetActive(false);
               break;
         }
      }

      public void OnDrop(PointerEventData eventData, out Vector2 anchoredPos)
      {
         throw new System.NotImplementedException();
      }
   }
}

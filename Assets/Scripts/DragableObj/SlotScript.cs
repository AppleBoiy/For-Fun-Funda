using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

      [Header("Slot attribute")] 
      [SerializeField] private string slotID;
      
      private WaitForSeconds _waitForSeconds;

      public void OnDrop(PointerEventData eventData)
      {
         Debug.Log("OnDrop");
         if (eventData.pointerDrag != null)
         {
            int id = eventData.pointerDrag.GetComponent<DragAndDrop>().id;
            Vector2 outSidePosition = new Vector2(10000, 10000);

            if (slotID != null)
            {
               if (slotID == "false")
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
            
            //OnDrop action case
            else
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
                  
                  //Default return to main
                  default:
                     Debug.Log("False!!");
                     eventData.pointerDrag.GetComponent<DragAndDrop>().ResetPosition();
                     break;
               }
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

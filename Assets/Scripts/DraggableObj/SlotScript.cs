using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DraggableObj
{
   public class SlotScript : MonoBehaviour, IDropHandler
   {

      #region Get Attribute Field 

      //Collect item
      [Header("Score Collect item scene")]
      [SerializeField] private int collectCount;
      private int _totalCount;
      private int _totalScore;
   
      //Scene when finish
      [Header("Counter scene")]
      [SerializeField] public GameObject nextGameDialogBox;
      [SerializeField] public Counter gameCounter;
      [SerializeField] public bool addScoreCounter;
      
      //Collect status
      [Header("Status")]
      [SerializeField] private GameObject wrongItemGet;
      [SerializeField] private GameObject correctItemGet;
      
      //Hide and show object
      [Header("Object to show and hide")]
      [SerializeField] private GameObject objectToShow;
      [SerializeField] private GameObject objectToHide;
      [SerializeField] private GameObject showObj1;
      [SerializeField] private GameObject showObj2;
      [SerializeField] private GameObject hideObj1;
      [SerializeField] private GameObject hideObj2;
      
      //Get item type
      [FormerlySerializedAs("slotID")]
      [Header("Slot attribute")] 
      [SerializeField] private string slotType;
      [SerializeField] private bool trashWithAttribute;

      //Game score
      [Header("Game score")] 
      [SerializeField] private Counter sceneCounter;
      
      #endregion
      
      private WaitForSeconds _waitForSeconds;

      
      public void OnDrop(PointerEventData eventData)
      {
         Debug.Log("OnDrop");
         if (eventData.pointerDrag == null) return;

         #region dragged object info
            
         DragAndDrop dragObject = eventData.pointerDrag.GetComponent<DragAndDrop>();
         int dragObjectID = dragObject.id;
         string dragObjectType = dragObject.type;
         string dragObjectRoom = dragObject.room;
         Vector2 outSidePosition = new Vector2(10000, 10000);
            
         #endregion

         #region Trash slot
         if (dragObjectType == "S" || dragObjectType == "T")
         {
            
            Debug.Log("Trash can");
            if (dragObjectType == slotType)
            {
               //Send to Hide and Show status 
               //When True
               if (trashWithAttribute)
               {
                  Debug.Log("Trash with attribute");
                  OndropMoreAction(0, outSidePosition, eventData);
               }
               else
               {
                  Debug.Log($"Trash without attribute");
                  OnDropAction(1, outSidePosition, eventData);
               }
                  
               
               gameCounter.CorrectAnswer();
               Debug.Log("Correct!");
            }
            else
            {
               //When False
               //Send to Hide and Show status
               OnDropAction(3, outSidePosition, eventData);
               gameCounter.WrongAnswer();
            }
            return;
         }
         #endregion

         #region Catheter room

         if (dragObjectRoom == "CatheterScene")
         {
               
            //Catheter
            switch (dragObjectType)
            {
               case "catheter":
                  OnDropAction(slotType == $"trueVein" ? 1 : 3, outSidePosition, eventData);
                  break;
               
               case "finger":
                  Debug.Log("Get finger");
                  sceneCounter.UpdateSceneScore();
                  OnDropAction(8, outSidePosition, eventData);
                  break;
               
               case "set":
                  Debug.Log("Get set");
                  OnDropAction(slotType == $"set" ? 8 : 3, outSidePosition, eventData);
                  if (slotType == "set")
                     OnDropAction(9, outSidePosition, eventData);
                  break;
               
               case "Transpore":
                  Debug.Log("Get Transpore");
                  OnDropAction(slotType == $"Transpore" ? 4 : 3, outSidePosition, eventData);
                  if (slotType == "Transpore")
                     OnDropAction(9, outSidePosition, eventData);
                  break;
               
               case "Tegadrem":
                  Debug.Log("Get Tegadrem");
                  OnDropAction(slotType == $"Tegadrem" ? 8 : 3, outSidePosition, eventData);
                  if (slotType == "Tegadrem")
                     OnDropAction(9, outSidePosition, eventData);
                  break;
            }
            return;
         }

         #endregion

         #region Check only true false
         if (slotType == "except")
         {
            if (slotType == "false")
            {
               StartCoroutine(Wait(3));
            }
            else
            {
               StartCoroutine(Wait(2));
            }
            return;
         }
         #endregion
            
         //Any-else slot
         OndropMoreAction(dragObjectID, outSidePosition, eventData);
      }

      #region Ondrop action by item id
      private void OndropMoreAction(int id, Vector2 outSidePosition,PointerEventData eventData)
      {
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
      #endregion
      
      #region Ondrop and then skip to next scene
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
      #endregion

      #region Ondrop and then do something function
      private void OnDropAction(int id, Vector2 outSidePosition, PointerEventData eventData)
      {
         switch (id)
         {
            //Show more object ondrop
            case 0:
            {
               eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
               _totalCount++;
               if (_totalCount >= collectCount)
               {
                  showObj1.SetActive(true);
                  showObj2.SetActive(true);
               }

               break;
            }
            //Drop to Hide and show object
            case 1:
               eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = outSidePosition;
               objectToHide.SetActive(false);
               objectToShow.SetActive(true);
               break;
            
            //Drop in score count
            case 2:
            {
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
            }
            
            //Reset and check type
            case 3:
               eventData.pointerDrag.GetComponent<DragAndDrop>().ResetPosition();
               StartCoroutine(Wait(3));
               break;
            
            //Collect object
            case 4:
               eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                  GetComponent<RectTransform>().anchoredPosition;
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
            
            //Set prepare room
            case 7:
               eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                  GetComponent<RectTransform>().anchoredPosition;
               showObj1.SetActive(true);
               showObj2.SetActive(true);
               break;
            
            //Just get it
            case 8:
               Debug.Log("case 8");
               eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                  GetComponent<RectTransform>().anchoredPosition;
               break;
            
            //Get only score
            case 9:
               _totalCount++;
               if (_totalCount >= collectCount)
               {
                  gameCounter.CorrectAnswer();
               }
               break;
            
            //Default reset position
            default:
               Debug.Log("False!!");
               eventData.pointerDrag.GetComponent<DragAndDrop>().ResetPosition();
               break;
         }
      }
      #endregion
      
      #region Wait for a sec 
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
      #endregion
   }
}

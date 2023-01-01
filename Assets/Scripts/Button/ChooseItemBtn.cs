using System.Collections;
using UnityEngine;

public class ChooseItemBtn : MonoBehaviour
{
    #region get object

    [Header("Choose item to use")]
    [SerializeField] private GameObject wrongStatusObj;
    [SerializeField] private GameObject itemDetail;
    [SerializeField] private bool getCorrectItem;

    [Header("Slot Btn")]
    [SerializeField] private GameObject trashBtn;
    [SerializeField] private GameObject kitsBtn;

    [Header("Hide when get correct item")] 
    [SerializeField] private GameObject itemSlot;
    
    #endregion
    
    public void GetItem()
    {
        
        //If get incorrect item
        if (!getCorrectItem)
        {
            itemDetail.SetActive(false);
            StartCoroutine(GetWrongItemShow());
            return;
        }

        #region if get correct item

        itemSlot.SetActive(false);
        itemDetail.SetActive(false);
        trashBtn.SetActive(true);
        kitsBtn.SetActive(true);
        
        #endregion
       
            
    }
    
    private IEnumerator GetWrongItemShow()
    {
        wrongStatusObj.SetActive(true);
        yield return new WaitForSeconds(1);
        wrongStatusObj.SetActive(false);
    }

}

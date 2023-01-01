using System.Collections;
using UnityEngine;

public class ChooseItemBtn : MonoBehaviour
{
    #region get object

    [Header("Choose item to use")]
    [SerializeField] private GameObject itemDetail;
    [SerializeField] private bool getCorrectItem;

    [Header("Select item controller")] [SerializeField]
    private ItemSelectController itemSelectController;
    
    #endregion
    
    public void GetItem()
    {
        
        //If get incorrect item
        if (!getCorrectItem)
        {
            StartCoroutine(itemSelectController.GetWrongItem());
            return;
        }

        #region if get correct item
        
        itemDetail.SetActive(false);
        itemSelectController.ShowContainerBtn();

        #endregion
        
    }

}

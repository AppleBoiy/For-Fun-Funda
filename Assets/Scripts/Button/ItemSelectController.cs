using System.Collections;
using UnityEngine;

public class ItemSelectController : MonoBehaviour
{
    #region get object

    [Header("Incorrect status")]
    [SerializeField] private GameObject wrongStatusObj;

    [Header("Slot Btn")]
    [SerializeField] private GameObject trashBtn;
    [SerializeField] private GameObject kitsBtn;
    [SerializeField] private GameObject itemSlot;
    #endregion
    
    
    public IEnumerator GetWrongItem()
    {
        wrongStatusObj.SetActive(true);
        yield return new WaitForSeconds(1);
        wrongStatusObj.SetActive(false);
    }

    public void ShowContainerBtn()
    {
        trashBtn.SetActive(true);
        kitsBtn.SetActive(true);
        itemSlot.SetActive(false);
    }

}

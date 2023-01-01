using System.Collections;
using UnityEngine;

public class ChooseItemBtn : MonoBehaviour
{
    [SerializeField] private GameObject wrongItemGet;
    [SerializeField] private GameObject itemIncorrect;

    public void GetWrongItem()
    {
        StartCoroutine(GetWrongItemShow());
    }
    
    private IEnumerator GetWrongItemShow()
    {
        wrongItemGet.SetActive(true);
        yield return new WaitForSeconds(1);
        itemIncorrect.SetActive(false);
        wrongItemGet.SetActive(false);
    }

}

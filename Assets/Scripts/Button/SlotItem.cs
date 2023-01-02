using UnityEngine;

public class SlotItem : MonoBehaviour
{
    #region slot item
    
    private int _index = 1;
    private const int SlotSize = 3;
    [Header("Slot item")]
    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;
    [SerializeField] private GameObject slot3;

    private void ShowSlot(int index)
    {
        switch (index)
        {
            case 1:
                slot1.SetActive(true);
                slot2.SetActive(false);
                slot3.SetActive(false);
                break;
            
            case 2:
                slot2.SetActive(true);
                slot1.SetActive(false);
                slot3.SetActive(false);
                break;
            
            case 3:
                slot3.SetActive(true);
                slot2.SetActive(false);
                slot1.SetActive(false);
                break;

        }
    }

    public void Previous()
    {
        if (_index == 1)
        {
            ShowSlot(_index);
            return;
        }

        _index--;
        ShowSlot(_index);
    }

    public void Next()
    {
        if (_index == SlotSize)
        {
            ShowSlot(3);
            return;
        }

        _index++;
        ShowSlot(_index);

    }
    
    #endregion
}

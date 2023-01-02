using UnityEngine;

public class BtnSlotController : MonoBehaviour
{
    [SerializeField] private SlotItem slotItem;
    
    public void PreviosSlot()
    {
        slotItem.Previous();
    }

    public void NextSlot()
    {
        slotItem.Next();
    }
    
    
}


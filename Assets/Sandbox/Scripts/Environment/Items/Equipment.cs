using System;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField]
    private Item slot;

    public bool IsEquipped => slot != null;

    /*
    public void Equip(EquipmentItem item)
    {
        var slot = equipmentSlots.FirstOrDefault(s => s.type == item.type);
        if (slot == null)
        {
            Debug.LogError($"No slot for {item.type} found.", this);
            return;
        }

        slot.Equip(item);
    }*/
    public void Unequip()
    {
        throw new NotImplementedException();
    }

    public void Equip(Item item)
    {
        throw new NotImplementedException();
    }
}
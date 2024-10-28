using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Base Item")]
public class BaseItem : ScriptableObject, IUsable
{
    public string itemName;
    public Sprite itemIcon;
    public int maxStackSize = 60;

    public virtual void Use(bool isHold)
    {
        Debug.Log($"Picked up {itemName}.");
    }
}
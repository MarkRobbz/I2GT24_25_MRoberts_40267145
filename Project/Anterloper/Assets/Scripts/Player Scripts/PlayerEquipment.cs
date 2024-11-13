using System;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public BaseItem equippedItem;
    public GameObject equippedItemModel;
    public Transform itemHolder;

    private int _equippedItemLayer;
    private int _pickupsLayer;
    

    private void Start()
    {
        _equippedItemLayer = LayerMask.NameToLayer("EquippedItem");
        _pickupsLayer = LayerMask.NameToLayer("Pickups");
        
    }

    public void EquipItem(BaseItem item)
    {
        equippedItem = item;

        if (equippedItemModel != null)
        {
            Destroy(equippedItemModel); // Destroy current equipped item model
        }

        if (item.itemPrefab != null)
        {
            equippedItemModel = Instantiate(item.itemPrefab, itemHolder);

            equippedItemModel.transform.localPosition = Vector3.zero;
            equippedItemModel.transform.localRotation = Quaternion.identity;
            equippedItemModel.transform.localScale = Vector3.one;

            // Set layer to equipped item layer
            SetLayerRecursively(equippedItemModel, _equippedItemLayer);

            // Disable Rb physics on equipped item
            Rigidbody rb = equippedItemModel.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            // Disable colliders on the equipped item
            Collider[] colliders = equippedItemModel.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning($"{item.itemName} has no itemPrefab assigned.");
        }
    }





    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    
    public void UnequipItem()
    {
        if (equippedItemModel != null)
        {
            Destroy(equippedItemModel);
            equippedItemModel = null;
        }

        equippedItem = null;
    }


    private void DropEquippedItem()
    {
        SetLayerRecursively(equippedItemModel, _pickupsLayer);
        
        equippedItemModel.transform.parent = null;  // Detach from player
        Rigidbody rb = equippedItemModel.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        equippedItemModel = null;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    None,
    Axe,
    Pickaxe,
}

[CreateAssetMenu(fileName = "New Tool Item", menuName = "Items/Tool Item")]
public class ToolItem : CraftableItem
{
    public ToolType toolType;
    public float toolDamage = 25f; 

   

    public void UseTool()
    {
        Debug.Log($"Using {itemName} as a {toolType}");
    
        Interaction interaction = FindObjectOfType<Interaction>();

        if (interaction != null)
        {
            GameObject targetObject = interaction.GetCurrentInteractableObject();

            if (targetObject != null)
            {
                if (toolType == ToolType.Axe)
                {
                    Tree tree = targetObject.GetComponent<Tree>();
                    if (tree != null)
                    {
                        
                        Vector3 playerPosition = interaction.transform.position; //so tree falls away from player
                        
                        tree.ApplyDamage(toolDamage, playerPosition);
                        Debug.Log($"Applied {toolDamage} damage to the tree. Tree health is now {tree.health}");
                    }
                    else
                    {
                        Debug.Log("Target is not a tree.");
                    }
                }
                else if (toolType == ToolType.Pickaxe)
                {
                    // Handle mining actions here
                }
            }
            else
            {
                Debug.Log("No interactable object in range.");
            }
        }
        else
        {
            Debug.LogError("Interaction component not found on the player.");
        }
    }


}

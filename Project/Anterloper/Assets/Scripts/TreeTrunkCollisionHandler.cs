using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTrunkCollisionHandler : MonoBehaviour
{
    private Tree treeParent;

    private void Start()
    {
        treeParent = GetComponentInParent<Tree>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (treeParent != null)
        {
            treeParent.OnTrunkCollisionEnter(collision);
        }
    }
}

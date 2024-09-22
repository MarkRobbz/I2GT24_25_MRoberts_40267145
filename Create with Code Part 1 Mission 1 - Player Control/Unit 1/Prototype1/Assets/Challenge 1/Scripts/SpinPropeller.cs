using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPropeller : MonoBehaviour
{
    [SerializeField] private int _propellerSpeed = 10;

    

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0,0,_propellerSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

public class SetLockableItemParent : MonoBehaviour
{
    private ItemLockingToPoint itemLockingToPointScript;
    public GameObject lockableItem;
    public Transform parent;
    private void Awake()
    {
        itemLockingToPointScript = lockableItem.GetComponent<ItemLockingToPoint>();
    }
    private void Update()
    {
        if(itemLockingToPointScript.isItemSnapped == true)
        {
            transform.SetParent(parent);
        }
        if (itemLockingToPointScript.isItemSnapped == false)
        {
            transform.parent = null;
        }
    }
}

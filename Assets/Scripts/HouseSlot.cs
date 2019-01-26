using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HouseSlot : MonoBehaviour
{
    public House house;
    public List<Item> items;
    public PlayerType type;

    private Collider[] checkedColliders;
    private new BoxCollider collider;
    private Item tempItem;

    private void Start()
    {
        items = new List<Item>();
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void Update()
    {
        
    }

    public void CheckSlotCargo()
    {
        checkedColliders = Physics.OverlapBox(transform.position, collider.bounds.extents, transform.rotation, 1 << 9);
        items = new List<Item>();
        foreach(Collider c in checkedColliders)
        {
            tempItem = c.GetComponent<Item>();
            if (!tempItem) continue;
            else
            {
                if (items.Contains(tempItem))
                {
                    continue;
                }
                else
                {
                    if(tempItem.type == type)
                    {
                        items.Add(tempItem);
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class House : MonoBehaviour
{
    public List<HouseSlot> slots;
    public float useFuelDelay;
    public float health;
    public float hungerDamage;

    private float time;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= useFuelDelay)
        {
            time = 0;
            UseFuel();
        }
    }

    private void UseFuel()
    {
        bool enoughFuel = true;
        int takeDamage = 0;
        foreach (HouseSlot slot in slots)
        {
            slot.CheckSlotCargo();
            if(slot.items.Count < 1)
            {
                enoughFuel = false;
                takeDamage++;
            }
            else
            {
                Item itemToConsume = slot.items.FindLast(i => i.type == slot.type);
                slot.items.Remove(itemToConsume);
                Destroy(itemToConsume.gameObject);
            }
        }
        if (!enoughFuel)
        {
            health -= hungerDamage*takeDamage;
        }
    }
}

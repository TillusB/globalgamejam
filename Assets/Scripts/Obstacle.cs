using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ResourceConsumer
{
    public float damageValue;
    public override bool Consume(Item item)
    {
        if(item.type == type)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }



    private void OnTriggerEnter(Collider other)
    {
        House house = other.gameObject.GetComponent<House>();
        if (house)
        {
            house.TakeDamage(damageValue);
            Destroy(gameObject);
        }
    }
}

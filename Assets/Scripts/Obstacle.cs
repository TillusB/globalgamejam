using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ResourceConsumer
{
    public override bool Consume(Item item)
    {
        if(item.type == type)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}

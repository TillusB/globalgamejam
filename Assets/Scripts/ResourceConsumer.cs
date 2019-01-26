using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceConsumer : MonoBehaviour
{
    public PlayerType type;
    public abstract bool Consume(Item item);
}   
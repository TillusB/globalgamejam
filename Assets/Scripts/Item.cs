using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    private Rigidbody rb;
    public PlayerType type;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 9;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<ResourceConsumer>())
        {
            CheckType(collision.gameObject.GetComponent<ResourceConsumer>());
        }
    }

    public void CheckType(ResourceConsumer toCheck)
    {
        if(toCheck.type == type)
        {
            if (toCheck.Consume(this))
            {
                Destroy(gameObject);
            }
        }
    }
}

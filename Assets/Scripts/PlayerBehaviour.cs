using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Default,
    Carry,
    Carried,
    Dead
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerBehaviour : MonoBehaviour
{
    private PlayerState state;
    public PlayerState State { get => state; set => state = value; }
    public int index;
    public float maxVelocity;
    public float jumpForce;
    public float throwForce;
    public float accelleration;
    public float moveSpeed;
    public float groundDistance;
    public float fallMultiplier = 2.5f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        State = PlayerState.Default;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Action" + index))
        {
            switch (state)
            {
                case PlayerState.Default:
                    if (CanPickUp())
                    {
                        Pickup();
                    }
                    else
                    {
                        Jump();
                    }
                    break;
                case PlayerState.Carry:
                    Throw();
                    break;
                case PlayerState.Carried:
                    break;
                case PlayerState.Dead:
                    break;
            }
        }

        HorizontalMovement();

        if (rb.velocity.y < 1)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool CanPickUp()
    {
        return false;
    }

    public void Init()
    {
        state = PlayerState.Default;
        //put player in default pos etc
    }

    public void HorizontalMovement()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal" + index) * moveSpeed * Time.deltaTime * 100, rb.velocity.y);
    }

    public void Jump()
    {
        if (Grounded())
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
    public void Throw()
    {

    }
    public void Pickup()
    {
        State = PlayerState.Carry;
    }
    public void Die()
    {
        State = PlayerState.Dead;
    }

    private bool Grounded()
    {
        
        if(Physics2D.Raycast(rb.position, Vector2.down, groundDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
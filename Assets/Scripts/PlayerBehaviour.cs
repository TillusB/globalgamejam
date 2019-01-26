using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PlayerState
{
    Default,
    Carry,
    Carried,
    Climbing,
    Dead
}

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
    public float pickUpRadius = 1.5f;
    public float fallMultiplier = 2.5f;

    public Vector3 carryPos;
    private GameObject currentCargo;
    private PlayerState otherPlayerCargoState;

    private int layerMask = 1 << 9;

    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        State = PlayerState.Default;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PerformAction();
        HorizontalMovement();
        VerticalMovement();
    }

    public void PerformAction()
    {
        if (Input.GetButtonDown("Action" + index))
        {
            //Debug.Log("called action by" + gameObject.name);
            switch (state)
            {
                case PlayerState.Default:
                    if (!CanPickUp())
                    {
                        Jump();
                    }
                    break;
                case PlayerState.Carry:
                    Throw();
                    break;
                case PlayerState.Carried:
                    break;
                case PlayerState.Climbing:
                    StopClimbing(true);
                    State = default;
                    break;
                case PlayerState.Dead:
                    break;
            }
        }
    }

    public void Init()
    {
        state = PlayerState.Default;
        //put player in default pos etc
    }

    /// <summary> ////////////////////////
    /// How Movement is calculated + Input
    /// </summary>
    public void HorizontalMovement()
    {
        if (state == PlayerState.Default || state == PlayerState.Carry || state == PlayerState.Climbing)
        {
            if (rb.velocity.x < maxVelocity && rb.velocity.x > -maxVelocity)
            {   
                rb.velocity = new Vector3(Input.GetAxis("Horizontal" + index) * moveSpeed * Time.deltaTime * 100, rb.velocity.y, 0);

                // Current Animation Integration -- 
                if(rb.velocity.x != 0) {
                    anim.SetBool("isMoving", true);
                    if(rb.velocity.x < 0)
                    {
                        sr.flipX = true;
                    }
                    if(rb.velocity.x > 0)
                    {
                        sr.flipX = false;
                    }
                } else {
                    anim.SetBool("isMoving", false);
                }
            }
        }
    }
    public void VerticalMovement()
    {
        if (state == PlayerState.Default || state == PlayerState.Carry)
        {
            if (rb.velocity.y < 1)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
        }

        if (state == PlayerState.Climbing)
        {
            transform.Translate(0, Input.GetAxis("Vertical" + index) * moveSpeed * -0.025f, 0);
        }
    }
    /////////////////////////////////////
    public void Jump()
    {
        if (Grounded())
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
        }
    }

    public void StopClimbing(bool applyJump)
    {
        rb.isKinematic = false;
        //rb.velocity = Vector3.zero;
        //rb.AddForce(Input.GetAxis("Horizontal" + index), Input.GetAxis("Vertical" + index) * -jumpForce * 100, 0);
        state = PlayerState.Default;
        if (Input.GetAxis("Vertical" + index) < -0.5f && applyJump)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
        }
    }

    public void Throw()
    {
        currentCargo.transform.SetParent(null, true);
        currentCargo.transform.position = transform.position + carryPos;
        currentCargo.GetComponent<Rigidbody>().isKinematic = false;
        currentCargo.GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxis("Horizontal" + index), Input.GetAxis("Vertical" + index) * -1, 0) * throwForce, ForceMode.Impulse);
        Debug.Log(Input.GetAxis("Horizontal" + index));
        state = PlayerState.Default;
        if (currentCargo.GetComponent<PlayerBehaviour>())
        {
            currentCargo.GetComponent<PlayerBehaviour>().state = otherPlayerCargoState;
        }
    }

    public void PickUp(GameObject cargo)
    {
        cargo.GetComponent<Rigidbody>().isKinematic = true;
        if (cargo.GetComponent<PlayerBehaviour>())
        {
            otherPlayerCargoState = cargo.GetComponent<PlayerBehaviour>().state;
            cargo.GetComponent<PlayerBehaviour>().State = PlayerState.Carried;
        }
        cargo.transform.parent = gameObject.transform;
        cargo.transform.localPosition = carryPos;
        currentCargo = cargo;
        State = PlayerState.Carry;
    }

    public void Die()
    {
        State = PlayerState.Dead;
    }


    /// <summary>
    /// bools
    /// </summary>
    /// 
    private bool CanPickUp()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickUpRadius, layerMask);

        hitColliders = hitColliders.Where(hit => hit != gameObject.GetComponent<Collider>())
            .OrderBy(h => Vector2.Distance(transform.position, h.transform.position)).ToArray();

        if (hitColliders.Length > 0){
            PickUp(hitColliders[0].gameObject);
            return true;
        }    
        return false;
    }

    private bool Grounded()
    {
        
        if(Physics.Raycast(rb.position, Vector2.down, groundDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
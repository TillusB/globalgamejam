using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum PlayerState
{
    Default,
    Carry,
    Carried,
    Climbing,
    Stunned,
    Dead
}

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerState state;
    public PlayerState State { get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }
    public int index;
    public float maxVelocity;
    public float jumpForce;
    public float throwForce;
    public float accelleration;
    public float moveSpeed;
    public float groundDistance;
    public float pickUpRadius = 1.5f;
    public float fallMultiplier = 2.5f;
    public float respawnTimer = 10f;

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
                case PlayerState.Stunned:
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
            if (otherPlayerCargoState == PlayerState.Stunned)
            {
                currentCargo.GetComponent<PlayerBehaviour>().state = PlayerState.Default;
            }
            else
            {
                currentCargo.GetComponent<PlayerBehaviour>().state = otherPlayerCargoState;
            }
        }
        currentCargo.layer = 9;
    }

    public void PickUp(GameObject cargo)
    {
        cargo.GetComponent<Rigidbody>().isKinematic = true;
        cargo.layer = 0;
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

        if (hitColliders.Length > 0)
        {
            if (!hitColliders[0].gameObject.GetComponent<Orb>())
            {
                PickUp(hitColliders[0].gameObject);
            }
            else if (!hitColliders[0].gameObject.GetComponent<Orb>().locked || (hitColliders[0].gameObject.GetComponent<Orb>().locked && hitColliders[0].gameObject.GetComponent<Orb>().affiliation == index))
            {
                hitColliders[0].gameObject.GetComponent<Orb>().Tint(index);
                PickUp(hitColliders[0].gameObject);
            }
                
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

    public void SetPlayerState (PlayerState desiredState)
    {
        if (desiredState == state)
        {
            Debug.Log(gameObject.name + " is already in state : " + desiredState.ToString());
            return;
        }
        if (desiredState == PlayerState.Carried)
        {
            Debug.Log(gameObject.name + " is now in state : " + desiredState.ToString());
            state = desiredState;
            return;
        }
        if (desiredState == PlayerState.Carry)
        {
            Debug.Log(gameObject.name + " is now in state : " + desiredState.ToString());
            state = desiredState;
            return;
        }
        if (desiredState == PlayerState.Climbing)
        {
            Debug.Log(gameObject.name + " is now in state : " + desiredState.ToString());
            state = desiredState;
            return;
        }
        if (desiredState == PlayerState.Dead)
        {
            Debug.Log(gameObject.name + " is now in state : " + desiredState.ToString());
            GameManager.instance.RespawnPlayerCoroutineStarter(respawnTimer, gameObject);
            state = desiredState;
            if (currentCargo != null)
            {
                currentCargo.transform.parent = null;
            }
            anim.SetBool("isDying", true);
            return;
        }
        if (desiredState == PlayerState.Default)
        {
            Debug.Log(gameObject.name + " is now in state : " + desiredState.ToString());
            state = desiredState;
            anim.SetBool("isDying", false);
            return;
        }

    }
}
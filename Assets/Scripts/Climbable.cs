using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    public List<GameObject> climbers;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetAxis("Vertical" + other.gameObject.GetComponent<PlayerBehaviour>().index) < -0.75f)
        {
            Debug.Log(other.gameObject.name);
            climbers.Add(other.gameObject);
            other.gameObject.GetComponent<PlayerBehaviour>().State = PlayerState.Climbing;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.position = new Vector3(gameObject.transform.position.x, other.transform.position.y, 0);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (climbers.Contains(other.gameObject))
        {
            other.gameObject.GetComponent<PlayerBehaviour>().StopClimbing(false);
            climbers.Remove(other.gameObject);
        }
    }
}

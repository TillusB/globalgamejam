using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int affiliation;
    public bool setRandom = false;
    public bool locked;

    private SpriteRenderer sr;
    private GameObject chain;
    private Item myItem;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        chain = transform.GetChild(0).gameObject;
        if (!locked)
        {
            Unlock();
        }
        myItem = GetComponent<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tint(int index)
    {
        if (index == 1)
        {
            sr.color = new Color(255, 0, 0, 1);
            affiliation = index;
        }
        if (index == 2)
        {
            sr.color = new Color(0, 255, 0, 1);
            affiliation = index;
        }
        if (index == 3)
        {
            sr.color = new Color(0, 0, 255, 1);
            affiliation = index;
        }
        if (index == 4)
        {
            sr.color = new Color(100, 100, 0, 1);
            affiliation = index;
        }

        if (locked)
        {
            Unlock();
        }
        myItem.type = (PlayerType)affiliation;
    }

    public void Unlock()
    {
        chain.SetActive(false);
    }
}

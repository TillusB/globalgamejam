using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    public List<HouseSlot> slots;
    public float useFuelDelay;
    public float health;
    public float hungerDamage;
    public float speedReward;
    public Text countDownText;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        countDownText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (countDownText)
        {
            countDownText.text = ""+(useFuelDelay - time);
        }
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
            TakeDamage(takeDamage * hungerDamage);
        }
        else
        {
            GameManager.instance.AlterSpeed(speedReward);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        GameManager.instance.AlterSpeed(-1/damage);
        if(health <= 0)
        {
            GameManager.instance.GG();
        }
    }
}

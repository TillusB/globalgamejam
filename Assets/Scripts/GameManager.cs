using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Dictionary<int, PlayerBehaviour> players;
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button7)){

        }
        if (Input.GetKey(KeyCode.Joystick2Button7))
        {

        }
        if (Input.GetKey(KeyCode.Joystick3Button7))
        {

        }
        if (Input.GetKey(KeyCode.Joystick4Button7))
        {

        }

    }

    private void RegisterPlayer(int number)
    {
        if (players.ContainsKey(number))
        {
            return;
        }
        else
        {
            //Instantiate PlayerPrefab
            GameObject newPlayer = Instantiate(playerPrefab);
            players.Add(number, newPlayer.GetComponent<PlayerBehaviour>());
            newPlayer.GetComponent<PlayerBehaviour>().Init();
        }
    }
}

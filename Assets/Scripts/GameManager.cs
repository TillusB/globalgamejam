﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Dictionary<int, PlayerBehaviour> players;
    public GameObject playerPrefab;
    public List<GameObject> levelTiles;
    public Queue<GameObject> currentLevel;
    public float scrollSpeed = 15f;
    private float thresholdX;
    // Start is called before the first frame update
    void Start()
    {
        thresholdX = Camera.main.ScreenToWorldPoint(new Vector3(-Camera.main.pixelWidth / 2, 0, 0)).x;
        currentLevel = new Queue<GameObject>();
        for(int i = 1; i < 4; i++)
        {
            GameObject nextObj = Instantiate(levelTiles[Random.Range(0, levelTiles.Count)]);
            currentLevel.Enqueue(nextObj);
            nextObj.transform.position = (Camera.main.ScreenToWorldPoint(new Vector3((Camera.main.pixelWidth) * i, 0, 0)));

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveLevel();
        CheckLevel();
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

    private void MoveLevel()
    {
        foreach(GameObject tile in currentLevel)
        {
            tile.transform.Translate(-scrollSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void CheckLevel()
    {
        if (currentLevel.ToArray()[0].transform.position.x <= thresholdX)
        {
            GameObject nextObj = Instantiate(levelTiles[Random.Range(0, levelTiles.Count)]);
            currentLevel.Enqueue(nextObj);
            nextObj.transform.position.Set(Camera.main.ScreenToWorldPoint(new Vector3((Camera.main.pixelWidth)*2, 0, 0)).x, 1, 0);
            Destroy(currentLevel.Dequeue());
        }
    }
   
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType
{
    Red=1,
    Blue,
    Green,
    Yellow
}

public class GameManager : MonoBehaviour
{
    Dictionary<int, PlayerBehaviour> players;
    public GameObject playerPrefab;
    public List<GameObject> levelTiles;
    public Queue<GameObject> currentLevel;
    public float scrollSpeed = 15f;

    private float thresholdX;
    private float w;

    public Text distanceText;
    public float distanceCovered;

    public Vector3 respawnPosition;

    public List<bool> playerStatus;
    public static GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        w = levelTiles[0].GetComponent<BoxCollider2D>().size.x;
        thresholdX = -20;
        currentLevel = new Queue<GameObject>();
        for(int i = 1; i < 5; i++)
        {
            GameObject nextObj = Instantiate(levelTiles[Random.Range(0, levelTiles.Count)]);
            currentLevel.Enqueue(nextObj);
            nextObj.transform.position = new Vector3(20/2*(i-1)*2, 0, 0);
        }
    }

    private void Update()
    {
        distanceCovered += Time.deltaTime * scrollSpeed;
        UpdateDistanceText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveLevel();
        CheckLevel();
    }

    public void UpdateDistanceText()
    {
        if (distanceText != null)
        {
            distanceText.text = distanceCovered.ToString();
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
            nextObj.transform.position = new Vector3((20 / 2 * 6), 0, 0);
            Destroy(currentLevel.Dequeue());
        }
    }

    public void CheckPlayerStatus()
    {
        if (playerStatus.Count > 3)
        {
            RestartLevel();
        }
    }

    public void RespawnPlayerCoroutineStarter (float respawnTime, GameObject playerObject)
    {
        StartCoroutine(RespawnPlayer(respawnTime, playerObject));
    }
   
    public IEnumerator RespawnPlayer (float respawnTime, GameObject playerObject)
    {
        playerStatus.Add(playerObject);
        CheckPlayerStatus();
        yield return new WaitForSeconds(respawnTime);
        playerObject.transform.position = respawnPosition;
        playerObject.GetComponent<PlayerBehaviour>().State = PlayerState.Default;
        playerStatus.Remove(playerObject);
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

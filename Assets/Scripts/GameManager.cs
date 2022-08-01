using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public GameObject gameOverUI;
    public GameObject startGameUI;
    public GameObject canvas;
    public GameObject player;

    public bool isGameRunning;

    public float timeSinceLastSpawn;
    public float spawnIntervall;
    public float spawnIntervallMax;
    public float spawnIntervallMin;

    public float boundsX;
    public float boundsY;

    public Vector2 defaultPos = new Vector2(0,0);

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        isGameRunning = false;

    }

    private void Update()
    {
        if (!isGameRunning)
            return;

        if (timeSinceLastSpawn > spawnIntervall)
        {
            SpawnManager.Instance.SpawnFish(boundsX, boundsY);
            timeSinceLastSpawn = 0;
            spawnIntervall = UnityEngine.Random.Range(spawnIntervallMin, spawnIntervallMax);
        }
        else 
        {
            timeSinceLastSpawn += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        startGameUI.SetActive(false);
        gameOverUI.SetActive(false);
        var allFish = FindObjectsOfType<Fish>();
        foreach (var fish in allFish)
        {
            Destroy(fish.gameObject);
        }
        scoreText.text = "Score: 0";
        player.transform.position = defaultPos;
        var pc = player.GetComponent<PlayerController>();
        pc.size = pc.defaultSize;
        pc.transform.localScale = new Vector2(pc.size, pc.size);
        player.SetActive(true);
        score = 0;
        isGameRunning = true;
        canvas.SetActive(true);

        SoundModule.Instance.PlayBlubb();
    }
    public void GameOver(bool hasWon)
    {
        canvas.SetActive(false);
        Debug.Log($"Game Over, Score: {score}");
        isGameRunning = false;

        var gameOverScript = gameOverUI.GetComponent<GameOverUIScript>();
        gameOverScript.HasWon = hasWon;
        gameOverScript.Score = score;

        gameOverUI.SetActive(true);
        player.SetActive(false);
    }

    public void AddScore()
    {
        score += 10;
        scoreText.text = "Score: "+ score;
    }

   
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public static GameManager Instance;

    public float boundsX;
    public float boundsY;

    public float minSize;
    public float maxSize;

    public float spawnIntervall;
    public float spawnIntervallMax;
    public float spawnIntervallMin;

    public bool isGameRunning;

    public float timeSinceLastSpawn;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    public List<ColorValue> colorScale;

    public GameObject gameOverUI;
    public GameObject canvas;
    public GameObject player;

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
            SpawnFish();
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
        var allFish = GameObject.FindObjectsOfType<Fish>();
        foreach (var fish in allFish)
        {
            Destroy(fish.gameObject);
        }
        scoreText.text = "Score: 0";
        player.transform.position = defaultPos;
        player.SetActive(true);
        score = 0;
        isGameRunning = true;
        canvas.SetActive(true);
        gameOverUI.SetActive(false);
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


        if (hasWon) 
        {
            //Show Win GUI
        }
        else 
        {
            //Show loose GUI
        }
    }

    public void AddScore()
    {
        score += 10;
        scoreText.text = "Score: "+ score;
    }

    void SpawnFish()
    {
        Vector2 spawnPos = new Vector2();
        var side = UnityEngine.Random.Range(-1, 2);
        if (side < 0)
            spawnPos.x = -boundsX;
        else
            spawnPos.x = boundsX;

        spawnPos.y = UnityEngine.Random.Range(-boundsY, boundsY);

        var fish = Instantiate(fishPrefab).GetComponent<Fish>();
        fish.moveDirection = side < 0 ? 1 : -1;
        fish.size = UnityEngine.Random.Range(minSize, maxSize);

      

        fish.gameObject.transform.position = spawnPos;
        fish.gameObject.transform.localScale = new Vector2(fish.size, fish.size);

        var spriteRenderer = fish.GetComponent<SpriteRenderer>();

        if (side >= 0)
           spriteRenderer.flipX = true;

        spriteRenderer.color = GetFishColor((int)(fish.size * 100));
    }

    public Color GetFishColor(int size)
    {
        Color color = new Color();
        var orderedColors = colorScale.OrderBy(m => m.Key);

        foreach (var kvp in orderedColors)
        {
            if (size > kvp.Key)
                color = kvp.Value;
        }

        return color;
    }

    [Serializable]
    public struct ColorValue
    {
        public int Key;
        public Color Value;
    }
}

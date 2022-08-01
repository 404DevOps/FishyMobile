using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public List<ColorValue> colorScale;
    public static SpawnManager Instance;

    public float minSize;
    public float maxSize;

    public float minSpeed;
    public float maxSpeed;

    public int spawnCount;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }    
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnFish(float boundsX, float boundsY)
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

        //every fifth fish should be smaller than the player
        if (spawnCount > 5)
        {
            spawnCount = 0;
            float playerSize = GameManager.Instance.player.GetComponent<PlayerController>().size;
            fish.size = UnityEngine.Random.Range(minSize, playerSize > maxSize ? maxSize : playerSize);
        }
        else
        {
            fish.size = UnityEngine.Random.Range(minSize, maxSize);
            spawnCount++;
        }

        fish.moveSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        fish.gameObject.transform.position = spawnPos;
        fish.gameObject.transform.localScale = new Vector2(fish.size, fish.size);

        var spriteRenderer = fish.GetComponent<SpriteRenderer>();

        if (side >= 0)
            fish.transform.rotation = new Quaternion(0, 180, 0, 0);

        var randColor = GetFishColor((int)(fish.size * 100));
        //spriteRenderer.material.color = randColor;
        spriteRenderer.color = randColor;
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

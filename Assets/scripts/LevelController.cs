using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public Vector3 size = new Vector3(10, 10, 10);
    public GameObject ground, roof, westWall, eastWall, northWall, southWall;
    public int numApples = 3;

    public GameObject applePrefab;
    public SnakeController snakePrefab;
    public GameObject blockPrefab;

    public GameController gameController;
    public SnakeController playerSnake;
    public Camera mainCamera;

    public List<SnakeController> snakes;
    public List<GameObject> apples;
    public List<GameObject> blocks;

    void Start ()
    {
        Create(size);
        playerSnake = snakes[0];
	}

    public void Create(Vector3 size)
    {
        this.size = size;
        ground.transform.position = new Vector3(0 - 0.5f, -size.y - 0.5f - 0.0001f, 0 - 0.5f);
        ground.transform.localScale = new Vector3(size.x / 10, 1, size.z / 10);
        roof.transform.position = new Vector3(0 - 0.5f, size.y - 0.5f + 0.0001f, 0 - 0.5f);
        roof.transform.localScale = new Vector3(size.x / 10, 1, size.z / 10);
        westWall.transform.position = new Vector3(0 - 0.5f, 0 - 0.5f, -size.z - 0.5f - 0.0001f);
        westWall.transform.localScale = new Vector3(size.x / 10, 1, size.y / 10);
        eastWall.transform.position = new Vector3(0 - 0.5f, 0 - 0.5f, size.z - 0.5f + 0.0001f);
        eastWall.transform.localScale = new Vector3(size.x / 10, 1, size.y / 10);
        southWall.transform.position = new Vector3(-size.x - 0.5f - 0.0001f, 0 - 0.5f, 0 - 0.5f);
        southWall.transform.localScale = new Vector3(size.z/10, 1, size.y / 10);
        northWall.transform.position = new Vector3(size.x - 0.5f + 0.0001f, 0 - 0.5f, 0 - 0.5f);
        northWall.transform.localScale = new Vector3(size.z / 10, 1, size.y / 10);

        foreach (var apple in apples) Destroy(apple);
        foreach (var block in blocks) Destroy(block);
        foreach (var snake in snakes) Destroy(snake);

        apples = new List<GameObject>();
        blocks = new List<GameObject>();
        snakes = new List<SnakeController>();

        playerSnake = Instantiate(snakePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        playerSnake.level = this;
        playerSnake.mainCamera = mainCamera;
        mainCamera.GetComponent<FollowCamera>().target = playerSnake.segments[0];
        mainCamera.GetComponent<FollowCamera>().SnapToTarget();
        snakes.Add(playerSnake);

        var random = new System.Random();
        while (blocks.Count < 20)
        {
            var x = random.Next((int)-size.x, (int)size.x);
            var y = random.Next((int)-size.y, (int)size.y);
            var z = random.Next((int)-size.z, (int)size.z);

            foreach (GameObject apple in apples)
            {
                if (Math.Floor(apple.transform.position.x) == x && Math.Floor(apple.transform.position.y) == y && Math.Floor(apple.transform.position.z) == z)
                    goto OUTER_CONTINUE;
            }

            foreach (GameObject block in blocks)
            {
                if (Math.Floor(block.transform.position.x) == x && Math.Floor(block.transform.position.y) == y && Math.Floor(block.transform.position.z) == z)
                    goto OUTER_CONTINUE;
            }

            foreach (SnakeController snake in snakes)
            {
                foreach (GameObject segment in snake.segments)
                {
                    if (Math.Floor(segment.transform.position.x) == x && Math.Floor(segment.transform.position.y) == y && Math.Floor(segment.transform.position.z) == z)
                        goto OUTER_CONTINUE;
                }
            }

            var newBlock = Instantiate(blockPrefab, new Vector3(x, y, z), Quaternion.identity);
            blocks.Add(newBlock);

            OUTER_CONTINUE:;
        }
    }

	void LateUpdate ()
    {
        SpawnApples();
	}

    private void SpawnApples()
    {
        var random = new System.Random();
        while (apples.Count < numApples)
        {
            var x = random.Next((int) -size.x, (int) size.x);
            var y = random.Next((int) -size.y, (int) size.y);
            var z = random.Next((int) -size.z, (int) size.z);

            foreach (GameObject apple in apples)
            {
                if (Math.Floor(apple.transform.position.x) == x && Math.Floor(apple.transform.position.y) == y && Math.Floor(apple.transform.position.z) == z)
                    goto OUTER_CONTINUE;
            }

            foreach (GameObject block in blocks)
            {
                if (Math.Floor(block.transform.position.x) == x && Math.Floor(block.transform.position.y) == y && Math.Floor(block.transform.position.z) == z)
                    goto OUTER_CONTINUE;
            }

            foreach (SnakeController snake in snakes)
            {
                foreach (GameObject segment in snake.segments)
                {
                    if (Math.Floor(segment.transform.position.x) == x && Math.Floor(segment.transform.position.y) == y && Math.Floor(segment.transform.position.z) == z)
                        goto OUTER_CONTINUE;
                }
            }

            var newApple = Instantiate(applePrefab, new Vector3(x, y, z), Quaternion.identity);
            apples.Add(newApple);

            OUTER_CONTINUE: ;
        }
    }

    public void GameOver()
    {
        gameController.GameOver();
    }
}

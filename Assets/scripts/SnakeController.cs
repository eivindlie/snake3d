using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SnakeController : MonoBehaviour {

    public GameObject mainCamera;
    public List<GameObject> segments;
    public Vector3 direction = new Vector3(0, 0, 0);
    public GameObject segmentPrefab;
    public LevelController level;

    private Stopwatch stopwatch;
    private int eatenApples = 0;

	// Use this for initialization
	void Start ()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }
	
	// Update is called once per frame
	void Update ()
    {
        SetDirection();

        if (stopwatch.ElapsedMilliseconds > 1000/8)
        {
            stopwatch.Reset();
            stopwatch.Start();
            Move();
        }
    }

    private void LateUpdate()
    {
        CheckCollision();
    }

    private void SetDirection()
    {
        if (Input.GetKeyDown("left"))
        {
            direction = new Vector3(0, -1, 0);
        }
        else if (Input.GetKeyDown("right"))
        {
            direction = new Vector3(0, 1, 0);
        }
        else if (Input.GetKeyDown("down"))
        {
            direction = new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown("up"))
        {
            direction = new Vector3(-1, 0, 0);
        }
    }

    private void Move()
    {
        if (eatenApples > 0)
        {
            var segment = Instantiate(segmentPrefab, segments[segments.Count - 1].transform.position, Quaternion.identity);
            segments.Add(segment);
            eatenApples--;
        }
        for (var i = segments.Count - 1; i > 0; i--)
        {
            segments[i].transform.position = segments[i - 1].transform.position;
        }
        if (direction.magnitude > 0)
        {
            if(segments[0].transform.eulerAngles.x != 0)
            {
                if (direction.x == -1)
                {
                    direction = new Vector3(0, 0, 0);
                }
                else if(direction.x == 1)
                {
                    direction = new Vector3(2, 0, 0);
                }
            } 
            var camDir = mainCamera.transform.eulerAngles;
            camDir = new Vector3(Mathf.Round(camDir.x / 90) * 90, Mathf.Round(camDir.y / 90) * 90, Mathf.Round(camDir.z / 90) * 90);
            segments[0].transform.eulerAngles = camDir + direction * 90;
            direction = new Vector3(0, 0, 0);
        }
        segments[0].transform.position += segments[0].transform.forward;
    }

    private void CheckCollision()
    {
        foreach (var apple in level.apples)
        {
            if (apple.transform.position == segments[0].transform.position)
            {
                level.apples.Remove(apple);
                UnityEngine.Object.Destroy(apple);
                eatenApples++;
                break;
            }
        }

        if (! (new Bounds(new Vector3(-0.5f, -0.5f, -0.5f), level.size * 2).Contains(segments[0].transform.position)))
        {
            UnityEngine.Debug.Log("Game Over 1! Final Score: " + segments.Count);
            Application.Quit();
        }

        foreach(var snake in level.snakes)
        {
            foreach (var segment in snake.segments)
            {
                if (segment != segments[0] && segment.transform.position == segments[0].transform.position)
                {
                    UnityEngine.Debug.Log("Game Over 2! Final Score: " + segments.Count);
                    Application.Quit();
                }
            }
        }
    }
}

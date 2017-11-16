using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SnakeController : MonoBehaviour {

    public GameObject mainCamera;
    public List<GameObject> segments;
    public Direction direction = Direction.FORWARD;
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
        if (Input.GetButtonDown("Left"))
        {
            direction = Direction.LEFT;
        }
        else if (Input.GetButtonDown("Right"))
        {
            direction = Direction.RIGHT;
        }
        else if (Input.GetButtonDown("Down"))
        {
            direction = Direction.DOWN;
        }
        else if (Input.GetButtonDown("Up"))
        {
            direction = Direction.UP;
        }
    }

    private Vector3 RightAnglify(Vector3 vec)
    {
        return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
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

        var vertical = segments[0].transform.forward.y != 0;
        Vector3 forward = new Vector3(0, 0, 0);
        Vector3 up = RightAnglify(mainCamera.transform.up);
        switch (direction)
        {
            case Direction.UP:
                if (vertical)
                {
                    forward = RightAnglify(mainCamera.transform.forward);
                }
                else
                {
                    forward = RightAnglify(mainCamera.transform.up);
                    up = RightAnglify(-mainCamera.transform.forward);
                }
                break;
            case Direction.DOWN:
                if(vertical)
                {
                    forward = RightAnglify(-mainCamera.transform.forward);
                }
                else
                {
                    forward = RightAnglify(-mainCamera.transform.up);
                    up = RightAnglify(mainCamera.transform.forward);
                }
                break;
            case Direction.LEFT:
                forward = RightAnglify(-mainCamera.transform.right);
                break;
            case Direction.RIGHT:
                forward = RightAnglify(mainCamera.transform.right);
                break;
        }
        if(forward.magnitude != 0)
            segments[0].transform.rotation = Quaternion.LookRotation(forward, up);
        segments[0].transform.position += segments[0].transform.forward;

        direction = Direction.FORWARD;
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

    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT, FORWARD
    }
}

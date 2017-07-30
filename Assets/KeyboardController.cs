using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
	public Player player;
	public float speed;

	private bool isRunning = false;
	private GameObject background;
	private EdgeCollider2D boundary;

	// Use this for initialization
	void Start()
	{
		background = GameObject.FindGameObjectWithTag("Background");
		boundary = background.GetComponentInChildren<EdgeCollider2D>();
	}

	void Update()
	{
		player.SetRunning(isRunning);
	}

	void FixedUpdate()
	{
		if (isRunning)
		{
			isRunning = false;
		}

		if (player == null)
		{
			return;
		}

		if (Input.GetKey(KeyCode.W))
		{
			// Up
			Vector3 newPos = player.transform.position + new Vector3(0f, Mathf.Lerp(0, 1f, Time.deltaTime), 0f) * speed;

			UpdatePosition(newPos);
		}

		if (Input.GetKey(KeyCode.A))
		{
			// Left
			Vector3 newPos = player.transform.position + new Vector3(Mathf.Lerp(0, -1f, Time.deltaTime), 0f, 0f) * speed;

			UpdatePosition(newPos);
		}

		if (Input.GetKey(KeyCode.S))
		{
			// Down
			Vector3 newPos = player.transform.position + new Vector3(0f, Mathf.Lerp(0, -1f, Time.deltaTime), 0f) * speed;

			UpdatePosition(newPos);
		}

		if (Input.GetKey(KeyCode.D))
		{
			// Right
			Vector3 newPos = player.transform.position + new Vector3(Mathf.Lerp(0, 1f, Time.deltaTime), 0f, 0f) * speed;

			UpdatePosition(newPos);
		}
	}

	private void UpdatePosition(Vector3 newPos)
	{
		if (boundary.bounds.Contains(newPos))
		{
			player.transform.position = newPos;

			isRunning = true;
		}
	}
}

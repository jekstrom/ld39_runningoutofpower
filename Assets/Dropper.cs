using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
	public GameObject[] drops;
	public float chance;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public GameObject GetDrop()
	{
		if (Random.Range(0f, 100f) >= chance)
		{
			return drops[Random.Range(0, drops.Length)];
		}
		return null;
	}
}

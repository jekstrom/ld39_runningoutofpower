using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public Player player;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetPlayer(Player p)
	{
		player = p;
	}

	public Henchmen Spawn(Henchmen henchmen)
	{
		Henchmen spawned = Instantiate(henchmen, transform.position, transform.rotation);
		spawned.player = player;

		return spawned;
	}

	public Boss Spawn(Boss ning)
	{
		//GameObject spawnedgo = Instantiate(ning.GameObject, transform.position, transform.rotation);
		Boss spawned = Instantiate(ning, transform.position, transform.rotation);

		//IBoss spawned = spawnedgo.GetComponent<IBoss>();

		spawned.Player = player;

		return spawned;
	}
}

using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
	[System.Serializable]
	public class Wave
	{
		public int count;
		public Henchmen enemy;
		public Boss boss;
		public float cooldown;

		[System.NonSerialized]
		public int spawned = 0;
	}

	public Player player;
	public Wave[] waves;
	public SpawnPoint[] spawnPoints;
	
	private float cooldownRemaining;
	private int lastWaveIndex = 0;
	private Boss _spawnedBoss;
	private bool _bossLevel = false;
	private int enemiesRemaining = 0;

	// Use this for initialization
	void Start()
	{
		_spawnedBoss = null;

		foreach (SpawnPoint sp in spawnPoints)
		{
			sp.player = player;
		}
	}

	// Update is called once per frame
	void Update()
	{
		cooldownRemaining -= Time.deltaTime;

		if (lastWaveIndex < waves.Length && !_bossLevel)
		{
			Wave wave = waves[lastWaveIndex];

			if (cooldownRemaining <= 0 && wave.spawned < wave.count)
			{
				Henchmen henchmen = spawnPoints[Random.Range(0, spawnPoints.Length)].Spawn(wave.enemy);

				henchmen.OnDeath = () =>
				{
					enemiesRemaining--;
				};

				enemiesRemaining++;
				wave.spawned++;
				cooldownRemaining = wave.cooldown;
			} else if (wave.spawned == wave.count && enemiesRemaining == 0)
			{
				_spawnedBoss = spawnPoints[Random.Range(0, spawnPoints.Length)].Spawn(wave.boss);

				_spawnedBoss.Health += 100 * lastWaveIndex;

				_spawnedBoss.OnDeath = () =>
				{
					_bossLevel = false;
					lastWaveIndex++;
				};

				_bossLevel = true;
			}
		} else
		{
			// Game over, all waves completed, you ran Ning out of Power.
		}
	}

	public int GetCurrentWave()
	{
		return lastWaveIndex + 1;
	}

	public int GetTotalWaves()
	{
		return waves.Length;
	}
}

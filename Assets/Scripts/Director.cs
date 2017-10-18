using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Director : MonoBehaviour 
{
	public TerrainMap terrainMap;
	public Transform player;
	public EnemyConfiguration[] enemyTypes;

	bool isInitialized = false;

	float nextBiomeTimeDelay = 5.0f;
	float nextBiomeTimer = 0.0f;

	void Start()
	{
		enemyTypes = GameController.GetController<ConfigurationController>().EnemyConfig;
	}

	// Update is called once per frame
	void Update () 
	{
		if (!isInitialized)
		{
			isInitialized = true;
			terrainMap.GenerateTerrain();

			SpawnNextBiome();

			player.position = terrainMap.GetRandomTileOnCurrentBiome().transform.position;
		}

		nextBiomeTimer -= Time.deltaTime;
		if (nextBiomeTimer <= 0.0f)
		{
			SpawnNextBiome();
		}
	}

	void SpawnNextBiome()
	{
		terrainMap.DisplayNextBiome();
		nextBiomeTimer = nextBiomeTimeDelay;

		EnemyConfiguration nextEnemyType = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];
		GameObject newEnemy = (GameObject)Instantiate(nextEnemyType.Prefab);

		newEnemy.GetComponent<NavMeshAgent>().speed = nextEnemyType.speed;
		newEnemy.transform.position = terrainMap.GetRandomTileOnCurrentBiome().transform.position;
		newEnemy.transform.parent = this.gameObject.transform;
		newEnemy.name = nextEnemyType.name;
		newEnemy.GetComponent<Enemy>().Health = 10;
	}
}

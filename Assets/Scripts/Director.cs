using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour 
{
	public TerrainMap terrainMap;
	public Transform player;
	public GameObject[] enemyPrefabs;

	bool isInitialized = false;

	float nextBiomeTimeDelay = 5.0f;
	float nextBiomeTimer = 0.0f;
	
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

		GameObject newEnemy = (GameObject)Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)]);
		newEnemy.transform.position = terrainMap.GetRandomTileOnCurrentBiome().transform.position;
		newEnemy.transform.parent = this.gameObject.transform;
	}
}

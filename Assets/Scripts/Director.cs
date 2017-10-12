using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour 
{
	public TerrainMap terrainMap;
	public Transform player;

	bool isInitialized = false;

	float nextBiomeTimeDelay = 5.0f;
	float nextBiomeTimer = 0.0f;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isInitialized)
		{
			isInitialized = true;
			terrainMap.GenerateTerrain();

			terrainMap.DisplayNextBiome();
			nextBiomeTimer = nextBiomeTimeDelay;

			player.position = terrainMap.GetRandomTileOnCurrentBiome().transform.position;
		}

		nextBiomeTimer -= Time.deltaTime;
		if (nextBiomeTimer <= 0.0f)
		{
			terrainMap.DisplayNextBiome();
			nextBiomeTimer = nextBiomeTimeDelay;
		}
	}
}

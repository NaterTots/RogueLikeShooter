using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour 
{
	public TerrainMap terrainMap;
	public Player player;

	bool isInitialized = false;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isInitialized)
		{
			terrainMap.GenerateTerrain();

			terrainMap.DisplayNextBiome();

			var startingPosition = terrainMap.GetRandomTileOnCurrentBiome().GetComponent<Transform>();
			player.transform.SetPositionAndRotation(startingPosition.position + Vector3.up * 3, Quaternion.identity);

			isInitialized = true;
		}
	}
}

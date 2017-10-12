using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TerrainMap : MonoBehaviour
{
	//for now, this is just a square
	private MapSquareInfo[,] terrainMap;

	private Biome[] biomes;

	private List<Biome> biomePath = new List<Biome>();
	private int currentBiome = -1;

	public TerrainConfiguration TerrainConfig;

	public int TerrainSize = 100;

	public GameObject terrainTile;

	// Use this for initialization
	void Start()
	{
		TerrainConfig = GameController.GetController<ConfigurationController>().TerrainConfig;
		TerrainSize = TerrainConfig.World.width;

		InitializeTerrainMap();
		//GenerateTerrain();
	}

	public void GenerateTerrain()
	{
		InitializeBiomes();

		PlaceBiomes();
		FloodFillBiomes();

		RefreshTerrainDisplay();

		ConstructBiomePath();
	}

	private void InitializeTerrainMap()
	{
		terrainMap = new MapSquareInfo[100, 100];
		for (int xMap = 0; xMap < TerrainSize; xMap++)
		{
			for (int yMap = 0; yMap < TerrainSize; yMap++)
			{
				GameObject newTerrainTile = (GameObject)Instantiate(terrainTile);
				//newTerrainTile.transform.position = new Vector2(
				//	xMap - (TerrainSize / 2),
				//	yMap - (TerrainSize / 2));

				newTerrainTile.transform.position = new Vector3(
					((xMap - (TerrainSize / 2)) * 4),
					0f,
					((yMap - (TerrainSize / 2)) * 4));

				newTerrainTile.transform.parent = this.gameObject.transform;
				newTerrainTile.name = "Terrain Tile [" + xMap + ", " + yMap + "]";
				terrainMap[xMap, yMap] = new MapSquareInfo();
				terrainMap[xMap, yMap].tile = newTerrainTile;
				terrainMap[xMap, yMap].mapPoint = new TerrainMapPoint() { x = xMap, y = yMap };
			}
		}
	}

	private void InitializeBiomes()
	{
		Debug.Log("Initializing Biomes " + Time.realtimeSinceStartup);

		biomes = new Biome[TerrainConfig.Biome.biomeCount];
		for (int i = 0; i < biomes.Length; i++)
		{
			biomes[i] = new Biome();
		}
	}

	private void PlaceBiomes()
	{
		Debug.Log("Spotting Random Biomes " + Time.realtimeSinceStartup);

		//spot random biomes throughout the map
		for (int i = 0; i < biomes.Length; i++)
		{
			biomes[i].Initialize(GetRandomTerrainType(), GetRandomEmptyPoint(), this);

			//this is done to make sure they have some size, although it can still result in biomes of a size of 1
			biomes[i].FloodFill(TerrainConfig.Biome.initialFloodFillAmount);
		}
	}

	private void FloodFillBiomes()
	{
		Debug.Log("Incremental Flood Filling Biomes " + Time.realtimeSinceStartup);
		//flood fill the biomes
		bool allComplete = true;
		do
		{
			allComplete = true;

			for (int i = 0; i < biomes.Length; i++)
			{
				if (!biomes[i].FloodComplete)
				{
					biomes[i].FloodFill(TerrainConfig.Biome.floodFillIncrement);
					allComplete &= biomes[i].FloodComplete;
				}
			}
		} while (!allComplete);
	}

	private void RefreshTerrainDisplay()
	{
		for (int x = 0; x < TerrainConfig.World.width; x++)
		{
			for (int y = 0; y < TerrainConfig.World.height; y++)
			{
				var tile = terrainMap[x, y].tile.GetComponent<TerrainTile>();
				tile.Init(terrainMap[x, y].type);
				tile.Hide();
			}
		}
	}

	public GameObject GetRandomTileOnCurrentBiome()
	{
		return biomePath[currentBiome].GetRandomMapSquareInfo().tile;
	}

	#region Biome Path

	public void DisplayNextBiome()
	{
		currentBiome++;
		if (biomePath.Count > currentBiome)
		{
			biomePath[currentBiome].Display();
		}
	}

	private void ConstructBiomePath()
	{
		Debug.Log("Getting Biome Neighbors " + Time.realtimeSinceStartup.ToString());
		GetBiomeNeighbors();
		Debug.Log("Finished Getting Biome Neighbors " + Time.realtimeSinceStartup.ToString());


		if (BruteForceTryToFindPath())
		{
			//biomePath[currentBiome].Display();
		}
		else
		{
			//DisplayRandomBiome();
		}
	}

	private void GetBiomeNeighbors()
	{
		foreach (Biome b in biomes)
		{
			b.FindAllNeighbors(this);
		}
	}

	private bool BruteForceTryToFindPath()
	{
		List<List<int>> biomeNodeSet = new List<List<int>>();
		var firstNode = new List<int>();
		for (int i = 0; i < biomes.Length; ++i)
		{
			firstNode.Add(biomes[i].BiomeID);
		}
		biomeNodeSet.Add(firstNode);

		while (biomeNodeSet.Count > 0 && biomeNodeSet.Count < TerrainConfig.Biome.biomeSequencePerLevel)
		{
			if (!TryToAddNewNeighbor(biomeNodeSet))
			{
				RemoveLatestNode(biomeNodeSet);
			}
		}

		if (biomeNodeSet.Count > 0)
		{
			foreach(var biomeNode in biomeNodeSet)
			{
				biomePath.Add(GetBiomeById(biomeNode[0]));
			}
			return true;
		}

		return false;
	}

	private bool TryToAddNewNeighbor(List<List<int>> biomePath)
	{
		List<int> nextNodeList = new List<int>();
		foreach (int neighborId in GetBiomeById(biomePath[biomePath.Count - 1][0]).Neighbors)
		{
			if (!BiomeIsInCurrentPath(biomePath, neighborId))
			{
				nextNodeList.Add(neighborId);
			}
		}

		if (nextNodeList.Count > 0)
		{
			biomePath.Add(nextNodeList);
			return true;
		}

		return false;
	}

	private Biome GetBiomeById(int biomeid)
	{
		foreach(Biome b in biomes)
		{
			if (b.BiomeID == biomeid)
			{
				return b;
			}
		}
		return null;
	}

	private void RemoveLatestNode(List<List<int>> biomePath)
	{
		int lastNode = biomePath.Count - 1;
		biomePath[lastNode].RemoveAt(0);
		if (biomePath[lastNode].Count == 0)
		{
			biomePath.RemoveAt(lastNode);
		}
	}

	private bool BiomeIsInCurrentPath(List<List<int>> biomePath, int b)
	{
		foreach(var biomeSet in biomePath)
		{
			if (biomeSet[0] == b)
			{
				return true;
			}
		}

		return false;
	}

	#endregion Biome Path

	public void DisplayRandomBiome()
	{
		int randomBiome = UnityEngine.Random.Range(0, biomes.Length);
		Debug.LogWarning("Random Biome Displayed: " + randomBiome.ToString());
		biomes[randomBiome].Display();
	}

	private TerrainMapPoint GetRandomEmptyPoint()
	{
		TerrainMapPoint randomPoint = new TerrainMapPoint();

		do
		{
			randomPoint.x = UnityEngine.Random.Range(0, TerrainSize);
			randomPoint.y = UnityEngine.Random.Range(0, TerrainSize);
		} while (GetTerrainType(randomPoint) != TerrainType.None);

		return randomPoint;
	}

	private TerrainType GetRandomTerrainType()
	{

		switch (UnityEngine.Random.Range(0, TerrainConfig.Biome.biomeTypeCount))
		{
			case 0:
				return TerrainType.Forest;
			case 1:
				return TerrainType.Lava;
			case 2:
				return TerrainType.Rock;
			case 3:
				return TerrainType.Grass;
			case 4:
				return TerrainType.Empty;
			default:
				return TerrainType.None;
		}
	}

	public bool TrySetPointForBiome(TerrainMapPoint point, Biome biome, out MapSquareInfo squareInfo)
	{
		if (GetTerrainType(point) != TerrainType.None)
		{
			squareInfo = null;
			return false;
		}
		else
		{
			terrainMap[point.x, point.y].biomeid = biome.BiomeID;
			terrainMap[point.x, point.y].type = biome.TerrainType;
			squareInfo = terrainMap[point.x, point.y];
			return true;
		}
	}

	public bool IsValid(TerrainMapPoint point)
	{
		return (
			point.x >= 0 &&
			point.x < TerrainSize &&
			point.y >= 0 &&
			point.y < TerrainSize
			);
	}

	public TerrainType GetTerrainType(TerrainMapPoint point)
	{
		return terrainMap[point.x, point.y].type;
	}

	public bool TryGetEmptyMapPoint(TerrainMapPoint point, Direction dir, out TerrainMapPoint newMapPoint)
	{
		return TryGetMapPoint(point, dir, true, out newMapPoint);
	}

	public bool TryGetMapPoint(TerrainMapPoint point, Direction dir, bool onlyIfEmpty, out TerrainMapPoint newMapPoint)
	{
		newMapPoint = new TerrainMapPoint()
		{
			x = point.x,
			y = point.y
		};

		switch (dir)
		{
			case Direction.Up:
				++newMapPoint.x;
				break;
			case Direction.Down:
				--newMapPoint.x;
				break;
			case Direction.Left:
				--newMapPoint.y;
				break;
			case Direction.Right:
				++newMapPoint.y;
				break;
		}

		return (IsValid(newMapPoint) && (onlyIfEmpty ? (GetTerrainType(newMapPoint) == TerrainType.None) : true));
	}

	public bool TryGetSquareInfo(TerrainMapPoint point, Direction dir, out MapSquareInfo squareInfo)
	{
		squareInfo = null;

		var newMapPoint = new TerrainMapPoint()
		{
			x = point.x,
			y = point.y
		};

		switch (dir)
		{
			case Direction.Up:
				++newMapPoint.x;
				break;
			case Direction.Down:
				--newMapPoint.x;
				break;
			case Direction.Left:
				--newMapPoint.y;
				break;
			case Direction.Right:
				++newMapPoint.y;
				break;
		}

		if (IsValid(newMapPoint))
		{
			squareInfo = terrainMap[newMapPoint.x, newMapPoint.y];
		}

		return (squareInfo != null);
	}

	public void ResetTerrain()
	{
		for (int x = 0; x < TerrainSize; x++)
		{
			for (int y = 0; y < TerrainSize; y++)
			{
				terrainMap[x, y].type = TerrainType.None;
				terrainMap[x, y].biomeid = 0;
			}
		}

		biomePath.Clear();
		currentBiome = 0;


		RefreshTerrainDisplay();
	}

	public TerrainMapPoint GetCenterPoint()
	{
		return new TerrainMapPoint()
		{
			x = (int)(TerrainSize / 2),
			y = (int)(TerrainSize / 2)
		};
	}
}

public class MapSquareInfo
{
	public TerrainType type;
	public GameObject tile;
	public TerrainMapPoint mapPoint;
	public int biomeid;
}

public struct TerrainMapPoint
{
	public int x;
	public int y;
}
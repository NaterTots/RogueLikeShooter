using UnityEngine;
using System;
using System.Collections;

public class TerrainMap : MonoBehaviour
{
	internal struct MapSquareInfo
	{
		internal TerrainType type;
		internal GameObject tile;
	}

	//for now, this is just a square
	private MapSquareInfo[,] terrainMap;

	private Biome[] biomes;

	public TerrainConfiguration TerrainConfig;

	public const long TerrainSize = 100;

	public GameObject terrainTile;

	// Use this for initialization
	void Start()
	{
		TerrainConfig = GameController.GetController<ConfigurationController>().TerrainConfig;

		InitializeTerrainMap();
		GenerateTerrain();
	}

	public void GenerateTerrain()
	{
		InitializeBiomes();

		PlaceBiomes();
		FloodFillBiomes();

		RefreshTerrainDisplay();
	}

	private void InitializeTerrainMap()
	{
		terrainMap = new MapSquareInfo[100, 100];
		for (int x = 0; x < TerrainSize; x++)
		{
			for (int y = 0; y < TerrainSize; y++)
			{
				GameObject newTerrainTile = (GameObject)Instantiate(terrainTile);
				newTerrainTile.transform.position = new Vector2(
					x - (TerrainSize / 2),
					y - (TerrainSize / 2));
				newTerrainTile.transform.parent = this.gameObject.transform;
				newTerrainTile.name = "Terrain Tile [" + x + ", " + y + "]";
				terrainMap[x, y].tile = newTerrainTile;
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
				terrainMap[x, y].tile.GetComponent<TerrainTile>().Init(terrainMap[x, y].type);
			}
		}
	}

	private TerrainMapPoint GetRandomEmptyPoint()
	{
		TerrainMapPoint randomPoint = new TerrainMapPoint();

		do
		{
			randomPoint.x = UnityEngine.Random.Range(0, 100);
			randomPoint.y = UnityEngine.Random.Range(0, 100);
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

	public bool SetPoint(TerrainMapPoint point, TerrainType type)
	{
		if (GetTerrainType(point) != TerrainType.None)
		{
			return false;
		}
		else
		{
			terrainMap[point.x, point.y].type = type;
		}

		return true;
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

	public bool TryGetMapPoint(TerrainMapPoint point, Direction dir, out TerrainMapPoint newMapPoint)
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

		return IsValid(newMapPoint) && (GetTerrainType(newMapPoint) == TerrainType.None);
	}

	public void ResetTerrain()
	{
		for (int x = 0; x < TerrainSize; x++)
		{
			for (int y = 0; y < TerrainSize; y++)
			{
				terrainMap[x, y].type = TerrainType.None;
			}
		}
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

public struct TerrainMapPoint
{
	public int x;
	public int y;
}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Biome
{
	public TerrainType TerrainType
	{
		get;
		private set;
	}
	private List<MapSquareInfo> mapSquareInfoPoints = new List<MapSquareInfo>();

	private TerrainMap terrain;

	public bool isDisplayed = false;

	private List<int> neighbors = new List<int>();
	public List<int> Neighbors
	{
		get
		{
			return neighbors;
		}
	}

	private static int biomeID = 1;
	private int thisBiomeID;
	public int BiomeID
	{
		get
		{
			return thisBiomeID;
		}
	}


	/// <summary>
	/// Denotes when the Biome can no longer Flood Fills 
	/// </summary>
	public bool FloodComplete = false;

	private int floodFillIndex = 0;

	public void Initialize(TerrainType type, TerrainMapPoint initPoint, TerrainMap t)
	{
		thisBiomeID = biomeID++;

		terrain = t;
		TerrainType = type;

		MapSquareInfo squareInfo;
		if (terrain.TrySetPointForBiome(initPoint, this, out squareInfo))
		{
			mapSquareInfoPoints.Add(squareInfo);
		}
	}

	public void FloodFill(int points)
	{
		for (int i = 0; i < points && !FloodComplete; i++)
		{
			FloodFill();
		}
	}

	public void FloodFill()
	{
		if (FloodComplete) return;

		bool bSuccess = false;

		do
		{
			if (FillFromPoint(mapSquareInfoPoints[floodFillIndex].mapPoint))
			{
				bSuccess = true;
			}
			else
			{
				floodFillIndex++;
			}
		} while (!bSuccess && (floodFillIndex < mapSquareInfoPoints.Count));

		if (!bSuccess)
		{
			FloodComplete = true;
		}
	}

	public void Reset()
	{
		TerrainType = TerrainType.None;
		mapSquareInfoPoints.Clear();

		floodFillIndex = 0;
		FloodComplete = false;
		isDisplayed = false;
	}

	public void Display()
	{
		if (!isDisplayed)
		{
			Debug.LogWarning("Not yet displayed, displaying squares: " + mapSquareInfoPoints.Count.ToString());
			foreach (MapSquareInfo squareInfo in mapSquareInfoPoints)
			{
				squareInfo.tile.GetComponent<TerrainTile>().Display();
			}
			isDisplayed = true;
		}
	}

	private bool FillFromPoint(TerrainMapPoint point)
	{
		Direction exclusionList = Direction.None;

		do
		{
			Direction nextDirection = GetRandomDirection(exclusionList);

			TerrainMapPoint nextPoint;
			if (terrain.TryGetEmptyMapPoint(point, nextDirection, out nextPoint))
			{
				MapSquareInfo squareInfo;
				if (terrain.TrySetPointForBiome(nextPoint, this, out squareInfo))
				{
					mapSquareInfoPoints.Add(squareInfo);
				}
				return true;
			}
			else
			{
				exclusionList |= nextDirection;
			}
		} while (exclusionList != Direction.AllDirections);

		return false;
	}

	private Direction GetRandomDirection(Direction exclusions)
	{
		Direction nextDirection = Direction.Up;

		do
		{
			switch (UnityEngine.Random.Range(0, 4))
			{
				case 0:
					nextDirection = Direction.Up;
					break;
				case 1:
					nextDirection = Direction.Down;
					break;
				case 2:
					nextDirection = Direction.Left;
					break;
				case 3:
					nextDirection = Direction.Right;
					break;
			}
		} while ((exclusions & nextDirection) != 0);

		return nextDirection;
	}

	public void FindAllNeighbors(TerrainMap map)
	{
		Dictionary<int, bool> neighborMap = new Dictionary<int, bool>();

		foreach(var square in mapSquareInfoPoints)
		{
			foreach(Direction dir in Enum.GetValues(typeof(Direction)))
			{
				MapSquareInfo neighboringPoint;
				if (map.TryGetSquareInfo(square.mapPoint, dir, out neighboringPoint))
				{
					if (neighboringPoint.biomeid != biomeID)
					{
						neighborMap[neighboringPoint.biomeid] = true;
					}
				}
			}
		}

		foreach(var neighborMapNode in neighborMap)
		{
			neighbors.Add(neighborMapNode.Key);
		}
	}

	public override int GetHashCode()
	{
		return thisBiomeID.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return thisBiomeID == ((Biome)obj).thisBiomeID;
	}
}
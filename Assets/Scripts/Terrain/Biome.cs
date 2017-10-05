using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Biome
{
	private TerrainType terrainType;
	private List<MapSquareInfo> mapSquareInfoPoints = new List<MapSquareInfo>();

	private TerrainMap terrain;

	public bool isDisplayed = false;

	/// <summary>
	/// Denotes when the Biome can no longer Flood Fills 
	/// </summary>
	public bool FloodComplete = false;

	private int floodFillIndex = 0;

	public void Initialize(TerrainType type, TerrainMapPoint initPoint, TerrainMap t)
	{
		terrain = t;
		terrainType = type;

		MapSquareInfo squareInfo;
		if (terrain.TrySetPointForBiome(initPoint, type, out squareInfo))
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
		terrainType = TerrainType.None;
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
			if (terrain.TryGetMapPoint(point, nextDirection, out nextPoint))
			{
				MapSquareInfo squareInfo;
				if (terrain.TrySetPointForBiome(nextPoint, terrainType, out squareInfo))
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
			switch (Random.Range(0, 4))
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
}
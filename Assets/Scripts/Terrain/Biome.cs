using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Biome
{
	private TerrainType terrainType;
	private List<TerrainMapPoint> mapPoints = new List<TerrainMapPoint>();

	private TerrainMap terrain;

	/// <summary>
	/// Denotes when the Biome can no longer Flood Fills 
	/// </summary>
	public bool FloodComplete = false;

	private int floodFillIndex = 0;

	public void Initialize(TerrainType type, TerrainMapPoint initPoint, TerrainMap t)
	{
		terrain = t;
		terrainType = type;
		terrain.SetPoint(initPoint, type);

		mapPoints.Add(initPoint);
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
			if (FillFromPoint(mapPoints[floodFillIndex]))
			{
				bSuccess = true;
			}
			else
			{
				floodFillIndex++;
			}
		} while (!bSuccess && (floodFillIndex < mapPoints.Count));

		if (!bSuccess)
		{
			FloodComplete = true;
		}
	}

	public void Reset()
	{
		terrainType = TerrainType.None;
		mapPoints.Clear();

		floodFillIndex = 0;
		FloodComplete = false;
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
				terrain.SetPoint(nextPoint, terrainType);
				mapPoints.Add(nextPoint);
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
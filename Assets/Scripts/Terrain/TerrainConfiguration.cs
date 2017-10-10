using System.Collections;
using UnityEngine;

[System.Serializable]
public class TerrainConfiguration 
{
	[System.Serializable]
	public class WorldSettings
	{
		public int width = 100;
		public int height = 100;
	}

	public WorldSettings World = new WorldSettings();

	[System.Serializable]
	public class BiomeSettings
	{
		[Tooltip("The amount a biome will fill per step.  Each biome, in turn, expands by this many tiles until all the tiles are full.")]
		public int floodFillIncrement = 1;

		[Tooltip("The number of different biome regions (colors)")]
		[Range(2, 5)]
		public int biomeTypeCount = 4;

		[Range(1, 1000)]
		public int biomeCount = 100;

		[Range(1, 10)]
		public int initialFloodFillAmount = 3;

		[Range(1, 100)]
		public int biomeSequencePerLevel = 10;
	}

	public BiomeSettings Biome = new BiomeSettings();
}

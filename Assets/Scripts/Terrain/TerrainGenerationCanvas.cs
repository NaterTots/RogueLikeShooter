using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerrainGenerationCanvas : MonoBehaviour
{
	public Slider biomeTypeSlider;
	public Text biomeTypeText;

	public Slider biomeCountSlider;
	public Text biomeCountText;

	public Slider floodFillIncrementSlider;
	public Text floodFillIncrementText;

	public Slider floodFillInitSlider;
	public Text floodFillInitText;

	public Slider biomeSequencePerLevelSlider;
	public Text biomeSequencePerLevelText;

	private TerrainMap terrain;

	void Start()
	{
		terrain = GameObject.Find("TerrainMap").GetComponent<TerrainMap>();
	}

	public void OnRedrawTerrain()
	{
		terrain.ResetTerrain();
		terrain.GenerateTerrain();
	}

	public void OnDisplayBiome()
	{
		terrain.DisplayNextBiome();
	}

	public void OnUIValueChanged()
	{
		int biomeTypeCount = (int)biomeTypeSlider.value;
		biomeTypeText.text = "Biome Type Count: " + biomeTypeCount.ToString();
		terrain.TerrainConfig.Biome.biomeTypeCount = biomeTypeCount;

		int biomeCount = (int)biomeCountSlider.value;
		biomeCountText.text = "Biome Count: " + biomeCount.ToString();
		terrain.TerrainConfig.Biome.biomeCount = biomeCount;

		int floodFillIncrement = (int)floodFillIncrementSlider.value;
		floodFillIncrementText.text = "Flood Fill Increment: " + floodFillIncrement.ToString();
		terrain.TerrainConfig.Biome.floodFillIncrement = floodFillIncrement;

		int floodFillInit = (int)floodFillInitSlider.value;
		floodFillInitText.text = "Flood Fill Init: " + floodFillInit.ToString();
		terrain.TerrainConfig.Biome.initialFloodFillAmount = floodFillInit;

		int biomeSequencePerLevel = (int)biomeSequencePerLevelSlider.value;
		biomeSequencePerLevelText.text = "Biome Sequence Per Level: " + biomeSequencePerLevel.ToString();
		terrain.TerrainConfig.Biome.biomeSequencePerLevel = biomeSequencePerLevel;
	}
}

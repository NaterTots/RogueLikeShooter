using UnityEngine;
using System.Collections;

public class TerrainTile : MonoBehaviour
{
	public const int Width = 50;
	public const int Height = 50;

	public Material grass;
	public Material lava;
	public Material forest;
	public Material rock;

	TerrainType terrainType;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Init(TerrainType type)
	{
		terrainType = type;

		switch (type)
		{
			case TerrainType.Forest:
				GetComponent<MeshRenderer>().material = forest;
				break;
			case TerrainType.Grass:
				GetComponent<MeshRenderer>().material = grass;
				break;
			case TerrainType.Lava:
				GetComponent<MeshRenderer>().material = lava;
				break;
			case TerrainType.Rock:
				GetComponent<MeshRenderer>().material = rock;
				break;
			case TerrainType.None:
				GetComponent<MeshRenderer>().material = null;
				break;
			case TerrainType.Empty:
				GetComponent<MeshRenderer>().material = null;
				break;
		}
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void Display()
	{
		gameObject.SetActive(true);
	}
}
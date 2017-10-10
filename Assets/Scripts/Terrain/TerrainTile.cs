using UnityEngine;
using System.Collections;

public class TerrainTile : MonoBehaviour
{
	public const int Width = 50;
	public const int Height = 50;

	public Sprite grass;
	public Sprite lava;
	public Sprite forest;
	public Sprite rock;
	public Sprite empty;

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
				GetComponent<SpriteRenderer>().sprite = forest;
				break;
			case TerrainType.Grass:
				GetComponent<SpriteRenderer>().sprite = grass;
				break;
			case TerrainType.Lava:
				GetComponent<SpriteRenderer>().sprite = lava;
				break;
			case TerrainType.Rock:
				GetComponent<SpriteRenderer>().sprite = rock;
				break;
			case TerrainType.None:
				GetComponent<SpriteRenderer>().sprite = empty;
				break;
			case TerrainType.Empty:
				GetComponent<SpriteRenderer>().sprite = empty;
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
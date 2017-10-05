using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCanvas : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnGoToGame()
	{
		GameController.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.Playing);
	}

	public void OnGoToTerrainSandbox()
	{
		GameController.GetController<GameStateEngine>().ChangeGameState(GameStateEngine.States.TerrainSandbox);
	}
}

using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
	public GameStateEngine.States startingState;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		GameController.GetController<GameStateEngine>().ChangeGameState(startingState);
		Destroy(this);
	}
}

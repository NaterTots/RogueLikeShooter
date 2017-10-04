using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
	public GameStateEngine.States startingState;

	public bool playBackgroundMusic = false;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (playBackgroundMusic)
		{
			AudioClip backgroundMusic;
			if (GameController.GetController<ResourceController>().TryGetMusic("testbackgroundmusic", out backgroundMusic))
			{
				GameController.GetController<SoundController>().PlayMusic(backgroundMusic);
			}
		}

		GameController.GetController<GameStateEngine>().ChangeGameState(startingState);
		Destroy(this);
	}
}

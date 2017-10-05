using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSceneController : MonoBehaviour , IPersistedController
{
	public string currentScene;

	void Awake()
	{
		GameController.AddController(this);
	}

	void Start()
	{
		GameController.GetController<GameStateEngine>().AddAllStateListener(OnGameStateChange);
	}

	void OnGameStateChange()
	{
		string nextScene = string.Empty;
		switch (GameController.GetController<GameStateEngine>().CurrentState)
		{
			case GameStateEngine.States.NullState:
				break;
			case GameStateEngine.States.Title:
				nextScene = "Title";
				break;
			case GameStateEngine.States.Playing:
				nextScene = "Game";
				break;
			case GameStateEngine.States.TerrainSandbox:
				nextScene = "TerrainSandbox";
				break;
			case GameStateEngine.States.GameOver:
				nextScene = "Title";
				break;
			case GameStateEngine.States.Credits:
				nextScene = "Credits";
				break;
			case GameStateEngine.States.Settings:
				nextScene = "Settings";
				break;
			default:
				Debug.LogError("Invalid state transition");
				break;
		}

		if (currentScene != string.Empty)
		{
			SceneManager.UnloadSceneAsync(currentScene);
		}

		if (nextScene != string.Empty)
		{
			SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
			currentScene = nextScene;
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
		}
	}
}
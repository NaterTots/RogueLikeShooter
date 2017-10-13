using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class GameStateEngine : MonoBehaviour, IPersistedController
{
	public enum States
	{
		NullState,
		Title,
		Playing,
		TerrainSandbox,
		NavMeshSandbox,
		PlayerSandbox,
		GameOver,
		Settings,
		Credits
	}

	#region Game State Engine

	private FiniteStateMachine _gameStateMachine = new FiniteStateMachine();

	public States CurrentState
	{
		get
		{
			return _gameStateMachine.GetCurrentState<States>();
		}
	}

	public void ChangeGameState(States newState)
	{
		Debug.Log("ChangeGameState: " + newState);
		_gameStateMachine.ChangeState<States>(newState);
	}

	public void AddListener(States state, UnityAction action, bool away)
	{
		_gameStateMachine.AddStateListener<States>(state, action, away);
	}

	public void AddAllStateListener(UnityAction action)
	{
		_gameStateMachine.AddTransitionListener(action);
	}

	#endregion Game State Engine

	#region MonoBehaviour

	void Awake()
	{
		_gameStateMachine.AddStates<States>(
			States.NullState,
			States.Title,
			States.Playing,
			States.TerrainSandbox,
			States.NavMeshSandbox,
			States.PlayerSandbox,
			States.GameOver,
			States.Settings,
			States.Credits);

		GameController.AddController(this);
	}

	#endregion MonoBehaviour
}
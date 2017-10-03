using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FiniteStateMachine
{
    private int currentState;

	private class StateEventGroup
	{
		public UnityEvent TransitionAway = new UnityEvent();
		public UnityEvent TransitionTo = new UnityEvent();
	}

	private Dictionary<int, StateEventGroup> stateCallbacks = new Dictionary<int, StateEventGroup>();
	private UnityEvent transitionCallback = new UnityEvent();

    #region Initialization

    public void AddStates(params int[] states)
    {
		foreach(int state in states)
		{
			stateCallbacks.Add(state, new StateEventGroup());
		}
    }

	public void AddStates<T>(params T[] states) where T : struct, IConvertible
	{
		foreach (T state in states)
		{
			stateCallbacks.Add(Convert.ToInt32(state), new StateEventGroup());
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="state"></param>
	/// <param name="action"></param>
	/// <param name="away">true is for the transitionAway event, false is for transitionTo</param>
	public void AddStateListener(int state, UnityAction action, bool away)
	{
		if (away)
		{
			stateCallbacks[state].TransitionAway.AddListener(action);
		}
		else
		{
			stateCallbacks[state].TransitionTo.AddListener(action);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="state"></param>
	/// <param name="action"></param>
	/// <param name="away">true is for the transitionAway event, false is for transitionTo</param>
	public void AddStateListener<T>(T state, UnityAction action, bool away) where T : struct, IConvertible
	{
		AddStateListener(Convert.ToInt32(state), action, away);
	}

	public void AddTransitionListener(UnityAction action)
	{
		transitionCallback.AddListener(action);
	}

    #endregion Initialization

    #region Getters

    public int CurrentState
    {
        get
        {
			return currentState;
        }
    }

    public T GetCurrentState<T>() where T : struct, IConvertible
    {
        return (T)Enum.ToObject(typeof(T), CurrentState);
    }

    #endregion Getters

    #region Changing and Executing States

    public bool ChangeState(int state)
    {
        bool bStateExists = false;

		if (stateCallbacks.ContainsKey(state) && currentState != state)
		{
			stateCallbacks[currentState].TransitionAway.Invoke();
			currentState = state;
			bStateExists = true;
			stateCallbacks[currentState].TransitionTo.Invoke();
			transitionCallback.Invoke();
		}

        return bStateExists;
    }

    public bool ChangeState<T>(T state) where T : struct, IConvertible
    {
        return ChangeState(Convert.ToInt32(state));
    }

    #endregion Changing and Executing States
}

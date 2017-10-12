﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour , IPersistedController
{
	void Awake()
	{
		GameController.AddController(this);
	}

	void Update()
	{
		foreach(KeyValuePair<KeyCode, UnityEvent> keyEvents in _keyCodeListeners)
		{
			if (Input.GetKeyDown(keyEvents.Key))
			{
				keyEvents.Value.Invoke();
			}
		}
	}

	public enum Axis
	{
		Horizontal,
		Vertical
	}

	public float GetAxis(Axis a)
	{
		switch (a)
		{
			case Axis.Horizontal:
				return Input.GetAxis("Horizontal");
			case Axis.Vertical:
				return Input.GetAxis("Vertical");
			default:
				return 0.0f;
		}
	}



	#region Move

	public bool IsAttemptSingleMove()
	{
		return Input.GetMouseButtonDown(0);
	}

	public bool IsSustainedMove()
	{
		return Input.GetMouseButton(1);
	}

	public Ray GetMoveAttemptRay()
	{
		return Camera.main.ScreenPointToRay(Input.mousePosition);
	}

	#endregion Move

	private Dictionary<KeyCode, UnityEvent> _keyCodeListeners = new Dictionary<KeyCode, UnityEvent>();

	public void AddKeyCodeListener(KeyCode code, UnityAction listener)
	{
		if (!_keyCodeListeners.ContainsKey(code))
		{
			_keyCodeListeners.Add(code, new UnityEvent());
		}

		_keyCodeListeners[code].AddListener(listener);
	}

	public void RemoveKeyCodeListener(KeyCode code, UnityAction listener)
	{
		if (_keyCodeListeners.ContainsKey(code))
		{
			_keyCodeListeners[code].RemoveListener(listener);
		}
	}
}

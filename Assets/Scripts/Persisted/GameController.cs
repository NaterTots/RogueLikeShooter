using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	#region Static dictionary and helper methods

	private static Dictionary<System.Type, IPersistedController> _controllers = new Dictionary<System.Type, IPersistedController>();

	public static void AddController(IPersistedController c)
	{
		// Only allow one object of each type
		if (!ContainsController(c.GetType()))
		{
			_controllers.Add(c.GetType(), c);
		}
	}

	public static void RemoveController(IPersistedController c)
	{
		if (ContainsController(c.GetType()))
		{
			_controllers.Remove(c.GetType());
		}
	}

	/// <summary>
	/// Adds the controller regardless of whether one of the
	/// same type already exists.  If one does already exist, it 
	/// will be removed before the new one is added.
	/// </summary>
	/// <param name="c"></param>
	public static void AddOrReplaceController<T>(T controller) where T : IPersistedController
	{
		if (ContainsController<T>())
		{
			_controllers.Remove(typeof(T));
		}
		AddController(controller);
	}

	public static bool ContainsController<T>() where T : IPersistedController
	{
		return _controllers.ContainsKey(typeof(T));
	}

	public static bool ContainsController(System.Type t)
	{
		return _controllers.ContainsKey(t);
	}

	public static T GetController<T>() where T : IPersistedController
	{
		return (T) _controllers[typeof(T)];
	}

	#endregion
}

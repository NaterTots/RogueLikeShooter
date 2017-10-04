using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConfigurationController : MonoBehaviour , IPersistedController 
{
	private const string DataFileName = "data";

	public bool loadFromPlayerPrefs = false;
	public bool saveToPlayerPrefs = true;

	Dictionary<string, Setting> _settings = new Dictionary<string, Setting>();

	private bool hasBeenLoaded = false;

	void Awake()
	{
		GameController.AddController(this);
	}

	// Use this for initialization
	void Start () 
	{
		if (!hasBeenLoaded)
		{
			LoadSettings();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	private void LoadSettings()
	{
		TextAsset dataFile = Resources.Load(DataFileName) as TextAsset;
		ConfigurationData configData = JsonUtility.FromJson<ConfigurationData>(dataFile.text);

		foreach (string configSettingName in Enum.GetNames(typeof(ConfigurationSettings)))
		{
			SettingData settingData;
			if (configData.TryGetSettingByName(configSettingName, out settingData))
			{
				if (loadFromPlayerPrefs && PlayerPrefs.HasKey(configSettingName))
				{
					GameController.GetController<ConfigurationController>().AddSetting(Setting.LoadFromPlayerPrefs(configSettingName, settingData.GetDataType()));
				}
				else
				{
					var newSetting = new Setting(settingData.name, settingData.value, settingData.GetDataType());
					newSetting.PersistAcrossSessions = true;
					GameController.GetController<ConfigurationController>().AddSetting(newSetting);
				}
			}
		}

		hasBeenLoaded = true;

		foreach (Setting s in GameController.GetController<ConfigurationController>().GetAllSettings())
		{
			Debug.LogWarning(s.Name + ": " + s.GetValueAsString());
		}
	}

	void OnDestroy()
	{
		if (saveToPlayerPrefs)
		{
			foreach (Setting s in GameController.GetController<ConfigurationController>().GetAllSettings())
			{
				s.SaveToPlayerPrefs();
			}
			PlayerPrefs.Save();
		}
	}

	public Setting AddSetting(string name, long value)
	{
		Setting newSetting = new Setting(name, value);
		_settings.Add(name, newSetting);
		return newSetting;
	}

	public Setting AddSetting(string name, string value)
	{
		Setting newSetting = new Setting(name, value);
		_settings.Add(name, newSetting);
		return newSetting;
	}

	public Setting AddSetting(string name, float value)
	{
		Setting newSetting = new Setting(name, value);
		_settings.Add(name, newSetting);
		return newSetting;
	}

	public void AddSetting(Setting s)
	{
		_settings.Add(s.Name, s);
	}

	public void RemoveSetting(string name)
	{
		_settings.Remove(name);
	}

	public bool Exists(string name)
	{
		if (!hasBeenLoaded)
		{
			LoadSettings();
		}

		return _settings.ContainsKey(name);
	}

	public Setting GetSetting(string name)
	{
		if (!hasBeenLoaded)
		{
			LoadSettings();
		}

		return _settings[name];
	}

	public bool Exists(ConfigurationSettings configSetting)
	{
		return Exists(Enum.GetName(typeof(ConfigurationSettings), configSetting));
	}

	public Setting GetSetting(ConfigurationSettings configSetting)
	{
		return GetSetting(Enum.GetName(typeof(ConfigurationSettings), configSetting));
	}

	public void AddListenerToSettingChanged(string name, UnityAction action)
	{
		_settings[name].AddChangedEventListener(action);
	}

	public IEnumerable<Setting> GetAllSettings()
	{
		foreach(KeyValuePair<string, Setting> s in _settings)
		{
			yield return s.Value;
		}
	}
}

/// <summary>
/// Not my best effort.  I think almost all settings will be one of these three types.  This is pretty clunky, especially if new
/// data types are required (especially complex data types).
/// </summary>
public class Setting
{
	public string Name { get; set; }
	public UnityEvent OnChangedEvent { get; set; }
	public SettingDataType DataType { get; set; }
	public bool PersistAcrossSessions { get; set; }

	private long? valueAsLong;
	private string valueAsString;
	private float? valueAsFloat;

	public enum SettingDataType
	{
		Long = 0,
		String = 1,
		Float = 2
	};

	public Setting(string name, long val)
	{
		Name = name;
		SetValue<long>(val);
	}

	public Setting(string name, string val)
	{
		Name = name;
		SetValue<string>(val);
	}

	public Setting(string name, string val, SettingDataType dataType)
	{
		Name = name;
		SetValue(val, dataType);
	}

	public Setting(string name, float val)
	{
		Name = name;
		SetValue<float>(val);
	}

	public void SetValue<T>(T initValue)
	{
		DataType = GetDataType(typeof(T));
		switch(DataType)
		{
			case SettingDataType.Float:
				valueAsFloat = initValue as float?;
				break;
			case SettingDataType.Long:
				valueAsLong = initValue as long?;
				break;
			case SettingDataType.String:
				valueAsString = initValue as string;
				break;
		}
		if (OnChangedEvent == null)
		{
			OnChangedEvent = new UnityEvent();
		}
		else
		{
			OnChangedEvent.Invoke();
		}
	}

	public void SetValue(string s, SettingDataType dataType)
	{
		DataType = dataType;
		switch(DataType)
		{
			case SettingDataType.Float:
				valueAsFloat = float.Parse(s);
				break;
			case SettingDataType.Long:
				valueAsLong = long.Parse(s);
				break;
			case SettingDataType.String:
				valueAsString = s;
				break;
		}
		if (OnChangedEvent == null)
		{
			OnChangedEvent = new UnityEvent();
		}
		else
		{
			OnChangedEvent.Invoke();
		}
	}

	public float GetValueAsFloat()
	{
		return valueAsFloat.GetValueOrDefault();
	}

	public long GetValueAsLong()
	{
		return valueAsLong.GetValueOrDefault();
	}

	public string GetValueAsString()
	{
		switch(DataType)
		{
			case SettingDataType.Float:
				return valueAsFloat.ToString();
			case SettingDataType.Long:
				return valueAsLong.ToString();
		}
		return valueAsString;
	}

	public long IncrementValueAsLong(long value)
	{
		valueAsLong = valueAsLong.GetValueOrDefault() + value;
		OnChangedEvent.Invoke();
		return valueAsLong.GetValueOrDefault();
	}

	public float IncrementValueAsFloat(float value)
	{
		valueAsFloat = valueAsFloat.GetValueOrDefault() + value;
		OnChangedEvent.Invoke();
		return valueAsFloat.GetValueOrDefault();
	}

	public void ResetValue()
	{
		valueAsFloat = 0.0f;
		valueAsLong = 0L;
		valueAsString = string.Empty;
		OnChangedEvent.Invoke();
	}

	public void AddChangedEventListener(UnityAction action)
	{
		OnChangedEvent.AddListener(action);
	}

	private SettingDataType GetDataType(System.Type t)
	{
		if (t == typeof(long) ||
			t == typeof(int))
		{
			return SettingDataType.Long;
		}
		else if (t == typeof(string))
		{
			return SettingDataType.String;
		}
		else if (t == typeof(float) ||
				 t == typeof(double))
		{
			return SettingDataType.Float;
		}

		throw new System.Exception("Setting is not a valid data type");
	}

	public override int GetHashCode()
	{
		return Name.GetHashCode();
	}

	public void SaveToPlayerPrefs()
	{
		switch (DataType)
		{
			case SettingDataType.Long:
				PlayerPrefs.SetInt(Name, (int)valueAsLong.GetValueOrDefault());
				break;
			case SettingDataType.Float:
				PlayerPrefs.SetFloat(Name, valueAsFloat.GetValueOrDefault());
				break;
			case SettingDataType.String:
				PlayerPrefs.SetString(Name, valueAsString);
				break;
		}
	}

	public static Setting LoadFromPlayerPrefs(string name, SettingDataType type, bool persistAcrossSessions = true)
	{
		Setting newSetting = null;
		switch(type)
		{
			case SettingDataType.Long:
				newSetting = new Setting(name, PlayerPrefs.GetInt(name));
				break;
			case SettingDataType.String:
				newSetting = new Setting(name, PlayerPrefs.GetString(name));
				break;
			case SettingDataType.Float:
				newSetting = new Setting(name, PlayerPrefs.GetFloat(name));
				break;
		}
		newSetting.PersistAcrossSessions = persistAcrossSessions;
		return newSetting;
	}
}
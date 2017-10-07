using UnityEngine;
using System.Collections;
using System;


public enum ConfigurationSettings
{
	FullScreen,
	SoundEffectsVolume,
	MusicVolume
}

[Serializable]
public class ConfigurationData
{
	public SettingData[] settings;
	public TerrainConfiguration terrainconfiguration;
	public PlayerConfiguration playerconfiguration;

	public bool TryGetSettingByName(string name, out SettingData setting)
	{
		foreach(SettingData s in settings)
		{
			if (s.name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
			{
				setting = s;
				return true;
			}
		}

		setting = null;
		return false;
	}
}

[Serializable]
public class SettingData
{
	public string name;
	public int datatype;
	public string value;

	public Setting.SettingDataType GetDataType()
	{
		return (Setting.SettingDataType)(datatype);
	}
}


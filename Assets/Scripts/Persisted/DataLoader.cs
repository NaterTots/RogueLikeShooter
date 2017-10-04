using UnityEngine;
using System;
using System.Collections;

public class DataLoader : MonoBehaviour
{
	private const string DataFileName = "data";

	public bool loadFromPlayerPrefs = false;
	public bool saveToPlayerPrefs = true;

	void Start()
	{
		TextAsset dataFile = Resources.Load(DataFileName) as TextAsset;
		ConfigurationData configData = JsonUtility.FromJson<ConfigurationData>(dataFile.text);
		
		foreach(string configSettingName in Enum.GetNames(typeof(ConfigurationSettings)))
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
					GameController.GetController<ConfigurationController>().AddSetting(new Setting(settingData.name, settingData.value, settingData.GetDataType()));
				}
			}
		}

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
}

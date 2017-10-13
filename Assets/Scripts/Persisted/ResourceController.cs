using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceController : MonoBehaviour , IPersistedController
{
	AssetBundle spriteAssetBundle;
	AssetBundle soundEffectsAssetBundle;
	AssetBundle musicAssetBundle;
	AssetBundle prefabAssetBundle;

	static string SpritesAssetBundlePath = "spriteassetbundle";
	static string SoundEffectsBundlePath = "soundeffectsassetbundle";
	static string MusicAssetBundlePath = "musicassetbundle";
	static string PrefabAssetBundlePath = "prefabassetbundle";

	void Awake()
	{
		GameController.AddController(this);
	}

	#region Visuals

	public bool TryGetSprite(string name, out Sprite value)
	{
		if (spriteAssetBundle == null)
		{
			spriteAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, SpritesAssetBundlePath));
		}

		value = spriteAssetBundle.LoadAsset<Sprite>(name);

		return (value != null);
	}

	public bool TryGetPrefab(string name, out GameObject prefab)
	{
		if (prefabAssetBundle == null)
		{
			prefabAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, PrefabAssetBundlePath));
		}

		prefab = prefabAssetBundle.LoadAsset<GameObject>(name);

		return (prefab != null);
	}

	#endregion Visuals

	#region Audio

	public bool TryGetSoundEffect(string name, out AudioClip value)
	{
		if (soundEffectsAssetBundle == null)
		{
			soundEffectsAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, SoundEffectsBundlePath));
		}

		value = soundEffectsAssetBundle.LoadAsset<AudioClip>(name);

		return (value != null);
	}

	public bool TryGetMusic(string name, out AudioClip value)
	{
		if (musicAssetBundle == null)
		{
			musicAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, MusicAssetBundlePath));
		}

		value = musicAssetBundle.LoadAsset<AudioClip>(name);

		return (value != null);
	}

	#endregion Audio
}

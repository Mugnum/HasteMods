using Landfall.Modding;
using Mugnum.HasteMods.AutoSave.Settings;
using UnityEngine.SceneManagement;

namespace Mugnum.HasteMods.AutoSave;

/// <summary>
/// Autosave mod.
/// </summary>
[LandfallPlugin]
public class AutoSave
{
	/// <summary>
	/// Counter for skipped saves.
	/// </summary>
	private static int SkippedSavesCount;

	/// <summary>
	/// List of ignored scenes.
	/// </summary>
	private static readonly string[] IgnoredScenes =
	{
		"DialogueScene",
		"MainMenu",
		"ShopScene",
		"RestScene_Current",
		"EndlessAwardScene",
		"PostGameScene",
		"FullHub",
		"FullHub Cutscene",
		"LevelSelection_Art",
		"Credits",
		"TutorialScene",
		"OpeningCutscene_Remake"
	};

	/// <summary>
	/// Constructor.
	/// </summary>
	static AutoSave()
	{
		SceneManager.sceneLoaded -= SaveGameOnSceneLoad;
		SceneManager.sceneLoaded += SaveGameOnSceneLoad;
	}

	/// <summary>
	/// Resets save counter.
	/// </summary>
	public static void ResetSkippedSavesCounter()
	{
		SkippedSavesCount = 0;
	}

	/// <summary>
	/// On scene loaded event handler.
	/// </summary>
	/// <param name="scene"> Scene. </param>
	/// <param name="loadMode"> Load scene mode. </param>
	private static void SaveGameOnSceneLoad(Scene scene, LoadSceneMode loadMode)
	{
		if (!RunHandler.InRun || IgnoredScenes.Contains(scene.name, StringComparer.OrdinalIgnoreCase))
		{
			return;
		}

		const int SaveEveryLevel = 1;
		var sceneSpacingSetting = GameHandler.Instance.SettingsHandler.GetSetting<AutoSaveSpacingSetting>().Value;

		if (sceneSpacingSetting <= SaveEveryLevel)
		{
			SaveSystem.Save();
			return;
		}
		if (++SkippedSavesCount < sceneSpacingSetting)
		{
			return;
		}

		SkippedSavesCount = 0;
		SaveSystem.Save();
	}
}

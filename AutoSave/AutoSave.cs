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
	public static int SkippedSavesCount;

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
	/// <param name="arg0"> Scene. </param>
	/// <param name="arg1"> Load scene mode. </param>
	private static void SaveGameOnSceneLoad(Scene arg0, LoadSceneMode arg1)
	{
		if (!RunHandler.InRun || IgnoredScenes.Contains(arg0.name, StringComparer.OrdinalIgnoreCase))
		{
			return;
		}

		var sceneSpacingSetting = GameHandler.Instance.SettingsHandler.GetSetting<AutoSaveSpacingSetting>().Value;

		if (sceneSpacingSetting <= 1)
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

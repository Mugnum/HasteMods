using HarmonyLib;
using Mugnum.HasteMods.MoreUpscalingOptions.Settings;
using UnityEngine;
using Zorro.Settings;

// ReSharper disable InconsistentNaming
namespace Mugnum.HasteMods.MoreUpscalingOptions.Patches;

/// <summary>
/// Patch for <see cref="HasteSettingsHandler"/> class.
/// </summary>
[HarmonyPatch(typeof(HasteSettingsHandler))]
internal class HasteSettingsHandlerPatch
{
	/// <summary>
	/// Postfix for <see cref="HasteSettingsHandler"/> constructor.
	/// </summary>
	/// <param name="___settings"> "settings" private field. </param>
	/// <param name="____settingsSaveLoad"> "_settingsSaveLoad" private vield. </param>
	[HarmonyPatch(typeof(HasteSettingsHandler), MethodType.Constructor)]
	[HarmonyPostfix]
	internal static void Postfix(List<Setting> ___settings, ISettingsSaveLoad ____settingsSaveLoad)
	{
		var upscalingModeExists = ___settings.Any(s => s is UpscalingSetting);

		if (!upscalingModeExists
			|| ___settings.FirstOrDefault(s => s is UpscalingQualitySetting) is not UpscalingQualitySetting upscalingQualityOldSetting)
		{
			Debug.LogError($"{nameof(HasteSettingsHandlerPatch)} constructor. Cannot find settings: " +
				$"{nameof(UpscalingSetting)}, {nameof(UpscalingQualitySetting)}.");
			return;
		}

		// Replacing existing "Upscaling Quality" setting with extended variant.
		// Old setting is kept in memory to satisfy UpscalingSetting's constructor.
		ReplaceSetting<UpscalingQualitySetting>(___settings, ____settingsSaveLoad, new ExtendedUpscalingQualitySetting());

		// Jank: re-initializing UpscalingSetting after original HasteSettingsHandler constructor
		// to later access newly created ExtendedUpscalingQualitySetting.
		ReplaceSetting<UpscalingSetting>(___settings, ____settingsSaveLoad, new UpscalingSetting(upscalingQualityOldSetting), true);
	}

	/// <summary>
	/// Replaces existing setting.
	/// </summary>
	/// <typeparam name="TOldType"> Setting type, which needs to be replaced. </typeparam>
	/// <param name="settings"> Settings. </param>
	/// <param name="saveLoad"> Save/load manager. </param>
	/// <param name="newSetting"> New setting. </param>
	/// <param name="isDisposing"> Is need to dispose old setting. </param>
	private static void ReplaceSetting<TOldType>(List<Setting>? settings, ISettingsSaveLoad saveLoad, Setting? newSetting, bool isDisposing = false)
		where TOldType : Setting
	{
		if (settings == null || newSetting == null)
		{
			return;
		}

		var oldSettingIndex = settings.FindIndex(s => s is TOldType);
		if (oldSettingIndex <= 0)
		{
			return;
		}

		var oldSetting = settings[oldSettingIndex];
		settings[oldSettingIndex] = newSetting;
		newSetting.Load(saveLoad);
		newSetting.ApplyValue();

		if (isDisposing)
		{
			oldSetting.Dispose();
		}
	}
}

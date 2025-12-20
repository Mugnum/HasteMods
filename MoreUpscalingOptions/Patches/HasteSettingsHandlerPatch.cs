using Mugnum.HasteMods.MoreUpscalingOptions.Common;
using Mugnum.HasteMods.MoreUpscalingOptions.Settings;
using UnityEngine;
using Zorro.Settings;

// ReSharper disable InconsistentNaming
namespace Mugnum.HasteMods.MoreUpscalingOptions.Patches;

/// <summary>
/// Patch for <see cref="HasteSettingsHandler"/> class.
/// </summary>
internal static class HasteSettingsHandlerPatch
{
	/// <summary>
	/// "settings" private field getter from <see cref="HasteSettingsHandler"/> class.
	/// </summary>
	private static readonly Func<HasteSettingsHandler, List<Setting>> SettingsGetter
		= AccessorHelper.CreateFieldGetter<HasteSettingsHandler, List<Setting>>("settings");

	/// <summary>
	/// "settingsSaveLoad" private field getter from <see cref="HasteSettingsHandler"/> class.
	/// </summary>
	private static readonly Func<HasteSettingsHandler, ISettingsSaveLoad> SettingsSaveLoadGetter
		= AccessorHelper.CreateFieldGetter<HasteSettingsHandler, ISettingsSaveLoad>("settingsSaveLoad");

	/// <summary>
	/// Handler for <see cref="HasteSettingsHandler"/>> contructor patch.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	internal static void OnConstructor(On.HasteSettingsHandler.orig_ctor orig, HasteSettingsHandler self)
	{
		var settings = SettingsGetter(self);
		var settingsSaveLoad = SettingsSaveLoadGetter(self);

		orig(self);
		var upscalingModeExists = settings.Any(s => s is UpscalingSetting);

		if (!upscalingModeExists
			|| settings.FirstOrDefault(s => s is UpscalingQualitySetting) is not UpscalingQualitySetting upscalingQualityOldSetting)
		{
			Debug.LogError($"{nameof(HasteSettingsHandlerPatch)} constructor. Cannot find settings: " +
				$"{nameof(UpscalingSetting)}, {nameof(UpscalingQualitySetting)}.");
			return;
		}

		// Replacing existing "Upscaling Quality" setting with extended variant.
		// Old setting is kept in memory to satisfy UpscalingSetting's constructor.
		ReplaceSetting<UpscalingQualitySetting>(settings, settingsSaveLoad, new ExtendedUpscalingQualitySetting());

		// Jank: re-initializing UpscalingSetting after original HasteSettingsHandler constructor
		// to later access newly created ExtendedUpscalingQualitySetting.
		ReplaceSetting<UpscalingSetting>(settings, settingsSaveLoad, new UpscalingSetting(upscalingQualityOldSetting), true);
	}

	/// <summary>
	/// Replaces existing setting.
	/// </summary>
	/// <typeparam name="TOldType"> Setting type, which needs to be replaced. </typeparam>
	/// <param name="settings"> Settings. </param>
	/// <param name="saveLoad"> Save/load manager. </param>
	/// <param name="newSetting"> New setting. </param>
	/// <param name="isDisposing"> Is need to dispose old setting. </param>
	private static void ReplaceSetting<TOldType>(List<Setting>? settings, ISettingsSaveLoad saveLoad, 
		Setting? newSetting, bool isDisposing = false)
		where TOldType : Setting
	{
		if (settings == null || newSetting == null)
		{
			return;
		}

		var oldSettingIndex = settings.FindIndex(s => s is TOldType);
		if (oldSettingIndex < 0)
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

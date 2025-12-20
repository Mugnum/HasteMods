using Mugnum.HasteMods.MoreUpscalingOptions.Settings;
using Zorro.Settings;

namespace Mugnum.HasteMods.MoreUpscalingOptions.Patches;

/// <summary>
/// Patch for <see cref="HasteSettingsHandler"/> class.
/// </summary>
internal static class HasteSettingsHandlerPatch
{
	/// <summary>
	/// Handler for <see cref="HasteSettingsHandler.AddSetting"/> method.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	/// <param name="setting"> Added setting. </param>
	internal static void OnAddSetting(On.HasteSettingsHandler.orig_AddSetting orig, HasteSettingsHandler self, Setting setting)
	{
		setting = setting switch
		{
			UpscalingQualitySetting => new ExtendedUpscalingQualitySetting(),
			_ => setting
		};

		orig(self, setting);
	}
}

using Landfall.Haste;
using UnityEngine.Localization;

namespace Mugnum.HasteMods.MoreUpscalingOptions.Patches;

/// <summary>
/// Patch for <see cref="UpscalingQualitySetting"/> class.
/// </summary>
internal static class UpscalingQualitySettingPatch
{
	/// <summary>
	/// Handler for <see cref="UpscalingQualitySetting.GetDisplayName"/> method.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	/// <returns> Display value. </returns>
	internal static LocalizedString OnGetDisplayName(On.UpscalingQualitySetting.orig_GetDisplayName orig, UpscalingQualitySetting self)
	{
		return new UnlocalizedString("Upscaling Quality (Disabled)");
	}
}

using HarmonyLib;
using Landfall.Haste;
using UnityEngine.Localization;

// ReSharper disable InconsistentNaming
namespace Mugnum.HasteMods.MoreUpscalingOptions.Patches;

/// <summary>
/// Patch for <see cref="UpscalingQualitySetting"/> class.
/// </summary>
[HarmonyPatch(typeof(UpscalingQualitySetting))]
internal class UpscalingQualitySettingPatch
{
	/// <summary>
	/// Patch for "GetDisplayName" method.
	/// </summary>
	/// <param name="__result"> Result value. </param>
	[HarmonyPatch(nameof(UpscalingQualitySetting.GetDisplayName))]
	[HarmonyPostfix]
	internal static void GetDisplayNamePostfix(ref LocalizedString __result)
	{
		__result = new UnlocalizedString("Upscaling Quality (Disabled)");
	}
}

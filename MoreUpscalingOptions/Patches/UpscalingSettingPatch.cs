using HarmonyLib;
using Mugnum.HasteMods.MoreUpscalingOptions.Settings;
using SCPE;
using TND.DLSS;
using TND.FSR;
using TND.XeSS;
using UpscalingQuality = Mugnum.HasteMods.MoreUpscalingOptions.Settings.UpscalingQuality;

// ReSharper disable InconsistentNaming
namespace Mugnum.HasteMods.MoreUpscalingOptions.Patches;

/// <summary>
/// Patch for "UpscalingSetting" class.
/// </summary>
[HarmonyPatch(typeof(UpscalingSetting))]
internal class UpscalingSettingPatch
{
	/// <summary>
	/// Extended upscaling quality setting.
	/// </summary>
	private static ExtendedUpscalingQualitySetting? _extendedUpscalingQualitySetting;

	/// <summary>
	/// Extended upscaling quality setting.
	/// </summary>
	private static ExtendedUpscalingQualitySetting? ExtendedUpscalingQualitySetting
	{
		get
		{
			return _extendedUpscalingQualitySetting ??= GameHandler.Instance.SettingsHandler.GetSetting<ExtendedUpscalingQualitySetting>();
		}
	}

	/// <summary>
	/// Patch for "GetDLSSQualityMode" method.
	/// </summary>
	/// <param name="__result"> Result value. </param>
	[HarmonyPatch("GetDLSSQualityMode")]
	[HarmonyPostfix]
	internal static void GetDLSSQualityModePostfix(ref DLSS_Quality __result)
	{
		if (ExtendedUpscalingQualitySetting == null)
		{
			return;
		}

		__result = ExtendedUpscalingQualitySetting.Value switch
		{
			UpscalingQuality.UltraPerformance => DLSS_Quality.UltraPerformance,
			UpscalingQuality.Performance => DLSS_Quality.Performance,
			UpscalingQuality.Balanced => DLSS_Quality.Balanced,
			UpscalingQuality.Quality => DLSS_Quality.Quality,
			UpscalingQuality.UltraQuality => DLSS_Quality.UltraQuality,
			UpscalingQuality.Native => DLSS_Quality.NativeAA,
			_ => DLSS_Quality.Quality
		};
	}

	/// <summary>
	/// Patch for "GetFSRQualityMode" method.
	/// </summary>
	/// <param name="__result"> Result value. </param>
	[HarmonyPatch("GetFSRQualityMode")]
	[HarmonyPostfix]
	internal static void GetFSRQualityModePostfix(ref FSR3_Quality __result)
	{
		if (ExtendedUpscalingQualitySetting == null)
		{
			return;
		}

		__result = ExtendedUpscalingQualitySetting.Value switch
		{
			UpscalingQuality.UltraPerformance => FSR3_Quality.UltraPerformance,
			UpscalingQuality.Performance => FSR3_Quality.Performance,
			UpscalingQuality.Balanced => FSR3_Quality.Balanced,
			UpscalingQuality.Quality => FSR3_Quality.Quality,
			UpscalingQuality.UltraQuality => FSR3_Quality.UltraQuality,
			UpscalingQuality.Native => FSR3_Quality.NativeAA,
			_ => FSR3_Quality.Quality
		};
	}

	/// <summary>
	/// Patch for "GetXeSSQualityMode" method.
	/// </summary>
	/// <param name="__result"> Result value. </param>
	[HarmonyPatch("GetXeSSQualityMode")]
	[HarmonyPostfix]
	internal static void GetXeSSQualityModePostfix(ref XeSS_Quality __result)
	{
		if (ExtendedUpscalingQualitySetting == null)
		{
			return;
		}

		__result = ExtendedUpscalingQualitySetting.Value switch
		{
			UpscalingQuality.UltraPerformance => XeSS_Quality.UltraPerformance,
			UpscalingQuality.Performance => XeSS_Quality.Performance,
			UpscalingQuality.Balanced => XeSS_Quality.Balanced,
			UpscalingQuality.Quality => XeSS_Quality.Quality,
			UpscalingQuality.UltraQuality => XeSS_Quality.UltraQuality,
			UpscalingQuality.Native => XeSS_Quality.NativeAA,
			_ => XeSS_Quality.Quality
		};
	}

	/// <summary>
	/// Patch for "ConfigureOutline" method.
	/// </summary>
	/// <param name="outline"> Edge detection outline. </param>
	/// <param name="__instance"> <see href="UpscalingSetting"/> instance. </param>
	[HarmonyPatch(nameof(UpscalingSetting.ConfigureOutline))]
	[HarmonyPostfix]
	internal static void ConfigureOutline(EdgeDetection outline, UpscalingSetting __instance)
	{
		if (__instance.Value == UpscalingSetting.UpscalingMode.Off
			|| ExtendedUpscalingQualitySetting == null
			|| outline.edgeOpacity == null)
		{
			return;
		}

		outline.edgeOpacity.value = ExtendedUpscalingQualitySetting.Value switch
		{
			UpscalingQuality.UltraPerformance => 0.3f,
			UpscalingQuality.Performance => 0.45f,
			UpscalingQuality.Balanced => 0.5f,
			UpscalingQuality.Quality => 0.6f,
			UpscalingQuality.UltraQuality => 0.75f,
			UpscalingQuality.Native => 0.9f,
			_ => 1f
		};
	}
}

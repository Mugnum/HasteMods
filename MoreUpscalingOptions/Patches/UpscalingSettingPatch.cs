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
internal static class UpscalingSettingPatch
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
	/// Handler for <see cref="UpscalingSetting.GetDLSSQualityMode"/> method.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	/// <returns> DLSS quality. </returns>
	internal static DLSS_Quality OnGetDLSSQualityMode(On.UpscalingSetting.orig_GetDLSSQualityMode orig, UpscalingSetting self)
	{
		if (ExtendedUpscalingQualitySetting == null)
		{
			return orig(self);
		}

		return ExtendedUpscalingQualitySetting.Value switch
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
	/// Handler for <see cref="UpscalingSetting.GetFSRQualityMode"/> method.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	/// <returns> FSR3 quality. </returns>
	internal static FSR3_Quality OnGetFSRQualityMode(On.UpscalingSetting.orig_GetFSRQualityMode orig, UpscalingSetting self)
	{
		if (ExtendedUpscalingQualitySetting == null)
		{
			return orig(self);
		}

		return ExtendedUpscalingQualitySetting.Value switch
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
	/// Handler for <see cref="UpscalingSetting.GetXeSSQualityMode"/> method.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	/// <returns> XeSS quality. </returns>
	internal static XeSS_Quality OnGetXeSSQualityMode(On.UpscalingSetting.orig_GetXeSSQualityMode orig, UpscalingSetting self)
	{
		if (ExtendedUpscalingQualitySetting == null)
		{
			return orig(self);
		}

		return ExtendedUpscalingQualitySetting.Value switch
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
	/// Handler for <see cref="UpscalingSetting.ConfigureOutline"/> method.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	/// <param name="outline"> Outline. </param>
	internal static void OnConfigureOutline(On.UpscalingSetting.orig_ConfigureOutline orig, UpscalingSetting self, EdgeDetection outline)
	{
		if (self.Value == UpscalingSetting.UpscalingMode.Off
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

using Landfall.Modding;
using Mugnum.HasteMods.MoreUpscalingOptions.Patches;

namespace Mugnum.HasteMods.MoreUpscalingOptions;

/// <summary>
/// More Upscaling Options mod.
/// </summary>
[LandfallPlugin]
public class MoreUpscalingOptions
{
	/// <summary>
	/// Constructor.
	/// </summary>
	static MoreUpscalingOptions()
	{
		On.UpscalingSetting.GetDLSSQualityMode += UpscalingSettingPatch.OnGetDLSSQualityMode;
		On.UpscalingSetting.GetFSRQualityMode += UpscalingSettingPatch.OnGetFSRQualityMode;
		On.UpscalingSetting.GetXeSSQualityMode += UpscalingSettingPatch.OnGetXeSSQualityMode;
		On.UpscalingSetting.ConfigureOutline += UpscalingSettingPatch.OnConfigureOutline;
		On.UpscalingQualitySetting.GetDisplayName += UpscalingQualitySettingPatch.OnGetDisplayName;
		On.HasteSettingsHandler.ctor += HasteSettingsHandlerPatch.OnConstructor;
	}
}

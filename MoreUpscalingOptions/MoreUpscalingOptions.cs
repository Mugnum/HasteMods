using HarmonyLib;
using Landfall.Modding;

namespace Mugnum.HasteMods.MoreUpscalingOptions;

/// <summary>
/// More Upscaling Options mod.
/// </summary>
[LandfallPlugin]
public class MoreUpscalingOptions
{
	/// <summary>
	/// Harmony plugin guid.
	/// </summary>
	private const string PluginGuid = $"Mugnum.{nameof(MoreUpscalingOptions)}";

	/// <summary>
	/// Constructor.
	/// </summary>
	static MoreUpscalingOptions()
	{
		new Harmony(PluginGuid).PatchAll();
	}
}

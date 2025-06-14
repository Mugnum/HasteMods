using HarmonyLib;
using Landfall.Modding;

namespace Mugnum.HasteMods.HighSpeedFovLimit;

/// <summary>
/// High speed FOV limit mod.
/// </summary>
[LandfallPlugin]
public class HighSpeedFovLimit
{
	/// <summary>
	/// Harmony plugin guid.
	/// </summary>
	private const string PluginGuid = "Mugnum.HighSpeedFovLimit";

	/// <summary>
	/// Constructor.
	/// </summary>
	static HighSpeedFovLimit()
	{
		new Harmony(PluginGuid).PatchAll();
	}
}

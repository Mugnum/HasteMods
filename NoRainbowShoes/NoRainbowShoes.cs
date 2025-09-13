using HarmonyLib;
using Landfall.Modding;

namespace Mugnum.HasteMods.NoRainbowShoes;

/// <summary>
/// No rainbow shoes mod.
/// </summary>
[LandfallPlugin]
public class NoRainbowShoes
{
	/// <summary>
	/// Harmony plugin guid.
	/// </summary>
	private const string PluginGuid = "Mugnum.NoRainbowShoes";

	/// <summary>
	/// Constructor.
	/// </summary>
	static NoRainbowShoes()
	{
		new Harmony(PluginGuid).PatchAll();
	}
}

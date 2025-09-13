using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming
namespace Mugnum.HasteMods.NoRainbowShoes.Patches;

/// <summary>
/// Patch for <see cref="VFX_BoostShoes"/> class.
/// </summary>
[HarmonyPatch(typeof(VFX_BoostShoes))]
internal class VfxBoostShoesPatch
{
	/// <summary>
	/// Postfix for "Update" method.
	/// </summary>
	/// <param name="___player"> "player" private field. </param>
	/// <param name="___gradient"> "gradient" private field. </param>
	/// <param name="___rend"> "rend" private field. </param>
	[HarmonyPatch("Update")]
	[HarmonyPostfix]
	internal static void UpdatePostfix(PlayerCharacter ___player, Gradient ___gradient, MeshRenderer ___rend)
	{
		const float FullBright = 1f;
		const string MaterialColorName = "_Color2";
		var boost = Math.Min(___player.data.GetBoost(), FullBright);
		var color = ___gradient.Evaluate(boost);
		___rend.material.SetColor(MaterialColorName, color);
	}
}

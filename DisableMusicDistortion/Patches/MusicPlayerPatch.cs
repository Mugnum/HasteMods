using HarmonyLib;
using Landfall.Haste.Music;

// ReSharper disable InconsistentNaming
namespace Mugnum.HasteMods.DisableMusicDistortion.Patches;

/// <summary>
/// Patch for "MusicPlayer" class.
/// </summary>
[HarmonyPatch(typeof(MusicPlayer))]
internal class MusicPlayerPatch
{
	/// <summary>
	/// Patch for "SetLevelDeath" method.
	/// </summary>
	/// <param name="b"> Input value. </param>
	/// <param name="___isLevelDeath"> "isLevelDeath" private field. </param>
	[HarmonyPatch(nameof(MusicPlayer.SetLevelDeath))]
	[HarmonyPostfix]
	private static void SetLevelDeathPostfix(bool b, ref bool ___isLevelDeath)
	{
		___isLevelDeath = false;
	}
}

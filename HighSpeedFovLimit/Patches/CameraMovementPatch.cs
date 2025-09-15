using HarmonyLib;
using Mugnum.HasteMods.HighSpeedFovLimit.Settings;

// ReSharper disable InconsistentNaming
namespace Mugnum.HasteMods.HighSpeedFovLimit.Patches;

/// <summary>
/// Patch for "CameraMovement" class.
/// </summary>
[HarmonyPatch(typeof(CameraMovement))]
internal class CameraMovementPatch
{
	/// <summary>
	/// Max FOV.
	/// </summary>
	public static float MaxFov = GameHandler.Instance.SettingsHandler.GetSetting<MaxFovSetting>().Value;

	/// <summary>
	/// Postfix for "Update" method.
	/// </summary>
	/// <param name="___cam"> "cam" private field. </param>
	[HarmonyPatch("Update")]
	[HarmonyPostfix]
	internal static void UpdatePostfix(MainCamera ___cam)
	{
		___cam.cam.fieldOfView = Math.Min(___cam.cam.fieldOfView, MaxFov);
	}
}

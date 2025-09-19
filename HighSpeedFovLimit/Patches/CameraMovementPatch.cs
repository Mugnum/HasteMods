using HarmonyLib;
using Mugnum.HasteMods.HighSpeedFovLimit.Settings;

// ReSharper disable InconsistentNaming
namespace Mugnum.HasteMods.HighSpeedFovLimit.Patches;

/// <summary>
/// Patch for "CameraMovement" class.
/// </summary>
[HarmonyPatch(typeof(CameraMovement))]
internal static class CameraMovementPatch
{
	/// <summary>
	/// Max fov value.
	/// </summary>
	public static float MaxFov = MaxFovSetting.DefaultValue;

	/// <summary>
	/// Postfix for "Update" method.
	/// </summary>
	/// <param name="___cam"> "cam" private field. </param>
	[HarmonyPatch("Update")]
	[HarmonyPostfix]
	internal static void UpdatePostfix(MainCamera ___cam)
	{
		if (MaxFov > 0)
		{
			___cam.cam.fieldOfView = Math.Min(___cam.cam.fieldOfView, MaxFov);
		}
	}
}

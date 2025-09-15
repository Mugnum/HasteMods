using Landfall.Haste;
using Mugnum.HasteMods.HighSpeedFovLimit.Patches;
using Unity.Mathematics;
using UnityEngine.Localization;
using Zorro.Settings;

namespace Mugnum.HasteMods.HighSpeedFovLimit.Settings;

/// <summary>
/// Max FOV setting.
/// </summary>
[HasteSetting]
public class MaxFovSetting : FloatSetting, IExposedSetting
{
	/// <summary>
	/// Process value change.
	/// </summary>
	public override void ApplyValue()
	{
		CameraMovementPatch.MaxFov = Value;
	}

	/// <summary>
	/// Returns default value.
	/// </summary>
	/// <returns> Default value. </returns>
	protected override float GetDefaultValue() => 120;

	/// <summary>
	/// Returns setting boundaries.
	/// </summary>
	/// <returns> Min and max values. </returns>
	protected override float2 GetMinMaxValue() => new(20, 180);

	/// <summary>
	/// Returns display name.
	/// </summary>
	/// <returns> Display name. </returns>
	public LocalizedString GetDisplayName() => new UnlocalizedString("Max FOV");

	/// <summary>
	/// Returns category.
	/// </summary>
	/// <returns> Category. </returns>
	public string GetCategory() => "Mods";
}

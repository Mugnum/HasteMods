using Landfall.Haste;
using Unity.Mathematics;
using UnityEngine.Localization;
using Zorro.Settings;

namespace Mugnum.HasteMods.FramerateLimiter.Settings;

/// <summary>
/// Background FPS limit setting.
/// </summary>
public class BackgroundFpsLimitSetting : FloatSetting, IExposedSetting
{
	/// <summary>
	/// Default value.
	/// </summary>
	internal const int DefaultValue = 30;

	/// <summary>
	/// Min FPS value.
	/// </summary>
	internal const int MinFps = 10;

	/// <summary>
	/// Max FPS value.
	/// </summary>
	internal const int MaxFps = 500;

	/// <summary>
	/// Process value change.
	/// </summary>
	public override void ApplyValue()
	{
	}

	/// <summary>
	/// Returns default value.
	/// </summary>
	/// <returns> Default value. </returns>
	protected override float GetDefaultValue() => DefaultValue;

	/// <summary>
	/// Returns setting boundaries.
	/// </summary>
	/// <returns> Min and max values. </returns>
	protected override float2 GetMinMaxValue() => new(MinFps, MaxFps);

	/// <summary>
	/// Returns display name.
	/// </summary>
	/// <returns> Display name. </returns>
	public virtual LocalizedString GetDisplayName() => new UnlocalizedString("FPS limit: Background");

	/// <summary>
	/// Returns category.
	/// </summary>
	/// <returns> Category. </returns>
	public string GetCategory() => "Graphics";

	/// <summary>
	/// Prepares display value.
	/// </summary>
	/// <param name="result"> Current value. </param>
	/// <returns> Display value. </returns>
	public override string Expose(float result)
	{
		const string NumericFormatWithNoDecimals = "F0";
		return result.ToString(NumericFormatWithNoDecimals);
	}
}

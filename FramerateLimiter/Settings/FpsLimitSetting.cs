using Landfall.Haste;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Settings;

namespace Mugnum.HasteMods.FramerateLimiter.Settings;

/// <summary>
/// FPS limit setting.
/// </summary>
public class FpsLimitSetting : FloatSetting, IExposedSetting
{
	/// <summary>
	/// Default value.
	/// </summary>
	internal const int DefaultValue = 240;

	/// <summary>
	/// Min FPS value.
	/// </summary>
	internal const int MinFps = 30;

	/// <summary>
	/// Max FPS value.
	/// </summary>
	internal const int MaxFps = 500;

	/// <summary>
	/// Process value change.
	/// </summary>
	public override void ApplyValue()
	{
		FramerateLimiter.UpdateLimiter(Value);
	}

	/// <summary>
	/// Loads setting.
	/// </summary>
	/// <param name="loader"> Settings loader. </param>
	public override void Load(ISettingsSaveLoad loader)
	{
		if (loader.TryLoadFloat(GetType(), out var value) && value >= MinFps)
		{
			Value = value;
		}
		else
		{
			Debug.Log("Failed to load setting of type " + GetType().FullName + " from PlayerPrefs.");
			Value = GetInitialValue();
		}

		var minMaxValue = GetMinMaxValue();
		MinValue = minMaxValue.x;
		MaxValue = minMaxValue.y;
		FramerateLimiter.UpdateLimiter(Value);
	}

	/// <summary>
	/// Prepares first-time setup value.
	/// </summary>
	/// <returns> Initial value. </returns>
	protected virtual float GetInitialValue()
	{
		return FramerateLimiter.GetRecommendedFpsLimit();
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
	public virtual LocalizedString GetDisplayName() => new UnlocalizedString("FPS limit");

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

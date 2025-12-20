using Landfall.Modding;
using Mugnum.HasteMods.FramerateLimiter.Settings;
using UnityEngine;

namespace Mugnum.HasteMods.FramerateLimiter;

/// <summary>
/// FPS limit mod.
/// </summary>
[LandfallPlugin]
public class FramerateLimiter
{
	/// <summary>
	/// Constructor.
	/// </summary>
	static FramerateLimiter()
	{
		Application.focusChanged -= OnFocusChanged;
		Application.focusChanged += OnFocusChanged;
	}

	/// <summary>
	/// Handle focus change.
	/// </summary>
	/// <param name="isFocused"> Is game in focus (not minimized). </param>
	private static void OnFocusChanged(bool isFocused)
	{
		var fpsLimit = isFocused
			? GameHandler.Instance.SettingsHandler.GetSetting<FpsLimitSetting>().Value
			: GameHandler.Instance.SettingsHandler.GetSetting<BackgroundFpsLimitSetting>().Value;

		UpdateLimiter(fpsLimit);
	}

	/// <summary>
	/// Updates current FPS limit.
	/// </summary>
	/// <param name="fpsLimit"> FPS limit value. </param>
	public static void UpdateLimiter(float fpsLimit)
	{
		fpsLimit = Mathf.Clamp(fpsLimit, BackgroundFpsLimitSetting.MinFps, FpsLimitSetting.MaxFps);
		Application.targetFrameRate = (int)Math.Round(fpsLimit);
	}

	/// <summary>
	/// Calculates recommended FPS limit value on first setup.
	/// </summary>
	/// <returns> FPS limit value. </returns>
	public static int GetRecommendedFpsLimit()
	{
		const int UnexpectedlyLowRefreshRate = 50;
		const int ExpectedVrrThreshold = 75;
		const int VrrFramerateOffset = 4;
		var refreshRate = (int)Math.Round(Screen.currentResolution.refreshRateRatio.value);
		
		if (refreshRate < UnexpectedlyLowRefreshRate)
		{
			return FpsLimitSetting.DefaultValue;
		}

		// If user has 76Hz+ display, assume they're on VRR.
		// If user is 75Hz and below, set limiter above refresh rate to avoid judder with VSync enabled.
		return refreshRate > ExpectedVrrThreshold
			? refreshRate - VrrFramerateOffset
			: refreshRate + VrrFramerateOffset;
	}
}

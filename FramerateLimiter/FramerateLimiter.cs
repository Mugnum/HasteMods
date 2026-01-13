using System.Linq.Expressions;
using Landfall.Modding;
using Mugnum.HasteMods.FramerateLimiter.Settings;
using UnityEngine;
using Zorro.Settings;

namespace Mugnum.HasteMods.FramerateLimiter;

/// <summary>
/// FPS limit mod.
/// </summary>
[LandfallPlugin]
public class FramerateLimiter
{
	/// <summary>
	/// "settings" private field getter for <see cref="HasteSettingsHandler"/>.
	/// </summary>
	private static readonly Func<HasteSettingsHandler, List<Setting>> SettingsGetter;

	/// <summary>
	/// Constructor.
	/// </summary>
	static FramerateLimiter()
	{
		SettingsGetter = CreateFieldGetter<HasteSettingsHandler, List<Setting>>("settings");
		On.VSyncSetting.ApplyValue += OnVSyncSettingApplyValue;
		On.HasteSettingsHandler.ctor += OnHasteSettingsHandlerConstructor;
		Application.focusChanged += OnFocusChanged;
	}

	/// <summary>
	/// Patch for applying value for VSync setting.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	private static void OnVSyncSettingApplyValue(On.VSyncSetting.orig_ApplyValue orig, VSyncSetting self)
	{
		// Force disables VSync, which prevents setting target FPS.
		QualitySettings.vSyncCount = (int)VSyncSetting.VSyncMode.None;
	}

	/// <summary>
	/// Handler for <see cref="HasteSettingsHandler"/> constructor.
	/// </summary>
	/// <param name="orig"> Original method. </param>
	/// <param name="self"> Instance. </param>
	private static void OnHasteSettingsHandlerConstructor(On.HasteSettingsHandler.orig_ctor orig, HasteSettingsHandler self)
	{
		orig(self);
		RegisterSettings(self);
	}

	/// <summary>
	/// Manually registers settings, while maintaining intended order in GUI.
	/// </summary>
	/// <param name="settingsHandler"> Settings handler. </param>
	private static void RegisterSettings(HasteSettingsHandler settingsHandler)
	{
		settingsHandler.AddSetting(new FpsLimitSetting());
		settingsHandler.AddSetting(new BackgroundFpsLimitSetting());
		var settings = SettingsGetter(settingsHandler);
		settings.RemoveAll(s => s is VSyncSetting);
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
		return refreshRate > ExpectedVrrThreshold
			? refreshRate - VrrFramerateOffset
			: refreshRate;
	}

	/// <summary>
	/// Creates private field getter from an instance.
	/// </summary>
	/// <typeparam name="TInstanceType"> Instance type. </typeparam>
	/// <typeparam name="TFieldType"> Field type. </typeparam>
	/// <param name="fieldName"> Field name. </param>
	/// <returns> Field getter delegate. </returns>
	internal static Func<TInstanceType, TFieldType> CreateFieldGetter<TInstanceType, TFieldType>(string fieldName)
	{
		var instance = Expression.Parameter(typeof(TInstanceType), "instance");
		var field = Expression.Field(instance, fieldName);
		return Expression.Lambda<Func<TInstanceType, TFieldType>>(field, instance).Compile();
	}
}

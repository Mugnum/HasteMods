using Landfall.Haste;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using Zorro.Settings;

namespace Mugnum.HasteMods.MoreUpscalingOptions.Settings;

/// <summary>
/// Extended upscaling quality setting.
/// </summary>
public class ExtendedUpscalingQualitySetting : EnumSetting<UpscalingQuality>, IExposedSetting
{
	/// <summary>
	/// Apply value handler.
	/// </summary>
	public override void ApplyValue()
	{
	}

	/// <summary>
	/// Returns default value.
	/// </summary>
	/// <returns> Default value. </returns>
	protected override UpscalingQuality GetDefaultValue() => UpscalingQuality.Quality;

	/// <summary>
	/// Returns display value.
	/// </summary>
	/// <returns> Localized display value. </returns>
	public LocalizedString GetDisplayName()
	{
		return new LocalizedString((TableReference)"Settings", (TableEntryReference)nameof(UpscalingQualitySetting));
	}

	/// <summary>
	/// Returns setting's category name.
	/// </summary>
	/// <returns> Category name. </returns>
	public string GetCategory() => "Graphics";

	/// <summary>
	/// Returns available choices for dropdown menu.
	/// </summary>
	/// <returns> Localized choices. </returns>
	public override List<LocalizedString> GetLocalizedChoices() =>
	[
		new UnlocalizedString("Native / DLAA"),
		new UnlocalizedString("Ultra Quality"),
		new UnlocalizedString("Quality"),
		new UnlocalizedString("Balanced"),
		new UnlocalizedString("Performance"),
		new UnlocalizedString("Ultra Performance")
	];
}

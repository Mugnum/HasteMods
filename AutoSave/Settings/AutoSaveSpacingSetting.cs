using Landfall.Haste;
using UnityEngine.Localization;
using Zorro.Settings;

namespace Mugnum.HasteMods.AutoSave.Settings;

/// <summary>
/// Autosave every X levels loaded setting.
/// </summary>
[HasteSetting]
public class AutoSaveSpacingSetting : IntSetting, IExposedSetting
{
	/// <summary>
	/// Min value.
	/// </summary>
	private const int MinValue = 1;

	/// <summary>
	/// Default value.
	/// </summary>
	private const int DefaultValue = 3;

	/// <summary>
	/// Process value change.
	/// </summary>
	public override void ApplyValue()
	{
		if (Value < MinValue)
		{
			Value = MinValue;
		}

		AutoSave.ResetSkippedSavesCounter();
	}

	/// <summary>
	/// Default value.
	/// </summary>
	/// <returns> Returns default value. </returns>
	protected override int GetDefaultValue() => DefaultValue;

	/// <summary>
	/// Returns display name.
	/// </summary>
	/// <returns> Display name. </returns>
	public LocalizedString GetDisplayName() => new UnlocalizedString("Autosave every X levels");

	/// <summary>
	/// Returns category name.
	/// </summary>
	/// <returns> Category name. </returns>
	public string GetCategory() => "Mods";
}

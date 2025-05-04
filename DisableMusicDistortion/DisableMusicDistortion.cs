using HarmonyLib;
using Landfall.Modding;

namespace Mugnum.HasteMods.DisableMusicDistortion;

/// <summary>
/// Disable music distortion mod.
/// </summary>
[LandfallPlugin]
public class DisableMusicDistortion
{
    /// <summary>
    /// Harmony plugin guid.
    /// </summary>
    private const string PluginGuid = "Mugnum.DisableMusicDistortion";

    /// <summary>
    /// Constructor.
    /// </summary>
    static DisableMusicDistortion()
    {
        new Harmony(PluginGuid).PatchAll();
    }
}

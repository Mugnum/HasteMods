using Landfall.Modding;

namespace Mugnum.HasteMods.DisableMusicDistortion;

/// <summary>
/// Disable music distortion mod.
/// </summary>
[LandfallPlugin]
public class DisableMusicDistortion
{
	/// <summary>
	/// Constructor.
	/// </summary>
	static DisableMusicDistortion()
	{
		const bool IsLevelDeath = false;

		On.Landfall.Haste.Music.MusicPlayer.SetLevelDeath += (original, self, _) =>
		{
			original(self, IsLevelDeath);
		};
	}
}

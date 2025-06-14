using UnityEngine;
using Landfall.Modding;
using Landfall.Haste.Music;

namespace Mugnum.HasteMods.PauseMusicOnMinimize
{
	/// <summary>
	/// Pause music on minimize mod.
	/// </summary>
	[LandfallPlugin]
	public class PauseMusicOnMinimize
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		static PauseMusicOnMinimize()
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
			if (isFocused)
			{
				MusicPlayer.Instance.m_AudioSourceCurrent.UnPause();
				return;
			}

			MusicPlayer.Instance.m_AudioSourceCurrent.Pause();
		}
	}
}

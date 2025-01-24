#if UNITY_EDITOR && UNITY_2019_2_OR_NEWER
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace RomDev
{

	class BuildProcessor : IPreprocessBuildWithReport
	{

		public int callbackOrder { get { return 0; } }


		public void OnPreprocessBuild (BuildReport report)
		{
			if (KickStarter.settingsManager)
			{
				switch (KickStarter.settingsManager.playerSwitching)
				{
					case PlayerSwitching.Allow:
						foreach (PlayerPrefab playerPrefab in KickStarter.settingsManager.players)
						{
							playerPrefab.GetPlayerData ();
						}
						break;

					case PlayerSwitching.DoNotAllow:
						KickStarter.settingsManager.PlayerPrefab.GetPlayerData ();
						break;

					default:
						break;
				}
			}
		}

	}
}

#endif
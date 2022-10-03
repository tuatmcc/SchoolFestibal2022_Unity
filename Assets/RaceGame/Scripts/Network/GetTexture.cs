using Cysharp.Threading.Tasks;
using RaceGame.Manager;
using UnityEngine;
using UnityEngine.Networking;

namespace RaceGame.Network
{
	public static class GetTexture
	{
		private static async UniTask<Texture> DownloadTexture(string url)
		{
			var r = UnityWebRequestTexture.GetTexture(url);
			await r.SendWebRequest();
			return DownloadHandlerTexture.GetContent(r);
		}
	}
}
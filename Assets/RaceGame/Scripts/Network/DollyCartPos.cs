using System;
using Mirror;

namespace RaceGame.Network
{
	[Obsolete("破棄", true)]
	public static class DollyCartPos
	{
		[SyncVar]
		public static float pos1;
		[SyncVar]
		public static float pos2;
		[SyncVar]
		public static float pos3;
		[SyncVar]
		public static float pos4;
		[SyncVar]
		public static float pos5;

		public static void Reset()
		{
			pos1 = 0.0f;
			pos2 = 0.0f;
			pos3 = 0.0f;
			pos4 = 0.0f;
			pos5 = 0.0f;
		}
	}
}
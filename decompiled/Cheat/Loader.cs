using System;
using Cheat.core;
using UnityEngine;

namespace Cheat
{
	// Token: 0x0200000B RID: 11
	public static class Loader
	{
		// Token: 0x06000035 RID: 53 RVA: 0x0000229F File Offset: 0x0000049F
		public static void Load()
		{
			Loader.cheatObject = new GameObject("CheatCore");
			UnityEngine.Object.DontDestroyOnLoad(Loader.cheatObject);
			Loader.cheatObject.AddComponent<Cheat.core.Main>();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000022C6 File Offset: 0x000004C6
		public static void Unload()
		{
			if (Loader.cheatObject != null)
			{
				UnityEngine.Object.Destroy(Loader.cheatObject);
				Loader.cheatObject = null;
			}
		}

		// Token: 0x0400002A RID: 42
		private static GameObject cheatObject;
	}
}

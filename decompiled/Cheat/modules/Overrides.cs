using System;
using System.Reflection;
using Cheat.core;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x02000019 RID: 25
	internal static class Overrides
	{
		// Token: 0x06000082 RID: 130 RVA: 0x000074DC File Offset: 0x000056DC
		public static void Awake()
		{
			Utils.OverrideMethod(typeof(Player), typeof(hkPlayer), "ReceiveTakeScreenshot", BindingFlags.Instance | BindingFlags.Public, BindingFlags.Instance | BindingFlags.NonPublic);
			try
			{
				MethodInfo method = typeof(DamageTool).GetMethod("raycast", BindingFlags.Static | BindingFlags.Public, null, new Type[]
				{
					typeof(Ray),
					typeof(float),
					typeof(int),
					typeof(Player)
				}, null);
				MethodInfo method2 = typeof(hkDamageTool).GetMethod("Ov_raycast", BindingFlags.Static | BindingFlags.Public);
				if (method != null && method2 != null)
				{
					Utils.RedirectCalls(method, method2);
				}
				else
				{
					Debug.LogWarning("[Nightmare] Не удалось найти методы для хука DamageTool!");
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Nightmare] Краш при установке хука DamageTool: " + ex.Message);
			}
			Utils.OverrideMethod(typeof(PlayerUI), typeof(hkPlayer), "onMoonUpdated", BindingFlags.Instance | BindingFlags.NonPublic, BindingFlags.Instance | BindingFlags.NonPublic);
		}

		// Token: 0x04000092 RID: 146
		public static bool bHideOnSpy = true;

		// Token: 0x04000093 RID: 147
		public static bool bBeingSpied = false;

		// Token: 0x04000094 RID: 148
		public static bool bPlaySpySound = true;
	}
}

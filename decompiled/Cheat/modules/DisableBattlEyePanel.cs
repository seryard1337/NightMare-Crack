using System;
using System.Collections;
using System.Reflection;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	public class DisableBattlEyePanel
	{
		public static IEnumerator DisablePanelRoutine()
		{
			for (int i = 0; i < 30; i++)
			{
				DisablePanel();
				yield return new WaitForSeconds(1f);
			}
		}

		public static void DisablePanel()
		{
			FieldInfo field = typeof(Dedicator).GetField("_hasThirdpartyAntiCheat", BindingFlags.Static | BindingFlags.NonPublic)
			                  ?? typeof(Dedicator).GetField("_hasBattlEye", BindingFlags.Static | BindingFlags.NonPublic);
			if (field != null)
			{
				field.SetValue(null, true);
			}
		}
	}
}

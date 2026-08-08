using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.core
{
	// Token: 0x02000027 RID: 39
	internal static class Utils
	{
		// Token: 0x060000DE RID: 222 RVA: 0x00012200 File Offset: 0x00010400
		internal static Transform GetLimb(Transform root, ELimb limb)
		{
			if (root == null)
			{
				return null;
			}
			Dictionary<ELimb, Transform> dictionary;
			if (!Utils.limbCache.TryGetValue(root, out dictionary))
			{
				dictionary = new Dictionary<ELimb, Transform>();
				Utils.limbCache[root] = dictionary;
			}
			Transform transform;
			if (dictionary.TryGetValue(limb, out transform) && transform != null)
			{
				return transform;
			}
			foreach (Transform transform2 in root.GetComponentsInChildren<Transform>())
			{
				if (DamageTool.getLimb(transform2) == limb)
				{
					dictionary[limb] = transform2;
					return transform2;
				}
			}
			return null;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00012280 File Offset: 0x00010480
		internal static void CleanLimbCache()
		{
			foreach (Transform transform in Utils.limbCache.Keys.Where<Transform>((Transform k) => k == null).ToList<Transform>())
			{
				Utils.limbCache.Remove(transform);
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00012308 File Offset: 0x00010508
		internal static bool SetPrivateField(object obj, string fieldName, object value)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			if (!(field == null))
			{
				field.SetValue(obj, value);
				return true;
			}
			return false;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00002737 File Offset: 0x00000937
		internal static object GetPrivateField(object obj, string fieldName)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			if (field != null)
			{
				return field.GetValue(obj);
			}
			return null;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00012338 File Offset: 0x00010538
		internal static void DrawBox(float x, float y, float w, float h, float thickness, Color color)
		{
			GUI.color = color;
			GUI.DrawTexture(new Rect(x, y, w, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(x, y + h - thickness, w, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(x, y, thickness, h), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(x + w - thickness, y, thickness, h), Texture2D.whiteTexture);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000123A8 File Offset: 0x000105A8
		internal static void DrawLine(float x1, float y1, float x2, float y2, float thickness, Color color)
		{
			GUI.color = color;
			Vector2 vector;
			vector = new Vector2(x1, y1);
			Vector2 vector2 = new Vector2(x2, y2) - vector;
			float magnitude = vector2.magnitude;
			if (magnitude < 0.001f)
			{
				return;
			}
			float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			Matrix4x4 matrix = GUI.matrix;
			GUIUtility.RotateAroundPivot(num, vector);
			GUI.DrawTexture(new Rect(vector.x, vector.y - thickness / 2f, magnitude, thickness), Texture2D.whiteTexture);
			GUI.matrix = matrix;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00012434 File Offset: 0x00010634
		internal static void DrawLineFixed(float x1, float y1, float x2, float y2, float thickness, Color color)
		{
			GUI.color = color;
			Vector2 vector;
			vector = new Vector2(x1, y1);
			Vector2 vector2 = new Vector2(x2, y2) - vector;
			float magnitude = vector2.magnitude;
			if (magnitude >= 1f)
			{
				float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
				Matrix4x4 matrix = GUI.matrix;
				GUI.matrix = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, num), Vector3.one);
				GUI.DrawTexture(new Rect(0f, -thickness / 2f, magnitude, thickness), Texture2D.whiteTexture);
				float num2 = thickness * 0.6f;
				GUI.color = new Color(color.r, color.g, color.b, color.a * 0.35f);
				GUI.DrawTexture(new Rect(-num2, -thickness / 2f, num2 * 2f, thickness), Texture2D.whiteTexture);
				GUI.DrawTexture(new Rect(magnitude - num2, -thickness / 2f, num2 * 2f, thickness), Texture2D.whiteTexture);
				GUI.matrix = matrix;
				GUI.color = color;
				return;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00012564 File Offset: 0x00010764
		internal static void DrawCircleFixed(Vector2 center, float radius, float thickness = 1.5f, Color? overrideColor = null)
		{
			Color? color = overrideColor;
			Color color2 = ((color == null) ? new Color(1f, 0.3f, 0.3f, 0.7f) : color.GetValueOrDefault());
			GUI.color = color2;
			for (int i = 0; i <= 140; i++)
			{
				float num = (float)i * 2.5714285f * 0.017453292f;
				float num2 = (float)(i + 1) * 2.5714285f * 0.017453292f;
				float num3 = center.x + Mathf.Cos(num) * radius;
				float num4 = center.y + Mathf.Sin(num) * radius;
				float num5 = center.x + Mathf.Cos(num2) * radius;
				float num6 = center.y + Mathf.Sin(num2) * radius;
				Utils.DrawLineFixed(num3, num4, num5, num6, thickness, color2);
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00012634 File Offset: 0x00010834
		internal static void DrawSmoothCircle(Vector2 center, float screenRadius, float thickness, Color color)
		{
			if (screenRadius > 0f && !float.IsNaN(screenRadius) && !float.IsInfinity(screenRadius))
			{
				if (Utils.cachedFovTexture == null || Utils.cachedColor != color || Mathf.Abs(Utils.cachedRadius - screenRadius) > 0.5f)
				{
					if (Utils.cachedFovTexture != null)
					{
						UnityEngine.Object.Destroy(Utils.cachedFovTexture);
					}
					float num = thickness + 3f;
					float num2 = (screenRadius + num) * 2f;
					int num3 = Mathf.Clamp(Mathf.CeilToInt(num2), 128, 1024);
					Utils.cachedFovTexture = new Texture2D(num3, num3, TextureFormat.RGBA32, false);
					Utils.cachedFovTexture.hideFlags = (HideFlags)61;
					Utils.cachedFovTexture.filterMode = (FilterMode)1;
					Utils.cachedFovTexture.wrapMode = (TextureWrapMode)1;
					Color[] array = new Color[num3 * num3];
					float num4 = (float)num3 / 2f;
					float num5 = (float)num3 / num2;
					float num6 = screenRadius * num5;
					float num7 = thickness * num5 / 2f;
					float num8 = 1f * num5;
					Color color2;
					color2 = new Color(color.r, color.g, color.b, 0f);
					for (int i = 0; i < num3; i++)
					{
						for (int j = 0; j < num3; j++)
						{
							float num9 = Mathf.Abs(Vector2.Distance(new Vector2((float)j + 0.5f, (float)i + 0.5f), new Vector2(num4, num4)) - num6);
							if (num9 <= num7 + num8)
							{
								float num10 = 1f;
								if (num9 > num7)
								{
									num10 = Mathf.Clamp01(1f - (num9 - num7) / num8);
								}
								array[i * num3 + j] = new Color(color.r, color.g, color.b, color.a * num10);
							}
							else
							{
								array[i * num3 + j] = color2;
							}
						}
					}
					Utils.cachedFovTexture.SetPixels(array);
					Utils.cachedFovTexture.Apply();
					Utils.cachedColor = color;
					Utils.cachedRadius = screenRadius;
				}
				float num11 = thickness + 3f;
				float num12 = (screenRadius + num11) * 2f;
				Color color3 = GUI.color;
				GUI.color = Color.white;
				GUI.DrawTexture(new Rect(center.x - num12 / 2f, center.y - num12 / 2f, num12, num12), Utils.cachedFovTexture);
				GUI.color = color3;
				return;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0001288C File Offset: 0x00010A8C
		internal static void DrawCircle(Vector2 center, float radius, int segments, float thickness, Color color)
		{
			GUI.color = color;
			float num = 360f / (float)segments;
			Vector2 vector = center + new Vector2(Mathf.Cos(0f) * radius, Mathf.Sin(0f) * radius);
			for (int i = 1; i <= segments; i++)
			{
				float num2 = (float)i * num * 0.017453292f;
				Vector2 vector2 = center + new Vector2(Mathf.Cos(num2) * radius, Mathf.Sin(num2) * radius);
				Utils.DrawLine(vector.x, vector.y, vector2.x, vector2.y, thickness, color);
				vector = vector2;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00012928 File Offset: 0x00010B28
		internal static Vector3 DirectionDiff(Vector3 worldPos, Camera cam)
		{
			if (!(cam == null))
			{
				Vector3 normalized = (worldPos - cam.transform.position).normalized;
				Vector3 forward = cam.transform.forward;
				return normalized - forward;
			}
			return Vector3.zero;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00012970 File Offset: 0x00010B70
		internal static void DrawSnapline(Vector2 startScreenPoint, Vector3 worldPos, Color color, float thickness, Camera cam)
		{
			if (!(cam == null))
			{
				Vector3 vector = cam.WorldToScreenPoint(worldPos);
				Vector3 vector2 = Utils.DirectionDiff(worldPos, cam);
				bool flag = Mathf.Abs(vector2.x) < 1f && Mathf.Abs(vector2.z) < 1f;
				if (vector.z < 0f && (!flag || vector2.y <= 0f))
				{
					vector.x = (float)Screen.width - vector.x;
					vector.y = (float)Screen.height;
				}
				else
				{
					vector.y = (float)Screen.height - vector.y;
				}
				Utils.DrawLine(startScreenPoint.x, startScreenPoint.y, vector.x, vector.y, thickness, color);
				return;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00012A38 File Offset: 0x00010C38
		internal static Vector2 CalcAngles(Vector3 playerPos, Vector3 targetPos)
		{
			Vector3 vector = targetPos - playerPos;
			Vector2 vector2;
			vector2.x = (float)(Math.Atan((double)(vector.z / vector.x)) * 57.29577951308232);
			vector2.y = (float)(Math.Acos((double)(vector.y / vector.magnitude)) * 57.29577951308232);
			vector2.x = -vector2.x;
			if (vector.x >= 0f)
			{
				vector2.x += 90f;
			}
			else
			{
				vector2.x -= 90f;
			}
			if (Player.LocalPlayer.look.perspective == EPlayerPerspective.THIRD)
			{
				float num = 5f;
				if (!Player.LocalPlayer.animator.side)
				{
					vector2.x += num;
				}
				else
				{
					vector2.x -= num;
				}
			}
			while (vector2.y < 0f)
			{
				vector2.y += 180f;
			}
			while (vector2.y > 180f)
			{
				vector2.y -= 180f;
			}
			return vector2;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00012B5C File Offset: 0x00010D5C
		internal static void OverrideMethod(Type defaultClass, Type overrideClass, string method, BindingFlags sourceFlags, BindingFlags destFlags)
		{
			MethodInfo method2 = defaultClass.GetMethod(method, sourceFlags);
			MethodInfo method3 = overrideClass.GetMethod("Ov_" + method, destFlags);
			if (!(method2 == null) && !(method3 == null))
			{
				Utils.RedirectCalls(method2, method3);
				return;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00012BA0 File Offset: 0x00010DA0
		internal static void RedirectCalls(MethodInfo from, MethodInfo to)
		{
			IntPtr functionPointer = from.MethodHandle.GetFunctionPointer();
			IntPtr functionPointer2 = to.MethodHandle.GetFunctionPointer();
			Utils.PatchJumpTo(functionPointer, functionPointer2);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00012BD0 File Offset: 0x00010DD0
		internal unsafe static void PatchJumpTo(IntPtr site, IntPtr target)
		{
			byte[] bytes = BitConverter.GetBytes(target.ToInt64());
			byte[] array = new byte[]
			{
				73, 187, 0, 0, 0, 0, 0, 0, 0, 0,
				65, byte.MaxValue, 227
			};
			array[2] = bytes[0];
			array[3] = bytes[1];
			array[4] = bytes[2];
			array[5] = bytes[3];
			array[6] = bytes[4];
			array[7] = bytes[5];
			array[8] = bytes[6];
			array[9] = bytes[7];
			byte[] array2 = array;
			uint num;
			Utils.VirtualProtect(site, (UIntPtr)((ulong)((long)array2.Length)), 64U, out num);
			fixed (byte* ptr = array2)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					*(byte*)(void*)IntPtr.Add(site, i) = ptr[i];
				}
			}
			uint num2;
			Utils.VirtualProtect(site, (UIntPtr)((ulong)((long)array2.Length)), num, out num2);
		}

		// Token: 0x060000EE RID: 238
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

		// Token: 0x060000EF RID: 239 RVA: 0x00012C98 File Offset: 0x00010E98
		internal static float GetGunRange()
		{
			Player localPlayer = Player.LocalPlayer;
			object obj;
			if (localPlayer != null)
			{
				PlayerEquipment equipment = localPlayer.equipment;
				obj = ((equipment == null) ? null : equipment.asset);
			}
			else
			{
				obj = null;
			}
			ItemGunAsset itemGunAsset = obj as ItemGunAsset;
			if (itemGunAsset == null)
			{
				return 15f;
			}
			return itemGunAsset.range;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00012CD8 File Offset: 0x00010ED8
		internal static bool IsFriendly(Player target)
		{
			if (!(target == null) && !(target == Player.LocalPlayer) && !target.life.isDead)
			{
				List<SteamPlayer> clients = Provider.clients;
				SteamPlayer steamPlayer = ((clients == null) ? null : clients.FirstOrDefault<SteamPlayer>((SteamPlayer sp) => ((sp == null) ? null : sp.player) == target));
				return steamPlayer != null && (Player.LocalPlayer.channel.owner.isMemberOfSameGroupAs(steamPlayer) || Utils.ManualFriends.Contains(steamPlayer.playerID.steamID.m_SteamID));
			}
			return true;
		}

		// Token: 0x04000183 RID: 387
		private static readonly Dictionary<Transform, Dictionary<ELimb, Transform>> limbCache = new Dictionary<Transform, Dictionary<ELimb, Transform>>();

		// Token: 0x04000184 RID: 388
		private static Texture2D cachedFovTexture;

		// Token: 0x04000185 RID: 389
		private static Color cachedColor = Color.clear;

		// Token: 0x04000186 RID: 390
		private static float cachedRadius = -1f;

		// Token: 0x04000187 RID: 391
		internal static readonly HashSet<ulong> ManualFriends = new HashSet<ulong>();
	}
}

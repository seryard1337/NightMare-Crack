using System;
using System.Collections.Generic;
using System.Linq;
using Cheat.modules;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.core
{
	// Token: 0x02000023 RID: 35
	internal class Menu
	{
		// Token: 0x060000BE RID: 190 RVA: 0x0000A400 File Offset: 0x00008600
		private string T(string key)
		{
			Dictionary<int, string> dictionary;
			string text;
			if (Menu.lang.TryGetValue(key, out dictionary) && dictionary.TryGetValue(Menu.currentLanguage, out text))
			{
				return text;
			}
			return key;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000A430 File Offset: 0x00008630
		internal Menu(Main main)
		{
			this.main = main;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000A4AC File Offset: 0x000086AC
		private Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] array = new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = col;
			}
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.hideFlags = (HideFlags)61;
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000A4F4 File Offset: 0x000086F4
		private Texture2D MakeGradient(int width, int height, Color colorStart, Color colorEnd, bool horizontal = false)
		{
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.hideFlags = (HideFlags)61;
			texture2D.filterMode = (FilterMode)1;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					float num = (horizontal ? ((float)j / (float)(width - 1)) : ((float)i / (float)(height - 1)));
					texture2D.SetPixel(j, i, Color.Lerp(colorStart, colorEnd, num));
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000A560 File Offset: 0x00008760
		private Texture2D MakeTopTabGradient(int width, int height)
		{
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.hideFlags = (HideFlags)61;
			texture2D.filterMode = (FilterMode)1;
			for (int i = 0; i < height; i++)
			{
				float num = (float)i / (float)(height - 1);
				Color color = Color.Lerp(new Color(0.8f, 0.15f, 0.25f, 0.35f), new Color(0f, 0f, 0f, 0.1f), num);
				if (i == 0)
				{
					color = new Color(0.8f, 0.15f, 0.25f, 0.6f);
				}
				for (int j = 0; j < width; j++)
				{
					texture2D.SetPixel(j, i, color);
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000A614 File Offset: 0x00008814
		private Texture2D MakeSmoothRoundedGradient(int baseWidth, int baseHeight, int baseRadius, Color colorStart, Color colorEnd)
		{
			int num = baseWidth * 4;
			int num2 = baseHeight * 4;
			int num3 = baseRadius * 4;
			Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGBA32, true);
			texture2D.hideFlags = (HideFlags)61;
			texture2D.filterMode = (FilterMode)2;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					float num4 = (float)j / (float)(num - 1);
					Color color = Color.Lerp(colorStart, colorEnd, num4);
					bool flag = false;
					float num5 = 0f;
					float num6 = 0f;
					if (j < num3 && i < num3)
					{
						flag = true;
						num5 = (float)num3 - 0.5f;
						num6 = (float)num3 - 0.5f;
					}
					else if (j >= num - num3 && i < num3)
					{
						flag = true;
						num5 = (float)(num - num3) - 0.5f;
						num6 = (float)num3 - 0.5f;
					}
					else if (j < num3 && i >= num2 - num3)
					{
						flag = true;
						num5 = (float)num3 - 0.5f;
						num6 = (float)(num2 - num3) - 0.5f;
					}
					else if (j >= num - num3 && i >= num2 - num3)
					{
						flag = true;
						num5 = (float)(num - num3) - 0.5f;
						num6 = (float)(num2 - num3) - 0.5f;
					}
					float num7 = 1f;
					if (flag)
					{
						float num8 = Vector2.Distance(new Vector2((float)j, (float)i), new Vector2(num5, num6));
						num7 = Mathf.Clamp01((float)num3 - num8);
					}
					texture2D.SetPixel(j, i, new Color(color.r, color.g, color.b, color.a * num7));
				}
			}
			texture2D.Apply(true);
			return texture2D;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000A7A4 File Offset: 0x000089A4
		private Texture2D MakeSmoothCircle(int baseRadius, Color col)
		{
			int num = baseRadius * 4;
			int num2 = num * 2;
			Texture2D texture2D = new Texture2D(num2, num2, TextureFormat.RGBA32, true);
			texture2D.hideFlags = (HideFlags)61;
			texture2D.filterMode = (FilterMode)2;
			float num3 = (float)num - 0.5f;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					float num4 = Vector2.Distance(new Vector2((float)j, (float)i), new Vector2(num3, num3));
					float num5 = Mathf.Clamp01((float)num - num4);
					texture2D.SetPixel(j, i, new Color(col.r, col.g, col.b, col.a * num5));
				}
			}
			texture2D.Apply(true);
			return texture2D;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000A854 File Offset: 0x00008A54
		private Texture2D MakePerfectCircle(int size, Color col)
		{
			Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGBA32, false);
			texture2D.hideFlags = (HideFlags)61;
			texture2D.filterMode = (FilterMode)1;
			float num = (float)size / 2f;
			float num2 = (float)size / 2f - 0.5f;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num3 = Vector2.Distance(new Vector2((float)j + 0.5f, (float)i + 0.5f), new Vector2(num, num));
					float num4 = Mathf.Clamp01(num2 - num3 + 0.5f);
					texture2D.SetPixel(j, i, new Color(col.r, col.g, col.b, col.a * num4));
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000A914 File Offset: 0x00008B14
		private void InitProceduralTextures()
		{
			if (!(this.bgGradient != null))
			{
				if (this.cursorTex == null)
				{
					this.cursorTex = Resources.FindObjectsOfTypeAll<Texture2D>().FirstOrDefault<Texture2D>((Texture2D x) => x.name.ToLower() == "cursor" || x.name == "ui_cursor");
				}
				this.bgGradient = this.MakeGradient(1, 128, new Color(0.35f, 0.05f, 0.1f, 1f), new Color(0.04f, 0.05f, 0.12f, 1f), false);
				this.sideTabGlow = this.MakeGradient(128, 1, new Color(0.8f, 0.15f, 0.25f, 0.7f), new Color(0f, 0f, 0f, 0f), true);
				this.topTabActiveBg = this.MakeTopTabGradient(1, 40);
				this.topTabInactiveBg = this.MakeTex(1, 40, new Color(0f, 0f, 0f, 0.4f));
				this.toggleBgOn = this.MakeSmoothRoundedGradient(36, 18, 9, new Color(0.9f, 0.15f, 0.3f, 1f), new Color(0.6f, 0.05f, 0.15f, 1f));
				this.toggleBgOff = this.MakeSmoothRoundedGradient(36, 18, 9, new Color(0.25f, 0.25f, 0.3f, 1f), new Color(0.18f, 0.18f, 0.22f, 1f));
				this.toggleKnobOn = this.MakeSmoothCircle(7, Color.white);
				this.toggleKnobOff = this.MakeSmoothCircle(7, new Color(0.65f, 0.65f, 0.65f, 1f));
				this.sliderKnobTex = this.MakePerfectCircle(14, Color.white);
				return;
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000AB04 File Offset: 0x00008D04
		private void DrawAnimatedSnow(Rect bounds)
		{
			if (this.snow == null)
			{
				this.snow = new List<Menu.Snowflake>();
				for (int i = 0; i < 75; i++)
				{
					this.snow.Add(new Menu.Snowflake
					{
						x = UnityEngine.Random.Range(bounds.x, bounds.width),
						y = UnityEngine.Random.Range(bounds.y, bounds.height),
						speed = UnityEngine.Random.Range(15f, 45f),
						size = UnityEngine.Random.Range(1f, 2.5f)
					});
				}
			}
			if (Event.current.type == EventType.Repaint)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.35f);
				float deltaTime = Time.deltaTime;
				foreach (Menu.Snowflake snowflake in this.snow)
				{
					snowflake.y += snowflake.speed * deltaTime;
					snowflake.x += Mathf.Sin(Time.time + snowflake.speed) * 0.2f;
					if (snowflake.y > bounds.height)
					{
						snowflake.y = bounds.y;
						snowflake.x = UnityEngine.Random.Range(bounds.x, bounds.width);
					}
					GUI.DrawTexture(new Rect(snowflake.x, snowflake.y, snowflake.size, snowflake.size), Texture2D.whiteTexture);
				}
				GUI.color = Color.white;
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000ACBC File Offset: 0x00008EBC
		private void InitModernStyles()
		{
			if (!this.stylesInitialized)
			{
				this.panelStyle = new GUIStyle();
				this.panelStyle.normal.background = this.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.25f));
				this.panelStyle.padding = new RectOffset(15, 15, 15, 15);
				this.panelStyle.margin = new RectOffset(5, 5, 5, 5);
				this.headerTextStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12,
					fontStyle = FontStyle.Bold,
					normal = 
					{
						textColor = new Color(0.5f, 0.5f, 0.5f, 1f)
					}
				};
				this.subLabelStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 11,
					normal = 
					{
						textColor = new Color(0.45f, 0.45f, 0.45f, 1f)
					}
				};
				this.toggleMainTextStyle = new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold,
					fontSize = 13,
					normal = 
					{
						textColor = new Color(0.95f, 0.95f, 0.95f, 1f)
					}
				};
				this.sidebarTabStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 15,
					alignment = TextAnchor.MiddleCenter,
					normal = 
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					},
					hover = 
					{
						textColor = Color.white
					}
				};
				this.topTabStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 14,
					alignment = TextAnchor.MiddleCenter,
					normal = 
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					},
					hover = 
					{
						textColor = Color.white
					}
				};
				this.hiddenScrollbar = new GUIStyle(GUIStyle.none);
				this.hiddenScrollbar.normal.background = null;
				this.hiddenScrollbar.fixedWidth = 0f;
				this.hiddenScrollbar.fixedHeight = 0f;
				this.hSliderStyle = new GUIStyle(GUIStyle.none);
				this.hSliderStyle.normal.background = this.MakeSmoothRoundedGradient(64, 4, 2, new Color(0.15f, 0.15f, 0.18f, 1f), new Color(0.12f, 0.12f, 0.15f, 1f));
				this.hSliderStyle.fixedHeight = 4f;
				this.hSliderStyle.margin = new RectOffset(0, 0, 10, 10);
				this.hSliderThumbStyle = new GUIStyle(GUIStyle.none);
				this.hSliderThumbStyle.normal.background = this.sliderKnobTex;
				this.hSliderThumbStyle.hover.background = this.sliderKnobTex;
				this.hSliderThumbStyle.active.background = this.sliderKnobTex;
				this.hSliderThumbStyle.focused.background = this.sliderKnobTex;
				this.hSliderThumbStyle.border = new RectOffset(0, 0, 0, 0);
				this.hSliderThumbStyle.fixedWidth = 14f;
				this.hSliderThumbStyle.fixedHeight = 14f;
				this.hSliderThumbStyle.margin = new RectOffset(0, 0, -5, 0);
				this.hudWindowStyle = new GUIStyle(GUI.skin.window);
				this.hudWindowStyle.normal.background = this.MakeTex(1, 1, new Color(0.06f, 0.06f, 0.06f, 0.85f));
				this.hudWindowStyle.focused.background = this.hudWindowStyle.normal.background;
				this.hudWindowStyle.onNormal.background = this.hudWindowStyle.normal.background;
				this.hudWindowStyle.border = new RectOffset(0, 0, 0, 0);
				this.hudWindowStyle.padding = new RectOffset(10, 10, 25, 10);
				this.hudItemStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 12,
					fontStyle = FontStyle.Bold,
					normal = 
					{
						textColor = new Color(0.85f, 0.85f, 0.85f, 1f)
					}
				};
				this.stylesInitialized = true;
				return;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000B134 File Offset: 0x00009334
		private bool DrawToggleSwitch(string mainText, string subText, bool value)
		{
			string text = string.Format("{0}_{1}_{2}", this.selectedMainTab, mainText, subText);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label(mainText, this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
			if (!string.IsNullOrEmpty(subText))
			{
				GUILayout.Label(subText, this.subLabelStyle, Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			Rect rect = GUILayoutUtility.GetRect(36f, 18f);
			if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
			{
				value = !value;
			}
			float num;
			if (!this.toggleAnims.TryGetValue(text, out num))
			{
				num = (value ? 1f : 0f);
				this.toggleAnims[text] = num;
			}
			if (Event.current.type == EventType.Repaint)
			{
				float num2 = (value ? 1f : (-1f));
				num = Mathf.Clamp01(num + num2 * Time.deltaTime * 5f);
				this.toggleAnims[text] = num;
			}
			float num3 = num * num * (3f - 2f * num);
			GUI.color = Color.white;
			GUI.DrawTexture(rect, this.toggleBgOff);
			if (num3 > 0f)
			{
				GUI.color = new Color(1f, 1f, 1f, num3);
				GUI.DrawTexture(rect, this.toggleBgOn);
			}
			float num4 = Mathf.Lerp(rect.x + 2f, rect.x + 20f, num3);
			Rect rect2;
			rect2 = new Rect(num4, rect.y + 2f, 14f, 14f);
			GUI.color = Color.white;
			GUI.DrawTexture(rect2, this.toggleKnobOff);
			if (num3 > 0f)
			{
				GUI.color = new Color(1f, 1f, 1f, num3);
				GUI.DrawTexture(rect2, this.toggleKnobOn);
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
			GUILayout.Space(12f);
			return value;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000B334 File Offset: 0x00009534
		internal void Draw()
		{
			this.InitProceduralTextures();
			this.InitModernStyles();
			if (!Overrides.bBeingSpied || !Overrides.bHideOnSpy)
			{
				if (Menu.showHudFeatures)
				{
					Menu.rectHudFeatures = GUILayout.Window(10, Menu.rectHudFeatures, new GUI.WindowFunction(this.HudFeaturesWindow), "", this.hudWindowStyle, new GUILayoutOption[] { GUILayout.Width(Menu.hudFeaturesWidth) });
				}
				if (Menu.showHudWeapon)
				{
					Menu.rectHudWeapon = GUILayout.Window(11, Menu.rectHudWeapon, new GUI.WindowFunction(this.HudWeaponWindow), "", this.hudWindowStyle, new GUILayoutOption[] { GUILayout.Width(Menu.hudWeaponWidth) });
				}
				if (Menu.showHudAdmins)
				{
					Menu.rectHudAdmins = GUILayout.Window(12, Menu.rectHudAdmins, new GUI.WindowFunction(this.HudAdminsWindow), "", this.hudWindowStyle, new GUILayoutOption[] { GUILayout.Width(Menu.hudAdminsWidth) });
				}
			}
			if (this.showMenu)
			{
				this.windowRect.width = Menu.windowWidth;
				this.windowRect.height = Menu.windowHeight;
				GUI.skin.window.normal.background = Texture2D.blackTexture;
				GUI.skin.window.border = new RectOffset(0, 0, 0, 0);
				GUI.skin.window.padding = new RectOffset(0, 0, 0, 0);
				this.windowRect = GUI.Window(0, this.windowRect, new GUI.WindowFunction(this.WindowFunction), "", GUIStyle.none);
				return;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000B4C4 File Offset: 0x000096C4
		private void HudFeaturesWindow(int id)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Menu.rectHudFeatures.width, 3f), this.sideTabGlow);
			GUI.Label(new Rect(0f, 5f, Menu.rectHudFeatures.width, 20f), this.T("HUD_Features"), new GUIStyle(this.hudItemStyle)
			{
				alignment = TextAnchor.MiddleCenter
			});
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			if (this.main.AimbotHoldToAim)
			{
				GUILayout.Label("> Aimbot Hold", this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			if (this.main.aimbot.enabled)
			{
				GUILayout.Label("> Aimbot", this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			if (this.main.esp.espEnabled)
			{
				GUILayout.Label("> ESP", this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			if (this.main.triggerbot.enabled)
			{
				GUILayout.Label("> Triggerbot", this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			if (this.main.freeCam.Enabled)
			{
				GUILayout.Label("> FreeCam", this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			if (this.main.vehicleNoclip.active)
			{
				GUILayout.Label("> Vehicle NoClip", this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			if (this.main.itemVacuum.Enabled)
			{
				GUILayout.Label("> Item Vacuum", this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			if (this.main.chatSpam.Enabled)
			{
				GUILayout.Label("> Chat Spam", this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndVertical();
			if (this.showMenu)
			{
				GUI.DragWindow();
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000B68C File Offset: 0x0000988C
		private void HudWeaponWindow(int id)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Menu.rectHudWeapon.width, 3f), this.sideTabGlow);
			GUI.Label(new Rect(0f, 5f, Menu.rectHudWeapon.width, 20f), this.T("HUD_Weapon"), new GUIStyle(this.hudItemStyle)
			{
				alignment = TextAnchor.MiddleCenter
			});
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			Player localPlayer = Player.LocalPlayer;
			bool flag;
			if (localPlayer != null)
			{
				PlayerEquipment equipment = localPlayer.equipment;
				flag = ((equipment == null) ? null : equipment.asset) != null;
			}
			else
			{
				flag = null != null;
			}
			if (!flag)
			{
				GUILayout.Label(this.T("None"), this.hudItemStyle, Array.Empty<GUILayoutOption>());
			}
			else
			{
				ItemAsset asset = localPlayer.equipment.asset;
				GUILayout.Label(string.Format("ID: {0}", asset.id), this.hudItemStyle, Array.Empty<GUILayoutOption>());
				GUILayout.Label("Name: " + asset.FriendlyName, this.hudItemStyle, Array.Empty<GUILayoutOption>());
				ItemGunAsset itemGunAsset = asset as ItemGunAsset;
				if (itemGunAsset != null)
				{
					GUILayout.Label(string.Format("Ammo ID: {0}", itemGunAsset.getMagazineID()), this.hudItemStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(string.Format("Distance: {0}m", itemGunAsset.range), this.hudItemStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(string.Format("Ballistic Travel: {0}", itemGunAsset.ballisticTravel), this.hudItemStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(string.Format("Ballistic Steps: {0}", itemGunAsset.ballisticSteps), this.hudItemStyle, Array.Empty<GUILayoutOption>());
					float num = (float)itemGunAsset.ballisticSteps * itemGunAsset.ballisticTravel;
					GUILayout.Label(string.Format("Prediction Factor: {0:F1}", num), this.hudItemStyle, Array.Empty<GUILayoutOption>());
				}
			}
			GUILayout.EndVertical();
			if (this.showMenu)
			{
				GUI.DragWindow();
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000B880 File Offset: 0x00009A80
		private void HudAdminsWindow(int id)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Menu.rectHudAdmins.width, 3f), this.sideTabGlow);
			GUI.Label(new Rect(0f, 5f, Menu.rectHudAdmins.width, 20f), this.T("HUD_Admins"), new GUIStyle(this.hudItemStyle)
			{
				alignment = TextAnchor.MiddleCenter
			});
			int num = 0;
			int num2 = 0;
			if (Provider.clients != null)
			{
				foreach (SteamPlayer steamPlayer in Provider.clients)
				{
					if (steamPlayer != null && !(steamPlayer.player == null))
					{
						string text = steamPlayer.playerID.characterName.ToUpperInvariant();
						if (steamPlayer.isAdmin || text.Contains("ADMIN") || text.Contains("MODER") || text.Contains("HELP") || text.Contains("АДМИН") || text.Contains("МОДЕР") || text.Contains("ХЕЛП"))
						{
							num++;
							if (Vector3.Distance(steamPlayer.player.transform.position, Vector3.zero) < 20f)
							{
								num2++;
							}
						}
					}
				}
			}
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label(string.Format("{0} {1}", this.T("AdminsOnline"), num), this.hudItemStyle, Array.Empty<GUILayoutOption>());
			if (num2 > 0)
			{
				this.hudItemStyle.normal.textColor = new Color(1f, 0.3f, 0.3f);
			}
			GUILayout.Label(string.Format("{0} {1}", this.T("AdminsVanish"), num2), this.hudItemStyle, Array.Empty<GUILayoutOption>());
			this.hudItemStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f, 1f);
			GUILayout.EndVertical();
			if (this.showMenu)
			{
				GUI.DragWindow();
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000BABC File Offset: 0x00009CBC
		private void WindowFunction(int id)
		{
			GUISkin skin = GUI.skin;
			if (this.cheatSkin == null && skin != null)
			{
				this.cheatSkin = UnityEngine.Object.Instantiate<GUISkin>(skin);
				this.cheatSkin.hideFlags = (HideFlags)61;
				List<GUIStyle> list = this.cheatSkin.customStyles.ToList<GUIStyle>();
				if (!list.Any<GUIStyle>((GUIStyle s) => s.name == "upbutton"))
				{
					list.Add(new GUIStyle
					{
						name = "upbutton"
					});
				}
				if (!list.Any<GUIStyle>((GUIStyle s) => s.name == "downbutton"))
				{
					list.Add(new GUIStyle
					{
						name = "downbutton"
					});
				}
				if (!list.Any<GUIStyle>((GUIStyle s) => s.name == "leftbutton"))
				{
					list.Add(new GUIStyle
					{
						name = "leftbutton"
					});
				}
				if (!list.Any<GUIStyle>((GUIStyle s) => s.name == "rightbutton"))
				{
					list.Add(new GUIStyle
					{
						name = "rightbutton"
					});
				}
				this.cheatSkin.customStyles = list.ToArray();
			}
			if (this.cheatSkin != null)
			{
				GUI.skin = this.cheatSkin;
			}
			GUI.DrawTexture(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height), this.bgGradient);
			this.DrawAnimatedSnow(new Rect(0f, 0f, this.windowRect.width, this.windowRect.height));
			GUILayout.BeginArea(new Rect(0f, 0f, this.windowRect.width, 58f));
			GUIStyle guistyle = new GUIStyle(GUI.skin.label)
			{
				fontSize = 32,
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.MiddleCenter
			};
			Rect rect;
			rect = new Rect(0f, 0f, this.windowRect.width, 58f);
			GUI.color = new Color(0f, 0f, 0f, 0.8f);
			GUI.Label(new Rect(rect.x + 2f, rect.y + 2f, rect.width, rect.height), "NIGHTMARE", guistyle);
			GUI.color = new Color(1f, 0.9f, 0.9f, 1f);
			GUI.Label(rect, "NIGHTMARE", guistyle);
			GUI.color = Color.white;
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(0f, 58f, 180f, this.windowRect.height - 58f));
			GUI.DrawTexture(new Rect(0f, 0f, 180f, this.windowRect.height - 58f), this.MakeTex(1, 1, new Color(0f, 0f, 0f, 0.3f)));
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			string[] array = new string[] { "Visuals", "Aimbot", "Triggerbot", "Weapon", "Other", "PlayersTab", "Settings", "Custom" };
			float num = (this.windowRect.height - 58f) / (float)array.Length;
			for (int i = 0; i < array.Length; i++)
			{
				Rect rect2 = GUILayoutUtility.GetRect(180f, num);
				if (this.selectedMainTab == i)
				{
					GUI.DrawTexture(rect2, this.sideTabGlow);
					this.sidebarTabStyle.normal.textColor = Color.white;
				}
				else
				{
					this.sidebarTabStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f, 1f);
				}
				if (GUI.Button(rect2, this.T(array[i]), this.sidebarTabStyle))
				{
					this.selectedMainTab = i;
				}
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(180f, 58f, this.windowRect.width - 180f, this.windowRect.height - 58f));
			if (this.selectedMainTab != 0)
			{
				if (this.selectedMainTab == 1)
				{
					this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, this.hiddenScrollbar, this.hiddenScrollbar, this.hiddenScrollbar, new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(true),
						GUILayout.ExpandHeight(true)
					});
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(255f) });
					GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(this.T("GENERAL"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Space(10f);
					bool enabled = this.main.aimbot.enabled;
					bool flag;
					if ((flag = this.DrawToggleSwitch(this.T("Enabled"), this.T("EnabledSub"), enabled)) != enabled)
					{
						this.main.aimbot.enabled = flag;
						Config.Save();
					}
					bool useVisibleCheck = this.main.aimbot.useVisibleCheck;
					bool flag2;
					if ((flag2 = this.DrawToggleSwitch(this.T("VisibleCheck"), this.T("VisibleCheckSub"), useVisibleCheck)) != useVisibleCheck)
					{
						this.main.aimbot.useVisibleCheck = flag2;
						Config.Save();
					}
					bool smooth = this.main.aimbot.smooth;
					bool flag3;
					if ((flag3 = this.DrawToggleSwitch(this.T("SmoothAim"), this.T("SmoothAimSub"), smooth)) != smooth)
					{
						this.main.aimbot.smooth = flag3;
						Config.Save();
					}
					if (flag3)
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Space(15f);
						GUILayout.Label(this.T("SmoothSpeed") + ":", this.subLabelStyle, new GUILayoutOption[] { GUILayout.Width(140f) });
						float num2 = GUILayout.HorizontalSlider(this.main.aimbot.smoothFactor, 1f, 30f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Space(10f);
						GUILayout.Label(string.Format("{0:F1}", num2), this.toggleMainTextStyle, new GUILayoutOption[] { GUILayout.Width(45f) });
						GUILayout.EndHorizontal();
						if (Mathf.Abs(num2 - this.main.aimbot.smoothFactor) > 0.1f)
						{
							this.main.aimbot.smoothFactor = num2;
							Config.Save();
						}
						GUILayout.Space(10f);
					}
					bool noFovMode = this.main.aimbot.noFovMode;
					bool flag4;
					if ((flag4 = this.DrawToggleSwitch(this.T("NoFovMode"), this.T("NoFovModeSub"), noFovMode)) != noFovMode)
					{
						this.main.aimbot.noFovMode = flag4;
						Config.Save();
					}
					bool drawFov = this.main.aimbot.drawFov;
					bool flag5;
					if ((flag5 = this.DrawToggleSwitch(this.T("DrawFOV"), "", drawFov)) != drawFov)
					{
						this.main.aimbot.drawFov = flag5;
						Config.Save();
					}
					bool aimAtZombies = this.main.aimbot.aimAtZombies;
					bool flag6;
					if ((flag6 = this.DrawToggleSwitch(this.T("AimZombies"), "", aimAtZombies)) != aimAtZombies)
					{
						this.main.aimbot.aimAtZombies = flag6;
						Config.Save();
					}
					GUILayout.Space(10f);
					bool useWeaponRange = this.main.aimbot.useWeaponRange;
					bool flag7;
					if ((flag7 = this.DrawToggleSwitch(this.T("UseWeaponRange"), this.T("UseWeaponRangeSub"), useWeaponRange)) != useWeaponRange)
					{
						this.main.aimbot.useWeaponRange = flag7;
						Config.Save();
					}
					float gunRange = Utils.GetGunRange();
					string text = ((gunRange <= 15f) ? this.T("None") : string.Format("{0:F0} m", gunRange));
					GUILayout.Label(this.T("WeapRangeText") + " " + text, this.subLabelStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Space(5f);
					GUI.enabled = !useWeaponRange;
					GUILayout.Label(string.Format("{0}: {1:F0} m", this.T("MaxDist"), this.main.aimbot.customMaxDistance), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
					float num3 = GUILayout.HorizontalSlider(this.main.aimbot.customMaxDistance, 0f, 800f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
					if (Mathf.Abs(num3 - this.main.aimbot.customMaxDistance) > 1f)
					{
						this.main.aimbot.customMaxDistance = Mathf.Round(num3);
						Config.Save();
					}
					GUI.enabled = true;
					GUILayout.EndVertical();
					GUILayout.EndVertical();
					GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(255f) });
					GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(this.T("SELECT"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Space(10f);
					bool usePrediction = this.main.aimbot.usePrediction;
					bool flag8;
					if ((flag8 = this.DrawToggleSwitch(this.T("Prediction"), this.T("PredictionSub"), usePrediction)) != usePrediction)
					{
						this.main.aimbot.usePrediction = flag8;
						Config.Save();
					}
					bool useBallisticPrediction = this.main.aimbot.useBallisticPrediction;
					bool flag9;
					if ((flag9 = this.DrawToggleSwitch(this.T("Ballistic"), this.T("BallisticSub"), useBallisticPrediction)) != useBallisticPrediction)
					{
						this.main.aimbot.useBallisticPrediction = flag9;
						Config.Save();
					}
					bool preferHead = this.main.aimbot.preferHead;
					bool flag10;
					if ((flag10 = this.DrawToggleSwitch(this.T("PreferHead"), this.T("PreferHeadSub"), preferHead)) != preferHead)
					{
						this.main.aimbot.preferHead = flag10;
						Config.Save();
					}
					bool silentAimEnabled = this.main.aimbot.silentAimEnabled;
					bool flag11;
					if ((flag11 = this.DrawToggleSwitch(this.T("SilentAim"), "", silentAimEnabled)) != silentAimEnabled)
					{
						this.main.aimbot.silentAimEnabled = flag11;
						Config.Save();
					}
					bool silentAlwaysHead = this.main.aimbot.silentAlwaysHead;
					bool flag12;
					if ((flag12 = this.DrawToggleSwitch(this.T("SilentHead"), "", silentAlwaysHead)) != silentAlwaysHead)
					{
						this.main.aimbot.silentAlwaysHead = flag12;
						Config.Save();
					}
					GUILayout.Space(10f);
					GUILayout.Label(string.Format("{0}: {1:F1}", this.T("FOV"), this.main.aimbot.fov), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
					float num4 = GUILayout.HorizontalSlider(this.main.aimbot.fov, 5f, 90f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
					if (Mathf.Abs(num4 - this.main.aimbot.fov) > 0.5f)
					{
						this.main.aimbot.fov = num4;
						Config.Save();
					}
					GUILayout.EndVertical();
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
					GUILayout.EndScrollView();
				}
				else if (this.selectedMainTab != 2)
				{
					if (this.selectedMainTab == 3)
					{
						this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, this.hiddenScrollbar, this.hiddenScrollbar, this.hiddenScrollbar, new GUILayoutOption[]
						{
							GUILayout.ExpandWidth(true),
							GUILayout.ExpandHeight(true)
						});
						GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(350f) });
						GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Label(this.T("GUN_ADJ"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Space(10f);
						bool noRecoil = this.main.weaponMods.noRecoil;
						bool flag13;
						if ((flag13 = this.DrawToggleSwitch(this.T("NoRecoil"), this.T("NoRecoilSub"), noRecoil)) != noRecoil)
						{
							this.main.weaponMods.noRecoil = flag13;
							Config.Save();
						}
						if (flag13)
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.Space(15f);
							float num5 = GUILayout.HorizontalSlider(this.main.weaponMods.recoilReduction, 0f, 100f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							GUILayout.Label(string.Format("{0:F0}%", num5), this.toggleMainTextStyle, new GUILayoutOption[] { GUILayout.Width(45f) });
							GUILayout.EndHorizontal();
							if (Mathf.Abs(num5 - this.main.weaponMods.recoilReduction) > 0.5f)
							{
								this.main.weaponMods.recoilReduction = num5;
								Config.Save();
							}
							GUILayout.Space(10f);
						}
						bool noSpread = this.main.weaponMods.noSpread;
						bool flag14;
						if ((flag14 = this.DrawToggleSwitch(this.T("NoSpread"), this.T("NoSpreadSub"), noSpread)) != noSpread)
						{
							this.main.weaponMods.noSpread = flag14;
							Config.Save();
						}
						if (flag14)
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.Space(15f);
							float num6 = GUILayout.HorizontalSlider(this.main.weaponMods.spreadReduction, 0f, 100f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							GUILayout.Label(string.Format("{0:F0}%", num6), this.toggleMainTextStyle, new GUILayoutOption[] { GUILayout.Width(45f) });
							GUILayout.EndHorizontal();
							if (Mathf.Abs(num6 - this.main.weaponMods.spreadReduction) > 0.5f)
							{
								this.main.weaponMods.spreadReduction = num6;
								Config.Save();
							}
							GUILayout.Space(10f);
						}
						bool noSway = this.main.weaponMods.noSway;
						bool flag15;
						if ((flag15 = this.DrawToggleSwitch(this.T("NoSway"), this.T("NoSwaySub"), noSway)) != noSway)
						{
							this.main.weaponMods.noSway = flag15;
							Config.Save();
						}
						if (flag15)
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.Space(15f);
							float num7 = GUILayout.HorizontalSlider(this.main.weaponMods.swayReduction, 0f, 100f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							GUILayout.Label(string.Format("{0:F0}%", num7), this.toggleMainTextStyle, new GUILayoutOption[] { GUILayout.Width(45f) });
							GUILayout.EndHorizontal();
							if (Mathf.Abs(num7 - this.main.weaponMods.swayReduction) > 0.5f)
							{
								this.main.weaponMods.swayReduction = num7;
								Config.Save();
							}
							GUILayout.Space(10f);
						}
						bool noShake = this.main.weaponMods.noShake;
						bool flag16;
						if ((flag16 = this.DrawToggleSwitch(this.T("NoShake"), this.T("NoShakeSub"), noShake)) != noShake)
						{
							this.main.weaponMods.noShake = flag16;
							Config.Save();
						}
						if (flag16)
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.Space(15f);
							float num8 = GUILayout.HorizontalSlider(this.main.weaponMods.shakeReduction, 0f, 100f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							GUILayout.Label(string.Format("{0:F0}%", num8), this.toggleMainTextStyle, new GUILayoutOption[] { GUILayout.Width(45f) });
							GUILayout.EndHorizontal();
							if (Mathf.Abs(num8 - this.main.weaponMods.shakeReduction) > 0.5f)
							{
								this.main.weaponMods.shakeReduction = num8;
								Config.Save();
							}
							GUILayout.Space(10f);
						}
						bool noBulletGravity = this.main.weaponMods.noBulletGravity;
						bool flag17;
						if ((flag17 = this.DrawToggleSwitch(this.T("NoBulGrav"), this.T("NoBulGravSub"), noBulletGravity)) != noBulletGravity)
						{
							this.main.weaponMods.noBulletGravity = flag17;
							Config.Save();
						}
						if (flag17)
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.Space(15f);
							float num9 = GUILayout.HorizontalSlider(this.main.weaponMods.dropReduction, 0f, 100f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							GUILayout.Label(string.Format("{0:F0}%", num9), this.toggleMainTextStyle, new GUILayoutOption[] { GUILayout.Width(45f) });
							GUILayout.EndHorizontal();
							if (Mathf.Abs(num9 - this.main.weaponMods.dropReduction) > 0.5f)
							{
								this.main.weaponMods.dropReduction = num9;
								Config.Save();
							}
							GUILayout.Space(10f);
						}
						GUILayout.EndVertical();
						GUILayout.EndVertical();
						GUILayout.EndScrollView();
					}
					else if (this.selectedMainTab != 4)
					{
						if (this.selectedMainTab == 5)
						{
							string[] array2 = new string[] { "Online", "Friends" };
							float num10 = (this.windowRect.width - 180f) / (float)array2.Length;
							GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Height(40f) });
							for (int j = 0; j < array2.Length; j++)
							{
								Rect rect3 = GUILayoutUtility.GetRect(num10, 40f);
								if (this.selectedPlayersTab == j)
								{
									GUI.DrawTexture(rect3, this.topTabActiveBg);
									this.topTabStyle.normal.textColor = Color.white;
								}
								else
								{
									GUI.DrawTexture(rect3, this.topTabInactiveBg);
									this.topTabStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f, 1f);
								}
								if (GUI.Button(rect3, this.T(array2[j]), this.topTabStyle))
								{
									this.selectedPlayersTab = j;
								}
							}
							GUILayout.EndHorizontal();
							this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, this.hiddenScrollbar, this.hiddenScrollbar, this.hiddenScrollbar, new GUILayoutOption[]
							{
								GUILayout.ExpandWidth(true),
								GUILayout.ExpandHeight(true)
							});
							GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
							if (this.selectedPlayersTab != 0)
							{
								GUILayout.Label(this.T("GLOBAL_FRIENDS"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
								if (Utils.ManualFriends.Count == 0)
								{
									GUILayout.Label("...", this.subLabelStyle, Array.Empty<GUILayoutOption>());
									goto IL_1A98;
								}
								using (List<ulong>.Enumerator enumerator = Utils.ManualFriends.ToList<ulong>().GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										ulong num11 = enumerator.Current;
										GUILayout.BeginHorizontal(new GUIStyle(GUI.skin.box), Array.Empty<GUILayoutOption>());
										GUILayout.Label(string.Format("SteamID64: {0}", num11), this.toggleMainTextStyle, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
										GUI.backgroundColor = new Color(0.8f, 0.15f, 0.25f);
										if (GUILayout.Button(this.T("Remove"), new GUILayoutOption[] { GUILayout.Width(80f) }))
										{
											Utils.ManualFriends.Remove(num11);
											Config.Save();
										}
										GUI.backgroundColor = Color.white;
										GUILayout.EndHorizontal();
									}
									goto IL_1A98;
								}
							}
							GUILayout.Label(this.T("SearchList"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.Label(this.T("Search"), new GUILayoutOption[] { GUILayout.Width(60f) });
							this.menuSearchText = GUILayout.TextField(this.menuSearchText, new GUILayoutOption[] { GUILayout.Height(28f) });
							if (GUILayout.Button(this.T("Clear"), new GUILayoutOption[]
							{
								GUILayout.Width(80f),
								GUILayout.Height(28f)
							}))
							{
								this.menuSearchText = "";
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(10f);
							if (Provider.clients != null && Provider.clients.Count > 1)
							{
								using (List<SteamPlayer>.Enumerator enumerator2 = Provider.clients.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										SteamPlayer sp = enumerator2.Current;
										if (sp != null && !(sp.player == null) && !(sp.player == Player.LocalPlayer))
										{
											string text2 = sp.playerID.characterName + " [" + sp.playerID.playerName + "]";
											if (string.IsNullOrWhiteSpace(this.menuSearchText) || text2.ToLowerInvariant().Contains(this.menuSearchText.ToLowerInvariant()))
											{
												bool flag18 = Player.LocalPlayer.channel.owner.isMemberOfSameGroupAs(sp);
												bool flag19 = Utils.ManualFriends.Contains(sp.playerID.steamID.m_SteamID);
												bool flag20 = sp.isAdmin || sp.playerID.characterName.ToUpperInvariant().Contains("ADMIN") || sp.playerID.characterName.ToUpperInvariant().Contains("MODER") || sp.playerID.characterName.ToUpperInvariant().Contains("HELP") || sp.playerID.characterName.ToUpperInvariant().Contains("АДМИН") || sp.playerID.characterName.ToUpperInvariant().Contains("МОДЕР") || sp.playerID.characterName.ToUpperInvariant().Contains("ХЕЛП");
												string text3 = "";
												if (flag18)
												{
													text3 += " [Group]";
												}
												if (flag19)
												{
													text3 += " [Friend]";
												}
												if (flag20)
												{
													text3 += " [ADMIN]";
												}
												Color color = ((!flag19 && !flag18) ? ((!flag20) ? this.textColor : new Color(0f, 0.85f, 1f)) : new Color(0f, 0.65f, 1f));
												GUI.backgroundColor = ((!flag19) ? new Color(0f, 0f, 0f, 0.4f) : new Color(0.8f, 0.15f, 0.25f, 0.7f));
												GUI.contentColor = color;
												if (GUILayout.Button(text2 + text3, new GUILayoutOption[] { GUILayout.Height(30f) }))
												{
													if (!this.openPlayers.Exists((SteamPlayerID pid) => pid == sp.playerID))
													{
														this.openPlayers.Add(sp.playerID);
													}
													else
													{
														this.openPlayers.Remove(sp.playerID);
													}
												}
												GUI.backgroundColor = Color.white;
												GUI.contentColor = Color.white;
												if (this.openPlayers.Exists((SteamPlayerID pid) => pid == sp.playerID))
												{
													GUILayout.BeginVertical(new GUIStyle(GUI.skin.box), Array.Empty<GUILayoutOption>());
													bool flag21;
													if ((flag21 = this.DrawToggleSwitch(this.T("AddFriend"), this.T("AddFriendSub"), flag19)) != flag19)
													{
														ulong steamID = sp.playerID.steamID.m_SteamID;
														if (!flag21)
														{
															Utils.ManualFriends.Remove(steamID);
														}
														else
														{
															Utils.ManualFriends.Add(steamID);
														}
														Config.Save();
													}
													string text4 = this.T("ShowWeapon");
													string text5 = ": ";
													PlayerEquipment equipment = sp.player.equipment;
													if (equipment == null)
													{
														goto IL_1998;
													}
													ItemAsset asset = equipment.asset;
													if (asset == null)
													{
														goto IL_1998;
													}
													string text6;
													if ((text6 = asset.FriendlyName) == null)
													{
														goto IL_1998;
													}
													IL_19A4:
													GUILayout.Label(text4 + text5 + text6, this.subLabelStyle, Array.Empty<GUILayoutOption>());
													GUILayout.Label(this.T("GroupID") + " " + sp.player.quests.groupID.ToString(), this.subLabelStyle, Array.Empty<GUILayoutOption>());
													GUILayout.Label(this.T("GroupRank") + " " + Enum.GetName(typeof(EPlayerGroupRank), sp.player.quests.groupRank), this.subLabelStyle, Array.Empty<GUILayoutOption>());
													GUILayout.EndVertical();
													goto IL_1A57;
													IL_1998:
													text6 = this.T("None");
													goto IL_19A4;
												}
												IL_1A57:
												GUILayout.Space(2f);
											}
										}
									}
									goto IL_1A98;
								}
							}
							GUILayout.Label(this.T("EmptyServer"), this.subLabelStyle, Array.Empty<GUILayoutOption>());
							IL_1A98:
							GUILayout.EndVertical();
							GUILayout.EndScrollView();
						}
						else if (this.selectedMainTab == 6)
						{
							this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, this.hiddenScrollbar, this.hiddenScrollbar, this.hiddenScrollbar, new GUILayoutOption[]
							{
								GUILayout.ExpandWidth(true),
								GUILayout.ExpandHeight(true)
							});
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(350f) });
							GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Label(this.T("KeybindsHeader"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							this.DrawKeybind(this.T("MenuToggle"), "Menu");
							this.DrawKeybind(this.T("ESP"), "ESP");
							this.DrawKeybind(this.T("Aimbot"), "Aimbot");
							this.DrawKeybind(this.T("AimbotHold"), "AimbotHold");
							this.DrawKeybind(this.T("Triggerbot"), "Triggerbot");
							this.DrawKeybind(this.T("ChatSpam"), "ChatSpam");
							this.DrawKeybind(this.T("NoClipKey"), "VehicleNoClip");
							this.DrawKeybind(this.T("AutoLootKey"), "ItemVacuum");
							this.DrawKeybind(this.T("FreeCamKey"), "FreeCam");
							this.DrawKeybind(this.T("FastDisconnectKey"), "FastDisconnect");
							this.DrawKeybind(this.T("CancelLoadingKey"), "CancelLoading");
							this.DrawKeybind(this.T("PanicKey"), "Panic");
							GUILayout.EndVertical();
							GUILayout.Space(15f);
							GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Label(this.T("AimbotBehHeader"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							bool aimbotHoldToAim = this.main.AimbotHoldToAim;
							bool flag22;
							if ((flag22 = this.DrawToggleSwitch(this.T("HoldToAim"), this.T("HoldToAimSub"), aimbotHoldToAim)) != aimbotHoldToAim)
							{
								this.main.AimbotHoldToAim = flag22;
								Config.Save();
							}
							GUILayout.EndVertical();
							GUILayout.EndVertical();
							GUILayout.EndHorizontal();
							GUILayout.EndScrollView();
						}
						else if (this.selectedMainTab == 7)
						{
							this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, this.hiddenScrollbar, this.hiddenScrollbar, this.hiddenScrollbar, new GUILayoutOption[]
							{
								GUILayout.ExpandWidth(true),
								GUILayout.ExpandHeight(true)
							});
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(255f) });
							GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Label(this.T("LangTitle"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							string[] array3 = new string[] { "English", "Русский", "Polski", "Türkçe" };
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							for (int k = 0; k < array3.Length; k++)
							{
								GUI.backgroundColor = ((Menu.currentLanguage == k) ? new Color(0.8f, 0.15f, 0.25f, 1f) : new Color(0.15f, 0.15f, 0.2f));
								if (GUILayout.Button(array3[k], new GUILayoutOption[] { GUILayout.Height(32f) }))
								{
									Menu.currentLanguage = k;
									Config.Save();
								}
							}
							GUI.backgroundColor = Color.white;
							GUILayout.EndHorizontal();
							GUILayout.Space(20f);
							GUILayout.Label(this.T("CustomTitle"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							GUILayout.Label(string.Format("{0}: {1:F0}px", this.T("WinWidth"), Menu.windowWidth), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
							Menu.windowWidth = GUILayout.HorizontalSlider(Menu.windowWidth, 600f, 1000f, this.hSliderStyle, this.hSliderThumbStyle, new GUILayoutOption[] { GUILayout.Width(220f) });
							GUILayout.Space(10f);
							GUILayout.Label(string.Format("{0}: {1:F0}px", this.T("WinHeight"), Menu.windowHeight), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
							Menu.windowHeight = GUILayout.HorizontalSlider(Menu.windowHeight, 450f, 800f, this.hSliderStyle, this.hSliderThumbStyle, new GUILayoutOption[] { GUILayout.Width(220f) });
							GUILayout.EndVertical();
							GUILayout.EndVertical();
							GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(255f) });
							GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Label(this.T("HUD_Title"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							bool flag23;
							if ((flag23 = this.DrawToggleSwitch(this.T("HUD_Features"), "", Menu.showHudFeatures)) != Menu.showHudFeatures)
							{
								Menu.showHudFeatures = flag23;
								Config.Save();
							}
							if (Menu.showHudFeatures)
							{
								GUILayout.Label(string.Format("Width: {0:F0}px", Menu.hudFeaturesWidth), this.subLabelStyle, Array.Empty<GUILayoutOption>());
								Menu.hudFeaturesWidth = GUILayout.HorizontalSlider(Menu.hudFeaturesWidth, 120f, 350f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
								GUILayout.Space(10f);
							}
							bool flag24;
							if ((flag24 = this.DrawToggleSwitch(this.T("HUD_Weapon"), "", Menu.showHudWeapon)) != Menu.showHudWeapon)
							{
								Menu.showHudWeapon = flag24;
								Config.Save();
							}
							if (Menu.showHudWeapon)
							{
								GUILayout.Label(string.Format("Width: {0:F0}px", Menu.hudWeaponWidth), this.subLabelStyle, Array.Empty<GUILayoutOption>());
								Menu.hudWeaponWidth = GUILayout.HorizontalSlider(Menu.hudWeaponWidth, 160f, 400f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
								GUILayout.Space(10f);
							}
							bool flag25;
							if ((flag25 = this.DrawToggleSwitch(this.T("HUD_Admins"), "", Menu.showHudAdmins)) != Menu.showHudAdmins)
							{
								Menu.showHudAdmins = flag25;
								Config.Save();
							}
							if (Menu.showHudAdmins)
							{
								GUILayout.Label(string.Format("Width: {0:F0}px", Menu.hudAdminsWidth), this.subLabelStyle, Array.Empty<GUILayoutOption>());
								Menu.hudAdminsWidth = GUILayout.HorizontalSlider(Menu.hudAdminsWidth, 120f, 350f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
								GUILayout.Space(10f);
							}
							GUILayout.EndVertical();
							GUILayout.EndVertical();
							GUILayout.EndHorizontal();
							GUILayout.EndScrollView();
						}
					}
					else
					{
						this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, this.hiddenScrollbar, this.hiddenScrollbar, this.hiddenScrollbar, new GUILayoutOption[]
						{
							GUILayout.ExpandWidth(true),
							GUILayout.ExpandHeight(true)
						});
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(255f) });
						GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Label(this.T("MOVEMENT"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Space(10f);
						bool enabled2 = this.main.freeCam.Enabled;
						bool flag26;
						if ((flag26 = this.DrawToggleSwitch(this.T("FreeCam"), this.T("FreeCamSub"), enabled2)) != enabled2)
						{
							this.main.freeCam.Enabled = flag26;
							Config.Save();
						}
						GUILayout.Label(string.Format("{0}: {1:F1}", this.T("Speed"), this.main.freeCam.Speed), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
						this.main.freeCam.Speed = GUILayout.HorizontalSlider(this.main.freeCam.Speed, 1f, 100f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Space(15f);
						bool active = this.main.vehicleNoclip.active;
						bool flag27;
						if ((flag27 = this.DrawToggleSwitch(this.T("VehNoclip"), this.T("VehNoclipSub"), active)) != active)
						{
							this.main.vehicleNoclip.active = flag27;
							Config.Save();
						}
						if (this.main.vehicleNoclip.active)
						{
							GUILayout.Label(string.Format("{0}: ×{1:F1}", this.T("SpeedMult"), this.main.vehicleNoclip.speedMultiplier), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
							float num12 = GUILayout.HorizontalSlider(this.main.vehicleNoclip.speedMultiplier, 0.4f, 3.5f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
							if (Mathf.Abs(num12 - this.main.vehicleNoclip.speedMultiplier) > 0.05f)
							{
								this.main.vehicleNoclip.speedMultiplier = num12;
								Config.Save();
							}
							bool mouseControl = this.main.vehicleNoclip.mouseControl;
							bool flag28;
							if ((flag28 = this.DrawToggleSwitch(this.T("MouseControl"), "", mouseControl)) != mouseControl)
							{
								this.main.vehicleNoclip.mouseControl = flag28;
								Config.Save();
							}
							bool nullRoll = this.main.vehicleNoclip.nullRoll;
							bool flag29;
							if ((flag29 = this.DrawToggleSwitch(this.T("NullRoll"), "", nullRoll)) != nullRoll)
							{
								this.main.vehicleNoclip.nullRoll = flag29;
								Config.Save();
							}
							if (!this.main.vehicleNoclip.nullRoll)
							{
								bool stabilizeRoll = this.main.vehicleNoclip.stabilizeRoll;
								bool flag30;
								if ((flag30 = this.DrawToggleSwitch(this.T("StabilizeRoll"), "", stabilizeRoll)) != stabilizeRoll)
								{
									this.main.vehicleNoclip.stabilizeRoll = flag30;
									Config.Save();
								}
							}
							bool useArrowKeys = this.main.vehicleNoclip.useArrowKeys;
							bool flag31;
							if ((flag31 = this.DrawToggleSwitch(this.T("ArrowKeys"), "", useArrowKeys)) != useArrowKeys)
							{
								this.main.vehicleNoclip.useArrowKeys = flag31;
								Config.Save();
							}
							if (this.main.vehicleNoclip.useArrowKeys)
							{
								GUILayout.Label(string.Format("{0}: {1:F0}°/s", this.T("RotSpeed"), this.main.vehicleNoclip.arrowRotationSpeed), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
								float num13 = GUILayout.HorizontalSlider(this.main.vehicleNoclip.arrowRotationSpeed, 1f, 360f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
								if (Mathf.Abs(num13 - this.main.vehicleNoclip.arrowRotationSpeed) > 1f)
								{
									this.main.vehicleNoclip.arrowRotationSpeed = num13;
									Config.Save();
								}
							}
						}
						GUILayout.EndVertical();
						GUILayout.EndVertical();
						GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(255f) });
						GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Label(this.T("AUTO_LOOT"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Space(10f);
						bool enabled3 = this.main.itemVacuum.Enabled;
						bool flag32;
						if ((flag32 = this.DrawToggleSwitch(this.T("AutoLoot"), this.T("AutoLootSub"), enabled3)) != enabled3)
						{
							this.main.itemVacuum.Enabled = flag32;
							Config.Save();
						}
						if (this.main.itemVacuum.Enabled)
						{
							GUILayout.Label(string.Format("{0}: {1:F0}m", this.T("Range"), this.main.itemVacuum.Range), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
							this.main.itemVacuum.Range = GUILayout.HorizontalSlider(this.main.itemVacuum.Range, 2f, 30f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
							GUILayout.Space(10f);
							bool pickupEverything = this.main.itemVacuum.PickupEverything;
							bool flag33;
							if ((flag33 = this.DrawToggleSwitch(this.T("PickupEverything"), this.T("PickupEverySub"), pickupEverything)) != pickupEverything)
							{
								this.main.itemVacuum.PickupEverything = flag33;
								Config.Save();
							}
							bool pickupWeapons = this.main.itemVacuum.PickupWeapons;
							bool flag34;
							if ((flag34 = this.DrawToggleSwitch(this.T("Weapons"), "", pickupWeapons)) != pickupWeapons)
							{
								this.main.itemVacuum.PickupWeapons = flag34;
								Config.Save();
							}
							bool pickupClothing = this.main.itemVacuum.PickupClothing;
							bool flag35;
							if ((flag35 = this.DrawToggleSwitch(this.T("Clothing"), "", pickupClothing)) != pickupClothing)
							{
								this.main.itemVacuum.PickupClothing = flag35;
								Config.Save();
							}
							bool pickupSupplies = this.main.itemVacuum.PickupSupplies;
							bool flag36;
							if ((flag36 = this.DrawToggleSwitch(this.T("Supplies"), this.T("SuppliesSub"), pickupSupplies)) != pickupSupplies)
							{
								this.main.itemVacuum.PickupSupplies = flag36;
								Config.Save();
							}
						}
						GUILayout.EndVertical();
						GUILayout.Space(10f);
						GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Label(this.T("MISC"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Space(10f);
						bool noDisconnectTimer = this.main.NoDisconnectTimer;
						bool flag37;
						if ((flag37 = this.DrawToggleSwitch(this.T("NoDiscTimer"), this.T("NoDiscTimerSub"), noDisconnectTimer)) != noDisconnectTimer)
						{
							this.main.NoDisconnectTimer = flag37;
							Config.Save();
						}
						bool enabled4 = this.main.chatSpam.Enabled;
						bool flag38;
						if ((flag38 = this.DrawToggleSwitch(this.T("ChatSpam"), "", enabled4)) != enabled4)
						{
							this.main.chatSpam.Enabled = flag38;
							Config.Save();
						}
						GUILayout.Label(this.T("SpamText"), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
						string text7 = GUILayout.TextField(this.main.chatSpam.SpamText, new GUILayoutOption[] { GUILayout.Height(28f) });
						if (text7 != this.main.chatSpam.SpamText)
						{
							this.main.chatSpam.SpamText = text7;
							Config.Save();
						}
						GUILayout.Space(5f);
						GUILayout.Label(string.Format("{0} {1:F2} {2}", this.T("Interval"), this.main.chatSpam.Interval, this.T("Sec")), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
						float num14 = GUILayout.HorizontalSlider(this.main.chatSpam.Interval, 0.1f, 5f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
						if (Mathf.Abs(num14 - this.main.chatSpam.Interval) > 0.02f)
						{
							this.main.chatSpam.Interval = num14;
							Config.Save();
						}
						GUILayout.Space(10f);
						bool bHideOnSpy = Overrides.bHideOnSpy;
						bool flag39;
						if ((flag39 = this.DrawToggleSwitch(this.T("HideSpy"), this.T("HideSpySub"), bHideOnSpy)) != bHideOnSpy)
						{
							Overrides.bHideOnSpy = flag39;
							Config.Save();
						}
						bool bPlaySpySound = Overrides.bPlaySpySound;
						bool flag40;
						if ((flag40 = this.DrawToggleSwitch(this.T("SpySound"), this.T("SpySoundSub"), bPlaySpySound)) != bPlaySpySound)
						{
							Overrides.bPlaySpySound = flag40;
							Config.Save();
						}
						GUILayout.EndVertical();
						GUILayout.EndVertical();
						GUILayout.EndHorizontal();
						GUILayout.EndScrollView();
					}
				}
				else
				{
					this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, this.hiddenScrollbar, this.hiddenScrollbar, this.hiddenScrollbar, new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(true),
						GUILayout.ExpandHeight(true)
					});
					GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(350f) });
					GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(this.T("GENERAL"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Space(10f);
					bool enabled5 = this.main.triggerbot.enabled;
					bool flag41;
					if ((flag41 = this.DrawToggleSwitch(this.T("Enabled"), this.T("AutoShoot"), enabled5)) != enabled5)
					{
						this.main.triggerbot.enabled = flag41;
						Config.Save();
					}
					bool useWeaponRange2 = this.main.triggerbot.useWeaponRange;
					bool flag42;
					if ((flag42 = this.DrawToggleSwitch(this.T("UseWeaponRange"), this.T("UseWeaponRangeSub"), useWeaponRange2)) != useWeaponRange2)
					{
						this.main.triggerbot.useWeaponRange = flag42;
						Config.Save();
					}
					GUI.enabled = !useWeaponRange2;
					GUILayout.Label(string.Format("{0}: {1:F0} m", this.T("MaxDist"), this.main.triggerbot.customMaxDistance), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
					float num15 = GUILayout.HorizontalSlider(this.main.triggerbot.customMaxDistance, 0f, 800f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
					if (Mathf.Abs(num15 - this.main.triggerbot.customMaxDistance) > 2f)
					{
						this.main.triggerbot.customMaxDistance = Mathf.Round(num15);
						Config.Save();
					}
					GUI.enabled = true;
					GUILayout.EndVertical();
					GUILayout.EndVertical();
					GUILayout.EndScrollView();
				}
			}
			else
			{
				string[] array4 = new string[] { "ESP", "World", "Others" };
				float num16 = (this.windowRect.width - 180f) / (float)array4.Length;
				GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Height(40f) });
				for (int l = 0; l < array4.Length; l++)
				{
					Rect rect4 = GUILayoutUtility.GetRect(num16, 40f);
					if (this.selectedVisualTab == l)
					{
						GUI.DrawTexture(rect4, this.topTabActiveBg);
						this.topTabStyle.normal.textColor = Color.white;
					}
					else
					{
						GUI.DrawTexture(rect4, this.topTabInactiveBg);
						this.topTabStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f, 1f);
					}
					if (GUI.Button(rect4, this.T(array4[l]), this.topTabStyle))
					{
						this.selectedVisualTab = l;
					}
				}
				GUILayout.EndHorizontal();
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, this.hiddenScrollbar, this.hiddenScrollbar, this.hiddenScrollbar, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true),
					GUILayout.ExpandHeight(true)
				});
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (this.selectedVisualTab != 0)
				{
					if (this.selectedVisualTab == 1)
					{
						GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(520f) });
						GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Label(this.T("TIME_CHANGER"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Space(10f);
						bool alwaysDay = this.main.visuals.AlwaysDay;
						bool flag43;
						if ((flag43 = this.DrawToggleSwitch(this.T("AlwaysDay"), this.T("AlwaysDaySub"), alwaysDay)) != alwaysDay)
						{
							this.main.visuals.AlwaysDay = flag43;
							Config.Save();
						}
						if (this.main.visuals.AlwaysDay)
						{
							GUILayout.Label(string.Format("{0}: {1:F1}", this.T("DayTime"), this.main.visuals.CustomDayTime / 100f), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
							float num17 = GUILayout.HorizontalSlider(this.main.visuals.CustomDayTime, 0f, 2400f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
							if (Mathf.Abs(num17 - this.main.visuals.CustomDayTime) > 3f)
							{
								this.main.visuals.CustomDayTime = (uint)Mathf.Round(num17);
								Config.Save();
							}
						}
						GUILayout.EndVertical();
						GUILayout.EndVertical();
					}
					else if (this.selectedVisualTab == 2)
					{
						GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(520f) });
						GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Label(this.T("INTERFACE"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
						GUILayout.Space(10f);
						bool alwaysSatellite = this.main.visuals.AlwaysSatellite;
						bool flag44;
						if ((flag44 = this.DrawToggleSwitch(this.T("SatMap"), this.T("SatMapSub"), alwaysSatellite)) != alwaysSatellite)
						{
							this.main.visuals.AlwaysSatellite = flag44;
							Config.Save();
						}
						bool alwaysCompass = this.main.visuals.AlwaysCompass;
						bool flag45;
						if ((flag45 = this.DrawToggleSwitch(this.T("AlwaysCompass"), this.T("AlwaysCompassSub"), alwaysCompass)) != alwaysCompass)
						{
							this.main.visuals.AlwaysCompass = flag45;
							Config.Save();
						}
						GUILayout.EndVertical();
						GUILayout.EndVertical();
					}
				}
				else
				{
					GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(255f) });
					GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(this.T("PlayersHeader"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Space(10f);
					bool espEnabled = this.main.esp.espEnabled;
					bool flag46;
					if ((flag46 = this.DrawToggleSwitch(this.T("EnableESP"), this.T("MasterSwitch"), espEnabled)) != espEnabled)
					{
						this.main.esp.espEnabled = flag46;
						Config.Save();
					}
					bool showSnaplines = this.main.esp.showSnaplines;
					bool flag47;
					if ((flag47 = this.DrawToggleSwitch(this.T("Snaplines"), this.T("SnaplinesSub"), showSnaplines)) != showSnaplines)
					{
						this.main.esp.showSnaplines = flag47;
						Config.Save();
					}
					bool showBoxes = this.main.esp.showBoxes;
					bool flag48;
					if ((flag48 = this.DrawToggleSwitch(this.T("Boxes"), this.T("BoxesSub"), showBoxes)) != showBoxes)
					{
						this.main.esp.showBoxes = flag48;
						Config.Save();
					}
					bool showSkeleton = this.main.esp.showSkeleton;
					bool flag49;
					if ((flag49 = this.DrawToggleSwitch(this.T("Skeleton"), this.T("SkeletonSub"), showSkeleton)) != showSkeleton)
					{
						this.main.esp.showSkeleton = flag49;
						Config.Save();
					}
					bool showGlow = this.main.esp.showGlow;
					bool flag50;
					if ((flag50 = this.DrawToggleSwitch(this.T("PlayerGlow"), this.T("PlayerGlowSub"), showGlow)) != showGlow)
					{
						this.main.esp.showGlow = flag50;
						Config.Save();
					}
					bool showInfoText = this.main.esp.showInfoText;
					bool flag51;
					if ((flag51 = this.DrawToggleSwitch(this.T("ShowInfo"), this.T("ShowInfoSub"), showInfoText)) != showInfoText)
					{
						this.main.esp.showInfoText = flag51;
						Config.Save();
					}
					if (showInfoText)
					{
						bool infoTextShowWeapon = this.main.esp.infoTextShowWeapon;
						bool flag52;
						if ((flag52 = this.DrawToggleSwitch(this.T("ShowWeapon"), this.T("ShowWeaponSub"), infoTextShowWeapon)) != infoTextShowWeapon)
						{
							this.main.esp.infoTextShowWeapon = flag52;
							Config.Save();
						}
						GUILayout.Label(string.Format("{0}: {1}", this.T("TextSize"), this.main.esp.infoTextSize), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
						float num18 = GUILayout.HorizontalSlider((float)this.main.esp.infoTextSize, 8f, 24f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
						if (Mathf.RoundToInt(num18) != this.main.esp.infoTextSize)
						{
							this.main.esp.infoTextSize = Mathf.RoundToInt(num18);
							Config.Save();
						}
						GUILayout.Space(5f);
					}
					GUILayout.Space(10f);
					GUILayout.Label(string.Format("{0}: {1:F0}m", this.T("MaxDist"), this.main.esp.maxDistance), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
					float num19 = GUILayout.HorizontalSlider(this.main.esp.maxDistance, 0f, 2500f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
					if (Mathf.Abs(num19 - this.main.esp.maxDistance) > 5f)
					{
						this.main.esp.maxDistance = num19;
						Config.Save();
					}
					GUILayout.EndVertical();
					GUILayout.EndVertical();
					GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(255f) });
					GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(this.T("ItemsHeader"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Space(10f);
					bool showItemGlow = this.main.esp.showItemGlow;
					bool flag53;
					if ((flag53 = this.DrawToggleSwitch(this.T("ItemsGlow"), this.T("ItemsGlowSub"), showItemGlow)) != showItemGlow)
					{
						this.main.esp.showItemGlow = flag53;
						Config.Save();
					}
					bool showItemIcons = this.main.esp.showItemIcons;
					bool flag54;
					if ((flag54 = this.DrawToggleSwitch(this.T("ItemsIcons"), this.T("ItemsIconsSub"), showItemIcons)) != showItemIcons)
					{
						this.main.esp.showItemIcons = flag54;
						Config.Save();
					}
					bool showItemName = this.main.esp.showItemName;
					bool flag55;
					if ((flag55 = this.DrawToggleSwitch(this.T("ItemsNames"), this.T("ItemsNamesSub"), showItemName)) != showItemName)
					{
						this.main.esp.showItemName = flag55;
						Config.Save();
					}
					if (showItemIcons)
					{
						GUILayout.Label(string.Format("{0}: {1:F2}", this.T("IconScale"), this.main.esp.itemIconScale), this.toggleMainTextStyle, Array.Empty<GUILayoutOption>());
						float num20 = GUILayout.HorizontalSlider(this.main.esp.itemIconScale, 0.5f, 1.5f, this.hSliderStyle, this.hSliderThumbStyle, Array.Empty<GUILayoutOption>());
						if (Mathf.Abs(num20 - this.main.esp.itemIconScale) > 0.01f)
						{
							this.main.esp.itemIconScale = num20;
							Config.Save();
						}
					}
					GUILayout.EndVertical();
					GUILayout.Space(10f);
					GUILayout.BeginVertical(this.panelStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Label(this.T("WorldObjHeader"), this.headerTextStyle, Array.Empty<GUILayoutOption>());
					GUILayout.Space(10f);
					bool showVehicleGlow = this.main.esp.showVehicleGlow;
					bool flag56;
					if ((flag56 = this.DrawToggleSwitch(this.T("VehGlow"), "", showVehicleGlow)) != showVehicleGlow)
					{
						this.main.esp.showVehicleGlow = flag56;
						Config.Save();
					}
					bool showVehicleName = this.main.esp.showVehicleName;
					bool flag57;
					if ((flag57 = this.DrawToggleSwitch(this.T("VehNames"), "", showVehicleName)) != showVehicleName)
					{
						this.main.esp.showVehicleName = flag57;
						Config.Save();
					}
					bool showBedGlow = this.main.esp.showBedGlow;
					bool flag58;
					if ((flag58 = this.DrawToggleSwitch(this.T("BedGlow"), "", showBedGlow)) != showBedGlow)
					{
						this.main.esp.showBedGlow = flag58;
						Config.Save();
					}
					bool showBedName = this.main.esp.showBedName;
					bool flag59;
					if ((flag59 = this.DrawToggleSwitch(this.T("BedNames"), "", showBedName)) != showBedName)
					{
						this.main.esp.showBedName = flag59;
						Config.Save();
					}
					bool showClaimGlow = this.main.esp.showClaimGlow;
					bool flag60;
					if ((flag60 = this.DrawToggleSwitch(this.T("ClaimGlow"), "", showClaimGlow)) != showClaimGlow)
					{
						this.main.esp.showClaimGlow = flag60;
						Config.Save();
					}
					bool showClaimName = this.main.esp.showClaimName;
					bool flag61;
					if ((flag61 = this.DrawToggleSwitch(this.T("ClaimName"), "", showClaimName)) != showClaimName)
					{
						this.main.esp.showClaimName = flag61;
						Config.Save();
					}
					bool showFurnitureGlow = this.main.esp.showFurnitureGlow;
					bool flag62;
					if ((flag62 = this.DrawToggleSwitch(this.T("FurnGlow"), "", showFurnitureGlow)) != showFurnitureGlow)
					{
						this.main.esp.showFurnitureGlow = flag62;
						Config.Save();
					}
					bool showFurnitureName = this.main.esp.showFurnitureName;
					bool flag63;
					if ((flag63 = this.DrawToggleSwitch(this.T("FurnNames"), this.T("FurnGlowSub"), showFurnitureName)) != showFurnitureName)
					{
						this.main.esp.showFurnitureName = flag63;
						Config.Save();
					}
					GUILayout.EndVertical();
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();
				GUILayout.EndScrollView();
			}
			GUILayout.EndArea();
			if (this.cursorTex != null)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				GUI.color = Color.white;
				GUI.DrawTexture(new Rect(mousePosition.x, mousePosition.y, 24f, 24f), this.cursorTex);
			}
			GUI.DragWindow();
			GUI.skin = skin;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000F470 File Offset: 0x0000D670
		private void DrawKeybind(string label, string module)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(label + ":", this.toggleMainTextStyle, new GUILayoutOption[] { GUILayout.Width(160f) });
			KeyCode key = this.GetKey(module);
			string text = ((!(this.RebindingModule == module)) ? key.ToString() : this.T("PressAny"));
			GUI.backgroundColor = ((!(this.RebindingModule == module)) ? new Color(0f, 0f, 0f, 0.4f) : Color.red);
			if (GUILayout.Button(text, new GUILayoutOption[]
			{
				GUILayout.Width(140f),
				GUILayout.Height(30f)
			}))
			{
				this.RebindingModule = module;
			}
			GUI.backgroundColor = Color.white;
			GUILayout.EndHorizontal();
			GUILayout.Space(12f);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000F55C File Offset: 0x0000D75C
		private KeyCode GetKey(string module)
		{
			if (module != null)
			{
				switch (module.Length)
				{
				case 3:
					if (module == "ESP")
					{
						return this.main.KeyToggleESP;
					}
					break;
				case 4:
					if (module == "Menu")
					{
						return this.main.KeyToggleMenu;
					}
					break;
				case 5:
					if (module == "Panic")
					{
						return this.main.KeyPanic;
					}
					break;
				case 6:
					if (module == "Aimbot")
					{
						return this.main.KeyToggleAimbot;
					}
					break;
				case 7:
					if (module == "FreeCam")
					{
						return this.main.KeyFreeCam;
					}
					break;
				case 8:
					if (module == "ChatSpam")
					{
						return this.main.KeyToggleChatSpam;
					}
					break;
				case 10:
				{
					char c = module[0];
					if (c != 'A')
					{
						if (c == 'I')
						{
							if (module == "ItemVacuum")
							{
								return this.main.KeyItemVacuum;
							}
						}
						else if (c == 'T' && module == "Triggerbot")
						{
							return this.main.KeyToggleTrigger;
						}
					}
					else if (module == "AimbotHold")
					{
						return this.main.KeyAimbotHold;
					}
					break;
				}
				case 13:
				{
					char c = module[0];
					if (c == 'C')
					{
						if (module == "CancelLoading")
						{
							return this.main.KeyCancelLoading;
						}
					}
					else if (c == 'V' && module == "VehicleNoClip")
					{
						return this.main.KeyToggleVehicleNoclip;
					}
					break;
				}
				case 14:
					if (module == "FastDisconnect")
					{
						return this.main.KeyFastDisconnect;
					}
					break;
				}
			}
			return 0;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000F724 File Offset: 0x0000D924
		private void SetKey(string module, KeyCode key)
		{
			if (module != null)
			{
				switch (module.Length)
				{
				case 3:
					if (module == "ESP")
					{
						this.main.KeyToggleESP = key;
						return;
					}
					return;
				case 4:
					if (module == "Menu")
					{
						this.main.KeyToggleMenu = key;
						return;
					}
					return;
				case 5:
					if (!(module == "Panic"))
					{
						return;
					}
					this.main.KeyPanic = key;
					break;
				case 6:
					if (module == "Aimbot")
					{
						this.main.KeyToggleAimbot = key;
						return;
					}
					return;
				case 7:
					if (!(module == "FreeCam"))
					{
						return;
					}
					this.main.KeyFreeCam = key;
					return;
				case 8:
					if (!(module == "ChatSpam"))
					{
						return;
					}
					this.main.KeyToggleChatSpam = key;
					return;
				case 9:
				case 11:
				case 12:
					break;
				case 10:
				{
					char c = module[0];
					if (c == 'A')
					{
						if (!(module == "AimbotHold"))
						{
							return;
						}
						this.main.KeyAimbotHold = key;
						return;
					}
					else if (c == 'I')
					{
						if (!(module == "ItemVacuum"))
						{
							return;
						}
						this.main.KeyItemVacuum = key;
						return;
					}
					else
					{
						if (c != 'T')
						{
							return;
						}
						if (!(module == "Triggerbot"))
						{
							return;
						}
						this.main.KeyToggleTrigger = key;
						return;
					}
					break;
				}
				case 13:
				{
					char c = module[0];
					if (c == 'C')
					{
						if (!(module == "CancelLoading"))
						{
							return;
						}
						this.main.KeyCancelLoading = key;
						return;
					}
					else
					{
						if (c != 'V')
						{
							return;
						}
						if (!(module == "VehicleNoClip"))
						{
							return;
						}
						this.main.KeyToggleVehicleNoclip = key;
						return;
					}
					break;
				}
				case 14:
					if (module == "FastDisconnect")
					{
						this.main.KeyFastDisconnect = key;
						return;
					}
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x04000146 RID: 326
		private readonly Main main;

		// Token: 0x04000147 RID: 327
		public bool showMenu;

		// Token: 0x04000148 RID: 328
		public string RebindingModule;

		// Token: 0x04000149 RID: 329
		public static readonly KeyCode[] allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

		// Token: 0x0400014A RID: 330
		public static float windowWidth = 720f;

		// Token: 0x0400014B RID: 331
		public static float windowHeight = 575f;

		// Token: 0x0400014C RID: 332
		private Rect windowRect = new Rect(40f, 40f, 720f, 575f);

		// Token: 0x0400014D RID: 333
		private GUISkin cheatSkin;

		// Token: 0x0400014E RID: 334
		public static bool showHudFeatures = false;

		// Token: 0x0400014F RID: 335
		public static bool showHudWeapon = false;

		// Token: 0x04000150 RID: 336
		public static bool showHudAdmins = false;

		// Token: 0x04000151 RID: 337
		public static float hudFeaturesWidth = 160f;

		// Token: 0x04000152 RID: 338
		public static float hudWeaponWidth = 220f;

		// Token: 0x04000153 RID: 339
		public static float hudAdminsWidth = 180f;

		// Token: 0x04000154 RID: 340
		public static Rect rectHudFeatures = new Rect(20f, 20f, 160f, 20f);

		// Token: 0x04000155 RID: 341
		public static Rect rectHudWeapon = new Rect(20f, 300f, 220f, 20f);

		// Token: 0x04000156 RID: 342
		public static Rect rectHudAdmins = new Rect(20f, 500f, 180f, 20f);

		// Token: 0x04000157 RID: 343
		private GUIStyle hudWindowStyle;

		// Token: 0x04000158 RID: 344
		private GUIStyle hudItemStyle;

		// Token: 0x04000159 RID: 345
		public static int currentLanguage = 0;

		// Token: 0x0400015A RID: 346
		private readonly Color textColor = new Color(0.95f, 0.96f, 1f, 1f);

		// Token: 0x0400015B RID: 347
		private GUIStyle panelStyle;

		// Token: 0x0400015C RID: 348
		private GUIStyle headerTextStyle;

		// Token: 0x0400015D RID: 349
		private GUIStyle subLabelStyle;

		// Token: 0x0400015E RID: 350
		private GUIStyle toggleMainTextStyle;

		// Token: 0x0400015F RID: 351
		private GUIStyle sidebarTabStyle;

		// Token: 0x04000160 RID: 352
		private GUIStyle topTabStyle;

		// Token: 0x04000161 RID: 353
		private GUIStyle hiddenScrollbar;

		// Token: 0x04000162 RID: 354
		private GUIStyle hSliderStyle;

		// Token: 0x04000163 RID: 355
		private GUIStyle hSliderThumbStyle;

		// Token: 0x04000164 RID: 356
		private int selectedMainTab;

		// Token: 0x04000165 RID: 357
		private int selectedVisualTab;

		// Token: 0x04000166 RID: 358
		private int selectedPlayersTab;

		// Token: 0x04000167 RID: 359
		private string menuSearchText = "";

		// Token: 0x04000168 RID: 360
		private List<SteamPlayerID> openPlayers = new List<SteamPlayerID>();

		// Token: 0x04000169 RID: 361
		private Vector2 scrollPos;

		// Token: 0x0400016A RID: 362
		private Dictionary<string, float> toggleAnims = new Dictionary<string, float>();

		// Token: 0x0400016B RID: 363
		private Texture2D bgGradient;

		// Token: 0x0400016C RID: 364
		private Texture2D sideTabGlow;

		// Token: 0x0400016D RID: 365
		private Texture2D topTabActiveBg;

		// Token: 0x0400016E RID: 366
		private Texture2D topTabInactiveBg;

		// Token: 0x0400016F RID: 367
		private Texture2D toggleBgOn;

		// Token: 0x04000170 RID: 368
		private Texture2D toggleBgOff;

		// Token: 0x04000171 RID: 369
		private Texture2D toggleKnobOn;

		// Token: 0x04000172 RID: 370
		private Texture2D toggleKnobOff;

		// Token: 0x04000173 RID: 371
		private Texture2D sliderKnobTex;

		// Token: 0x04000174 RID: 372
		private Texture2D cursorTex;

		// Token: 0x04000175 RID: 373
		private List<Menu.Snowflake> snow;

		// Token: 0x04000176 RID: 374
		private bool stylesInitialized;

		// Token: 0x04000177 RID: 375
		private static readonly Dictionary<string, Dictionary<int, string>> lang = new Dictionary<string, Dictionary<int, string>>
		{
			{
				"Visuals",
				new Dictionary<int, string>
				{
					{ 0, "Visuals" },
					{ 1, "Визуал" },
					{ 2, "Wizualia" },
					{ 3, "Görseller" }
				}
			},
			{
				"Aimbot",
				new Dictionary<int, string>
				{
					{ 0, "Aimbot" },
					{ 1, "Аимбот" },
					{ 2, "Aimbot" },
					{ 3, "Aimbot" }
				}
			},
			{
				"Triggerbot",
				new Dictionary<int, string>
				{
					{ 0, "Triggerbot" },
					{ 1, "Триггербот" },
					{ 2, "Triggerbot" },
					{ 3, "Tetikçi" }
				}
			},
			{
				"Weapon",
				new Dictionary<int, string>
				{
					{ 0, "Weapon" },
					{ 1, "Оружие" },
					{ 2, "Broń" },
					{ 3, "Silah" }
				}
			},
			{
				"Other",
				new Dictionary<int, string>
				{
					{ 0, "Other" },
					{ 1, "Разное" },
					{ 2, "Inne" },
					{ 3, "Diğer" }
				}
			},
			{
				"PlayersTab",
				new Dictionary<int, string>
				{
					{ 0, "Players" },
					{ 1, "Игроки" },
					{ 2, "Gracze" },
					{ 3, "Oyuncular" }
				}
			},
			{
				"Settings",
				new Dictionary<int, string>
				{
					{ 0, "Settings" },
					{ 1, "Настройки" },
					{ 2, "Ustawienia" },
					{ 3, "Ayarlar" }
				}
			},
			{
				"Custom",
				new Dictionary<int, string>
				{
					{ 0, "Custom" },
					{ 1, "Кастом" },
					{ 2, "Własne" },
					{ 3, "Özel" }
				}
			},
			{
				"ESP",
				new Dictionary<int, string>
				{
					{ 0, "ESP" },
					{ 1, "ЕСП" },
					{ 2, "ESP" },
					{ 3, "ESP" }
				}
			},
			{
				"World",
				new Dictionary<int, string>
				{
					{ 0, "World" },
					{ 1, "Мир" },
					{ 2, "Świat" },
					{ 3, "Dünya" }
				}
			},
			{
				"Others",
				new Dictionary<int, string>
				{
					{ 0, "Others" },
					{ 1, "Прочее" },
					{ 2, "Inne" },
					{ 3, "Diğer" }
				}
			},
			{
				"PlayersHeader",
				new Dictionary<int, string>
				{
					{ 0, "PLAYERS" },
					{ 1, "ИГРОКИ" },
					{ 2, "GRACZE" },
					{ 3, "OYUNCULAR" }
				}
			},
			{
				"EnableESP",
				new Dictionary<int, string>
				{
					{ 0, "Enable ESP" },
					{ 1, "Включить ЕСП" },
					{ 2, "Włącz ESP" },
					{ 3, "ESP Aktif" }
				}
			},
			{
				"MasterSwitch",
				new Dictionary<int, string>
				{
					{ 0, "Master ESP switch" },
					{ 1, "Главный переключатель" },
					{ 2, "Główny przełącznik" },
					{ 3, "Ana anahtar" }
				}
			},
			{
				"Snaplines",
				new Dictionary<int, string>
				{
					{ 0, "Snaplines" },
					{ 1, "Линии до игроков" },
					{ 2, "Linie (Snaplines)" },
					{ 3, "Çizgiler" }
				}
			},
			{
				"SnaplinesSub",
				new Dictionary<int, string>
				{
					{ 0, "Draw lines to players" },
					{ 1, "Рисует линии от центра" },
					{ 2, "Rysuj linie do graczy" },
					{ 3, "Oyunculara çizgi çeker" }
				}
			},
			{
				"Boxes",
				new Dictionary<int, string>
				{
					{ 0, "Boxes" },
					{ 1, "Боксы" },
					{ 2, "Pudełka (Boxes)" },
					{ 3, "Kutular" }
				}
			},
			{
				"BoxesSub",
				new Dictionary<int, string>
				{
					{ 0, "2D bounding boxes" },
					{ 1, "Квадраты вокруг врагов" },
					{ 2, "Kwadraty 2D" },
					{ 3, "2D kutu çizer" }
				}
			},
			{
				"Skeleton",
				new Dictionary<int, string>
				{
					{ 0, "Skeleton" },
					{ 1, "Скелет" },
					{ 2, "Szkielet" },
					{ 3, "İskelet" }
				}
			},
			{
				"SkeletonSub",
				new Dictionary<int, string>
				{
					{ 0, "Bone ESP" },
					{ 1, "Показывает кости игроков" },
					{ 2, "Pokazuje kości" },
					{ 3, "Kemikleri gösterir" }
				}
			},
			{
				"PlayerGlow",
				new Dictionary<int, string>
				{
					{ 0, "Player Glow" },
					{ 1, "Подсветка игроков" },
					{ 2, "Podświetlenie graczy" },
					{ 3, "Oyuncu Parlaması" }
				}
			},
			{
				"PlayerGlowSub",
				new Dictionary<int, string>
				{
					{ 0, "Chams effect" },
					{ 1, "Эффект заливки (Chams)" },
					{ 2, "Efekt Chams" },
					{ 3, "Chams efekti" }
				}
			},
			{
				"ShowInfo",
				new Dictionary<int, string>
				{
					{ 0, "Show Info" },
					{ 1, "Информация" },
					{ 2, "Pokaż info" },
					{ 3, "Bilgi Göster" }
				}
			},
			{
				"ShowInfoSub",
				new Dictionary<int, string>
				{
					{ 0, "Name and distance" },
					{ 1, "Имя и дистанция" },
					{ 2, "Imię i dystans" },
					{ 3, "İsim ve mesafe" }
				}
			},
			{
				"ShowWeapon",
				new Dictionary<int, string>
				{
					{ 0, "Show Weapon" },
					{ 1, "Оружие в руках" },
					{ 2, "Pokaż broń" },
					{ 3, "Silah Göster" }
				}
			},
			{
				"ShowWeaponSub",
				new Dictionary<int, string>
				{
					{ 0, "Active item in hands" },
					{ 1, "Показывает текущий предмет" },
					{ 2, "Aktywny przedmiot w rękach" },
					{ 3, "Eldeki aktif eşya" }
				}
			},
			{
				"TextSize",
				new Dictionary<int, string>
				{
					{ 0, "Text Size" },
					{ 1, "Размер текста" },
					{ 2, "Rozmiar tekstu" },
					{ 3, "Metin Boyutu" }
				}
			},
			{
				"MaxDist",
				new Dictionary<int, string>
				{
					{ 0, "Max Distance" },
					{ 1, "Макс. дистанция" },
					{ 2, "Maks. dystans" },
					{ 3, "Maks. Mesafe" }
				}
			},
			{
				"ItemsHeader",
				new Dictionary<int, string>
				{
					{ 0, "ITEMS" },
					{ 1, "ПРЕДМЕТЫ" },
					{ 2, "PRZEDMIOTY" },
					{ 3, "EŞYALAR" }
				}
			},
			{
				"ItemsGlow",
				new Dictionary<int, string>
				{
					{ 0, "Items Glow" },
					{ 1, "Подсветка лута" },
					{ 2, "Podświetlenie łupu" },
					{ 3, "Eşya Parlaması" }
				}
			},
			{
				"ItemsGlowSub",
				new Dictionary<int, string>
				{
					{ 0, "Highlight loot" },
					{ 1, "Свечение предметов" },
					{ 2, "Podświetla przedmioty" },
					{ 3, "Ganimeti vurgular" }
				}
			},
			{
				"ItemsIcons",
				new Dictionary<int, string>
				{
					{ 0, "Items Icons" },
					{ 1, "Иконки лута" },
					{ 2, "Ikony przedmiotów" },
					{ 3, "Eşya Simgeleri" }
				}
			},
			{
				"ItemsIconsSub",
				new Dictionary<int, string>
				{
					{ 0, "Draw 2D images" },
					{ 1, "Показывает картинки" },
					{ 2, "Rysuje obrazki 2D" },
					{ 3, "2D resim çizer" }
				}
			},
			{
				"ItemsNames",
				new Dictionary<int, string>
				{
					{ 0, "Items Names" },
					{ 1, "Названия лута" },
					{ 2, "Nazwy przedmiotów" },
					{ 3, "Eşya İsimleri" }
				}
			},
			{
				"ItemsNamesSub",
				new Dictionary<int, string>
				{
					{ 0, "Text ESP" },
					{ 1, "Текстовое ЕСП" },
					{ 2, "Tekstowy ESP" },
					{ 3, "Metin ESP" }
				}
			},
			{
				"IconScale",
				new Dictionary<int, string>
				{
					{ 0, "Icon Scale" },
					{ 1, "Размер иконок" },
					{ 2, "Skala ikon" },
					{ 3, "Simge Ölçeği" }
				}
			},
			{
				"WorldObjHeader",
				new Dictionary<int, string>
				{
					{ 0, "WORLD OBJECTS" },
					{ 1, "ОБЪЕКТЫ МИРА" },
					{ 2, "OBIEKTY ŚWIATA" },
					{ 3, "DÜNYA NESNELERİ" }
				}
			},
			{
				"VehGlow",
				new Dictionary<int, string>
				{
					{ 0, "Vehicle Glow" },
					{ 1, "Подсветка машин" },
					{ 2, "Podświetlenie pojazdów" },
					{ 3, "Araç Parlaması" }
				}
			},
			{
				"VehNames",
				new Dictionary<int, string>
				{
					{ 0, "Vehicle Names" },
					{ 1, "Названия машин" },
					{ 2, "Nazwy pojazdów" },
					{ 3, "Araç İsimleri" }
				}
			},
			{
				"BedGlow",
				new Dictionary<int, string>
				{
					{ 0, "Bedroll Glow" },
					{ 1, "Подсветка спальников" },
					{ 2, "Podświetlenie śpiworów" },
					{ 3, "Yatak Parlaması" }
				}
			},
			{
				"BedNames",
				new Dictionary<int, string>
				{
					{ 0, "Bedroll Names" },
					{ 1, "Названия спальников" },
					{ 2, "Nazwy śpiworów" },
					{ 3, "Yatak İsimleri" }
				}
			},
			{
				"FurnGlow",
				new Dictionary<int, string>
				{
					{ 0, "Furniture Glow" },
					{ 1, "Подсветка мебели" },
					{ 2, "Podświetlenie mebli" },
					{ 3, "Mobilya Parlaması" }
				}
			},
			{
				"FurnGlowSub",
				new Dictionary<int, string>
				{
					{ 0, "Safes, lockers" },
					{ 1, "Сейфы, шкафы" },
					{ 2, "Sejfy, szafki" },
					{ 3, "Kasalar, dolaplar" }
				}
			},
			{
				"AlwaysDay",
				new Dictionary<int, string>
				{
					{ 0, "Always Day" },
					{ 1, "Всегда день" },
					{ 2, "Zawsze dzień" },
					{ 3, "Daima Gündüz" }
				}
			},
			{
				"AlwaysDaySub",
				new Dictionary<int, string>
				{
					{ 0, "Override server time" },
					{ 1, "Переопределяет время сервера" },
					{ 2, "Nadpisuje czas serwera" },
					{ 3, "Sunucu zamanını ezer" }
				}
			},
			{
				"DayTime",
				new Dictionary<int, string>
				{
					{ 0, "Day Time" },
					{ 1, "Время дня" },
					{ 2, "Pora dnia" },
					{ 3, "Gündüz Vakti" }
				}
			},
			{
				"SatMap",
				new Dictionary<int, string>
				{
					{ 0, "Satellite Map" },
					{ 1, "Спутниковая карта" },
					{ 2, "Mapa satelitarna" },
					{ 3, "Uydu Haritası" }
				}
			},
			{
				"SatMapSub",
				new Dictionary<int, string>
				{
					{ 0, "Show full GPS map" },
					{ 1, "Открывает всю GPS карту" },
					{ 2, "Pokaż pełną mapę GPS" },
					{ 3, "Tam GPS haritasını göster" }
				}
			},
			{
				"AlwaysCompass",
				new Dictionary<int, string>
				{
					{ 0, "Always Compass" },
					{ 1, "Всегда компас" },
					{ 2, "Zawsze kompas" },
					{ 3, "Daima Pusula" }
				}
			},
			{
				"AlwaysCompassSub",
				new Dictionary<int, string>
				{
					{ 0, "Force compass HUD" },
					{ 1, "Принудительно показывает компас" },
					{ 2, "Wymusza HUD kompasu" },
					{ 3, "Pusula HUD'ını zorla" }
				}
			},
			{
				"Enabled",
				new Dictionary<int, string>
				{
					{ 0, "Enabled" },
					{ 1, "Включить" },
					{ 2, "Włączony" },
					{ 3, "Etkin" }
				}
			},
			{
				"EnabledSub",
				new Dictionary<int, string>
				{
					{ 0, "Master switch" },
					{ 1, "Главный рубильник" },
					{ 2, "Główny przełącznik" },
					{ 3, "Ana anahtar" }
				}
			},
			{
				"VisibleCheck",
				new Dictionary<int, string>
				{
					{ 0, "Visible Check" },
					{ 1, "Проверка видимости" },
					{ 2, "Sprawdzanie widoczności" },
					{ 3, "Görünürlük Kontrolü" }
				}
			},
			{
				"VisibleCheckSub",
				new Dictionary<int, string>
				{
					{ 0, "Check walls" },
					{ 1, "Не стрелять через стены" },
					{ 2, "Sprawdzaj ściany" },
					{ 3, "Duvarları kontrol et" }
				}
			},
			{
				"SmoothAim",
				new Dictionary<int, string>
				{
					{ 0, "Smooth Aim" },
					{ 1, "Плавная наводка" },
					{ 2, "Płynne celowanie" },
					{ 3, "Pürüzsüz Nişan" }
				}
			},
			{
				"SmoothAimSub",
				new Dictionary<int, string>
				{
					{ 0, "Humanized tracking" },
					{ 1, "Имитирует мышку" },
					{ 2, "Udawaj ruch myszki" },
					{ 3, "İnsansı takip" }
				}
			},
			{
				"SmoothSpeed",
				new Dictionary<int, string>
				{
					{ 0, "Smooth Speed" },
					{ 1, "Скорость сглаживания" },
					{ 2, "Prędkość celowania" },
					{ 3, "Pürüzsüzlük Hızı" }
				}
			},
			{
				"NoFovMode",
				new Dictionary<int, string>
				{
					{ 0, "No FOV Mode" },
					{ 1, "Без ограничений FOV" },
					{ 2, "Brak limitu FOV" },
					{ 3, "FOV Sınırı Yok" }
				}
			},
			{
				"NoFovModeSub",
				new Dictionary<int, string>
				{
					{ 0, "360° Silent Aim" },
					{ 1, "Сайлент аим на 360°" },
					{ 2, "360° Silent Aim" },
					{ 3, "360° Silent Aim" }
				}
			},
			{
				"UseWeaponRange",
				new Dictionary<int, string>
				{
					{ 0, "Use Weapon Range" },
					{ 1, "Дистанция по оружию" },
					{ 2, "Użyj zasięgu broni" },
					{ 3, "Silah Menzilini Kullan" }
				}
			},
			{
				"UseWeaponRangeSub",
				new Dictionary<int, string>
				{
					{ 0, "Auto range limit" },
					{ 1, "Дистанция зависит от пушки в руках" },
					{ 2, "Auto limit zasięgu" },
					{ 3, "Otomatik menzil sınırı" }
				}
			},
			{
				"Prediction",
				new Dictionary<int, string>
				{
					{ 0, "Prediction" },
					{ 1, "Упреждение на ход" },
					{ 2, "Przewidywanie" },
					{ 3, "Öngörü" }
				}
			},
			{
				"PredictionSub",
				new Dictionary<int, string>
				{
					{ 0, "Velocity prediction" },
					{ 1, "Упреждение по скорости" },
					{ 2, "Przewidywanie prędkości" },
					{ 3, "Hız öngörüsü" }
				}
			},
			{
				"Ballistic",
				new Dictionary<int, string>
				{
					{ 0, "Ballistic Prediction" },
					{ 1, "Баллистика (Падение)" },
					{ 2, "Opadanie kuli" },
					{ 3, "Balistik Öngörü" }
				}
			},
			{
				"BallisticSub",
				new Dictionary<int, string>
				{
					{ 0, "Drop prediction" },
					{ 1, "Учет падения пули" },
					{ 2, "Przewidywanie opadania" },
					{ 3, "Düşüş öngörüsü" }
				}
			},
			{
				"PreferHead",
				new Dictionary<int, string>
				{
					{ 0, "Prefer Head" },
					{ 1, "Фокус в голову" },
					{ 2, "Preferuj głowę" },
					{ 3, "Kafaya Odaklan" }
				}
			},
			{
				"PreferHeadSub",
				new Dictionary<int, string>
				{
					{ 0, "Aim for the skull" },
					{ 1, "Стараться целиться в голову" },
					{ 2, "Celuj w czaszkę" },
					{ 3, "Kafatasına nişan al" }
				}
			},
			{
				"FOV",
				new Dictionary<int, string>
				{
					{ 0, "FOV" },
					{ 1, "Радиус обзора (FOV)" },
					{ 2, "FOV" },
					{ 3, "Görüş Alanı (FOV)" }
				}
			},
			{
				"AutoShoot",
				new Dictionary<int, string>
				{
					{ 0, "Auto shoot" },
					{ 1, "Автоматический выстрел" },
					{ 2, "Automatyczny strzał" },
					{ 3, "Otomatik ateş" }
				}
			},
			{
				"NoRecoil",
				new Dictionary<int, string>
				{
					{ 0, "No Recoil" },
					{ 1, "Без отдачи" },
					{ 2, "Brak odrzutu" },
					{ 3, "Geri Tepme Yok" }
				}
			},
			{
				"NoRecoilSub",
				new Dictionary<int, string>
				{
					{ 0, "Remove vertical/horizontal kick" },
					{ 1, "Убирает тряску при стрельбе" },
					{ 2, "Usuwa odrzut" },
					{ 3, "Geri tekmeyi kaldırır" }
				}
			},
			{
				"NoSpread",
				new Dictionary<int, string>
				{
					{ 0, "No Spread" },
					{ 1, "Без разброса" },
					{ 2, "Brak rozrzutu" },
					{ 3, "Dağılma Yok" }
				}
			},
			{
				"NoSpreadSub",
				new Dictionary<int, string>
				{
					{ 0, "100% accuracy" },
					{ 1, "Пули летят ровно в точку" },
					{ 2, "100% celności" },
					{ 3, "%100 isabet" }
				}
			},
			{
				"NoSway",
				new Dictionary<int, string>
				{
					{ 0, "No Sway" },
					{ 1, "Без раскачивания" },
					{ 2, "Brak kołysania" },
					{ 3, "Sallanma Yok" }
				}
			},
			{
				"NoSwaySub",
				new Dictionary<int, string>
				{
					{ 0, "Remove scope breathing" },
					{ 1, "Прицел не трясется при дыхании" },
					{ 2, "Usuwa kołysanie celownika" },
					{ 3, "Dürbün sallanmasını kaldırır" }
				}
			},
			{
				"NoShake",
				new Dictionary<int, string>
				{
					{ 0, "No Shake" },
					{ 1, "Без тряски экрана" },
					{ 2, "Brak trzęsienia" },
					{ 3, "Ekran Titremesi Yok" }
				}
			},
			{
				"NoShakeSub",
				new Dictionary<int, string>
				{
					{ 0, "Remove screen shake" },
					{ 1, "Экран не дергается" },
					{ 2, "Usuwa trzęsienie ekranu" },
					{ 3, "Ekran titremesini kaldırır" }
				}
			},
			{
				"NoBulGrav",
				new Dictionary<int, string>
				{
					{ 0, "No Bullet Gravity" },
					{ 1, "Без гравитации пуль" },
					{ 2, "Brak grawitacji kuli" },
					{ 3, "Mermi Yerçekimi Yok" }
				}
			},
			{
				"NoBulGravSub",
				new Dictionary<int, string>
				{
					{ 0, "Straight bullets" },
					{ 1, "Пули летят по прямой" },
					{ 2, "Kule lecą prosto" },
					{ 3, "Mermiler düz gider" }
				}
			},
			{
				"FreeCam",
				new Dictionary<int, string>
				{
					{ 0, "Free Camera" },
					{ 1, "Свободная камера" },
					{ 2, "Wolna kamera" },
					{ 3, "Serbest Kamera" }
				}
			},
			{
				"FreeCamSub",
				new Dictionary<int, string>
				{
					{ 0, "Fly around" },
					{ 1, "Полет камерой по карте" },
					{ 2, "Lataj dookoła" },
					{ 3, "Etrafta uç" }
				}
			},
			{
				"Speed",
				new Dictionary<int, string>
				{
					{ 0, "Speed" },
					{ 1, "Скорость" },
					{ 2, "Prędkość" },
					{ 3, "Hız" }
				}
			},
			{
				"VehNoclip",
				new Dictionary<int, string>
				{
					{ 0, "Vehicle NoClip" },
					{ 1, "Ноуклип на машинах" },
					{ 2, "Noclip pojazdu" },
					{ 3, "Araç Noclip" }
				}
			},
			{
				"VehNoclipSub",
				new Dictionary<int, string>
				{
					{ 0, "Fly in cars" },
					{ 1, "Полет сквозь стены на машине" },
					{ 2, "Lataj samochodami" },
					{ 3, "Arabalarla uç" }
				}
			},
			{
				"SpeedMult",
				new Dictionary<int, string>
				{
					{ 0, "Speed Multiplier" },
					{ 1, "Множитель скорости" },
					{ 2, "Mnożnik prędkości" },
					{ 3, "Hız Çarpanı" }
				}
			},
			{
				"MouseControl",
				new Dictionary<int, string>
				{
					{ 0, "Mouse Control" },
					{ 1, "Управление мышкой" },
					{ 2, "Sterowanie myszą" },
					{ 3, "Fare Kontrolü" }
				}
			},
			{
				"NullRoll",
				new Dictionary<int, string>
				{
					{ 0, "Null Roll" },
					{ 1, "Заморозка вращения" },
					{ 2, "Zablokuj obrót" },
					{ 3, "Dönüşü Kilitle" }
				}
			},
			{
				"AutoLoot",
				new Dictionary<int, string>
				{
					{ 0, "Enable Auto Loot" },
					{ 1, "Включить Автолут" },
					{ 2, "Włącz Auto Loot" },
					{ 3, "Oto Toplamayı Aç" }
				}
			},
			{
				"AutoLootSub",
				new Dictionary<int, string>
				{
					{ 0, "Suck items" },
					{ 1, "Пылесосит предметы вокруг" },
					{ 2, "Przyciągaj przedmioty" },
					{ 3, "Eşyaları çeker" }
				}
			},
			{
				"Range",
				new Dictionary<int, string>
				{
					{ 0, "Range" },
					{ 1, "Радиус" },
					{ 2, "Zasięg" },
					{ 3, "Menzil" }
				}
			},
			{
				"Weapons",
				new Dictionary<int, string>
				{
					{ 0, "Weapons" },
					{ 1, "Оружие" },
					{ 2, "Bronie" },
					{ 3, "Silahlar" }
				}
			},
			{
				"Clothing",
				new Dictionary<int, string>
				{
					{ 0, "Clothing" },
					{ 1, "Одежда" },
					{ 2, "Ubrania" },
					{ 3, "Kıyafetler" }
				}
			},
			{
				"Supplies",
				new Dictionary<int, string>
				{
					{ 0, "Supplies" },
					{ 1, "Расходники" },
					{ 2, "Zasoby" },
					{ 3, "Erzaklar" }
				}
			},
			{
				"SuppliesSub",
				new Dictionary<int, string>
				{
					{ 0, "Meds, food" },
					{ 1, "Медикаменты, еда" },
					{ 2, "Medykamenty, jedzenie" },
					{ 3, "İlaç, yemek" }
				}
			},
			{
				"HideSpy",
				new Dictionary<int, string>
				{
					{ 0, "Hide on Spy" },
					{ 1, "скрывать чит при скриншотах" },
					{ 2, "Ukryj przed Spy" },
					{ 3, "Casustan Gizle" }
				}
			},
			{
				"HideSpySub",
				new Dictionary<int, string>
				{
					{ 0, "Anti-Screenshot" },
					{ 1, "Защита от скриншотов админами /Spy" },
					{ 2, "Anty-Screenshot" },
					{ 3, "Ekran Görüntüsü Koruması" }
				}
			},
			{
				"SpySound",
				new Dictionary<int, string>
				{
					{ 0, "Spy Sound" },
					{ 1, "Звук при скриншоте" },
					{ 2, "Dźwięk Spy" },
					{ 3, "Casus Sesi" }
				}
			},
			{
				"SpySoundSub",
				new Dictionary<int, string>
				{
					{ 0, "Notification" },
					{ 1, "Уведомление при проверке" },
					{ 2, "Powiadomienie" },
					{ 3, "Bildirim" }
				}
			},
			{
				"SearchList",
				new Dictionary<int, string>
				{
					{ 0, "SEARCH & LIST" },
					{ 1, "ПОИСК И СПИСОК" },
					{ 2, "WYSZUKIWANIE I LISTA" },
					{ 3, "ARAMA & LİSTE" }
				}
			},
			{
				"Online",
				new Dictionary<int, string>
				{
					{ 0, "Online" },
					{ 1, "На сервере" },
					{ 2, "Online" },
					{ 3, "Çevrimiçi" }
				}
			},
			{
				"Friends",
				new Dictionary<int, string>
				{
					{ 0, "Friends" },
					{ 1, "Друзья" },
					{ 2, "Znajomi" },
					{ 3, "Arkadaşlar" }
				}
			},
			{
				"Search",
				new Dictionary<int, string>
				{
					{ 0, "Search:" },
					{ 1, "Поиск:" },
					{ 2, "Szukaj:" },
					{ 3, "Ara:" }
				}
			},
			{
				"Clear",
				new Dictionary<int, string>
				{
					{ 0, "Clear" },
					{ 1, "Очистить" },
					{ 2, "Wyczyść" },
					{ 3, "Temizle" }
				}
			},
			{
				"EmptyServer",
				new Dictionary<int, string>
				{
					{ 0, "Server is empty" },
					{ 1, "Сервер пуст" },
					{ 2, "Serwer jest pusty" },
					{ 3, "Sunucu boş" }
				}
			},
			{
				"AddFriend",
				new Dictionary<int, string>
				{
					{ 0, "Add to Friends" },
					{ 1, "Добавить в друзья" },
					{ 2, "Dodaj do znajomych" },
					{ 3, "Arkadaş Ekle" }
				}
			},
			{
				"AddFriendSub",
				new Dictionary<int, string>
				{
					{ 0, "Never shoot this player" },
					{ 1, "Аимбот не будет стрелять в него" },
					{ 2, "Aimbot go zignoruje" },
					{ 3, "Bu oyuncuya asla ateş etme" }
				}
			},
			{
				"Remove",
				new Dictionary<int, string>
				{
					{ 0, "Remove" },
					{ 1, "Удалить" },
					{ 2, "Usuń" },
					{ 3, "Kaldır" }
				}
			},
			{
				"PressAny",
				new Dictionary<int, string>
				{
					{ 0, "Press any key..." },
					{ 1, "Нажмите клавишу..." },
					{ 2, "Naciśnij klawisz..." },
					{ 3, "Bir tuşa basın..." }
				}
			},
			{
				"MenuToggle",
				new Dictionary<int, string>
				{
					{ 0, "Menu Toggle" },
					{ 1, "Открыть меню" },
					{ 2, "Przełącz menu" },
					{ 3, "Menüyü Aç/Kapat" }
				}
			},
			{
				"AimbotHold",
				new Dictionary<int, string>
				{
					{ 0, "Aimbot Hold" },
					{ 1, "Удержание Аима" },
					{ 2, "Przytrzymanie Aimbota" },
					{ 3, "Aimbot Basılı Tut" }
				}
			},
			{
				"ChatSpam",
				new Dictionary<int, string>
				{
					{ 0, "Chat Spam" },
					{ 1, "Спам в чат" },
					{ 2, "Spam na czacie" },
					{ 3, "Sohbet Spam" }
				}
			},
			{
				"HoldToAimSub",
				new Dictionary<int, string>
				{
					{ 0, "Aim only when holding key" },
					{ 1, "Стрелять только при зажатой клавише" },
					{ 2, "Celuj tylko trzymając klawisz" },
					{ 3, "Sadece tuşa basılıyken nişan al" }
				}
			},
			{
				"ClaimGlow",
				new Dictionary<int, string>
				{
					{ 0, "Claim Flag Glow" },
					{ 1, "Подсветка флагов" },
					{ 2, "Podświetlenie flag" },
					{ 3, "Bayrak Parlaması" }
				}
			},
			{
				"ClaimName",
				new Dictionary<int, string>
				{
					{ 0, "Claim Flag Names" },
					{ 1, "Названия флагов" },
					{ 2, "Nazwy flag" },
					{ 3, "Bayrak İsimleri" }
				}
			},
			{
				"SpamText",
				new Dictionary<int, string>
				{
					{ 0, "Spam Text:" },
					{ 1, "Текст спама:" },
					{ 2, "Tekst spamu:" },
					{ 3, "Spam Metni:" }
				}
			},
			{
				"Interval",
				new Dictionary<int, string>
				{
					{ 0, "Interval:" },
					{ 1, "Интервал:" },
					{ 2, "Interwał:" },
					{ 3, "Aralık:" }
				}
			},
			{
				"Sec",
				new Dictionary<int, string>
				{
					{ 0, "sec" },
					{ 1, "сек" },
					{ 2, "sek" },
					{ 3, "sn" }
				}
			},
			{
				"NoClipKey",
				new Dictionary<int, string>
				{
					{ 0, "NoClip" },
					{ 1, "Ноклип (NoClip)" },
					{ 2, "NoClip" },
					{ 3, "NoClip" }
				}
			},
			{
				"AutoLootKey",
				new Dictionary<int, string>
				{
					{ 0, "Auto Loot" },
					{ 1, "Автолут" },
					{ 2, "Auto Loot" },
					{ 3, "Oto Toplama" }
				}
			},
			{
				"FreeCamKey",
				new Dictionary<int, string>
				{
					{ 0, "Free Cam" },
					{ 1, "Свободная камера" },
					{ 2, "Wolna Kamera" },
					{ 3, "Serbest Kamera" }
				}
			},
			{
				"PanicKey",
				new Dictionary<int, string>
				{
					{ 0, "Panic (Disable All)" },
					{ 1, "ВЫКЛЮЧИТЬ ЧИТ (Panic)" },
					{ 2, "Wyłącz Cheat (Panic)" },
					{ 3, "Hileyi Kapat (Panic)" }
				}
			},
			{
				"FastDisconnectKey",
				new Dictionary<int, string>
				{
					{ 0, "Fast Disconnect" },
					{ 1, "Быстрый выход" },
					{ 2, "Szybkie wyjście" },
					{ 3, "Hızlı Çıkış" }
				}
			},
			{
				"CancelLoadingKey",
				new Dictionary<int, string>
				{
					{ 0, "Cancel Loading" },
					{ 1, "Отмена загрузки" },
					{ 2, "Anuluj ładowanie" },
					{ 3, "Yüklemeyi İptal Et" }
				}
			},
			{
				"FurnNames",
				new Dictionary<int, string>
				{
					{ 0, "Furniture Names" },
					{ 1, "Названия мебели" },
					{ 2, "Nazwy mebli" },
					{ 3, "Mobilya İsimleri" }
				}
			},
			{
				"PickupEverything",
				new Dictionary<int, string>
				{
					{ 0, "Pickup Everything" },
					{ 1, "Пылесосить ВСЁ" },
					{ 2, "Podnieś wszystko" },
					{ 3, "Her Şeyi Topla" }
				}
			},
			{
				"PickupEverySub",
				new Dictionary<int, string>
				{
					{ 0, "Ignore filters" },
					{ 1, "Игнорировать фильтры" },
					{ 2, "Ignoruj filtry" },
					{ 3, "Filtreleri yoksay" }
				}
			},
			{
				"ArrowKeys",
				new Dictionary<int, string>
				{
					{ 0, "Arrow Keys Rotation" },
					{ 1, "Вращение стрелочками" },
					{ 2, "Obrót strzałkami" },
					{ 3, "Ok Tuşları ile Dönüş" }
				}
			},
			{
				"RotSpeed",
				new Dictionary<int, string>
				{
					{ 0, "Rotation Speed" },
					{ 1, "Скорость вращения" },
					{ 2, "Prędkość obrotu" },
					{ 3, "Dönüş Hızı" }
				}
			},
			{
				"StabilizeRoll",
				new Dictionary<int, string>
				{
					{ 0, "Stabilize Roll" },
					{ 1, "Стабилизация крена" },
					{ 2, "Stabilizacja przechyłu" },
					{ 3, "Yatışı Sabitle" }
				}
			},
			{
				"WeapRangeText",
				new Dictionary<int, string>
				{
					{ 0, "Weapon Range:" },
					{ 1, "Дальность оружия:" },
					{ 2, "Zasięg broni:" },
					{ 3, "Silah Menzili:" }
				}
			},
			{
				"GroupID",
				new Dictionary<int, string>
				{
					{ 0, "Group ID:" },
					{ 1, "ID Группы:" },
					{ 2, "ID Grupy:" },
					{ 3, "Grup ID:" }
				}
			},
			{
				"GroupRank",
				new Dictionary<int, string>
				{
					{ 0, "Group Rank:" },
					{ 1, "Ранг в группе:" },
					{ 2, "Ranga w grupie:" },
					{ 3, "Grup Rütbesi:" }
				}
			},
			{
				"None",
				new Dictionary<int, string>
				{
					{ 0, "None" },
					{ 1, "Нет" },
					{ 2, "Brak" },
					{ 3, "Yok" }
				}
			},
			{
				"WinWidth",
				new Dictionary<int, string>
				{
					{ 0, "Window Width" },
					{ 1, "Ширина меню" },
					{ 2, "Szerokość okna" },
					{ 3, "Pencere Genişliği" }
				}
			},
			{
				"WinHeight",
				new Dictionary<int, string>
				{
					{ 0, "Window Height" },
					{ 1, "Высота меню" },
					{ 2, "Wysokość okna" },
					{ 3, "Pencere Yüksekliği" }
				}
			},
			{
				"GENERAL",
				new Dictionary<int, string>
				{
					{ 0, "GENERAL" },
					{ 1, "ОСНОВНОЕ" },
					{ 2, "OGÓLNE" },
					{ 3, "GENEL" }
				}
			},
			{
				"SELECT",
				new Dictionary<int, string>
				{
					{ 0, "SELECT" },
					{ 1, "ВЫБОР ЦЕЛИ" },
					{ 2, "WYBÓR CELU" },
					{ 3, "SEÇİM" }
				}
			},
			{
				"TIME_CHANGER",
				new Dictionary<int, string>
				{
					{ 0, "TIME CHANGER" },
					{ 1, "УПРАВЛЕНИЕ ВРЕМЕНЕМ" },
					{ 2, "ZMIANA CZASU" },
					{ 3, "ZAMAN DEĞİŞTİRİCİ" }
				}
			},
			{
				"INTERFACE",
				new Dictionary<int, string>
				{
					{ 0, "INTERFACE" },
					{ 1, "ИНТЕРФЕЙС" },
					{ 2, "INTERFEJS" },
					{ 3, "ARAYÜZ" }
				}
			},
			{
				"MOVEMENT",
				new Dictionary<int, string>
				{
					{ 0, "MOVEMENT & CAMERA" },
					{ 1, "ДВИЖЕНИЕ И КАМЕРА" },
					{ 2, "RUCH I KAMERA" },
					{ 3, "HAREKET & KAMERA" }
				}
			},
			{
				"AUTO_LOOT",
				new Dictionary<int, string>
				{
					{ 0, "AUTO LOOT (VACUUM)" },
					{ 1, "АВТОЛУТ (ВАКУУМ)" },
					{ 2, "AUTO ZBIERANIE" },
					{ 3, "OTOMATİK TOPLAMA" }
				}
			},
			{
				"MISC",
				new Dictionary<int, string>
				{
					{ 0, "MISC" },
					{ 1, "РАЗНОЕ" },
					{ 2, "RÓŻNE" },
					{ 3, "ÇEŞİTLİ" }
				}
			},
			{
				"GLOBAL_FRIENDS",
				new Dictionary<int, string>
				{
					{ 0, "GLOBAL FRIENDS" },
					{ 1, "ГЛОБАЛЬНЫЕ ДРУЗЬЯ" },
					{ 2, "GLOBALNI ZNAJOMI" },
					{ 3, "KÜRESEL ARKADAŞLAR" }
				}
			},
			{
				"GUN_ADJ",
				new Dictionary<int, string>
				{
					{ 0, "GUN ADJUSTMENTS" },
					{ 1, "НАСТРОЙКИ ОРУЖИЯ" },
					{ 2, "MODYFIKACJE BRONI" },
					{ 3, "SİLAH AYARLARI" }
				}
			},
			{
				"KeybindsHeader",
				new Dictionary<int, string>
				{
					{ 0, "KEYBINDS" },
					{ 1, "БИНДЫ КЛАВИШ" },
					{ 2, "KLAWISZE" },
					{ 3, "TUŞ ATAMALARI" }
				}
			},
			{
				"AimbotBehHeader",
				new Dictionary<int, string>
				{
					{ 0, "AIMBOT BEHAVIOR" },
					{ 1, "ПОВЕДЕНИЕ АИМБОТА" },
					{ 2, "ZACHOWANIE AIMBOTA" },
					{ 3, "NİŞAN ALMA DAVRANIŞI" }
				}
			},
			{
				"HoldToAim",
				new Dictionary<int, string>
				{
					{ 0, "Hold to Aim" },
					{ 1, "Удержание для аима" },
					{ 2, "Przytrzymaj, aby celować" },
					{ 3, "Nişan Almak İçin Basılı Tut" }
				}
			},
			{
				"LangTitle",
				new Dictionary<int, string>
				{
					{ 0, "Language / Язык / Język / Dil" },
					{ 1, "Язык интерфейса" },
					{ 2, "Język interfejsu" },
					{ 3, "Arayüz Dili" }
				}
			},
			{
				"CustomTitle",
				new Dictionary<int, string>
				{
					{ 0, "Menu Customization" },
					{ 1, "Кастомизация меню" },
					{ 2, "Dostosowanie menu" },
					{ 3, "Menü Özelleştirme" }
				}
			},
			{
				"HUD_Title",
				new Dictionary<int, string>
				{
					{ 0, "HUD Overlays" },
					{ 1, "Оверлеи на экране (HUD)" },
					{ 2, "Nakładki HUD" },
					{ 3, "HUD Arayüzü" }
				}
			},
			{
				"HUD_Features",
				new Dictionary<int, string>
				{
					{ 0, "ENABLED FEATURES" },
					{ 1, "ВКЛЮЧЕННЫЕ ФУНКЦИИ" },
					{ 2, "WŁĄCZONE FUNKCJE" },
					{ 3, "AKTİF ÖZELLİKLER" }
				}
			},
			{
				"HUD_Weapon",
				new Dictionary<int, string>
				{
					{ 0, "WEAPON INFO" },
					{ 1, "ИНФО ОБ ОРУЖИИ" },
					{ 2, "INFORMACJE O BRONI" },
					{ 3, "SİLAH BİLGİSİ" }
				}
			},
			{
				"HUD_Admins",
				new Dictionary<int, string>
				{
					{ 0, "ADMINS INFO" },
					{ 1, "ИНФО ОБ АДМИНАХ" },
					{ 2, "INFO O ADMINACH" },
					{ 3, "ADMİN BİLGİSİ" }
				}
			},
			{
				"AdminsOnline",
				new Dictionary<int, string>
				{
					{ 0, "Admins Online:" },
					{ 1, "Всего админов:" },
					{ 2, "Adminów online:" },
					{ 3, "Çevrimiçi Admin:" }
				}
			},
			{
				"AdminsVanish",
				new Dictionary<int, string>
				{
					{ 0, "In Vanish:" },
					{ 1, "В ванише:" },
					{ 2, "W Vanish:" },
					{ 3, "Görünmez:" }
				}
			},
			{
				"DrawFOV",
				new Dictionary<int, string>
				{
					{ 0, "Draw FOV Circle" },
					{ 1, "Рисовать круг FOV" },
					{ 2, "Rysuj koło FOV" },
					{ 3, "FOV Çemberi Çiz" }
				}
			},
			{
				"AimZombies",
				new Dictionary<int, string>
				{
					{ 0, "Aim at Zombies" },
					{ 1, "Аим на зомби" },
					{ 2, "Celuj w zombie" },
					{ 3, "Zombilere Nişan Al" }
				}
			},
			{
				"NoDiscTimer",
				new Dictionary<int, string>
				{
					{ 0, "No Disconnect Timer" },
					{ 1, "Без таймера выхода" },
					{ 2, "Brak timera wyjścia" },
					{ 3, "Çıkış Süresi Yok" }
				}
			},
			{
				"NoDiscTimerSub",
				new Dictionary<int, string>
				{
					{ 0, "Instant exit in menu" },
					{ 1, "Моментальный выход по ESC" },
					{ 2, "Natychmiastowe wyjście" },
					{ 3, "Anında çıkış" }
				}
			},
			{
				"SilentAim",
				new Dictionary<int, string>
				{
					{ 0, "Silent Aim" },
					{ 1, "Сайлент Аим (Magic)" },
					{ 2, "Silent Aim" },
					{ 3, "Silent Aim" }
				}
			},
			{
				"SilentHead",
				new Dictionary<int, string>
				{
					{ 0, "Silent Always Head" },
					{ 1, "Сайлент всегда в голову" },
					{ 2, "Zawsze w głowę" },
					{ 3, "Daima Kafa" }
				}
			}
		};

		// Token: 0x02000024 RID: 36
		private class Snowflake
		{
			// Token: 0x04000178 RID: 376
			public float x;

			// Token: 0x04000179 RID: 377
			public float y;

			// Token: 0x0400017A RID: 378
			public float speed;

			// Token: 0x0400017B RID: 379
			public float size;
		}
	}
}

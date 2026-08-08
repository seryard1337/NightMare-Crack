using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cheat.core;
using HighlightingSystem;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x02000010 RID: 16
	public class ESP : MonoBehaviour
	{
		// Token: 0x06000052 RID: 82 RVA: 0x000043D0 File Offset: 0x000025D0
		private void InitGUIStyles()
		{
			if (this.objectTextStyle == null)
			{
				this.objectTextStyle = new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					fontSize = 12,
					fontStyle = FontStyle.Bold
				};
			}
			if (this.itemTextStyle == null)
			{
				this.itemTextStyle = new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold
				};
			}
			if (this.playerInfoTextStyle == null)
			{
				this.playerInfoTextStyle = new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter
				};
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000238D File Offset: 0x0000058D
		private void Awake()
		{
			Cheat.core.Main main = UnityEngine.Object.FindObjectOfType<Cheat.core.Main>();
			this.entities = ((main == null) ? null : main.entities);
			if (this.entities == null)
			{
				this.entities = base.gameObject.AddComponent<Cheat.core.Entities>();
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000023C5 File Offset: 0x000005C5
		private void Start()
		{
			this.CheckHighlightingRenderer();
			this.InitializeHighlighters();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004460 File Offset: 0x00002660
		private void InitializeHighlighters()
		{
			if (!(this.entities == null))
			{
				foreach (Player player in this.entities.Players)
				{
					if (player != null && player != Player.LocalPlayer && !player.life.isDead)
					{
						this.AddHighlighter(player.transform);
					}
				}
				if (this.showItemGlow)
				{
					foreach (InteractableItem interactableItem in this.entities.Items)
					{
						if (interactableItem != null && interactableItem.asset != null)
						{
							this.AddHighlighter(interactableItem.transform);
						}
					}
				}
				return;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000455C File Offset: 0x0000275C
		private void AddHighlighter(Transform target)
		{
			if (!(target == null) && !(target.gameObject == null))
			{
				Highlighter highlighter;
				if (!this.highlighters.TryGetValue(target, out highlighter) || highlighter == null)
				{
					highlighter = target.GetComponent<Highlighter>();
					if (highlighter == null)
					{
						highlighter = target.gameObject.AddComponent<Highlighter>();
						highlighter.ConstantOff(0.25f);
						highlighter.overlay = true;
						highlighter.enabled = true;
					}
					this.highlighters[target] = highlighter;
				}
				return;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000045DC File Offset: 0x000027DC
		private void Update()
		{
			if (this.espEnabled && !(Player.LocalPlayer == null) && !(this.entities == null))
			{
				foreach (Player player in this.entities.Players)
				{
					if (!(player == null) && !(player.transform == null) && !(player == Player.LocalPlayer) && !(player.life == null) && !player.life.isDead)
					{
						float num = this.entities.DistanceToLocal(player.transform.position);
						bool flag = this.espEnabled && this.showGlow && num <= this.maxDistance;
						Highlighter highlighter;
						if (this.highlighters.TryGetValue(player.transform, out highlighter) && !(highlighter == null))
						{
							if (!flag)
							{
								highlighter.ConstantOff(0.25f);
								highlighter.enabled = false;
							}
							else
							{
								highlighter.ConstantOn(Color.white, 0.25f);
								highlighter.overlay = true;
								highlighter.enabled = true;
							}
						}
						else
						{
							this.AddHighlighter(player.transform);
						}
					}
				}
				foreach (InteractableItem interactableItem in this.entities.Items)
				{
					if (!(interactableItem == null) && !(interactableItem.transform == null) && interactableItem.asset != null)
					{
						float num2 = this.entities.DistanceToLocal(interactableItem.transform.position);
						bool flag2 = this.espEnabled && this.showItemGlow && num2 <= this.maxDistance;
						Highlighter highlighter2;
						if (this.highlighters.TryGetValue(interactableItem.transform, out highlighter2) && !(highlighter2 == null))
						{
							if (!flag2)
							{
								highlighter2.ConstantOff(0.25f);
								highlighter2.enabled = false;
							}
							else
							{
								highlighter2.ConstantOn(ItemTool.getRarityColorHighlight(interactableItem.asset.rarity), 0.25f);
								highlighter2.overlay = true;
								highlighter2.enabled = true;
							}
						}
						else
						{
							this.AddHighlighter(interactableItem.transform);
						}
					}
				}
				foreach (InteractableVehicle interactableVehicle in this.entities.Vehicles)
				{
					if (!(interactableVehicle == null) && !(interactableVehicle.transform == null) && interactableVehicle.asset != null)
					{
						float num3 = this.entities.DistanceToLocal(interactableVehicle.transform.position);
						bool flag3 = this.espEnabled && this.showVehicleGlow && num3 <= this.maxDistance;
						Highlighter highlighter3;
						if (this.highlighters.TryGetValue(interactableVehicle.transform, out highlighter3) && !(highlighter3 == null))
						{
							if (flag3)
							{
								highlighter3.ConstantOn(new Color(1f, 0.65f, 0f, 1f), 0.25f);
								highlighter3.overlay = true;
								highlighter3.enabled = true;
							}
							else
							{
								highlighter3.ConstantOff(0.25f);
								highlighter3.enabled = false;
							}
						}
						else
						{
							this.AddHighlighter(interactableVehicle.transform);
						}
					}
				}
				foreach (BarricadeDrop barricadeDrop in this.entities.Beds)
				{
					if (barricadeDrop != null && !(barricadeDrop.model == null))
					{
						float num4 = this.entities.DistanceToLocal(barricadeDrop.model.position);
						bool flag4 = this.espEnabled && this.showBedGlow && num4 <= this.maxDistance;
						Highlighter highlighter4;
						if (this.highlighters.TryGetValue(barricadeDrop.model, out highlighter4) && !(highlighter4 == null))
						{
							if (!flag4)
							{
								highlighter4.ConstantOff(0.25f);
								highlighter4.enabled = false;
							}
							else
							{
								highlighter4.ConstantOn(new Color(0f, 0.8f, 1f, 1f), 0.25f);
								highlighter4.overlay = true;
								highlighter4.enabled = true;
							}
						}
						else
						{
							this.AddHighlighter(barricadeDrop.model);
						}
					}
				}
				foreach (BarricadeDrop barricadeDrop2 in this.entities.Claims)
				{
					if (barricadeDrop2 != null && !(barricadeDrop2.model == null))
					{
						float num5 = this.entities.DistanceToLocal(barricadeDrop2.model.position);
						bool flag5 = this.espEnabled && this.showClaimGlow && num5 <= this.maxDistance;
						Highlighter highlighter5;
						if (this.highlighters.TryGetValue(barricadeDrop2.model, out highlighter5) && !(highlighter5 == null))
						{
							if (flag5)
							{
								highlighter5.ConstantOn(new Color(0f, 0.8f, 1f, 1f), 0.25f);
								highlighter5.overlay = true;
								highlighter5.enabled = true;
							}
							else
							{
								highlighter5.ConstantOff(0.25f);
								highlighter5.enabled = false;
							}
						}
						else
						{
							this.AddHighlighter(barricadeDrop2.model);
						}
					}
				}
				foreach (BarricadeDrop barricadeDrop3 in this.entities.Furniture)
				{
					if (barricadeDrop3 != null && !(barricadeDrop3.model == null))
					{
						float num6 = this.entities.DistanceToLocal(barricadeDrop3.model.position);
						bool flag6 = this.espEnabled && this.showFurnitureGlow && num6 <= this.maxDistance;
						Highlighter highlighter6;
						if (this.highlighters.TryGetValue(barricadeDrop3.model, out highlighter6) && !(highlighter6 == null))
						{
							if (!flag6)
							{
								highlighter6.ConstantOff(0.25f);
								highlighter6.enabled = false;
							}
							else
							{
								highlighter6.ConstantOn(new Color(1f, 0.55f, 0f, 1f), 0.25f);
								highlighter6.overlay = true;
								highlighter6.enabled = true;
							}
						}
						else
						{
							this.AddHighlighter(barricadeDrop3.model);
						}
					}
				}
				if (Time.time - this.lastCleanupTime > 10f)
				{
					this.CleanUpCaches();
					this.lastCleanupTime = Time.time;
				}
				if (Time.frameCount % 480 == 0)
				{
					this.InitializeHighlighters();
				}
				return;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004D6C File Offset: 0x00002F6C
		private void CleanUpCaches()
		{
			List<InteractableItem> list = new List<InteractableItem>();
			foreach (InteractableItem interactableItem in this.textPositionCache.Keys)
			{
				if (interactableItem == null)
				{
					list.Add(interactableItem);
				}
			}
			foreach (InteractableItem interactableItem2 in list)
			{
				this.textPositionCache.Remove(interactableItem2);
			}
			List<Transform> list2 = new List<Transform>();
			foreach (Transform transform in this.highlighters.Keys)
			{
				if (transform == null)
				{
					list2.Add(transform);
				}
			}
			foreach (Transform transform2 in list2)
			{
				this.highlighters.Remove(transform2);
			}
			if (ESP.itemIconCache.Count > 1000)
			{
				ESP.itemIconCache.Clear();
			}
			Utils.CleanLimbCache();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004EE0 File Offset: 0x000030E0
		private void CheckHighlightingRenderer()
		{
			if (this.cameraChecked)
			{
				return;
			}
			this.cameraChecked = true;
			Camera cachedCamera = Cheat.core.Main.CachedCamera;
			if (cachedCamera == null)
			{
				return;
			}
			if (cachedCamera.GetComponent<HighlightingRenderer>() == null)
			{
				cachedCamera.gameObject.AddComponent<HighlightingRenderer>();
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000023D3 File Offset: 0x000005D3
		public void method_0()
		{
			this.espEnabled = !this.espEnabled;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000023E4 File Offset: 0x000005E4
		public void ToggleBoxes()
		{
			this.showBoxes = !this.showBoxes;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000023F5 File Offset: 0x000005F5
		public void ToggleSkeleton()
		{
			this.showSkeleton = !this.showSkeleton;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002406 File Offset: 0x00000606
		public void ToggleGlow()
		{
			this.showGlow = !this.showGlow;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004F28 File Offset: 0x00003128
		public void Draw()
		{
			Camera cachedCamera = Cheat.core.Main.CachedCamera;
			if (this.espEnabled && (!Overrides.bBeingSpied || !Overrides.bHideOnSpy) && !(cachedCamera == null) && !(Player.LocalPlayer == null) && !(this.entities == null))
			{
				this.InitGUIStyles();
				if (this.showItemIcons || this.showItemName)
				{
					foreach (InteractableItem interactableItem in this.entities.Items)
					{
						if (!(interactableItem == null) && !(interactableItem.transform == null) && interactableItem.asset != null)
						{
							Vector3 vector = interactableItem.transform.position + Vector3.up * 0.45f;
							Vector3 vector2 = cachedCamera.WorldToScreenPoint(vector);
							if (vector2.z > 0.1f)
							{
								float num = this.entities.DistanceToLocal(interactableItem.transform.position);
								if (num <= this.maxDistance)
								{
									float num2 = 1f;
									Vector3 vector3 = vector2;
									ValueTuple<Vector3, float> valueTuple;
									if (this.textPositionCache.TryGetValue(interactableItem, out valueTuple))
									{
										float num3 = Time.time - valueTuple.Item2;
										float num4 = Vector3.Distance(valueTuple.Item1, vector2);
										if (num3 < 0.4f && num4 < 8f)
										{
											vector3 = valueTuple.Item1;
										}
									}
									this.textPositionCache[interactableItem] = new ValueTuple<Vector3, float>(vector3, Time.time);
									if (this.showItemIcons)
									{
										Texture2D itemIcon = this.GetItemIcon(interactableItem.asset.id, interactableItem.item.quality, interactableItem.item.state);
										if (itemIcon != null)
										{
											Vector3 vector4 = Cheat.core.Main.CachedCamera.WorldToScreenPoint(interactableItem.transform.position);
											if (vector4.z <= 0.05f)
											{
												continue;
											}
											float num5 = 25f * this.itemIconScale;
											float num6 = (float)interactableItem.asset.size_x * num5;
											float num7 = (float)interactableItem.asset.size_y * num5;
											GUI.DrawTexture(new Rect(vector4.x - num6 / 2f, (float)Screen.height - vector4.y - num7 / 2f, num6, num7), itemIcon, (ScaleMode)2);
										}
									}
									if (this.showItemName)
									{
										string text = ((!this.showItemName) ? "" : interactableItem.asset.FriendlyName);
										if (!string.IsNullOrEmpty(text))
										{
											this.itemTextStyle.fontSize = ((num >= 35f) ? ((num >= 70f) ? 9 : 11) : 12);
											this.itemTextStyle.normal.textColor = new Color(1f, 1f, 1f, num2);
											Vector2 vector5 = this.itemTextStyle.CalcSize(new GUIContent(text));
											float num8 = (float)Screen.height - vector3.y - 55f;
											GUI.color = new Color(0f, 0f, 0f, 0.95f);
											GUI.Label(new Rect(vector3.x - vector5.x / 2f - 1f, num8 - 1f, vector5.x, vector5.y), text, this.itemTextStyle);
											GUI.color = this.itemTextStyle.normal.textColor;
											GUI.Label(new Rect(vector3.x - vector5.x / 2f, num8, vector5.x, vector5.y), text, this.itemTextStyle);
										}
									}
								}
							}
						}
					}
				}
				GUI.color = Color.white;
				foreach (InteractableVehicle interactableVehicle in this.entities.Vehicles)
				{
					if (!(interactableVehicle == null) && !(interactableVehicle.transform == null) && interactableVehicle.asset != null)
					{
						float num9 = this.entities.DistanceToLocal(interactableVehicle.transform.position);
						if (num9 <= this.maxDistance)
						{
							Vector3 vector6 = cachedCamera.WorldToScreenPoint(interactableVehicle.transform.position + Vector3.up * 2.5f);
							if (vector6.z > 0.05f)
							{
								if (this.showVehicleGlow)
								{
									Highlighter highlighter;
									if (this.highlighters.TryGetValue(interactableVehicle.transform, out highlighter) && !(highlighter == null))
									{
										Color color = ((!interactableVehicle.isLocked) ? new Color(0.2f, 1f, 0.2f, 1f) : new Color(1f, 0.2f, 0.2f, 1f));
										highlighter.ConstantOn(color, 0.25f);
										highlighter.overlay = true;
										highlighter.enabled = true;
									}
									else
									{
										this.AddHighlighter(interactableVehicle.transform);
									}
								}
								if (this.showVehicleName)
								{
									Vector3 vector7 = vector6;
									string text2 = string.Format("{0} {1}м", interactableVehicle.asset.vehicleName, Mathf.RoundToInt(num9));
									string text3 = ((!interactableVehicle.isLocked) ? "UNLOCK" : "LOCK");
									Color color2;
									color2 = new Color(1f, 0.8f, 0f, 1f);
									this.DrawObjectText(vector7, text2, color2);
									this.DrawObjectText(vector7 + new Vector3(0f, -14f, 0f), text3, color2);
								}
							}
						}
					}
				}
				foreach (BarricadeDrop barricadeDrop in this.entities.Beds)
				{
					if (barricadeDrop != null && !(barricadeDrop.model == null))
					{
						float num10 = this.entities.DistanceToLocal(barricadeDrop.model.position);
						if (num10 <= this.maxDistance)
						{
							Vector3 vector8 = cachedCamera.WorldToScreenPoint(barricadeDrop.model.position + Vector3.up * 1.2f);
							if (vector8.z > 0.05f)
							{
								if (this.showBedGlow)
								{
									Highlighter highlighter2;
									if (this.highlighters.TryGetValue(barricadeDrop.model, out highlighter2) && !(highlighter2 == null))
									{
										highlighter2.ConstantOn(new Color(0f, 0.8f, 1f, 1f), 0.25f);
										highlighter2.overlay = true;
										highlighter2.enabled = true;
									}
									else
									{
										this.AddHighlighter(barricadeDrop.model);
									}
								}
								if (this.showBedName)
								{
									this.DrawObjectText(vector8, barricadeDrop.asset.FriendlyName + " " + Mathf.RoundToInt(num10).ToString() + "м", new Color(0f, 0.8f, 1f, 1f));
								}
							}
						}
					}
				}
				foreach (BarricadeDrop barricadeDrop2 in this.entities.Claims)
				{
					if (barricadeDrop2 != null && !(barricadeDrop2.model == null))
					{
						float num11 = this.entities.DistanceToLocal(barricadeDrop2.model.position);
						if (num11 <= this.maxDistance)
						{
							Vector3 vector9 = cachedCamera.WorldToScreenPoint(barricadeDrop2.model.position + Vector3.up * 1.5f);
							if (vector9.z > 0.05f)
							{
								if (this.showClaimGlow)
								{
									Highlighter highlighter3;
									if (this.highlighters.TryGetValue(barricadeDrop2.model, out highlighter3) && !(highlighter3 == null))
									{
										highlighter3.ConstantOn(new Color(0f, 0.8f, 1f, 1f), 0.25f);
										highlighter3.overlay = true;
										highlighter3.enabled = true;
									}
									else
									{
										this.AddHighlighter(barricadeDrop2.model);
									}
								}
								if (this.showClaimName)
								{
									this.DrawObjectText(vector9, barricadeDrop2.asset.FriendlyName + " " + Mathf.RoundToInt(num11).ToString() + "м", new Color(0f, 0.8f, 1f, 1f));
								}
							}
						}
					}
				}
				foreach (BarricadeDrop barricadeDrop3 in this.entities.Furniture)
				{
					if (barricadeDrop3 != null && !(barricadeDrop3.model == null))
					{
						float num12 = this.entities.DistanceToLocal(barricadeDrop3.model.position);
						if (num12 <= this.maxDistance)
						{
							Vector3 vector10 = cachedCamera.WorldToScreenPoint(barricadeDrop3.model.position + Vector3.up * 1.2f);
							if (vector10.z > 0.05f)
							{
								if (this.showFurnitureGlow)
								{
									Highlighter highlighter4;
									if (this.highlighters.TryGetValue(barricadeDrop3.model, out highlighter4) && !(highlighter4 == null))
									{
										highlighter4.ConstantOn(new Color(1f, 0.55f, 0f, 1f), 0.25f);
										highlighter4.overlay = true;
										highlighter4.enabled = true;
									}
									else
									{
										this.AddHighlighter(barricadeDrop3.model);
									}
								}
								if (this.showFurnitureName)
								{
									this.DrawObjectText(vector10, barricadeDrop3.asset.FriendlyName + " " + Mathf.RoundToInt(num12).ToString() + "м", new Color(1f, 0.55f, 0f, 1f));
								}
							}
						}
					}
				}
				using (List<Player>.Enumerator enumerator4 = this.entities.Players.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Player player = enumerator4.Current;
						if (!(player == null) && !(player.transform == null) && !(player == Player.LocalPlayer) && !(player.life == null) && !player.life.isDead)
						{
							Vector3 position = player.transform.position;
							float num13 = this.entities.DistanceToLocal(position);
							if (num13 <= this.maxDistance)
							{
								if (this.showSnaplines)
								{
									Vector3 vector11 = ((!this.snaplineToFeet) ? (position + Vector3.up * this.GetHeadHeight(player.stance.stance)) : position);
									Vector3 position2 = Player.LocalPlayer.look.aim.position;
									Vector3 aimPosition = this.GetAimPosition(player);
									bool flag = this.IsVisible(position2, aimPosition, player);
									bool flag2 = Utils.IsFriendly(player);
									bool flag3 = false;
									List<SteamPlayer> clients = Provider.clients;
									SteamPlayer steamPlayer = ((clients == null) ? null : clients.FirstOrDefault<SteamPlayer>((SteamPlayer s) => ((s == null) ? null : s.player) == player));
									if (steamPlayer != null)
									{
										string text4 = steamPlayer.playerID.characterName.ToUpperInvariant();
										flag3 = steamPlayer.isAdmin || text4.Contains("ADMIN") || text4.Contains("MODER") || text4.Contains("HELP") || text4.Contains("АДМИН") || text4.Contains("МОДЕР") || text4.Contains("ХЕЛП");
									}
									Color color3;
									if (!flag2)
									{
										if (!flag3)
										{
											if (!flag)
											{
												color3 = new Color(1f, 0.2f, 0.2f, 0.92f);
											}
											else
											{
												color3 = new Color(0.2f, 1f, 0.2f, 0.92f);
											}
										}
										else
										{
											color3 = new Color(0f, 0.85f, 1f, 0.95f);
										}
									}
									else
									{
										color3 = new Color(0f, 0.65f, 1f, 0.95f);
									}
									float num14 = ((!this.snaplineAlphaFalloff) ? 1f : Mathf.Clamp01(1.85f - num13 / 750f));
									color3.a *= num14;
									Utils.DrawSnapline(new Vector2((float)Screen.width * 0.5f, (float)Screen.height - (float)Screen.height / 8f), vector11, color3, this.snaplineThickness, cachedCamera);
								}
								if (this.showBoxes)
								{
									this.DrawPlayerBox(player, num13, cachedCamera);
								}
								if (this.showSkeleton)
								{
									this.DrawSkeleton(player, num13, cachedCamera);
								}
								if (this.showInfoText)
								{
									Vector3 vector12 = position + Vector3.up * this.infoTextOffsetY;
									this.DrawPlayerInfoText(player, vector12, num13, cachedCamera);
								}
							}
						}
					}
				}
				return;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005D28 File Offset: 0x00003F28
		private void DrawObjectText(Vector3 screen, string text, Color color)
		{
			if (screen.z > 0.05f)
			{
				this.objectTextStyle.normal.textColor = color;
				Vector2 vector = this.objectTextStyle.CalcSize(new GUIContent(text));
				float num = (float)Screen.height - screen.y - 10f;
				GUI.color = new Color(0f, 0f, 0f, 0.9f);
				GUI.Label(new Rect(screen.x - vector.x / 2f - 1f, num - 1f, vector.x, vector.y), text, this.objectTextStyle);
				GUI.color = color;
				GUI.Label(new Rect(screen.x - vector.x / 2f, num, vector.x, vector.y), text, this.objectTextStyle);
				return;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005E10 File Offset: 0x00004010
		private Texture2D GetItemIcon(ushort id, byte quality, byte[] state)
		{
			Texture2D texture2D;
			if (ESP.itemIconCache.TryGetValue(id, out texture2D) && texture2D != null)
			{
				return texture2D;
			}
			ItemTool.getIcon(id, quality, state, delegate(int handle, Texture2D texture)
			{
				if (texture != null && !ESP.itemIconCache.ContainsKey(id))
				{
					ESP.itemIconCache[id] = texture;
				}
			});
			return null;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002417 File Offset: 0x00000617
		private float GetHeadHeight(EPlayerStance stance)
		{
			switch ((int)stance)
			{
			case 1:
			case 5:
				return 0.4f;
			default:
				return 1.95f;
			case 4:
				return 1.4f;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005E64 File Offset: 0x00004064
		private void DrawPlayerInfoText(Player player, Vector3 worldPos, float distance, Camera cam)
		{
			Vector3 vector = cam.WorldToScreenPoint(worldPos);
			if (vector.z <= 0.05f)
			{
				return;
			}
			SteamChannel channel = player.channel;
			string text;
			if (channel != null)
			{
				SteamPlayer owner = channel.owner;
				if (owner != null)
				{
					SteamPlayerID playerID = owner.playerID;
					if (playerID != null)
					{
						if ((text = playerID.characterName) != null)
						{
							goto IL_004A;
						}
					}
				}
			}
			text = "???";
			IL_004A:
			string text2 = Mathf.RoundToInt(distance).ToString() + "м";
			string text3 = "нет";
			PlayerEquipment equipment = player.equipment;
			if (((equipment == null) ? null : equipment.asset) != null)
			{
				text3 = player.equipment.asset.FriendlyName;
				if (text3.Length > 18)
				{
					text3 = text3.Substring(0, 15) + "...";
				}
			}
			string text4 = text + "  •  " + text2;
			if (this.infoTextShowWeapon)
			{
				text4 = text4 + "\n" + text3;
			}
			this.playerInfoTextStyle.fontSize = this.infoTextSize;
			Vector2 vector2 = this.playerInfoTextStyle.CalcSize(new GUIContent(text4));
			float num = vector.x - vector2.x / 2f;
			float num2 = (float)Screen.height - vector.y - 5f;
			GUI.color = ESP.COLOR_TEXT_SHADOW;
			GUI.Label(new Rect(num - 1f, num2 - 1f, vector2.x + 2f, vector2.y + 2f), text4, this.playerInfoTextStyle);
			GUI.color = ESP.COLOR_TEXT;
			GUI.Label(new Rect(num, num2, vector2.x, vector2.y), text4, this.playerInfoTextStyle);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00006000 File Offset: 0x00004200
		private void DrawPlayerBox(Player player, float distance, Camera cam)
		{
			Vector3 position = player.transform.position;
			Vector3 vector = position;
			EPlayerStance stance = player.stance.stance;
			Vector3 vector2;
			if ((int)stance == 4)
			{
				vector2 = position + Vector3.up * 1.4f;
			}
			else
			{
				if ((int)stance != 5)
				{
					if ((int)stance != 1)
					{
						vector2 = position + Vector3.up * 1.95f;
						goto IL_006E;
					}
				}
				vector2 = position + Vector3.up * 0.4f;
			}
			IL_006E:
			Vector3 vector3 = cam.WorldToScreenPoint(vector2);
			Vector3 vector4 = cam.WorldToScreenPoint(vector);
			if (vector3.z > 0.05f && vector4.z > 0.05f)
			{
				float num = Mathf.Abs((float)Screen.height - vector3.y - ((float)Screen.height - vector4.y));
				float num2 = num * 0.55f;
				float num3 = vector3.x - num2 / 2f;
				float num4 = (float)Screen.height - vector3.y;
				Utils.DrawBox(num3, num4, num2, num, 2f, ESP.COLOR_BOX);
				return;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006110 File Offset: 0x00004310
		private void DrawSkeleton(Player p, float distance, Camera cam)
		{
			Transform limb = Utils.GetLimb(p.transform, (ELimb)12);
			if (!(limb == null))
			{
				Vector3 position = limb.position;
				EPlayerStance stance = p.stance.stance;
				float num = (((int)stance == 3 || (int)stance == 2) ? 1f : (((int)stance == 4) ? 0.75f : (((int)stance == 5 || (int)stance == 1) ? 0f : 0.8f)));
				Vector3 vector = position + Vector3.up * num;
				this.DrawBoneFromPoint(vector, (ELimb)5, p, distance, cam);
				Transform limb2 = Utils.GetLimb(p.transform, (ELimb)5);
				this.DrawBoneFromPoint((limb2 == null) ? vector : limb2.position, (ELimb)4, p, distance, cam);
				this.DrawBoneFromPoint(vector, (ELimb)7, p, distance, cam);
				Transform limb3 = Utils.GetLimb(p.transform, (ELimb)7);
				this.DrawBoneFromPoint((limb3 == null) ? vector : limb3.position, (ELimb)6, p, distance, cam);
				this.DrawBoneFromPoint(vector, (ELimb)1, p, distance, cam);
				Transform limb4 = Utils.GetLimb(p.transform, (ELimb)1);
				this.DrawBoneFromPoint((limb4 == null) ? vector : limb4.position, (ELimb)0, p, distance, cam);
				this.DrawBoneFromPoint(vector, (ELimb)3, p, distance, cam);
				Transform limb5 = Utils.GetLimb(p.transform, (ELimb)3);
				this.DrawBoneFromPoint((limb5 == null) ? vector : limb5.position, (ELimb)2, p, distance, cam);
				return;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006240 File Offset: 0x00004440
		private void DrawBoneFromPoint(Vector3 startPos, ELimb limb, Player p, float distance, Camera cam)
		{
			Transform limb2 = Utils.GetLimb(p.transform, limb);
			if (limb2 == null)
			{
				return;
			}
			Vector3 position = limb2.position;
			Vector3 vector = cam.WorldToScreenPoint(startPos);
			Vector3 vector2 = cam.WorldToScreenPoint(position);
			if (vector.z > 0.05f && vector2.z > 0.05f)
			{
				vector.y = (float)Screen.height - vector.y;
				vector2.y = (float)Screen.height - vector2.y;
				float num = Mathf.Lerp(0.8f, 2.2f, Mathf.Clamp01(1f - distance / 1200f));
				float num2 = Mathf.Clamp01(1.3f - distance / 1000f);
				Color color;
				color = new Color(ESP.COLOR_SKELETON.r, ESP.COLOR_SKELETON.g, ESP.COLOR_SKELETON.b, num2 * ESP.COLOR_SKELETON.a);
				Utils.DrawLine(vector.x, vector.y, vector2.x, vector2.y, num, color);
				return;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000635C File Offset: 0x0000455C
		private Vector3 GetAimPosition(Player target)
		{
			if (this.preferHead)
			{
				Transform limb = Utils.GetLimb(target.transform, (ELimb)13);
				if (limb != null)
				{
					return limb.position + Vector3.up * 0.22f;
				}
			}
			Transform limb2 = Utils.GetLimb(target.transform, (ELimb)12);
			if (limb2 != null)
			{
				EPlayerStance stance = target.stance.stance;
				float num = (((int)stance == 3 || (int)stance == 2) ? 1f : (((int)stance == 4) ? 0.75f : 0.15f));
				return limb2.position + Vector3.up * num;
			}
			return target.transform.position + Vector3.up * 1.4f;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000641C File Offset: 0x0000461C
		private bool IsVisible(Vector3 from, Vector3 to, Player target)
		{
			Vector3 normalized = (to - from).normalized;
			float num = Vector3.Distance(from, to);
			RaycastHit raycastHit;
			return (Physics.Raycast(from + Vector3.up * 0.08f, normalized, out raycastHit, num + 0.4f, RayMasks.DAMAGE_CLIENT, QueryTriggerInteraction.UseGlobal) && DamageTool.getPlayer(raycastHit.transform) == target) || (Physics.Raycast(from + Vector3.up * 0.03f, normalized, out raycastHit, num + 0.4f, RayMasks.DAMAGE_CLIENT, QueryTriggerInteraction.UseGlobal) && DamageTool.getPlayer(raycastHit.transform) == target);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000064C4 File Offset: 0x000046C4
		public void HardCleanGlow()
		{
			if (this.highlighters != null)
			{
				foreach (Transform transform in this.highlighters.Keys.ToList<Transform>())
				{
					Highlighter highlighter;
					if (this.highlighters.TryGetValue(transform, out highlighter) && highlighter != null)
					{
						try
						{
							highlighter.ConstantOff(0.25f);
							UnityEngine.Object.Destroy(highlighter);
						}
						catch
						{
						}
					}
				}
				this.highlighters.Clear();
				return;
			}
		}

		// Token: 0x04000049 RID: 73
		private Cheat.core.Entities entities;

		// Token: 0x0400004A RID: 74
		public bool espEnabled = true;

		// Token: 0x0400004B RID: 75
		public float maxDistance = 2000f;

		// Token: 0x0400004C RID: 76
		public bool showInfoText = true;

		// Token: 0x0400004D RID: 77
		public float infoTextOffsetY = -0.15f;

		// Token: 0x0400004E RID: 78
		public bool infoTextShowWeapon = true;

		// Token: 0x0400004F RID: 79
		public int infoTextSize = 11;

		// Token: 0x04000050 RID: 80
		public bool showBoxes = true;

		// Token: 0x04000051 RID: 81
		public bool showSkeleton = true;

		// Token: 0x04000052 RID: 82
		public bool showGlow = true;

		// Token: 0x04000053 RID: 83
		public bool showSnaplines = true;

		// Token: 0x04000054 RID: 84
		public float snaplineThickness = 1.4f;

		// Token: 0x04000055 RID: 85
		public bool snaplineToFeet = true;

		// Token: 0x04000056 RID: 86
		public bool snaplineAlphaFalloff = true;

		// Token: 0x04000057 RID: 87
		private bool preferHead = true;

		// Token: 0x04000058 RID: 88
		public bool showItemGlow = true;

		// Token: 0x04000059 RID: 89
		public bool showItemIcons = true;

		// Token: 0x0400005A RID: 90
		public float itemIconScale = 0.8f;

		// Token: 0x0400005B RID: 91
		public bool showItemName = true;

		// Token: 0x0400005C RID: 92
		private float lastCleanupTime;

		// Token: 0x0400005D RID: 93
		public bool showVehicleGlow = true;

		// Token: 0x0400005E RID: 94
		public bool showVehicleName = true;

		// Token: 0x0400005F RID: 95
		public bool showBedGlow = true;

		// Token: 0x04000060 RID: 96
		public bool showBedName = true;

		// Token: 0x04000061 RID: 97
		public bool showClaimGlow = true;

		// Token: 0x04000062 RID: 98
		public bool showClaimName = true;

		// Token: 0x04000063 RID: 99
		public bool showFurnitureGlow = true;

		// Token: 0x04000064 RID: 100
		public bool showFurnitureName = true;

		// Token: 0x04000065 RID: 101
		private const float POSITION_CACHE_DURATION = 0.4f;

		// Token: 0x04000066 RID: 102
		private static readonly Color COLOR_BOX = new Color(1f, 1f, 1f, 0.92f);

		// Token: 0x04000067 RID: 103
		private static readonly Color COLOR_SKELETON = new Color(1f, 0f, 0f, 0.85f);

		// Token: 0x04000068 RID: 104
		private static readonly Color COLOR_TEXT = Color.white;

		// Token: 0x04000069 RID: 105
		private static readonly Color COLOR_TEXT_SHADOW = new Color(0f, 0f, 0f, 0.9f);

		// Token: 0x0400006A RID: 106
		private GUIStyle objectTextStyle;

		// Token: 0x0400006B RID: 107
		private GUIStyle itemTextStyle;

		// Token: 0x0400006C RID: 108
		private GUIStyle playerInfoTextStyle;

		// Token: 0x0400006D RID: 109
		private bool cameraChecked;

		// Token: 0x0400006E RID: 110
		public readonly Dictionary<Transform, Highlighter> highlighters = new Dictionary<Transform, Highlighter>();

		// Token: 0x0400006F RID: 111
		private static readonly Dictionary<ushort, Texture2D> itemIconCache = new Dictionary<ushort, Texture2D>();

		// Token: 0x04000070 RID: 112
		private Dictionary<InteractableItem, (Vector3 screenPos, float lastUpdateTime)> textPositionCache = new Dictionary<InteractableItem, (Vector3, float)>();
	}
}

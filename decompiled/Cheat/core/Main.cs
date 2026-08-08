using System;
using System.Collections.Generic;
using System.Reflection;
using Cheat.modules;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.core
{
	// Token: 0x0200002A RID: 42
	[DefaultExecutionOrder(-1000)]
	public class Main : MonoBehaviour
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000027AB File Offset: 0x000009AB
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x000027B2 File Offset: 0x000009B2
		public static Main Instance { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000027BA File Offset: 0x000009BA
		// (set) Token: 0x060000FA RID: 250 RVA: 0x000027C1 File Offset: 0x000009C1
		public static Camera CachedCamera { get; private set; }

		// Token: 0x060000FB RID: 251 RVA: 0x00012D7C File Offset: 0x00010F7C
		private void Awake()
		{
			if (!(Main.Instance != null))
			{
				Main.Instance = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				base.StartCoroutine(DisableBattlEyePanel.DisablePanelRoutine());
				this.entities = base.gameObject.AddComponent<Entities>();
				this.esp = base.gameObject.AddComponent<ESP>();
				this.aimbot = base.gameObject.AddComponent<Cheat.modules.Aimbot>();
				this.visuals = base.gameObject.AddComponent<Visuals>();
				this.triggerbot = base.gameObject.AddComponent<Triggerbot>();
				this.weaponMods = base.gameObject.AddComponent<WeaponMods>();
				this.chatSpam = base.gameObject.AddComponent<ChatSpam>();
				this.vehicleNoclip = base.gameObject.AddComponent<VehicleNoclip>();
				this.itemVacuum = base.gameObject.AddComponent<ItemVacuum>();
				this.freeCam = base.gameObject.AddComponent<FreeCam>();
				this.menu = new Menu(this);
				Config.Load();
				Overrides.Awake();
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000027C9 File Offset: 0x000009C9
		private void OnGUI()
		{
			Menu menu = this.menu;
			if (menu != null)
			{
				menu.Draw();
			}
			ESP esp = this.esp;
			if (esp != null)
			{
				esp.Draw();
			}
			Cheat.modules.Aimbot aimbot = this.aimbot;
			if (aimbot != null)
			{
				aimbot.Draw();
				return;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00012E84 File Offset: 0x00011084
		private void Update()
		{
			if (Main.CachedCamera == null || !Main.CachedCamera.isActiveAndEnabled)
			{
				Main.CachedCamera = Camera.main ?? Camera.current;
			}
			if (!string.IsNullOrEmpty(this.menu.RebindingModule))
			{
				if (Input.anyKeyDown)
				{
					foreach (KeyCode keyCode in Menu.allKeyCodes)
					{
						if (Input.GetKeyDown(keyCode))
						{
							this.SetKey(this.menu.RebindingModule, (keyCode == KeyCode.Escape) ? 0 : keyCode);
							this.menu.RebindingModule = null;
							Config.Save();
							Input.ResetInputAxes();
							return;
						}
					}
				}
				return;
			}
			if (!Overrides.bBeingSpied)
			{
				if (this.NoDisconnectTimer && PlayerPauseUI.active)
				{
					if (Main.fieldInfo_0 == null)
					{
						FieldInfo[] fields = typeof(PlayerPauseUI).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						List<FieldInfo> list = new List<FieldInfo>();
						foreach (FieldInfo fieldInfo in fields)
						{
							if (fieldInfo.FieldType == typeof(float))
							{
								list.Add(fieldInfo);
							}
						}
						Main.fieldInfo_0 = list.ToArray();
					}
					FieldInfo[] array = Main.fieldInfo_0;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].SetValue(null, -99999f);
					}
				}
				if (Input.GetKeyDown(this.KeyCancelLoading) && Provider.isLoading)
				{
					Provider.disconnect();
				}
				if (Input.GetKeyDown(this.KeyFastDisconnect))
				{
					Provider.disconnect();
				}
				if (Input.GetKeyDown(this.KeyToggleESP))
				{
					this.esp.espEnabled = !this.esp.espEnabled;
				}
				if (Input.GetKeyDown(this.KeyToggleAimbot))
				{
					this.aimbot.enabled = !this.aimbot.enabled;
				}
				if (Input.GetKeyDown(this.KeyToggleTrigger))
				{
					this.triggerbot.enabled = !this.triggerbot.enabled;
				}
				if (Input.GetKeyDown(this.KeyToggleChatSpam))
				{
					this.chatSpam.Enabled = !this.chatSpam.Enabled;
				}
				if (Input.GetKeyDown(this.KeyFreeCam))
				{
					this.freeCam.Enabled = !this.freeCam.Enabled;
				}
				if (Input.GetKeyDown(this.KeyToggleVehicleNoclip))
				{
					this.vehicleNoclip.active = !this.vehicleNoclip.active;
				}
				if (Input.GetKeyDown(this.KeyPanic))
				{
					this.PanicDisable();
				}
				if (Input.GetKeyDown(this.KeyItemVacuum))
				{
					this.itemVacuum.Enabled = !this.itemVacuum.Enabled;
				}
				if (Input.GetKeyDown(this.KeyToggleMenu) && this.menu != null)
				{
					this.menu.showMenu = !this.menu.showMenu;
				}
				return;
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0001314C File Offset: 0x0001134C
		private void PanicDisable()
		{
			if (this.esp != null)
			{
				this.esp.espEnabled = false;
				if (this.esp != null)
				{
					this.esp.HardCleanGlow();
				}
			}
			if (this.visuals != null)
			{
				this.visuals.AlwaysDay = false;
				this.visuals.AlwaysSatellite = false;
				this.visuals.AlwaysCompass = false;
				this.visuals.HandleAlwaysDay();
				this.visuals.HandleSatellite();
				this.visuals.HandleCompass();
			}
			if (this.aimbot != null)
			{
				this.aimbot.enabled = false;
			}
			if (this.triggerbot != null)
			{
				this.triggerbot.enabled = false;
			}
			if (this.chatSpam != null)
			{
				this.chatSpam.Enabled = false;
			}
			if (this.vehicleNoclip != null)
			{
				this.vehicleNoclip.active = false;
			}
			if (this.menu != null)
			{
				this.menu.showMenu = false;
			}
			if (this.itemVacuum != null)
			{
				this.itemVacuum.Enabled = false;
			}
			if (this.freeCam != null)
			{
				this.freeCam.Enabled = false;
			}
			if (this.weaponMods != null)
			{
				this.weaponMods.noRecoil = false;
				this.weaponMods.noSpread = false;
				this.weaponMods.noSway = false;
				this.weaponMods.noShake = false;
				this.weaponMods.noBulletGravity = false;
				this.weaponMods.RestoreAll();
			}
			Config.Save();
			Loader.Unload();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000132F0 File Offset: 0x000114F0
		private void SetKey(string module, KeyCode key)
		{
			if (module != null)
			{
				switch (module.Length)
				{
				case 3:
					if (!(module == "ESP"))
					{
						return;
					}
					this.KeyToggleESP = key;
					return;
				case 4:
					if (module == "Menu")
					{
						this.KeyToggleMenu = key;
						return;
					}
					return;
				case 5:
					if (!(module == "Panic"))
					{
						return;
					}
					this.KeyPanic = key;
					break;
				case 6:
					if (!(module == "Aimbot"))
					{
						return;
					}
					this.KeyToggleAimbot = key;
					return;
				case 7:
					if (module == "FreeCam")
					{
						this.KeyFreeCam = key;
						return;
					}
					return;
				case 8:
					if (!(module == "ChatSpam"))
					{
						return;
					}
					this.KeyToggleChatSpam = key;
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
						this.KeyAimbotHold = key;
						return;
					}
					else if (c != 'I')
					{
						if (c != 'T')
						{
							return;
						}
						if (!(module == "Triggerbot"))
						{
							return;
						}
						this.KeyToggleTrigger = key;
						return;
					}
					else
					{
						if (module == "ItemVacuum")
						{
							this.KeyItemVacuum = key;
							return;
						}
						return;
					}
					break;
				}
				case 13:
					if (module == "VehicleNoClip")
					{
						this.KeyToggleVehicleNoclip = key;
						return;
					}
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x0400018D RID: 397
		public ESP esp;

		// Token: 0x0400018E RID: 398
		public Cheat.modules.Aimbot aimbot;

		// Token: 0x0400018F RID: 399
		public Visuals visuals;

		// Token: 0x04000190 RID: 400
		public Triggerbot triggerbot;

		// Token: 0x04000191 RID: 401
		public WeaponMods weaponMods;

		// Token: 0x04000192 RID: 402
		public ChatSpam chatSpam;

		// Token: 0x04000193 RID: 403
		public Entities entities;

		// Token: 0x04000194 RID: 404
		public VehicleNoclip vehicleNoclip;

		// Token: 0x04000195 RID: 405
		public ItemVacuum itemVacuum;

		// Token: 0x04000196 RID: 406
		public FreeCam freeCam;

		// Token: 0x04000197 RID: 407
		public KeyCode KeyToggleMenu = KeyCode.F1;

		// Token: 0x04000198 RID: 408
		public KeyCode KeyToggleESP = KeyCode.F2;

		// Token: 0x04000199 RID: 409
		public KeyCode KeyToggleAimbot = KeyCode.F3;

		// Token: 0x0400019A RID: 410
		public KeyCode KeyAimbotHold = KeyCode.Mouse1;

		// Token: 0x0400019B RID: 411
		public KeyCode KeyToggleTrigger = KeyCode.F4;

		// Token: 0x0400019C RID: 412
		public KeyCode KeyToggleChatSpam = KeyCode.F5;

		// Token: 0x0400019D RID: 413
		public KeyCode KeyToggleVehicleNoclip = KeyCode.LeftShift;

		// Token: 0x0400019E RID: 414
		public KeyCode KeyItemVacuum = KeyCode.F6;

		// Token: 0x0400019F RID: 415
		public KeyCode KeyFreeCam = KeyCode.F7;

		// Token: 0x040001A0 RID: 416
		public KeyCode KeyCancelLoading = KeyCode.Escape;

		// Token: 0x040001A1 RID: 417
		public KeyCode KeyFastDisconnect = KeyCode.F8;

		// Token: 0x040001A2 RID: 418
		public KeyCode KeyPanic = KeyCode.Delete;

		// Token: 0x040001A3 RID: 419
		public bool AimbotHoldToAim = true;

		// Token: 0x040001A4 RID: 420
		internal Menu menu;

		// Token: 0x040001A5 RID: 421
		public bool NoDisconnectTimer = true;

		// Token: 0x040001A6 RID: 422
		private static FieldInfo[] fieldInfo_0;
	}
}

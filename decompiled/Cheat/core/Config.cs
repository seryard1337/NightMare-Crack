using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cheat.modules;
using Newtonsoft.Json;
using UnityEngine;

namespace Cheat.core
{
	// Token: 0x0200001F RID: 31
	internal static class Config
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00008834 File Offset: 0x00006A34
		internal static void ApplyDefaultPreset(Main main)
		{
			if (!(main == null))
			{
				main.esp.espEnabled = true;
				main.esp.maxDistance = 853.1746f;
				main.esp.showBoxes = false;
				main.esp.showSkeleton = false;
				main.esp.showGlow = true;
				main.esp.showInfoText = true;
				main.esp.infoTextShowWeapon = true;
				main.esp.infoTextSize = 11;
				main.esp.showSnaplines = true;
				main.esp.snaplineThickness = 1.4f;
				main.esp.snaplineToFeet = true;
				main.esp.snaplineAlphaFalloff = true;
				main.esp.showItemGlow = true;
				main.esp.showItemIcons = false;
				main.esp.itemIconScale = 0.6845238f;
				main.esp.showItemName = false;
				main.esp.showVehicleGlow = false;
				main.esp.showVehicleName = false;
				main.esp.showBedGlow = false;
				main.esp.showBedName = false;
				main.esp.showClaimGlow = false;
				main.esp.showClaimName = false;
				main.esp.showFurnitureGlow = false;
				main.esp.showFurnitureName = false;
				main.aimbot.enabled = true;
				main.aimbot.silentAimEnabled = false;
				main.aimbot.silentAlwaysHead = true;
				main.aimbot.fov = 14.444445f;
				main.aimbot.smooth = false;
				main.aimbot.smoothFactor = 12f;
				main.aimbot.useVisibleCheck = true;
				main.aimbot.usePrediction = false;
				main.aimbot.predictionFactor = 38f;
				main.aimbot.useBallisticPrediction = false;
				main.aimbot.ballisticFactor = 5.2f;
				main.aimbot.preferHead = true;
				main.aimbot.noFovMode = false;
				main.aimbot.useWeaponRange = true;
				main.aimbot.customMaxDistance = 200f;
				main.aimbot.drawFov = true;
				main.aimbot.aimAtZombies = false;
				main.triggerbot.enabled = false;
				main.triggerbot.useWeaponRange = true;
				main.triggerbot.customMaxDistance = 200f;
				main.weaponMods.noRecoil = true;
				main.weaponMods.noSpread = true;
				main.weaponMods.noSway = true;
				main.weaponMods.noShake = true;
				main.weaponMods.noBulletGravity = true;
				main.weaponMods.recoilReduction = 100f;
				main.weaponMods.spreadReduction = 100f;
				main.weaponMods.swayReduction = 100f;
				main.weaponMods.shakeReduction = 100f;
				main.weaponMods.dropReduction = 100f;
				main.visuals.AlwaysDay = true;
				main.visuals.CustomDayTime = 1200U;
				main.visuals.AlwaysSatellite = true;
				main.visuals.AlwaysCompass = true;
				main.chatSpam.Enabled = false;
				main.chatSpam.SpamText = "";
				main.chatSpam.Interval = 0.4863462f;
				Overrides.bHideOnSpy = true;
				main.vehicleNoclip.active = false;
				main.vehicleNoclip.speedMultiplier = 1f;
				main.vehicleNoclip.stabilizeRoll = true;
				main.vehicleNoclip.nullRoll = true;
				main.vehicleNoclip.mouseControl = true;
				main.vehicleNoclip.useArrowKeys = true;
				main.vehicleNoclip.arrowRotationSpeed = 90f;
				main.itemVacuum.Enabled = false;
				main.itemVacuum.Range = 15f;
				main.itemVacuum.PickupWeapons = true;
				main.itemVacuum.PickupClothing = true;
				main.itemVacuum.PickupSupplies = true;
				main.itemVacuum.PickupEverything = false;
				main.freeCam.Enabled = false;
				main.freeCam.Speed = 15f;
				Menu.currentLanguage = 0;
				Menu.windowWidth = 720f;
				Menu.windowHeight = 575f;
				Menu.showHudFeatures = false;
				Menu.showHudWeapon = false;
				Menu.showHudAdmins = false;
				Menu.hudFeaturesWidth = 160f;
				Menu.hudWeaponWidth = 220f;
				Menu.hudAdminsWidth = 180f;
				Menu.rectHudFeatures = new Rect(20f, 20f, 160f, 20f);
				Menu.rectHudWeapon = new Rect(20f, 300f, 220f, 20f);
				Menu.rectHudAdmins = new Rect(20f, 500f, 180f, 20f);
				main.NoDisconnectTimer = true;
				main.KeyToggleESP = KeyCode.F2;
				main.KeyToggleAimbot = KeyCode.F3;
				main.KeyToggleTrigger = KeyCode.F4;
				main.KeyToggleChatSpam = KeyCode.F5;
				main.KeyItemVacuum = KeyCode.F6;
				main.KeyFreeCam = KeyCode.F7;
				main.KeyAimbotHold = KeyCode.Mouse1;
				main.KeyToggleMenu = KeyCode.F1;
				main.KeyToggleVehicleNoclip = KeyCode.LeftShift;
				main.KeyPanic = KeyCode.Delete;
				main.KeyCancelLoading = KeyCode.Escape;
				main.KeyFastDisconnect = KeyCode.F8;
				main.AimbotHoldToAim = true;
				Utils.ManualFriends.Clear();
				return;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00008D74 File Offset: 0x00006F74
		internal static void Load()
		{
			Main main = UnityEngine.Object.FindObjectOfType<Main>();
			if (main == null)
			{
				return;
			}
			if (!File.Exists(Config.path))
			{
				Config.ApplyDefaultPreset(main);
				Config.Save();
				return;
			}
			Config.ConfigData configData = JsonConvert.DeserializeObject<Config.ConfigData>(File.ReadAllText(Config.path));
			main.esp.espEnabled = configData.ESP_Enabled;
			main.esp.maxDistance = configData.ESP_MaxDistance;
			main.esp.showBoxes = configData.ESP_Boxes;
			main.esp.showSkeleton = configData.ESP_Skeleton;
			main.esp.showGlow = configData.ESP_Glow;
			main.esp.showInfoText = configData.ESP_InfoText;
			main.esp.infoTextShowWeapon = configData.ESP_InfoTextShowWeapon;
			main.esp.infoTextSize = configData.ESP_InfoTextSize;
			main.esp.showSnaplines = configData.ESP_Snaplines;
			main.esp.snaplineThickness = configData.ESP_SnaplineThickness;
			main.esp.snaplineToFeet = configData.ESP_SnaplineToFeet;
			main.esp.snaplineAlphaFalloff = configData.ESP_SnaplineAlphaFalloff;
			main.esp.showItemGlow = configData.ESP_ItemGlow;
			main.esp.showItemIcons = configData.ESP_ItemIcons;
			main.esp.itemIconScale = configData.ESP_ItemIconScale;
			main.esp.showItemName = configData.ESP_ItemName;
			main.esp.showVehicleGlow = configData.ESP_VehicleGlow;
			main.esp.showVehicleName = configData.ESP_VehicleName;
			main.esp.showBedGlow = configData.ESP_BedGlow;
			main.esp.showBedName = configData.ESP_BedName;
			main.esp.showClaimGlow = configData.ESP_ClaimGlow;
			main.esp.showClaimName = configData.ESP_ClaimName;
			main.esp.showFurnitureGlow = configData.ESP_FurnitureGlow;
			main.esp.showFurnitureName = configData.ESP_FurnitureName;
			main.aimbot.enabled = configData.Aimbot_Enabled;
			main.aimbot.silentAimEnabled = configData.Aimbot_SilentEnabled;
			main.aimbot.silentAlwaysHead = configData.Aimbot_SilentAlwaysHead;
			main.aimbot.fov = configData.Aimbot_FOV;
			main.aimbot.smooth = configData.Aimbot_Smooth;
			main.aimbot.smoothFactor = configData.Aimbot_SmoothFactor;
			main.aimbot.useVisibleCheck = configData.Aimbot_VisibleCheck;
			main.aimbot.usePrediction = configData.Aimbot_Prediction;
			main.aimbot.predictionFactor = configData.Aimbot_PredictionFactor;
			main.aimbot.useBallisticPrediction = configData.Aimbot_BallisticPrediction;
			main.aimbot.ballisticFactor = configData.Aimbot_BallisticFactor;
			main.aimbot.preferHead = configData.Aimbot_PreferHead;
			main.aimbot.noFovMode = configData.Aimbot_NoFovMode;
			main.aimbot.useWeaponRange = configData.Aimbot_UseWeaponRange;
			main.aimbot.customMaxDistance = configData.Aimbot_CustomMaxDistance;
			main.aimbot.drawFov = configData.Aimbot_DrawFov;
			main.aimbot.aimAtZombies = configData.Aimbot_AimAtZombies;
			main.triggerbot.enabled = configData.Triggerbot_Enabled;
			main.triggerbot.useWeaponRange = configData.Triggerbot_UseWeaponRange;
			main.triggerbot.customMaxDistance = configData.Triggerbot_CustomMaxDistance;
			main.weaponMods.noRecoil = configData.WeaponMods_NoRecoil;
			main.weaponMods.noSpread = configData.WeaponMods_NoSpread;
			main.weaponMods.noSway = configData.WeaponMods_NoSway;
			main.weaponMods.noShake = configData.WeaponMods_NoShake;
			main.weaponMods.noBulletGravity = configData.WeaponMods_NoBulletGravity;
			main.weaponMods.recoilReduction = configData.WeaponMods_RecoilReduction;
			main.weaponMods.spreadReduction = configData.WeaponMods_SpreadReduction;
			main.weaponMods.swayReduction = configData.WeaponMods_SwayReduction;
			main.weaponMods.shakeReduction = configData.WeaponMods_ShakeReduction;
			main.weaponMods.dropReduction = configData.WeaponMods_GravityReduction;
			main.visuals.AlwaysDay = configData.AlwaysDay;
			main.visuals.CustomDayTime = configData.CustomDayTime;
			main.visuals.AlwaysSatellite = configData.AlwaysSatellite;
			main.visuals.AlwaysCompass = configData.AlwaysCompass;
			main.chatSpam.Enabled = configData.ChatSpam_Enabled;
			main.chatSpam.SpamText = configData.ChatSpam_Text;
			main.chatSpam.Interval = configData.ChatSpam_Interval;
			Overrides.bHideOnSpy = configData.AntiSpy_HideOnSpy;
			main.vehicleNoclip.active = configData.VehicleNoClip_Enabled;
			main.vehicleNoclip.speedMultiplier = configData.VehicleNoClip_SpeedMultiplier;
			main.vehicleNoclip.stabilizeRoll = configData.VehicleNoClip_StabilizeRoll;
			main.vehicleNoclip.nullRoll = configData.VehicleNoClip_NullRoll;
			main.vehicleNoclip.mouseControl = configData.VehicleNoClip_MouseControl;
			main.vehicleNoclip.useArrowKeys = configData.VehicleNoClip_UseArrowKeys;
			main.vehicleNoclip.arrowRotationSpeed = configData.VehicleNoClip_ArrowSpeed;
			main.itemVacuum.Enabled = configData.ItemVacuum_Enabled;
			main.itemVacuum.Range = configData.ItemVacuum_Range;
			main.itemVacuum.PickupWeapons = configData.ItemVacuum_Weapons;
			main.itemVacuum.PickupClothing = configData.ItemVacuum_Clothing;
			main.itemVacuum.PickupSupplies = configData.ItemVacuum_Supplies;
			main.itemVacuum.PickupEverything = configData.ItemVacuum_Everything;
			main.freeCam.Enabled = configData.FreeCam_Enabled;
			main.freeCam.Speed = configData.FreeCam_Speed;
			Menu.currentLanguage = configData.Menu_Language;
			Menu.windowWidth = configData.Menu_WindowWidth;
			Menu.windowHeight = configData.Menu_WindowHeight;
			Menu.showHudFeatures = configData.HUD_Features_Enabled;
			Menu.showHudWeapon = configData.HUD_Weapon_Enabled;
			Menu.showHudAdmins = configData.HUD_Admins_Enabled;
			Menu.hudFeaturesWidth = configData.HUD_Features_Width;
			Menu.hudWeaponWidth = configData.HUD_Weapon_Width;
			Menu.hudAdminsWidth = configData.HUD_Admins_Width;
			Menu.rectHudFeatures = new Rect(configData.HUD_Features_X, configData.HUD_Features_Y, configData.HUD_Features_Width, 20f);
			Menu.rectHudWeapon = new Rect(configData.HUD_Weapon_X, configData.HUD_Weapon_Y, configData.HUD_Weapon_Width, 20f);
			Menu.rectHudAdmins = new Rect(configData.HUD_Admins_X, configData.HUD_Admins_Y, configData.HUD_Admins_Width, 20f);
			main.NoDisconnectTimer = configData.Misc_NoDisconnectTimer;
			Utils.ManualFriends.Clear();
			if (configData.Friends != null)
			{
				foreach (ulong num in configData.Friends)
				{
					Utils.ManualFriends.Add(num);
				}
			}
			main.KeyToggleESP = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyESP);
			main.KeyToggleAimbot = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyAimbot);
			main.KeyToggleTrigger = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyTriggerbot);
			main.KeyToggleChatSpam = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyChatSpam);
			main.KeyAimbotHold = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyAimbotHold);
			main.KeyToggleMenu = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyMenu);
			main.KeyToggleVehicleNoclip = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyVehicleNoClip);
			main.KeyItemVacuum = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyItemVacuum);
			main.KeyFreeCam = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyFreeCam);
			main.KeyPanic = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyPanic);
			main.KeyCancelLoading = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyCancelLoading);
			main.KeyFastDisconnect = (KeyCode)Enum.Parse(typeof(KeyCode), configData.KeyFastDisconnect);
			main.AimbotHoldToAim = configData.AimbotHoldToAim;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00009590 File Offset: 0x00007790
		internal static void Save()
		{
			Main main = UnityEngine.Object.FindObjectOfType<Main>();
			if (!(main == null))
			{
				string text = JsonConvert.SerializeObject(new Config.ConfigData
				{
					ESP_Enabled = main.esp.espEnabled,
					ESP_MaxDistance = main.esp.maxDistance,
					ESP_Boxes = main.esp.showBoxes,
					ESP_Skeleton = main.esp.showSkeleton,
					ESP_Glow = main.esp.showGlow,
					ESP_InfoText = main.esp.showInfoText,
					ESP_InfoTextShowWeapon = main.esp.infoTextShowWeapon,
					ESP_InfoTextSize = main.esp.infoTextSize,
					ESP_Snaplines = main.esp.showSnaplines,
					ESP_SnaplineThickness = main.esp.snaplineThickness,
					ESP_SnaplineToFeet = main.esp.snaplineToFeet,
					ESP_SnaplineAlphaFalloff = main.esp.snaplineAlphaFalloff,
					ESP_ItemGlow = main.esp.showItemGlow,
					ESP_ItemIcons = main.esp.showItemIcons,
					ESP_ItemIconScale = main.esp.itemIconScale,
					ESP_ItemName = main.esp.showItemName,
					ESP_VehicleGlow = main.esp.showVehicleGlow,
					ESP_VehicleName = main.esp.showVehicleName,
					ESP_BedGlow = main.esp.showBedGlow,
					ESP_BedName = main.esp.showBedName,
					ESP_ClaimGlow = main.esp.showClaimGlow,
					ESP_ClaimName = main.esp.showClaimName,
					ESP_FurnitureGlow = main.esp.showFurnitureGlow,
					ESP_FurnitureName = main.esp.showFurnitureName,
					Aimbot_Enabled = main.aimbot.enabled,
					Aimbot_SilentEnabled = main.aimbot.silentAimEnabled,
					Aimbot_SilentAlwaysHead = main.aimbot.silentAlwaysHead,
					Aimbot_FOV = main.aimbot.fov,
					Aimbot_Smooth = main.aimbot.smooth,
					Aimbot_SmoothFactor = main.aimbot.smoothFactor,
					Aimbot_VisibleCheck = main.aimbot.useVisibleCheck,
					Aimbot_Prediction = main.aimbot.usePrediction,
					Aimbot_PredictionFactor = main.aimbot.predictionFactor,
					Aimbot_BallisticPrediction = main.aimbot.useBallisticPrediction,
					Aimbot_BallisticFactor = main.aimbot.ballisticFactor,
					Aimbot_PreferHead = main.aimbot.preferHead,
					Aimbot_NoFovMode = main.aimbot.noFovMode,
					Aimbot_UseWeaponRange = main.aimbot.useWeaponRange,
					Aimbot_CustomMaxDistance = main.aimbot.customMaxDistance,
					Aimbot_DrawFov = main.aimbot.drawFov,
					Aimbot_AimAtZombies = main.aimbot.aimAtZombies,
					Triggerbot_Enabled = main.triggerbot.enabled,
					Triggerbot_UseWeaponRange = main.triggerbot.useWeaponRange,
					Triggerbot_CustomMaxDistance = main.triggerbot.customMaxDistance,
					WeaponMods_NoRecoil = main.weaponMods.noRecoil,
					WeaponMods_NoSpread = main.weaponMods.noSpread,
					WeaponMods_NoSway = main.weaponMods.noSway,
					WeaponMods_NoShake = main.weaponMods.noShake,
					WeaponMods_NoBulletGravity = main.weaponMods.noBulletGravity,
					WeaponMods_RecoilReduction = main.weaponMods.recoilReduction,
					WeaponMods_SpreadReduction = main.weaponMods.spreadReduction,
					WeaponMods_SwayReduction = main.weaponMods.swayReduction,
					WeaponMods_ShakeReduction = main.weaponMods.shakeReduction,
					WeaponMods_GravityReduction = main.weaponMods.dropReduction,
					AlwaysDay = main.visuals.AlwaysDay,
					CustomDayTime = main.visuals.CustomDayTime,
					AlwaysSatellite = main.visuals.AlwaysSatellite,
					AlwaysCompass = main.visuals.AlwaysCompass,
					ChatSpam_Enabled = main.chatSpam.Enabled,
					ChatSpam_Text = main.chatSpam.SpamText,
					ChatSpam_Interval = main.chatSpam.Interval,
					AntiSpy_HideOnSpy = Overrides.bHideOnSpy,
					VehicleNoClip_Enabled = main.vehicleNoclip.active,
					VehicleNoClip_SpeedMultiplier = main.vehicleNoclip.speedMultiplier,
					VehicleNoClip_StabilizeRoll = main.vehicleNoclip.stabilizeRoll,
					VehicleNoClip_NullRoll = main.vehicleNoclip.nullRoll,
					VehicleNoClip_MouseControl = main.vehicleNoclip.mouseControl,
					VehicleNoClip_UseArrowKeys = main.vehicleNoclip.useArrowKeys,
					VehicleNoClip_ArrowSpeed = main.vehicleNoclip.arrowRotationSpeed,
					ItemVacuum_Enabled = main.itemVacuum.Enabled,
					ItemVacuum_Range = main.itemVacuum.Range,
					ItemVacuum_Weapons = main.itemVacuum.PickupWeapons,
					ItemVacuum_Clothing = main.itemVacuum.PickupClothing,
					ItemVacuum_Supplies = main.itemVacuum.PickupSupplies,
					ItemVacuum_Everything = main.itemVacuum.PickupEverything,
					FreeCam_Enabled = main.freeCam.Enabled,
					FreeCam_Speed = main.freeCam.Speed,
					Menu_Language = Menu.currentLanguage,
					Menu_WindowWidth = Menu.windowWidth,
					Menu_WindowHeight = Menu.windowHeight,
					HUD_Features_Enabled = Menu.showHudFeatures,
					HUD_Weapon_Enabled = Menu.showHudWeapon,
					HUD_Admins_Enabled = Menu.showHudAdmins,
					HUD_Features_Width = Menu.hudFeaturesWidth,
					HUD_Weapon_Width = Menu.hudWeaponWidth,
					HUD_Admins_Width = Menu.hudAdminsWidth,
					HUD_Features_X = Menu.rectHudFeatures.x,
					HUD_Features_Y = Menu.rectHudFeatures.y,
					HUD_Weapon_X = Menu.rectHudWeapon.x,
					HUD_Weapon_Y = Menu.rectHudWeapon.y,
					HUD_Admins_X = Menu.rectHudAdmins.x,
					HUD_Admins_Y = Menu.rectHudAdmins.y,
					Misc_NoDisconnectTimer = main.NoDisconnectTimer,
					KeyESP = main.KeyToggleESP.ToString(),
					KeyAimbot = main.KeyToggleAimbot.ToString(),
					KeyTriggerbot = main.KeyToggleTrigger.ToString(),
					KeyChatSpam = main.KeyToggleChatSpam.ToString(),
					KeyAimbotHold = main.KeyAimbotHold.ToString(),
					KeyMenu = main.KeyToggleMenu.ToString(),
					KeyVehicleNoClip = main.KeyToggleVehicleNoclip.ToString(),
					KeyItemVacuum = main.KeyItemVacuum.ToString(),
					KeyFreeCam = main.KeyFreeCam.ToString(),
					KeyPanic = main.KeyPanic.ToString(),
					KeyCancelLoading = main.KeyCancelLoading.ToString(),
					KeyFastDisconnect = main.KeyFastDisconnect.ToString(),
					AimbotHoldToAim = main.AimbotHoldToAim,
					Friends = Utils.ManualFriends.ToList<ulong>()
				}, Formatting.Indented);
				File.WriteAllText(Config.path, text);
				return;
			}
		}

		// Token: 0x040000D1 RID: 209
		private static readonly string path = "C:\\Users\\Public\\CheatConfig.json";

		// Token: 0x02000020 RID: 32
		[Serializable]
		private class ConfigData
		{
			// Token: 0x040000D2 RID: 210
			public bool ESP_Enabled = true;

			// Token: 0x040000D3 RID: 211
			public float ESP_MaxDistance = 853.1746f;

			// Token: 0x040000D4 RID: 212
			public bool ESP_Boxes;

			// Token: 0x040000D5 RID: 213
			public bool ESP_Skeleton;

			// Token: 0x040000D6 RID: 214
			public bool ESP_Glow = true;

			// Token: 0x040000D7 RID: 215
			public bool ESP_InfoText = true;

			// Token: 0x040000D8 RID: 216
			public bool ESP_InfoTextShowWeapon = true;

			// Token: 0x040000D9 RID: 217
			public int ESP_InfoTextSize = 11;

			// Token: 0x040000DA RID: 218
			public bool ESP_Snaplines = true;

			// Token: 0x040000DB RID: 219
			public float ESP_SnaplineThickness = 1.4f;

			// Token: 0x040000DC RID: 220
			public bool ESP_SnaplineToFeet = true;

			// Token: 0x040000DD RID: 221
			public bool ESP_SnaplineAlphaFalloff = true;

			// Token: 0x040000DE RID: 222
			public bool ESP_ItemGlow = true;

			// Token: 0x040000DF RID: 223
			public bool ESP_ItemIcons;

			// Token: 0x040000E0 RID: 224
			public float ESP_ItemIconScale = 0.6845238f;

			// Token: 0x040000E1 RID: 225
			public bool ESP_ItemName;

			// Token: 0x040000E2 RID: 226
			public bool ESP_VehicleGlow;

			// Token: 0x040000E3 RID: 227
			public bool ESP_VehicleName;

			// Token: 0x040000E4 RID: 228
			public bool ESP_BedGlow;

			// Token: 0x040000E5 RID: 229
			public bool ESP_BedName;

			// Token: 0x040000E6 RID: 230
			public bool ESP_ClaimGlow;

			// Token: 0x040000E7 RID: 231
			public bool ESP_ClaimName;

			// Token: 0x040000E8 RID: 232
			public bool ESP_FurnitureGlow;

			// Token: 0x040000E9 RID: 233
			public bool ESP_FurnitureName;

			// Token: 0x040000EA RID: 234
			public bool Aimbot_Enabled = true;

			// Token: 0x040000EB RID: 235
			public bool Aimbot_SilentEnabled;

			// Token: 0x040000EC RID: 236
			public bool Aimbot_SilentAlwaysHead = true;

			// Token: 0x040000ED RID: 237
			public float Aimbot_FOV = 14.444445f;

			// Token: 0x040000EE RID: 238
			public bool Aimbot_Smooth;

			// Token: 0x040000EF RID: 239
			public float Aimbot_SmoothFactor = 12f;

			// Token: 0x040000F0 RID: 240
			public bool Aimbot_VisibleCheck = true;

			// Token: 0x040000F1 RID: 241
			public bool Aimbot_Prediction;

			// Token: 0x040000F2 RID: 242
			public float Aimbot_PredictionFactor = 38f;

			// Token: 0x040000F3 RID: 243
			public bool Aimbot_BallisticPrediction;

			// Token: 0x040000F4 RID: 244
			public float Aimbot_BallisticFactor = 5.2f;

			// Token: 0x040000F5 RID: 245
			public bool Aimbot_PreferHead = true;

			// Token: 0x040000F6 RID: 246
			public bool Aimbot_NoFovMode;

			// Token: 0x040000F7 RID: 247
			public bool Aimbot_DrawFov = true;

			// Token: 0x040000F8 RID: 248
			public bool Aimbot_AimAtZombies;

			// Token: 0x040000F9 RID: 249
			public bool Aimbot_UseWeaponRange = true;

			// Token: 0x040000FA RID: 250
			public float Aimbot_CustomMaxDistance = 200f;

			// Token: 0x040000FB RID: 251
			public bool Triggerbot_Enabled;

			// Token: 0x040000FC RID: 252
			public bool Triggerbot_UseWeaponRange = true;

			// Token: 0x040000FD RID: 253
			public float Triggerbot_CustomMaxDistance = 200f;

			// Token: 0x040000FE RID: 254
			public bool WeaponMods_NoRecoil = true;

			// Token: 0x040000FF RID: 255
			public bool WeaponMods_NoSpread = true;

			// Token: 0x04000100 RID: 256
			public bool WeaponMods_NoSway = true;

			// Token: 0x04000101 RID: 257
			public bool WeaponMods_NoShake = true;

			// Token: 0x04000102 RID: 258
			public bool WeaponMods_NoBulletGravity = true;

			// Token: 0x04000103 RID: 259
			public float WeaponMods_RecoilReduction = 100f;

			// Token: 0x04000104 RID: 260
			public float WeaponMods_SpreadReduction = 100f;

			// Token: 0x04000105 RID: 261
			public float WeaponMods_SwayReduction = 100f;

			// Token: 0x04000106 RID: 262
			public float WeaponMods_ShakeReduction = 100f;

			// Token: 0x04000107 RID: 263
			public float WeaponMods_GravityReduction = 100f;

			// Token: 0x04000108 RID: 264
			public bool AlwaysDay = true;

			// Token: 0x04000109 RID: 265
			public uint CustomDayTime = 1200U;

			// Token: 0x0400010A RID: 266
			public bool AlwaysSatellite = true;

			// Token: 0x0400010B RID: 267
			public bool AlwaysCompass = true;

			// Token: 0x0400010C RID: 268
			public bool ChatSpam_Enabled;

			// Token: 0x0400010D RID: 269
			public string ChatSpam_Text = "";

			// Token: 0x0400010E RID: 270
			public float ChatSpam_Interval = 0.4863462f;

			// Token: 0x0400010F RID: 271
			public bool AntiSpy_HideOnSpy = true;

			// Token: 0x04000110 RID: 272
			public bool VehicleNoClip_Enabled;

			// Token: 0x04000111 RID: 273
			public float VehicleNoClip_SpeedMultiplier = 1f;

			// Token: 0x04000112 RID: 274
			public bool VehicleNoClip_StabilizeRoll = true;

			// Token: 0x04000113 RID: 275
			public bool VehicleNoClip_NullRoll = true;

			// Token: 0x04000114 RID: 276
			public bool VehicleNoClip_MouseControl = true;

			// Token: 0x04000115 RID: 277
			public bool VehicleNoClip_UseArrowKeys = true;

			// Token: 0x04000116 RID: 278
			public float VehicleNoClip_ArrowSpeed = 90f;

			// Token: 0x04000117 RID: 279
			public bool ItemVacuum_Enabled;

			// Token: 0x04000118 RID: 280
			public float ItemVacuum_Range = 15f;

			// Token: 0x04000119 RID: 281
			public bool ItemVacuum_Weapons = true;

			// Token: 0x0400011A RID: 282
			public bool ItemVacuum_Clothing = true;

			// Token: 0x0400011B RID: 283
			public bool ItemVacuum_Supplies = true;

			// Token: 0x0400011C RID: 284
			public bool ItemVacuum_Everything;

			// Token: 0x0400011D RID: 285
			public bool FreeCam_Enabled;

			// Token: 0x0400011E RID: 286
			public float FreeCam_Speed = 15f;

			// Token: 0x0400011F RID: 287
			public int Menu_Language;

			// Token: 0x04000120 RID: 288
			public float Menu_WindowWidth = 720f;

			// Token: 0x04000121 RID: 289
			public float Menu_WindowHeight = 575f;

			// Token: 0x04000122 RID: 290
			public bool HUD_Features_Enabled;

			// Token: 0x04000123 RID: 291
			public bool HUD_Weapon_Enabled;

			// Token: 0x04000124 RID: 292
			public bool HUD_Admins_Enabled;

			// Token: 0x04000125 RID: 293
			public float HUD_Features_Width = 160f;

			// Token: 0x04000126 RID: 294
			public float HUD_Weapon_Width = 220f;

			// Token: 0x04000127 RID: 295
			public float HUD_Admins_Width = 180f;

			// Token: 0x04000128 RID: 296
			public float HUD_Features_X = 20f;

			// Token: 0x04000129 RID: 297
			public float HUD_Features_Y = 20f;

			// Token: 0x0400012A RID: 298
			public float HUD_Weapon_X = 20f;

			// Token: 0x0400012B RID: 299
			public float HUD_Weapon_Y = 300f;

			// Token: 0x0400012C RID: 300
			public float HUD_Admins_X = 20f;

			// Token: 0x0400012D RID: 301
			public float HUD_Admins_Y = 500f;

			// Token: 0x0400012E RID: 302
			public bool Misc_NoDisconnectTimer = true;

			// Token: 0x0400012F RID: 303
			public string KeyESP = "F2";

			// Token: 0x04000130 RID: 304
			public string KeyAimbot = "F3";

			// Token: 0x04000131 RID: 305
			public string KeyTriggerbot = "F4";

			// Token: 0x04000132 RID: 306
			public string KeyChatSpam = "F5";

			// Token: 0x04000133 RID: 307
			public string KeyItemVacuum = "F6";

			// Token: 0x04000134 RID: 308
			public string KeyFreeCam = "F7";

			// Token: 0x04000135 RID: 309
			public string KeyAimbotHold = "Mouse1";

			// Token: 0x04000136 RID: 310
			public string KeyMenu = "F1";

			// Token: 0x04000137 RID: 311
			public string KeyVehicleNoClip = "LeftShift";

			// Token: 0x04000138 RID: 312
			public string KeyPanic = "Delete";

			// Token: 0x04000139 RID: 313
			public string KeyCancelLoading = "Escape";

			// Token: 0x0400013A RID: 314
			public string KeyFastDisconnect = "F8";

			// Token: 0x0400013B RID: 315
			public bool AimbotHoldToAim = true;

			// Token: 0x0400013C RID: 316
			public List<ulong> Friends = new List<ulong>();
		}
	}
}

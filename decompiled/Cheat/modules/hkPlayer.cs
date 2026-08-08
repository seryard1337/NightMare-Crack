using System;
using System.Collections;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using SDG.Unturned;
using UnityEngine;
using HighlightingSystem;
using Cheat.core;

namespace Cheat.modules
{
	internal class hkPlayer : MonoBehaviour
	{
		internal static StaticResourceRef<AudioClip> hitCriticalSound = new StaticResourceRef<AudioClip>("Sounds/General/Hit");

		internal void Ov_onMoonUpdated(bool isFullMoon)
		{
		}

		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askScreenshot")]
		internal void Ov_ReceiveTakeScreenshot()
		{
			if (!Overrides.bBeingSpied)
			{
				base.StartCoroutine(TakeScreenshotRoutine());
			}
		}

		private IEnumerator TakeScreenshotRoutine()
		{
			Overrides.bBeingSpied = true;

			if (Overrides.bPlaySpySound && Main.CachedCamera != null)
			{
				AudioSource audio = Main.CachedCamera.GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.PlayOneShot((AudioClip)hitCriticalSound, 0.5f);
				}
			}

			Main main = Main.Instance;
			bool shouldHide = Overrides.bHideOnSpy && main != null;

			bool pEsp = false, pAim = false, pDay = false, pSat = false, pComp = false;
			bool pRecoil = false, pSpread = false, pSway = false, pShake = false, pGrav = false, pMenu = false;

			if (shouldHide)
			{
				pMenu = main.menu != null && main.menu.showMenu;
				if (main.menu != null) main.menu.showMenu = false;

				pEsp = main.esp.espEnabled;
				pAim = main.aimbot.enabled;
				pDay = main.visuals.AlwaysDay;
				pSat = main.visuals.AlwaysSatellite;
				pComp = main.visuals.AlwaysCompass;
				pRecoil = main.weaponMods.noRecoil;
				pSpread = main.weaponMods.noSpread;
				pSway = main.weaponMods.noSway;
				pShake = main.weaponMods.noShake;
				pGrav = main.weaponMods.noBulletGravity;

				main.esp.espEnabled = false;
				main.aimbot.enabled = false;
				main.visuals.AlwaysDay = false;
				main.visuals.AlwaysSatellite = false;
				main.visuals.AlwaysCompass = false;
				main.weaponMods.noRecoil = false;
				main.weaponMods.noSpread = false;
				main.weaponMods.noSway = false;
				main.weaponMods.noShake = false;
				main.weaponMods.noBulletGravity = false;

				if (pDay) main.visuals.HandleAlwaysDay();
				if (pSat) main.visuals.HandleSatellite();
				if (pComp) main.visuals.HandleCompass();

				if (main.esp.highlighters != null)
				{
					foreach (var hl in main.esp.highlighters)
					{
						if (hl.Value != null)
						{
							hl.Value.ConstantOff(0.25f);
							hl.Value.enabled = false;
						}
					}
				}

				yield return new WaitForFixedUpdate();
				yield return new WaitForEndOfFrame();
				yield return new WaitForSeconds(0.15f);
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}

			try
			{
				Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
				if (screenshot != null)
				{
					RenderTexture rt = RenderTexture.GetTemporary(640, 480, 0, screenshot.graphicsFormat);
					Graphics.Blit(screenshot, rt);
					UnityEngine.Object.Destroy(screenshot);

					Texture2D resized = new Texture2D(640, 480, TextureFormat.RGB24, false);
					RenderTexture.active = rt;
					resized.ReadPixels(new Rect(0f, 0f, 640f, 480f), 0, 0, false);
					RenderTexture.active = null;
					RenderTexture.ReleaseTemporary(rt);

					byte[] jpegData = ImageConversion.EncodeToJPG(resized, 33);
					UnityEngine.Object.Destroy(resized);

					if (jpegData != null && jpegData.Length < 30000)
					{
						ServerInstanceMethod.Get(typeof(Player), "ReceiveScreenshotRelay").Invoke(
							Player.LocalPlayer.GetNetId(),
							(ENetReliability)0,
							(Action<NetPakWriter>)delegate(NetPakWriter writer)
							{
								ushort len = (ushort)jpegData.Length;
								SystemNetPakWriterEx.WriteUInt16(writer, len);
								writer.WriteBytes(jpegData, len);
							}
						);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Nightmare] Spy Error: " + ex.Message);
			}

			if (shouldHide)
			{
				if (main.menu != null) main.menu.showMenu = pMenu;
				main.esp.espEnabled = pEsp;
				main.aimbot.enabled = pAim;
				main.visuals.AlwaysDay = pDay;
				main.visuals.AlwaysSatellite = pSat;
				main.visuals.AlwaysCompass = pComp;
				main.weaponMods.noRecoil = pRecoil;
				main.weaponMods.noSpread = pSpread;
				main.weaponMods.noSway = pSway;
				main.weaponMods.noShake = pShake;
				main.weaponMods.noBulletGravity = pGrav;

				if (pDay) main.visuals.HandleAlwaysDay();
				if (pSat) main.visuals.HandleSatellite();
				if (pComp) main.visuals.HandleCompass();

				if (main.esp.highlighters != null && pEsp)
				{
					foreach (var hl in main.esp.highlighters)
					{
						if (hl.Value != null)
						{
							hl.Value.enabled = true;
						}
					}
				}
			}

			Overrides.bBeingSpied = false;
		}
	}
}

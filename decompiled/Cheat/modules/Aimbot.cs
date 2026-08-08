using System;
using System.Reflection;
using Cheat.core;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x0200000C RID: 12
	public class Aimbot : MonoBehaviour
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000022E5 File Offset: 0x000004E5
		private void Awake()
		{
			Cheat.core.Main main = UnityEngine.Object.FindObjectOfType<Cheat.core.Main>();
			Cheat.core.Entities entities;
			if (main != null)
			{
				if ((entities = main.entities) != null)
				{
					goto IL_001B;
				}
			}
			entities = new Cheat.core.Entities();
			IL_001B:
			this.entities = entities;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002307 File Offset: 0x00000507
		public void Toggle()
		{
			this.enabled = !this.enabled;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000036A0 File Offset: 0x000018A0
		private void Update()
		{
			if (!this.enabled || Player.LocalPlayer == null || !Provider.isConnected || Provider.isLoading)
			{
				this.currentTarget = null;
				return;
			}
			Camera cachedCamera = Cheat.core.Main.CachedCamera;
			KeyCode keyAimbotHold = Cheat.core.Main.Instance.KeyAimbotHold;
			bool key;
			if (!Cheat.core.Main.Instance.AimbotHoldToAim)
			{
				if (Input.GetKeyDown(keyAimbotHold))
				{
					this.toggleAiming = !this.toggleAiming;
				}
				key = this.toggleAiming;
			}
			else
			{
				key = Input.GetKey(keyAimbotHold);
			}
			if (!key)
			{
				this.currentTarget = null;
				this.lastValidTarget = null;
				this.lastValidTime = 0f;
				return;
			}
			this.currentTarget = this.GetBestTarget();
			if (this.currentTarget == null)
			{
				if (this.lastValidTarget != null && Time.time - this.lastValidTime < 0.45f)
				{
					if (this.noFovMode)
					{
						this.currentTarget = this.lastValidTarget;
					}
					else
					{
						Vector3 aimPosition = this.GetAimPosition(this.lastValidTarget);
						Vector3 vector = cachedCamera.WorldToScreenPoint(aimPosition);
						if (vector.z > 0.05f && Vector2.Distance(new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f), new Vector2(vector.x, (float)Screen.height - vector.y)) <= this.method_0())
						{
							this.currentTarget = this.lastValidTarget;
							return;
						}
						this.currentTarget = null;
						this.lastValidTarget = null;
						return;
					}
				}
				return;
			}
			this.lastValidTarget = this.currentTarget;
			this.lastTargetPosition = (this.currentTarget as Component).transform.position;
			this.lastValidTime = Time.time;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002318 File Offset: 0x00000518
		private void LateUpdate()
		{
			if (this.currentTarget != null)
			{
				this.Aim(this.currentTarget);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003850 File Offset: 0x00001A50
		private float method_0()
		{
			float num = this.fov * 0.5f * 0.017453292f;
			float num2 = (float)0.7853981573134661;
			return Mathf.Tan(num) / Mathf.Tan(num2) * ((float)Screen.height * 0.5f);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003894 File Offset: 0x00001A94
		private Vector3 GetAimPosition(object targetObj)
		{
			Component component = targetObj as Component;
			Transform transform = ((component == null) ? null : component.transform);
			if (transform == null)
			{
				return Vector3.zero;
			}
			if (this.preferHead)
			{
				Transform limb = Utils.GetLimb(transform, (ELimb)13);
				if (limb != null)
				{
					return limb.position + Vector3.up * 0.22f;
				}
			}
			Transform limb2 = Utils.GetLimb(transform, (ELimb)12);
			if (!(limb2 != null))
			{
				return transform.position + Vector3.up * 1.4f;
			}
			float num = 0.8f;
			Player player = targetObj as Player;
			if (player != null)
			{
				EPlayerStance stance = player.stance.stance;
				num = (((int)stance == 3 || (int)stance == 2) ? 1f : (((int)stance == 4) ? 0.75f : (((int)stance == 5 || (int)stance == 1) ? 0.15f : 0.8f)));
			}
			return limb2.position + Vector3.up * num;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003990 File Offset: 0x00001B90
		private bool IsVisible(Vector3 from, Vector3 to, object targetObj)
		{
			Vector3 normalized = (to - from).normalized;
			float num = Vector3.Distance(from, to);
			RaycastHit raycastHit;
			if (Physics.Raycast(from + Vector3.up * 0.08f, normalized, out raycastHit, num + 0.4f, RayMasks.DAMAGE_CLIENT, QueryTriggerInteraction.UseGlobal))
			{
				Player player = targetObj as Player;
				if (player != null && DamageTool.getPlayer(raycastHit.transform) == player)
				{
					return true;
				}
				Zombie zombie = targetObj as Zombie;
				if (zombie != null && DamageTool.getZombie(raycastHit.transform) == zombie)
				{
					return true;
				}
			}
			if (Physics.Raycast(from + Vector3.up * 0.03f, normalized, out raycastHit, num + 0.4f, RayMasks.DAMAGE_CLIENT, QueryTriggerInteraction.UseGlobal))
			{
				Player player2 = targetObj as Player;
				if (player2 != null && DamageTool.getPlayer(raycastHit.transform) == player2)
				{
					return true;
				}
				Zombie zombie2 = targetObj as Zombie;
				if (zombie2 != null && DamageTool.getZombie(raycastHit.transform) == zombie2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003A98 File Offset: 0x00001C98
		private object GetBestTarget()
		{
			Camera cachedCamera = Cheat.core.Main.CachedCamera;
			object obj = null;
			float num = float.MaxValue;
			Vector3 position = Player.LocalPlayer.look.aim.position;
			Vector3 forward = Player.LocalPlayer.look.aim.forward;
			Vector2 vector;
			vector = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
			float num2 = this.method_0();
			float num3 = ((!this.useWeaponRange) ? this.customMaxDistance : Utils.GetGunRange());
			foreach (Player player in this.entities.Players)
			{
				if (!(player == null) && !(player == Player.LocalPlayer) && !player.life.isDead && !Utils.IsFriendly(player))
				{
					float num4 = this.EvaluateTarget(player, player.transform.position, position, forward, vector, num2, num3, cachedCamera);
					if (num4 < num)
					{
						num = num4;
						obj = player;
					}
				}
			}
			if (this.aimAtZombies)
			{
				foreach (Zombie zombie in this.entities.Zombies)
				{
					if (!(zombie == null) && !zombie.isDead)
					{
						float num5 = this.EvaluateTarget(zombie, zombie.transform.position, position, forward, vector, num2, num3, cachedCamera);
						if (num5 < num)
						{
							num = num5;
							obj = zombie;
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003C48 File Offset: 0x00001E48
		private float EvaluateTarget(object targetObj, Vector3 worldPos, Vector3 eyePos, Vector3 localForward, Vector2 screenCenter, float fovRadius, float maxAllowedDistance, Camera cam)
		{
			Vector3 aimPosition = this.GetAimPosition(targetObj);
			float num = this.entities.DistanceToLocal(worldPos);
			if (num > maxAllowedDistance)
			{
				return float.MaxValue;
			}
			if (this.useVisibleCheck && !this.IsVisible(eyePos, aimPosition, targetObj))
			{
				return float.MaxValue;
			}
			if (!this.noFovMode)
			{
				Vector3 vector = cam.WorldToScreenPoint(aimPosition);
				if (vector.z <= 0.05f)
				{
					return float.MaxValue;
				}
				Vector2 vector2;
				vector2 = new Vector2(vector.x, (float)Screen.height - vector.y);
				if (Vector2.Distance(screenCenter, vector2) > fovRadius)
				{
					return float.MaxValue;
				}
			}
			float num2 = num * 0.75f;
			if (!this.noFovMode)
			{
				Vector3 normalized = (aimPosition - eyePos).normalized;
				num2 += Vector3.Angle(localForward, normalized) * 1.8f;
			}
			return num2;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003D18 File Offset: 0x00001F18
		private void Aim(object target)
		{
			Component cachedCamera = Cheat.core.Main.CachedCamera;
			Vector3 aimPosition = this.GetAimPosition(target);
			Transform transform = (target as Component).transform;
			Vector3 vector = transform.position - this.lastTargetPosition;
			Vector3 vector2 = Vector3.zero;
			if (this.usePrediction)
			{
				vector2 += vector * this.entities.DistanceToLocal(transform.position) / (this.CalcPrediction() / this.predictionFactor);
			}
			if (this.useBallisticPrediction)
			{
				vector2 -= Physics.gravity * 0.018f * this.entities.DistanceToLocal(transform.position) / (this.CalcBallistics() * this.ballisticFactor);
			}
			Vector3 position = cachedCamera.transform.position;
			Vector3 vector3 = aimPosition + vector2;
			Vector2 vector4 = Utils.CalcAngles(position, vector3);
			if (!this.smooth)
			{
				typeof(PlayerLook).GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.LocalPlayer.look, vector4.x);
				typeof(PlayerLook).GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.LocalPlayer.look, vector4.y);
				return;
			}
			float num = (float)typeof(PlayerLook).GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Player.LocalPlayer.look);
			float num2 = (float)typeof(PlayerLook).GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Player.LocalPlayer.look);
			num = Mathf.LerpAngle(num, vector4.x, Time.deltaTime * this.smoothFactor);
			num2 = Mathf.LerpAngle(num2, vector4.y, Time.deltaTime * this.smoothFactor);
			typeof(PlayerLook).GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.LocalPlayer.look, num);
			typeof(PlayerLook).GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Player.LocalPlayer.look, num2);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003F44 File Offset: 0x00002144
		private float CalcPrediction()
		{
			ItemGunAsset itemGunAsset = Player.LocalPlayer.equipment.asset as ItemGunAsset;
			if (itemGunAsset != null)
			{
				return (float)itemGunAsset.ballisticSteps * itemGunAsset.ballisticTravel;
			}
			return 1f;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003F80 File Offset: 0x00002180
		private float CalcBallistics()
		{
			UseableGun useableGun = Player.LocalPlayer.equipment.useable as UseableGun;
			if (useableGun != null)
			{
				Attachments attachments = (Attachments)typeof(UseableGun).GetField("thirdAttachments", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(useableGun);
				if (((attachments == null) ? null : attachments.barrelAsset) != null)
				{
					return attachments.barrelAsset.ballisticDrop;
				}
			}
			return 1f;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003FE8 File Offset: 0x000021E8
		public object GetSilentTarget(Ray ray)
		{
			if (this.silentAimEnabled)
			{
				Camera cachedCamera = Cheat.core.Main.CachedCamera;
				object obj = null;
				float num = float.MaxValue;
				Vector3 origin = ray.origin;
				Vector3 direction = ray.direction;
				Vector2 vector;
				vector = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
				float num2 = this.method_0();
				float num3 = ((!this.useWeaponRange) ? this.customMaxDistance : Utils.GetGunRange());
				foreach (Player player in this.entities.Players)
				{
					if (!(player == null) && !(player == Player.LocalPlayer) && !player.life.isDead && !Utils.IsFriendly(player))
					{
						float num4 = this.EvaluateTarget(player, player.transform.position, origin, direction, vector, num2, num3, cachedCamera);
						if (num4 < num)
						{
							num = num4;
							obj = player;
						}
					}
				}
				if (this.aimAtZombies)
				{
					foreach (Zombie zombie in this.entities.Zombies)
					{
						if (!(zombie == null) && !zombie.isDead)
						{
							float num5 = this.EvaluateTarget(zombie, zombie.transform.position, origin, direction, vector, num2, num3, cachedCamera);
							if (num5 < num)
							{
								num = num5;
								obj = zombie;
							}
						}
					}
				}
				return obj;
			}
			return null;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000418C File Offset: 0x0000238C
		public void Draw()
		{
			Camera cachedCamera = Cheat.core.Main.CachedCamera;
			if (!this.enabled || (Overrides.bBeingSpied && Overrides.bHideOnSpy) || !Provider.isConnected || Provider.isLoading || cachedCamera == null)
			{
				return;
			}
			if (!this.noFovMode && this.drawFov)
			{
				Vector2 vector = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
				float num = this.method_0();
				Utils.DrawSmoothCircle(vector, num, 0.8f, new Color(1f, 0.3f, 0.3f, 0.85f));
				return;
			}
		}

		// Token: 0x0400002B RID: 43
		private Cheat.core.Entities entities;

		// Token: 0x0400002C RID: 44
		public new bool enabled;

		// Token: 0x0400002D RID: 45
		private float lastValidTime;

		// Token: 0x0400002E RID: 46
		public bool noFovMode;

		// Token: 0x0400002F RID: 47
		public float fov = 35f;

		// Token: 0x04000030 RID: 48
		public bool smooth = true;

		// Token: 0x04000031 RID: 49
		public float smoothFactor = 12f;

		// Token: 0x04000032 RID: 50
		public bool useVisibleCheck = true;

		// Token: 0x04000033 RID: 51
		public bool usePrediction = true;

		// Token: 0x04000034 RID: 52
		public float predictionFactor = 38f;

		// Token: 0x04000035 RID: 53
		public bool useBallisticPrediction = true;

		// Token: 0x04000036 RID: 54
		public float ballisticFactor = 5.2f;

		// Token: 0x04000037 RID: 55
		public bool preferHead = true;

		// Token: 0x04000038 RID: 56
		private bool toggleAiming;

		// Token: 0x04000039 RID: 57
		public bool useWeaponRange = true;

		// Token: 0x0400003A RID: 58
		public float customMaxDistance = 300f;

		// Token: 0x0400003B RID: 59
		public bool silentAimEnabled;

		// Token: 0x0400003C RID: 60
		public bool silentAlwaysHead = true;

		// Token: 0x0400003D RID: 61
		private Vector3 lastTargetPosition;

		// Token: 0x0400003E RID: 62
		public bool drawFov = true;

		// Token: 0x0400003F RID: 63
		public bool aimAtZombies;

		// Token: 0x04000040 RID: 64
		private object currentTarget;

		// Token: 0x04000041 RID: 65
		private object lastValidTarget;
	}
}

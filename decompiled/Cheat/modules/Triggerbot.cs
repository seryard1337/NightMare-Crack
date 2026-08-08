using System;
using Cheat.core;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x0200001A RID: 26
	public class Triggerbot : MonoBehaviour
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00002519 File Offset: 0x00000719
		public void Toggle()
		{
			this.enabled = !this.enabled;
			if (!this.enabled && this.bModified)
			{
				this.StopFire();
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000075E8 File Offset: 0x000057E8
		private void Update()
		{
			if (!this.enabled || Player.LocalPlayer == null || !Provider.isConnected || Provider.isLoading)
			{
				if (this.bModified)
				{
					this.StopFire();
				}
				return;
			}
			PlayerEquipment equipment = Player.LocalPlayer.equipment;
			if (equipment == null || equipment.asset == null || !equipment.IsEquipAnimationFinished)
			{
				if (this.bModified)
				{
					this.StopFire();
				}
				return;
			}
			float num = ((!this.useWeaponRange) ? this.customMaxDistance : Utils.GetGunRange());
			this.currentFiremode = this.GetFiremode(equipment.asset);
			Vector3 position = Player.LocalPlayer.look.aim.position;
			Vector3 forward = Player.LocalPlayer.look.aim.forward;
			Player player = null;
			RaycastHit raycastHit;
			if (Physics.Raycast(position, forward, out raycastHit, num, RayMasks.DAMAGE_CLIENT, QueryTriggerInteraction.UseGlobal))
			{
				player = DamageTool.getPlayer(raycastHit.transform);
			}
			if (!(player == null) && !(player == Player.LocalPlayer) && !player.life.isDead && !Utils.IsFriendly(player))
			{
				this.StartFire();
				return;
			}
			if (this.bModified)
			{
				this.StopFire();
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00007724 File Offset: 0x00005924
		private EFiremode GetFiremode(ItemAsset asset)
		{
			ItemGunAsset itemGunAsset = asset as ItemGunAsset;
			if (itemGunAsset != null)
			{
				object privateField = Utils.GetPrivateField(itemGunAsset, "firemode");
				if (privateField is EFiremode)
				{
					return (EFiremode)privateField;
				}
			}
			return (EFiremode)2;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000775C File Offset: 0x0000595C
		private void StartFire()
		{
			PlayerEquipment equipment = Player.LocalPlayer.equipment;
			if ((int)this.currentFiremode != 1)
			{
				if ((int)this.currentFiremode != 3)
				{
					Utils.SetPrivateField(equipment, "localWasPrimaryPressedBetweenSimulationFrames", true);
					Utils.SetPrivateField(equipment, "localWasPrimaryHeldLastFrame", false);
					goto IL_0069;
				}
			}
			Utils.SetPrivateField(equipment, "localWasPrimaryPressedBetweenSimulationFrames", true);
			Utils.SetPrivateField(equipment, "localWasPrimaryHeldLastFrame", true);
			IL_0069:
			this.bModified = true;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000077DC File Offset: 0x000059DC
		private void StopFire()
		{
			if (this.bModified)
			{
				PlayerEquipment equipment = Player.LocalPlayer.equipment;
				Utils.SetPrivateField(equipment, "localWasPrimaryPressedBetweenSimulationFrames", false);
				Utils.SetPrivateField(equipment, "localWasPrimaryHeldLastFrame", true);
				Utils.SetPrivateField(equipment, "localWasPrimaryReleasedBetweenSimulationFrames", true);
				this.bModified = false;
				return;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002540 File Offset: 0x00000740
		private void OnDisable()
		{
			if (this.bModified)
			{
				this.StopFire();
			}
		}

		// Token: 0x04000095 RID: 149
		public bool enabled;

		// Token: 0x04000096 RID: 150
		public bool useWeaponRange = true;

		// Token: 0x04000097 RID: 151
		public float customMaxDistance = 300f;

		// Token: 0x04000098 RID: 152
		private bool bModified;

		// Token: 0x04000099 RID: 153
		private EFiremode currentFiremode = (EFiremode)2;
	}
}

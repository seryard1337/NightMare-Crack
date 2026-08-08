using System;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x02000013 RID: 19
	public class FreeCam : MonoBehaviour
	{
		// Token: 0x0600006F RID: 111 RVA: 0x000066D8 File Offset: 0x000048D8
		private void Update()
		{
			if (!Provider.isConnected || Provider.isLoading || Player.LocalPlayer == null)
			{
				return;
			}
			if (Overrides.bBeingSpied && this.Enabled)
			{
				this.DisableFreeCam();
				return;
			}
			if (!this.Enabled)
			{
				if (this.modified)
				{
					this.DisableFreeCam();
				}
				return;
			}
			Player localPlayer = Player.LocalPlayer;
			localPlayer.movement.controller.enabled = false;
			localPlayer.movement.itemGravityMultiplier = 0f;
			this.modified = true;
			if (!PlayerUI.window.showCursor && !PlayerLifeUI.chatting)
			{
				float num = this.Speed;
				if (Input.GetKey((KeyCode)304))
				{
					num *= this.BoostMultiplier;
				}
				Transform aim = localPlayer.look.aim;
				Vector3 vector = Vector3.zero;
				if (Input.GetKey((KeyCode)119))
				{
					vector += aim.forward;
				}
				if (Input.GetKey((KeyCode)115))
				{
					vector -= aim.forward;
				}
				if (Input.GetKey((KeyCode)97))
				{
					vector -= aim.right;
				}
				if (Input.GetKey((KeyCode)100))
				{
					vector += aim.right;
				}
				if (Input.GetKey((KeyCode)32))
				{
					vector += Vector3.up;
				}
				if (Input.GetKey((KeyCode)306))
				{
					vector -= Vector3.up;
				}
				localPlayer.transform.position += vector.normalized * num * Time.deltaTime;
				return;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00006864 File Offset: 0x00004A64
		private void DisableFreeCam()
		{
			Player localPlayer = Player.LocalPlayer;
			if (localPlayer == null)
			{
				return;
			}
			localPlayer.movement.controller.enabled = true;
			ItemCloudAsset itemCloudAsset = localPlayer.equipment.asset as ItemCloudAsset;
			if (itemCloudAsset != null)
			{
				localPlayer.movement.itemGravityMultiplier = itemCloudAsset.gravity;
			}
			else
			{
				localPlayer.movement.itemGravityMultiplier = 1f;
			}
			this.Enabled = false;
			this.modified = false;
		}

		// Token: 0x04000073 RID: 115
		public new bool Enabled;

		// Token: 0x04000074 RID: 116
		public float Speed = 15f;

		// Token: 0x04000075 RID: 117
		public float BoostMultiplier = 4f;

		// Token: 0x04000076 RID: 118
		private bool modified;
	}
}

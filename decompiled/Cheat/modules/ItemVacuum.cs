using System;
using System.Collections.Generic;
using Cheat.core;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x02000018 RID: 24
	public class ItemVacuum : MonoBehaviour
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00007294 File Offset: 0x00005494
		private void Update()
		{
			if (!this.Enabled || !Provider.isConnected || Provider.isLoading || Player.LocalPlayer == null || Player.LocalPlayer.life.isDead)
			{
				return;
			}
			if (Time.time - this.lastActionTime < this.ActionDelay)
			{
				return;
			}
			Vector3 position = Player.LocalPlayer.transform.position;
			foreach (InteractableItem interactableItem in Cheat.core.Main.Instance.entities.Items)
			{
				if (!(interactableItem == null) && interactableItem.asset != null && interactableItem.gameObject.activeInHierarchy && Vector3.Distance(position, interactableItem.transform.position) <= this.Range)
				{
					EItemType type = interactableItem.asset.type;
					byte b = 0, b2 = 0, b3 = 0, b4 = 0;
					
					if ((this.PickupEverything || (this.PickupWeapons && this.weaponTypes.Contains(type)) || (this.PickupClothing && this.clothingTypes.Contains(type)) || (this.PickupSupplies && type == (EItemType)25)) && Player.LocalPlayer.inventory.tryFindSpace(interactableItem.asset.size_x, interactableItem.asset.size_y, out b, out b2, out b3, out b4))
					{
						interactableItem.use();
						this.lastActionTime = Time.time;
						break;
					}
				}
			}
		}

		// Token: 0x04000088 RID: 136
		public new bool Enabled;

		// Token: 0x04000089 RID: 137
		public float Range = 15.5f;

		// Token: 0x0400008A RID: 138
		public float ActionDelay = 0.1f;

		// Token: 0x0400008B RID: 139
		public bool PickupWeapons = true;

		// Token: 0x0400008C RID: 140
		public bool PickupClothing = true;

		// Token: 0x0400008D RID: 141
		public bool PickupSupplies = true;

		// Token: 0x0400008E RID: 142
		public bool PickupEverything = true;

		// Token: 0x0400008F RID: 143
		private float lastActionTime;

		// Token: 0x04000090 RID: 144
		private readonly HashSet<EItemType> clothingTypes = new HashSet<EItemType> { (EItemType)3, (EItemType)0, (EItemType)6, (EItemType)2, (EItemType)5, (EItemType)4, (EItemType)1 };

		// Token: 0x04000091 RID: 145
		private readonly HashSet<EItemType> weaponTypes = new HashSet<EItemType> { (EItemType)7, (EItemType)16 };
	}
}

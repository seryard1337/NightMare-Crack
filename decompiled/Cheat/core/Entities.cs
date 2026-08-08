using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.core
{
	// Token: 0x02000021 RID: 33
	public class Entities : MonoBehaviour
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000025F7 File Offset: 0x000007F7
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000025FF File Offset: 0x000007FF
		internal List<Player> Players { get; private set; } = new List<Player>();

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00002608 File Offset: 0x00000808
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00002610 File Offset: 0x00000810
		internal List<Zombie> Zombies { get; private set; } = new List<Zombie>();

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00002619 File Offset: 0x00000819
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00002621 File Offset: 0x00000821
		internal List<InteractableItem> Items { get; private set; } = new List<InteractableItem>();

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000262A File Offset: 0x0000082A
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00002632 File Offset: 0x00000832
		internal List<InteractableVehicle> Vehicles { get; private set; } = new List<InteractableVehicle>();

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000263B File Offset: 0x0000083B
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00002643 File Offset: 0x00000843
		internal List<BarricadeDrop> Beds { get; private set; } = new List<BarricadeDrop>();

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000264C File Offset: 0x0000084C
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00002654 File Offset: 0x00000854
		internal List<BarricadeDrop> Claims { get; private set; } = new List<BarricadeDrop>();

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000265D File Offset: 0x0000085D
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00002665 File Offset: 0x00000865
		internal List<BarricadeDrop> Furniture { get; private set; } = new List<BarricadeDrop>();

		// Token: 0x060000B9 RID: 185 RVA: 0x0000266E File Offset: 0x0000086E
		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00009FD4 File Offset: 0x000081D4
		private void Update()
		{
			if (!(Player.LocalPlayer == null))
			{
				this.Players.Clear();
				if (Provider.isConnected && Provider.clients != null)
				{
					foreach (SteamPlayer steamPlayer in Provider.clients)
					{
						if (!(((steamPlayer == null) ? null : steamPlayer.player) == null) && !(steamPlayer.player == Player.LocalPlayer) && !steamPlayer.player.life.isDead)
						{
							this.Players.Add(steamPlayer.player);
						}
					}
				}
				if (!this.Players.Contains(Player.LocalPlayer) && !Player.LocalPlayer.life.isDead)
				{
					this.Players.Add(Player.LocalPlayer);
				}
				this.Vehicles.Clear();
				foreach (InteractableVehicle interactableVehicle in VehicleManager.vehicles)
				{
					if (interactableVehicle != null)
					{
						this.Vehicles.Add(interactableVehicle);
					}
				}
				this.Zombies.Clear();
				foreach (ZombieRegion zombieRegion in ZombieManager.regions)
				{
					if (zombieRegion != null)
					{
						foreach (Zombie zombie in zombieRegion.zombies)
						{
							if (zombie != null && !zombie.isDead)
							{
								this.Zombies.Add(zombie);
							}
						}
					}
				}
				if (Time.time - this.lastItemScanTime >= 2f)
				{
					this.lastItemScanTime = Time.time;
					this.Items.Clear();
					foreach (InteractableItem interactableItem in UnityEngine.Object.FindObjectsOfType<InteractableItem>())
					{
						if (interactableItem != null && interactableItem.asset != null && interactableItem.gameObject.activeInHierarchy)
						{
							this.Items.Add(interactableItem);
						}
					}
					this.Beds.Clear();
					this.Claims.Clear();
					this.Furniture.Clear();
					BarricadeRegion[,] regions2 = BarricadeManager.regions;
					int i = regions2.GetUpperBound(0);
					int upperBound = regions2.GetUpperBound(1);
					for (int j = regions2.GetLowerBound(0); j <= i; j++)
					{
						for (int k = regions2.GetLowerBound(1); k <= upperBound; k++)
						{
							BarricadeRegion barricadeRegion = regions2[j, k];
							if (barricadeRegion != null)
							{
								foreach (BarricadeDrop barricadeDrop in barricadeRegion.drops)
								{
									if (!(((barricadeDrop == null) ? null : barricadeDrop.interactable) == null))
									{
										if (!(barricadeDrop.interactable is InteractableBed))
										{
											if (!(barricadeDrop.interactable is InteractableClaim))
											{
												if (barricadeDrop.interactable is InteractableStorage || barricadeDrop.interactable is InteractableDoor || barricadeDrop.interactable is InteractableTrap || barricadeDrop.interactable is InteractableGenerator)
												{
													this.Furniture.Add(barricadeDrop);
												}
											}
											else
											{
												this.Claims.Add(barricadeDrop);
											}
										}
										else
										{
											this.Beds.Add(barricadeDrop);
										}
									}
								}
							}
						}
					}
				}
				return;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000267B File Offset: 0x0000087B
		internal float DistanceToLocal(Vector3 pos)
		{
			if (!(Player.LocalPlayer == null))
			{
				return Vector3.Distance(Player.LocalPlayer.transform.position, pos);
			}
			return float.MaxValue;
		}

		// Token: 0x04000144 RID: 324
		private float lastItemScanTime = -10f;

		// Token: 0x04000145 RID: 325
		private const float ITEM_SCAN_INTERVAL = 2f;
	}
}

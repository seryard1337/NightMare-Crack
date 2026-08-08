using System;
using System.Collections.Generic;
using System.Reflection;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x0200001D RID: 29
	public class WeaponMods : MonoBehaviour
	{
		// Token: 0x0600009F RID: 159 RVA: 0x0000829C File Offset: 0x0000649C
		private void FixedUpdate()
		{
			if (!(Player.LocalPlayer == null))
			{
				PlayerEquipment equipment = Player.LocalPlayer.equipment;
				if (!(equipment == null) && equipment.asset != null)
				{
					ItemGunAsset itemGunAsset = equipment.asset as ItemGunAsset;
					if (itemGunAsset != null)
					{
						if (WeaponMods.spreadField == null)
						{
							WeaponMods.spreadField = typeof(ItemGunAsset).GetField("<baseSpreadAngleRadians>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
						}
						if (WeaponMods.gravityField == null)
						{
							WeaponMods.gravityField = typeof(ItemGunAsset).GetField("<bulletGravityMultiplier>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
						}
						if (!WeaponMods.pristineData.ContainsKey(itemGunAsset.id))
						{
							this.SavePristine(itemGunAsset);
						}
						this.ApplyOrRestore(itemGunAsset);
						return;
					}
				}
				return;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000835C File Offset: 0x0000655C
		private void SavePristine(ItemGunAsset gun)
		{
			WeaponMods.WeaponOriginalData weaponOriginalData = default(WeaponMods.WeaponOriginalData);
			weaponOriginalData.recoilMax_x = gun.recoilMax_x;
			weaponOriginalData.recoilMax_y = gun.recoilMax_y;
			weaponOriginalData.recoilMin_x = gun.recoilMin_x;
			weaponOriginalData.recoilMin_y = gun.recoilMin_y;
			weaponOriginalData.spreadAim = gun.spreadAim;
			weaponOriginalData.spreadSprint = gun.spreadSprint;
			weaponOriginalData.spreadCrouch = gun.spreadCrouch;
			weaponOriginalData.spreadProne = gun.spreadProne;
			weaponOriginalData.shakeMax_x = gun.shakeMax_x;
			weaponOriginalData.shakeMax_y = gun.shakeMax_y;
			weaponOriginalData.shakeMax_z = gun.shakeMax_z;
			weaponOriginalData.shakeMin_x = gun.shakeMin_x;
			weaponOriginalData.shakeMin_y = gun.shakeMin_y;
			weaponOriginalData.shakeMin_z = gun.shakeMin_z;
			weaponOriginalData.bulletGravityMultiplier = gun.bulletGravityMultiplier;
			PlayerAnimator animator = Player.LocalPlayer.animator;
			weaponOriginalData.scopeSway = ((animator == null) ? Vector3.zero : animator.scopeSway);
			PlayerAnimator animator2 = Player.LocalPlayer.animator;
			weaponOriginalData.viewmodelSwayMultiplier = ((animator2 == null) ? 1f : animator2.viewmodelSwayMultiplier);
			weaponOriginalData.baseSpreadAngleRadians = ((!(WeaponMods.spreadField != null)) ? 0f : ((float)WeaponMods.spreadField.GetValue(gun)));
			WeaponMods.WeaponOriginalData weaponOriginalData2 = weaponOriginalData;
			WeaponMods.pristineData[gun.id] = weaponOriginalData2;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000084B4 File Offset: 0x000066B4
		private void ApplyOrRestore(ItemGunAsset gun)
		{
			WeaponMods.WeaponOriginalData weaponOriginalData = WeaponMods.pristineData[gun.id];
			float num = ((!this.noRecoil) ? 1f : (1f - this.recoilReduction / 100f));
			gun.recoilMax_x = weaponOriginalData.recoilMax_x * num;
			gun.recoilMax_y = weaponOriginalData.recoilMax_y * num;
			gun.recoilMin_x = weaponOriginalData.recoilMin_x * num;
			gun.recoilMin_y = weaponOriginalData.recoilMin_y * num;
			float num2 = ((!this.noSpread) ? 1f : (1f - this.spreadReduction / 100f));
			FieldInfo fieldInfo = WeaponMods.spreadField;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(gun, weaponOriginalData.baseSpreadAngleRadians * num2);
			}
			gun.spreadAim = weaponOriginalData.spreadAim * num2;
			gun.spreadSprint = weaponOriginalData.spreadSprint * num2;
			gun.spreadCrouch = weaponOriginalData.spreadCrouch * num2;
			gun.spreadProne = weaponOriginalData.spreadProne * num2;
			float num3 = ((!this.noSway) ? 1f : (1f - this.swayReduction / 100f));
			if (Player.LocalPlayer.animator != null)
			{
				Player.LocalPlayer.animator.viewmodelSwayMultiplier = weaponOriginalData.viewmodelSwayMultiplier * num3;
				Player.LocalPlayer.animator.scopeSway = weaponOriginalData.scopeSway * num3;
			}
			float num4 = ((!this.noShake) ? 1f : (1f - this.shakeReduction / 100f));
			gun.shakeMax_x = weaponOriginalData.shakeMax_x * num4;
			gun.shakeMax_y = weaponOriginalData.shakeMax_y * num4;
			gun.shakeMax_z = weaponOriginalData.shakeMax_z * num4;
			gun.shakeMin_x = weaponOriginalData.shakeMin_x * num4;
			gun.shakeMin_y = weaponOriginalData.shakeMin_y * num4;
			gun.shakeMin_z = weaponOriginalData.shakeMin_z * num4;
			float num5 = ((!this.noBulletGravity) ? 1f : (1f - this.dropReduction / 100f));
			FieldInfo fieldInfo2 = WeaponMods.gravityField;
			if (fieldInfo2 != null)
			{
				fieldInfo2.SetValue(gun, weaponOriginalData.bulletGravityMultiplier * num5);
				return;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000025D7 File Offset: 0x000007D7
		private void OnDisable()
		{
			this.RestoreAll();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000086C8 File Offset: 0x000068C8
		public void RestoreAll()
		{
			if (WeaponMods.pristineData != null && WeaponMods.pristineData.Count != 0)
			{
				this.noBulletGravity = false;
				this.noShake = false;
				this.noSway = false;
				this.noSpread = false;
				this.noRecoil = false;
				foreach (KeyValuePair<ushort, WeaponMods.WeaponOriginalData> keyValuePair in WeaponMods.pristineData)
				{
					ushort key = keyValuePair.Key;
					ItemGunAsset itemGunAsset = Assets.find((EAssetType)1, key) as ItemGunAsset;
					if (itemGunAsset != null)
					{
						this.ApplyOrRestore(itemGunAsset);
					}
				}
				if (Player.LocalPlayer != null && Player.LocalPlayer.animator != null)
				{
					Player.LocalPlayer.animator.viewmodelSwayMultiplier = 1f;
					Player.LocalPlayer.animator.scopeSway = Vector3.zero;
				}
				WeaponMods.pristineData.Clear();
				return;
			}
		}

		// Token: 0x040000B2 RID: 178
		public bool noRecoil = true;

		// Token: 0x040000B3 RID: 179
		public bool noSpread = true;

		// Token: 0x040000B4 RID: 180
		public bool noSway = true;

		// Token: 0x040000B5 RID: 181
		public bool noShake = true;

		// Token: 0x040000B6 RID: 182
		public bool noBulletGravity = true;

		// Token: 0x040000B7 RID: 183
		public float recoilReduction = 100f;

		// Token: 0x040000B8 RID: 184
		public float spreadReduction = 100f;

		// Token: 0x040000B9 RID: 185
		public float swayReduction = 100f;

		// Token: 0x040000BA RID: 186
		public float shakeReduction = 100f;

		// Token: 0x040000BB RID: 187
		public float dropReduction = 100f;

		// Token: 0x040000BC RID: 188
		private static readonly Dictionary<ushort, WeaponMods.WeaponOriginalData> pristineData = new Dictionary<ushort, WeaponMods.WeaponOriginalData>();

		// Token: 0x040000BD RID: 189
		private static FieldInfo spreadField;

		// Token: 0x040000BE RID: 190
		private static FieldInfo gravityField;

		// Token: 0x0200001E RID: 30
		private struct WeaponOriginalData
		{
			// Token: 0x040000BF RID: 191
			public float recoilMax_x;

			// Token: 0x040000C0 RID: 192
			public float recoilMax_y;

			// Token: 0x040000C1 RID: 193
			public float recoilMin_x;

			// Token: 0x040000C2 RID: 194
			public float recoilMin_y;

			// Token: 0x040000C3 RID: 195
			public float baseSpreadAngleRadians;

			// Token: 0x040000C4 RID: 196
			public float spreadAim;

			// Token: 0x040000C5 RID: 197
			public float spreadSprint;

			// Token: 0x040000C6 RID: 198
			public float spreadCrouch;

			// Token: 0x040000C7 RID: 199
			public float spreadProne;

			// Token: 0x040000C8 RID: 200
			public float shakeMax_x;

			// Token: 0x040000C9 RID: 201
			public float shakeMax_y;

			// Token: 0x040000CA RID: 202
			public float shakeMax_z;

			// Token: 0x040000CB RID: 203
			public float shakeMin_x;

			// Token: 0x040000CC RID: 204
			public float shakeMin_y;

			// Token: 0x040000CD RID: 205
			public float shakeMin_z;

			// Token: 0x040000CE RID: 206
			public float bulletGravityMultiplier;

			// Token: 0x040000CF RID: 207
			public Vector3 scopeSway;

			// Token: 0x040000D0 RID: 208
			public float viewmodelSwayMultiplier;
		}
	}
}

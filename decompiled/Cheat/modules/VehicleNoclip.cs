using System;
using System.Reflection;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x0200001B RID: 27
	public class VehicleNoclip : MonoBehaviour
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00007838 File Offset: 0x00005A38
		private void Update()
		{
			if (!this.active)
			{
				this.RestoreLastVehicle();
				return;
			}
			if (Player.LocalPlayer == null || !Provider.isConnected || Provider.isLoading)
			{
				this.RestoreLastVehicle();
				return;
			}
			PlayerMovement movement = Player.LocalPlayer.movement;
			if (movement == null)
			{
				this.RestoreLastVehicle();
				return;
			}
			this.currentVehicle = movement.getVehicle();
			if (this.currentVehicle != null && !this.wasInVehicle)
			{
				this.lastVehicle = this.currentVehicle;
				this.savedPosition = this.currentVehicle.transform.position;
				this.wasInVehicle = true;
				this.cachedCollider = this.currentVehicle.GetComponent<Collider>();
				this.cachedRb = this.currentVehicle.GetComponent<Rigidbody>();
				VehicleAsset asset = this.currentVehicle.asset;
				if (asset != null)
				{
					this.cachedSpeedField = asset.GetType().GetField("TargetForwardVelocity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? asset.GetType().GetField("speedMax");
					if (this.cachedSpeedField != null)
					{
						this.cachedBaseSpeed = (float)this.cachedSpeedField.GetValue(asset);
					}
				}
				if (this.cachedBaseSpeed < 10f)
				{
					this.cachedBaseSpeed = 18f;
				}
				this.ToggleVehicleMouseLock(true);
			}
			if (this.currentVehicle == null && this.wasInVehicle)
			{
				this.wasInVehicle = false;
				this.ToggleVehicleMouseLock(false);
				this.RestoreLastVehicle();
				return;
			}
			if (this.currentVehicle == null)
			{
				return;
			}
			if (this.cachedCollider != null)
			{
				this.cachedCollider.enabled = false;
			}
			if (this.cachedRb != null)
			{
				this.cachedRb.isKinematic = true;
				this.cachedRb.velocity = Vector3.zero;
				this.cachedRb.angularVelocity = Vector3.zero;
			}
			Vector3 vector = Vector3.zero;
			if (Input.GetKey((KeyCode)119))
			{
				vector += this.currentVehicle.transform.forward;
			}
			if (Input.GetKey((KeyCode)115))
			{
				vector -= this.currentVehicle.transform.forward;
			}
			if (Input.GetKey((KeyCode)97))
			{
				vector -= this.currentVehicle.transform.right;
			}
			if (Input.GetKey((KeyCode)100))
			{
				vector += this.currentVehicle.transform.right;
			}
			if (Input.GetKey((KeyCode)32))
			{
				vector += Vector3.up;
			}
			if (Input.GetKey((KeyCode)306))
			{
				vector -= Vector3.up;
			}
			if (this.mouseControl && this.currentVehicle.asset.hasLockMouse)
			{
				float mouseAimSensitivity = ControlsSettings.mouseAimSensitivity;
				this.currentVehicle.transform.Rotate(-Input.GetAxis("mouse_y") * mouseAimSensitivity, 0f, 0f);
				this.currentVehicle.transform.Rotate(0f, Input.GetAxis("mouse_x") * mouseAimSensitivity, 0f);
			}
			if (this.useArrowKeys)
			{
				float num = this.arrowRotationSpeed * Time.deltaTime;
				if (Input.GetKey((KeyCode)273))
				{
					this.currentVehicle.transform.Rotate(-num, 0f, 0f);
				}
				if (Input.GetKey((KeyCode)274))
				{
					this.currentVehicle.transform.Rotate(num, 0f, 0f);
				}
				if (Input.GetKey((KeyCode)276))
				{
					this.currentVehicle.transform.Rotate(0f, -num, 0f);
				}
				if (Input.GetKey((KeyCode)275))
				{
					this.currentVehicle.transform.Rotate(0f, num, 0f);
				}
			}
			if (vector.sqrMagnitude <= 0.01f)
			{
				if (this.savedPosition != Vector3.zero)
				{
					this.currentVehicle.transform.position = this.savedPosition;
				}
			}
			else
			{
				vector.Normalize();
				float num2 = this.cachedBaseSpeed * this.speedMultiplier;
				this.currentVehicle.transform.position += vector * num2 * Time.deltaTime;
				this.savedPosition = this.currentVehicle.transform.position;
			}
			if (!this.nullRoll)
			{
				if (this.stabilizeRoll)
				{
					Vector3 eulerAngles = this.currentVehicle.transform.eulerAngles;
					eulerAngles.z = Mathf.LerpAngle(eulerAngles.z, 0f, Time.deltaTime * 12f);
					this.currentVehicle.transform.eulerAngles = eulerAngles;
				}
				return;
			}
			Vector3 eulerAngles2 = this.currentVehicle.transform.eulerAngles;
			eulerAngles2.z = 0f;
			this.currentVehicle.transform.eulerAngles = eulerAngles2;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007D18 File Offset: 0x00005F18
		private void ToggleVehicleMouseLock(bool enable)
		{
			InteractableVehicle interactableVehicle = this.currentVehicle;
			if (((interactableVehicle == null) ? null : interactableVehicle.asset) != null)
			{
				FieldInfo field = typeof(VehicleAsset).GetField("_hasLockMouse", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(this.currentVehicle.asset, enable);
				}
				return;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00007D74 File Offset: 0x00005F74
		private void RestoreLastVehicle()
		{
			if (!(this.lastVehicle == null))
			{
				if (this.cachedCollider != null)
				{
					this.cachedCollider.enabled = true;
				}
				if (this.cachedRb != null)
				{
					this.cachedRb.isKinematic = false;
				}
				this.ToggleVehicleMouseLock(false);
				this.lastVehicle = null;
				this.currentVehicle = null;
				this.cachedCollider = null;
				this.cachedRb = null;
				this.wasInVehicle = false;
				return;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002571 File Offset: 0x00000771
		private void OnDisable()
		{
			this.RestoreLastVehicle();
		}

		// Token: 0x0400009A RID: 154
		public bool active;

		// Token: 0x0400009B RID: 155
		public bool stabilizeRoll = true;

		// Token: 0x0400009C RID: 156
		public bool nullRoll = true;

		// Token: 0x0400009D RID: 157
		public bool mouseControl = true;

		// Token: 0x0400009E RID: 158
		public bool useArrowKeys = true;

		// Token: 0x0400009F RID: 159
		public float arrowRotationSpeed = 90f;

		// Token: 0x040000A0 RID: 160
		public float speedMultiplier = 1f;

		// Token: 0x040000A1 RID: 161
		private InteractableVehicle currentVehicle;

		// Token: 0x040000A2 RID: 162
		private InteractableVehicle lastVehicle;

		// Token: 0x040000A3 RID: 163
		private Vector3 savedPosition;

		// Token: 0x040000A4 RID: 164
		private bool wasInVehicle;

		// Token: 0x040000A5 RID: 165
		private Collider cachedCollider;

		// Token: 0x040000A6 RID: 166
		private Rigidbody cachedRb;

		// Token: 0x040000A7 RID: 167
		private FieldInfo cachedSpeedField;

		// Token: 0x040000A8 RID: 168
		private float cachedBaseSpeed = 18f;
	}
}

using System;
using System.Reflection;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x0200001C RID: 28
	public class Visuals : MonoBehaviour
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00007E40 File Offset: 0x00006040
		private void Awake()
		{
			Provider.onClientConnected = (Provider.ClientConnected)Delegate.Combine(Provider.onClientConnected, new Provider.ClientConnected(this.OnConnected));
			Provider.onClientDisconnected = (Provider.ClientDisconnected)Delegate.Combine(Provider.onClientDisconnected, new Provider.ClientDisconnected(this.OnDisconnected));
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00007E90 File Offset: 0x00006090
		private void OnDestroy()
		{
			Provider.onClientConnected = (Provider.ClientConnected)Delegate.Remove(Provider.onClientConnected, new Provider.ClientConnected(this.OnConnected));
			Provider.onClientDisconnected = (Provider.ClientDisconnected)Delegate.Remove(Provider.onClientDisconnected, new Provider.ClientDisconnected(this.OnDisconnected));
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002579 File Offset: 0x00000779
		private void OnConnected()
		{
			this.ResetStates();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00002579 File Offset: 0x00000779
		private void OnDisconnected()
		{
			this.ResetStates();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002581 File Offset: 0x00000781
		private void ResetStates()
		{
			this.statesCaptured = false;
			this.dayOffset = 0U;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00007EE0 File Offset: 0x000060E0
		private void Update()
		{
			if (Overrides.bBeingSpied && Overrides.bHideOnSpy)
			{
				return;
			}
			if (!Provider.isConnected || Provider.isLoading || !Level.isLoaded)
			{
				return;
			}
			if (Player.LocalPlayer == null || PlayerUI.window == null)
			{
				return;
			}
			ModeConfigData modeConfigData = Provider.modeConfigData;
			if (((modeConfigData == null) ? null : modeConfigData.Gameplay) == null)
			{
				return;
			}
			if (!this.statesCaptured)
			{
				this.CaptureServerStates();
			}
			this.HandleAlwaysDay();
			this.HandleSatellite();
			this.HandleCompass();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00007F5C File Offset: 0x0000615C
		private void CaptureServerStates()
		{
			this.serverSatellite = Provider.modeConfigData.Gameplay.Satellite;
			this.serverChart = Provider.modeConfigData.Gameplay.Chart;
			this.serverCompass = Provider.modeConfigData.Gameplay.Compass;
			this.statesCaptured = true;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00007FB0 File Offset: 0x000061B0
		public void HandleAlwaysDay()
		{
			if (!this.AlwaysDay)
			{
				if (this.dayOffset != 0U)
				{
					LightingManager.time = Provider.time - this.dayOffset;
					this.dayOffset = 0U;
				}
			}
			else
			{
				if (this.dayOffset == 0U && LightingManager.offset != 0U)
				{
					this.dayOffset = LightingManager.offset;
				}
				if (this.dayOffset != 0U)
				{
					LightingManager.time = this.CustomDayTime;
					return;
				}
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00008018 File Offset: 0x00006218
		public void HandleSatellite()
		{
			if (this.statesCaptured)
			{
				bool flag = this.AlwaysSatellite || this.serverSatellite;
				if (Provider.modeConfigData.Gameplay.Satellite != flag)
				{
					Provider.modeConfigData.Gameplay.Satellite = flag;
					if (!this.AlwaysSatellite)
					{
						Provider.modeConfigData.Gameplay.Chart = this.serverChart;
					}
					else
					{
						Provider.modeConfigData.Gameplay.Chart = false;
					}
					this.RefreshMapUI();
				}
				return;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000809C File Offset: 0x0000629C
		public void HandleCompass()
		{
			if (this.statesCaptured)
			{
				bool flag = this.AlwaysCompass || this.serverCompass;
				if (Provider.modeConfigData.Gameplay.Compass != flag)
				{
					Provider.modeConfigData.Gameplay.Compass = flag;
					PlayerLifeUI.updateCompass();
				}
				return;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000080EC File Offset: 0x000062EC
		private void RefreshMapUI()
		{
			try
			{
				Type typeFromHandle = typeof(PlayerDashboardInformationUI);
				BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				bool flag = Provider.modeConfigData.Gameplay.Chart || Level.info.type > 0;
				bool flag2 = Provider.modeConfigData.Gameplay.Satellite || Level.info.type > 0;
				MethodInfo method = typeFromHandle.GetMethod("searchForMapsInInventory", bindingFlags);
				if (method != null)
				{
					object[] array = new object[] { flag, flag2 };
					method.Invoke(null, array);
					flag = (bool)array[0];
					flag2 = (bool)array[1];
				}
				FieldInfo field = typeFromHandle.GetField("hasChart", bindingFlags);
				if (field != null)
				{
					field.SetValue(null, flag);
				}
				FieldInfo field2 = typeFromHandle.GetField("hasGPS", bindingFlags);
				if (field2 != null)
				{
					field2.SetValue(null, flag2);
				}
				if (PlayerDashboardInformationUI.active)
				{
					int num = ((flag2) ? 1 : 0);
					FieldInfo field3 = typeFromHandle.GetField("mapButtonState", bindingFlags);
					if (field3 != null)
					{
						object value = field3.GetValue(null);
						if (value != null)
						{
							PropertyInfo property = value.GetType().GetProperty("state");
							if (property != null)
							{
								property.SetValue(value, num);
							}
						}
					}
					MethodInfo method2 = typeFromHandle.GetMethod("synchronizeMapVisibility", bindingFlags);
					if (method2 != null)
					{
						method2.Invoke(null, new object[] { num });
					}
					MethodInfo method3 = typeFromHandle.GetMethod("updateDynamicMap", bindingFlags);
					if (method3 != null)
					{
						method3.Invoke(null, null);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002591 File Offset: 0x00000791
		public void ToggleAlwaysDay()
		{
			this.AlwaysDay = !this.AlwaysDay;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000025A2 File Offset: 0x000007A2
		public void ToggleSatellite()
		{
			this.AlwaysSatellite = !this.AlwaysSatellite;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000025B3 File Offset: 0x000007B3
		public void ToggleCompass()
		{
			this.AlwaysCompass = !this.AlwaysCompass;
		}

		// Token: 0x040000A9 RID: 169
		public bool AlwaysDay;

		// Token: 0x040000AA RID: 170
		public uint CustomDayTime = 1200U;

		// Token: 0x040000AB RID: 171
		public bool AlwaysSatellite;

		// Token: 0x040000AC RID: 172
		public bool AlwaysCompass;

		// Token: 0x040000AD RID: 173
		private uint dayOffset;

		// Token: 0x040000AE RID: 174
		private bool statesCaptured;

		// Token: 0x040000AF RID: 175
		private bool serverSatellite;

		// Token: 0x040000B0 RID: 176
		private bool serverChart;

		// Token: 0x040000B1 RID: 177
		private bool serverCompass;
	}
}

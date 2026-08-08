using System;
using SDG.Unturned;
using UnityEngine;

namespace Cheat.modules
{
	// Token: 0x0200000D RID: 13
	public class ChatSpam : MonoBehaviour
	{
		// Token: 0x06000046 RID: 70 RVA: 0x000042AC File Offset: 0x000024AC
		private void Update()
		{
			if (this.Enabled && Provider.isConnected && !Provider.isLoading)
			{
				if (Time.time - this.lastSpamTime >= this.Interval)
				{
					ChatManager.sendChat(0, this.SpamText);
					this.lastSpamTime = Time.time;
				}
				return;
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000232E File Offset: 0x0000052E
		public void Toggle()
		{
			this.Enabled = !this.Enabled;
		}

		// Token: 0x04000042 RID: 66
		public bool Enabled;

		// Token: 0x04000043 RID: 67
		public string SpamText = "NIGHTMARE BEST CHEAT!";

		// Token: 0x04000044 RID: 68
		public float Interval = 0.5f;

		// Token: 0x04000045 RID: 69
		private float lastSpamTime;
	}
}

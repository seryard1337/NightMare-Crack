using System;
using UnityEngine;
using Cheat.core;

namespace SystemMetrics
{
	public class LoadScreen : MonoBehaviour
	{
		private Texture2D texture2D_0;
		private float float_0;

		public void Start()
		{
			this.texture2D_0 = new Texture2D(1, 1);
			this.texture2D_0.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.99f));
			this.texture2D_0.Apply();
			this.float_0 = 0f;
		}

		public void Update()
		{
			if (this.float_0 < 3f)
			{
				this.float_0 += Time.deltaTime;
				if (this.float_0 >= 3f)
				{
					if (Main.Instance != null && Main.Instance.menu != null)
					{
						Main.Instance.menu.showMenu = true;
					}
					base.enabled = false;
				}
			}
		}

		public void OnGUI()
		{
			if (this.float_0 < 3f)
			{
				float num = this.float_0 / 3f;
				float num2 = Mathf.Clamp01(1f - Mathf.Pow(num, 2f));
				GUI.depth = -999;
				Color color = GUI.color;
				GUI.color = new Color(1f, 1f, 1f, num2);
				GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.texture2D_0);
				int fontSize = (int)((float)Screen.width * 0.08f);
				GUIStyle guistyle = new GUIStyle("label")
				{
					fontSize = fontSize,
					alignment = TextAnchor.MiddleCenter,
					richText = true
				};
				float num3 = Mathf.Clamp01(1f - num * 1.5f);
				float num4 = (float)Screen.width * 0.008f * num3;
				float num5 = UnityEngine.Random.Range(-num4, num4);
				float num6 = UnityEngine.Random.Range(-num4, num4);
				float num7 = UnityEngine.Random.Range(-num4, num4);
				float num8 = UnityEngine.Random.Range(-num4, num4);
				float num9 = UnityEngine.Random.Range(-num4, num4);
				float num10 = UnityEngine.Random.Range(-num4, num4);
				if (num3 > 0.1f && UnityEngine.Random.Range(0, 100) > 85)
				{
					float num11 = (float)UnityEngine.Random.Range(0, Screen.height);
					float num12 = UnityEngine.Random.Range(1f, 5f);
					GUI.color = new Color(1f, 1f, 1f, num2 * 0.3f);
					GUI.DrawTexture(new Rect(0f, num11, (float)Screen.width, num12), Texture2D.whiteTexture);
				}
				string str = "Cracked by SLM Solution";
				if (num3 > 0f)
				{
					GUI.color = new Color(1f, 0f, 0f, num2 * 0.7f);
					GUI.Label(new Rect(num5, num6, (float)Screen.width, (float)Screen.height), "<b>" + str + "</b>", guistyle);
				}
				if (num3 > 0f)
				{
					GUI.color = new Color(0f, 0.5f, 1f, num2 * 0.7f);
					GUI.Label(new Rect(num7, num8, (float)Screen.width, (float)Screen.height), "<b>" + str + "</b>", guistyle);
				}
				if (num3 > 0.3f)
				{
					GUI.color = new Color(0f, 1f, 0f, num2 * 0.5f);
					GUI.Label(new Rect(num9, num10, (float)Screen.width, (float)Screen.height), "<b>" + str + "</b>", guistyle);
				}
				GUI.color = new Color(1f, 1f, 1f, num2);
				float num13 = (UnityEngine.Random.value <= 0.8f) ? 0f : (UnityEngine.Random.Range(-2f, 2f) * num3);
				float num14 = (UnityEngine.Random.value > 0.8f) ? (UnityEngine.Random.Range(-2f, 2f) * num3) : 0f;
				GUI.Label(new Rect(num13, num14, (float)Screen.width, (float)Screen.height), "<b>" + str + "</b>", guistyle);
				GUI.color = color;
			}
		}
	}
}

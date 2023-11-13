using System;
using UnityEngine;

namespace VortexStudios.PostProcessing
{
	[Serializable]
	public class TelevisionProfile : PostProcessingProfile
	{
		[SerializeField]
		public int lineCount = 224;

		[SerializeField]
		public Vector2 sync = new Vector2(0f, 0f);

		private Vector2 _sync = new Vector2(0f, 0f);

		[SerializeField]
		public float brightness;

		[SerializeField]
		public float contrast;

		[SerializeField]
		public float saturation = 0.5f;

		[SerializeField]
		public float sharpness = -1f;

		public override void OnEnable()
		{
		}

		public override void OnFixedUpdate()
		{
			_sync += sync * Time.fixedUnscaledDeltaTime;
		}

		public override RenderTexture OnRenderImage(RenderTexture source)
		{
			base.OnRenderImage(source);
			float num = (float)lineCount / (float)source.height;
			if (base.material != null && (brightness != 0f || contrast != 0f || saturation != 0f || sharpness != 0f))
			{
				base.material.SetFloat("_ScreenWidth", 1f / ((float)source.width * num));
				base.material.SetFloat("_ScreenHeight", 1f / ((float)source.height * num));
				base.material.SetVector("_Sync", sync);
				base.material.SetFloat("_Brightness", brightness);
				base.material.SetFloat("_Contrast", 1.016f * (contrast + 1f) / (1.016f * (1.016f - contrast)));
				base.material.SetFloat("_Saturation", saturation * 2f);
				base.material.SetFloat("_Sharpness", sharpness);
				Graphics.Blit(PostProcessingProfile.SOURCEBUFFER, PostProcessingProfile.DESTBUFFER, base.material);
				PostProcessingProfile.SWAPBUFFER();
			}
			return null;
		}
	}
}

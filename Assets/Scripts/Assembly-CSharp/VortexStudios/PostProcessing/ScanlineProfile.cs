using System;
using UnityEngine;

namespace VortexStudios.PostProcessing
{
	[Serializable]
	public class ScanlineProfile : PostProcessingProfile
	{
		[SerializeField]
		public Texture pattern;

		[SerializeField]
		public int lineCount = 224;

		[SerializeField]
		public float magnetude = 0.75f;

		public override void OnEnable()
		{
			if (pattern == null)
			{
				pattern = base.material.GetTexture("_PatternTex");
			}
		}

		public override RenderTexture OnRenderImage(RenderTexture source)
		{
			base.OnRenderImage(source);
			if (base.material != null && pattern != null && magnetude != 0f)
			{
				base.material.SetInt("_ScreenWidth", (int)((float)source.width * ((float)lineCount / (float)source.height)));
				base.material.SetInt("_ScreenHeight", lineCount);
				base.material.SetTexture("_PatternTex", pattern);
				base.material.SetFloat("_Magnitude", 1f - magnetude);
				Graphics.Blit(PostProcessingProfile.SOURCEBUFFER, PostProcessingProfile.DESTBUFFER, base.material);
				PostProcessingProfile.SWAPBUFFER();
			}
			return null;
		}
	}
}

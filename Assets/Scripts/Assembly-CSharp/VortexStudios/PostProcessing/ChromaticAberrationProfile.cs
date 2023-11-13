using System;
using UnityEngine;

namespace VortexStudios.PostProcessing
{
	[Serializable]
	public class ChromaticAberrationProfile : PostProcessingProfile
	{
		[SerializeField]
		public Texture pattern;

		[SerializeField]
		public float magnetude = 0.2f;

		public override void OnEnable()
		{
			if (pattern == null)
			{
				pattern = base.material.GetTexture("_MaskTex");
			}
		}

		public override RenderTexture OnRenderImage(RenderTexture source)
		{
			base.OnRenderImage(source);
			if (base.material != null && pattern != null && magnetude > 0f)
			{
				base.material.SetTexture("_MaskTex", pattern);
				base.material.SetFloat("_Magnitude", magnetude * magnetude);
				Graphics.Blit(PostProcessingProfile.SOURCEBUFFER, PostProcessingProfile.DESTBUFFER, base.material, 0);
				PostProcessingProfile.SWAPBUFFER();
			}
			return null;
		}
	}
}

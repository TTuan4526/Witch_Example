using System;
using UnityEngine;

namespace VortexStudios.PostProcessing
{
	[Serializable]
	public class TubeProfile : PostProcessingProfile
	{
		[SerializeField]
		public Texture maskPattern;

		[SerializeField]
		public Texture reflexPattern;

		[SerializeField]
		public float distortionMagnitude = 0.15f;

		[SerializeField]
		public float reflexMagnitude = 0.15f;

		public override void OnEnable()
		{
			if (maskPattern == null)
			{
				maskPattern = base.material.GetTexture("_MaskTex");
			}
		}

		public override RenderTexture OnRenderImage(RenderTexture source)
		{
			base.OnRenderImage(source);
			if (base.material != null && (maskPattern != null || distortionMagnitude != 0f || (reflexPattern != null && distortionMagnitude > 0f)))
			{
				base.material.SetTexture("_MaskTex", maskPattern);
				base.material.SetTexture("_ReflexTex", reflexPattern);
				base.material.SetFloat("_Distortion", distortionMagnitude);
				base.material.SetFloat("_Reflex", reflexMagnitude);
				Graphics.Blit(PostProcessingProfile.SOURCEBUFFER, PostProcessingProfile.DESTBUFFER, base.material, 0);
				PostProcessingProfile.SWAPBUFFER();
			}
			return null;
		}
	}
}

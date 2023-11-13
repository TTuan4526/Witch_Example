using System;
using UnityEngine;

namespace VortexStudios.PostProcessing
{
	[Serializable]
	public class NoiseProfile : PostProcessingProfile
	{
		[SerializeField]
		public Texture pattern;

		[SerializeField]
		public float magnetude = 0.25f;

		[SerializeField]
		public Vector2 scale = Vector2.one;

		private Vector2 _offset = Vector2.zero;

		public override void OnEnable()
		{
			if (pattern == null)
			{
				pattern = base.material.GetTexture("_PatternTex");
			}
		}

		public override void OnFixedUpdate()
		{
			_offset.x = _offset.y;
			_offset.y = UnityEngine.Random.Range(-1f, 1f);
		}

		public override RenderTexture OnRenderImage(RenderTexture source)
		{
			base.OnRenderImage(source);
			if (base.material != null && pattern != null && magnetude != 0f)
			{
				base.material.SetTexture("_PatternTex", pattern);
				base.material.SetFloat("_PatternOffsetX", _offset.x);
				base.material.SetFloat("_PatternOffsetY", _offset.y);
				base.material.SetFloat("_PatternScaleX", scale.x);
				base.material.SetFloat("_PatternScaleY", scale.y);
				base.material.SetFloat("_Magnitude", magnetude);
				Graphics.Blit(PostProcessingProfile.SOURCEBUFFER, PostProcessingProfile.DESTBUFFER, base.material);
				PostProcessingProfile.SWAPBUFFER();
			}
			return null;
		}
	}
}

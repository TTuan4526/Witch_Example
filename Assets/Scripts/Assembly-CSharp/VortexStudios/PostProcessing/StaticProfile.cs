using System;
using UnityEngine;

namespace VortexStudios.PostProcessing
{
	[Serializable]
	public class StaticProfile : PostProcessingProfile
	{
		[SerializeField]
		public Texture staticPattern;

		[SerializeField]
		public float staticMagnitude = 0.1f;

		[SerializeField]
		public float staticScale = 1f;

		[SerializeField]
		public float staticOffset;

		[SerializeField]
		public Texture dirtPattern;

		[SerializeField]
		public float dirtMagnitude = 0.35f;

		private Vector2 _offset = Vector2.zero;

		public override void OnEnable()
		{
			if (staticPattern == null)
			{
				staticPattern = base.material.GetTexture("_StaticTex");
			}
			if (dirtPattern == null)
			{
				dirtPattern = base.material.GetTexture("_DirtTex");
			}
		}

		public override void OnFixedUpdate()
		{
			_offset.x = UnityEngine.Random.Range(0f, 1f);
			_offset.y = 0f;
		}

		public override RenderTexture OnRenderImage(RenderTexture source)
		{
			base.OnRenderImage(source);
			if (base.material != null && ((staticPattern != null && staticMagnitude != 0f) || (dirtPattern != null && (double)dirtMagnitude != 0.0)))
			{
				base.material.SetTexture("_StaticTex", staticPattern);
				base.material.SetFloat("_PatternOffsetX", _offset.x);
				base.material.SetFloat("_PatternOffsetY", staticOffset);
				base.material.SetFloat("_PatternScaleY", staticScale);
				base.material.SetFloat("_StaticMagnitude", staticMagnitude);
				base.material.SetTexture("_DirtTex", dirtPattern);
				base.material.SetFloat("_DirtMagnitude", dirtMagnitude);
				Graphics.Blit(PostProcessingProfile.SOURCEBUFFER, PostProcessingProfile.DESTBUFFER, base.material);
				PostProcessingProfile.SWAPBUFFER();
			}
			return null;
		}
	}
}

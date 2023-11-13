using System;
using UnityEngine;

namespace VortexStudios.PostProcessing
{
	[Serializable]
	public class CompositeProfile : PostProcessingProfile
	{
		[SerializeField]
		private Material _materialBleeding;

		private bool _polarity;

		private float[] _polarityPositive = new float[3] { -1f, 2f, -1f };

		private float[] _polarityNegative = new float[3] { 1f, -2f, 1f };

		private Texture2D _bleedingTexture;

		[SerializeField]
		public int lineCount = 224;

		[SerializeField]
		public float distortion = 0.5f;

		[SerializeField]
		public float artifact = 0.2f;

		[SerializeField]
		public float fringing = 0.7f;

		[SerializeField]
		public float bleeding = 1f;

		private float t;

		public Material materialBleeding
		{
			get
			{
				if (_materialBleeding == null)
				{
					Shader shader = Shader.Find("Vortex Game Studios/Filters/OLD TV Filter/Bleeding");
					if (shader != null)
					{
						_materialBleeding = new Material(shader);
					}
				}
				return _materialBleeding;
			}
		}

		public override void OnEnable()
		{
		}

		public override void OnFixedUpdate()
		{
			t += Time.unscaledDeltaTime;
			if (t >= 0.25f)
			{
				t -= 0.25f;
				_polarity = !_polarity;
			}
		}

		public override RenderTexture OnRenderImage(RenderTexture source)
		{
			base.OnRenderImage(source);
			float num = (float)lineCount / (float)source.height;
			Vector2 vector = new Vector2(1f / ((float)source.width * num), 1f / ((float)source.height * num));
			if (base.material != null && (fringing > 0f || artifact > 0f))
			{
				base.material.SetFloat("_ScreenWidth", vector.x);
				base.material.SetFloat("_ScreenHeight", vector.y);
				base.material.SetFloat("_Distortion", distortion);
				base.material.SetFloat("_Fringing", fringing);
				base.material.SetFloat("_Artifact", artifact);
				base.material.SetFloatArray("_Kernel", (!_polarity) ? _polarityNegative : _polarityPositive);
				Graphics.Blit(PostProcessingProfile.SOURCEBUFFER, PostProcessingProfile.DESTBUFFER, base.material);
				PostProcessingProfile.SWAPBUFFER();
			}
			if (materialBleeding != null && bleeding > 0f)
			{
				materialBleeding.SetFloat("_ScreenWidth", vector.x);
				materialBleeding.SetFloat("_ScreenHeight", vector.y);
				materialBleeding.SetFloat("_Magnitude", bleeding);
				Graphics.Blit(PostProcessingProfile.SOURCEBUFFER, PostProcessingProfile.DESTBUFFER, materialBleeding);
				PostProcessingProfile.SWAPBUFFER();
			}
			return null;
		}
	}
}

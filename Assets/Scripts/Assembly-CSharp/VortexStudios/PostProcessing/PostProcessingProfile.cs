using System;
using UnityEngine;

namespace VortexStudios.PostProcessing
{
	[Serializable]
	public class PostProcessingProfile
	{
		protected RenderBuffer _BUFFER;

		private static bool _CURRENTBUFFER = false;

		private static RenderTexture[] _TEMPBUFFER = new RenderTexture[2];

		[SerializeField]
		private bool _foldout;

		[SerializeField]
		protected Material _material;

		[SerializeField]
		protected bool _enabled = true;

		public static RenderTexture SOURCEBUFFER
		{
			get
			{
				return _TEMPBUFFER[(!_CURRENTBUFFER) ? 1 : 0];
			}
			set
			{
				_TEMPBUFFER[(!_CURRENTBUFFER) ? 1 : 0] = value;
			}
		}

		public static RenderTexture DESTBUFFER
		{
			get
			{
				return _TEMPBUFFER[_CURRENTBUFFER ? 1 : 0];
			}
			set
			{
				_TEMPBUFFER[_CURRENTBUFFER ? 1 : 0] = value;
			}
		}

		public Material material
		{
			get
			{
				if (_material == null)
				{
					Shader shader = Shader.Find("Vortex Game Studios/Filters/OLD TV Filter/" + GetType().Name.Replace("Profile", string.Empty));
					if (shader != null)
					{
						_material = new Material(shader);
					}
				}
				if (_enabled && !_material.shader.isSupported)
				{
					enabled = false;
				}
				return _material;
			}
		}

		public bool enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
				if (value)
				{
					OnValidate();
				}
			}
		}

		public PostProcessingProfile()
		{
			_foldout = false;
		}

		public static void SWAPBUFFER()
		{
			_CURRENTBUFFER = !_CURRENTBUFFER;
		}

		public virtual void OnFixedUpdate()
		{
		}

		public virtual void OnEnable()
		{
		}

		public virtual void OnValidate()
		{
		}

		public virtual void OnReset()
		{
		}

		public virtual RenderTexture OnRenderImage(RenderTexture source)
		{
			return source;
		}
	}
}

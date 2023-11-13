using UnityEngine;

namespace VortexStudios.PostProcessing
{
	public class OLDTVPreset : ScriptableObject
	{
		[SerializeField]
		private NoiseProfile _noiseFilter = new NoiseProfile();

		[SerializeField]
		private CompositeProfile _compositeFilter = new CompositeProfile();

		[SerializeField]
		private StaticProfile _staticFilter = new StaticProfile();

		[SerializeField]
		private TelevisionProfile _televisionFilter = new TelevisionProfile();

		[SerializeField]
		private ChromaticAberrationProfile _chromaticAberrationFilter = new ChromaticAberrationProfile();

		[SerializeField]
		private ScanlineProfile _scanlineFilter = new ScanlineProfile();

		[SerializeField]
		private TubeProfile _tubeFilter = new TubeProfile();

		public NoiseProfile noiseFilter
		{
			get
			{
				return _noiseFilter;
			}
		}

		public CompositeProfile compositeFilter
		{
			get
			{
				return _compositeFilter;
			}
		}

		public StaticProfile staticFilter
		{
			get
			{
				return _staticFilter;
			}
		}

		public TelevisionProfile televisionFilter
		{
			get
			{
				return _televisionFilter;
			}
		}

		public ChromaticAberrationProfile chromaticAberrationFilter
		{
			get
			{
				return _chromaticAberrationFilter;
			}
		}

		public ScanlineProfile scanlineFilter
		{
			get
			{
				return _scanlineFilter;
			}
		}

		public TubeProfile tubeFilter
		{
			get
			{
				return _tubeFilter;
			}
		}

		private void OnEnable()
		{
			_noiseFilter.OnEnable();
			_compositeFilter.OnEnable();
			_staticFilter.OnEnable();
			_televisionFilter.OnEnable();
			_chromaticAberrationFilter.OnEnable();
			_scanlineFilter.OnEnable();
			_tubeFilter.OnEnable();
		}
	}
}

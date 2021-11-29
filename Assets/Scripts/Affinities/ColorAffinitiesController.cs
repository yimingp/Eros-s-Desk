using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unit;
using UnityEngine;

namespace Affinities
{
    public class ColorAffinitiesController : MonoBehaviour
    {
        public static ColorAffinitiesController Instance;
        
#pragma warning disable 414
        private bool _initialized = false;
#pragma warning restore 414

        [Title("Setting")]
        [Range(0,1)]public float highAffRatioMin;
        [Range(0,1)]public float highAffRatioMax;
        [Range(0,1)]public float lowAffRatioMin;
        [Range(0,1)]public float lowAffRatioMax;
        [Range(0,1)]public float highAffAllRatioMin;
        [Range(0,1)]public float highAffAllRatioMax;
        [Range(0,1)]public float lowAffAllRatioMin;
        [Range(0,1)]public float lowAffAllRatioMax;

        [Space]
        [Range(0, 1)] public float highGeneralAffinityMin;
        [Range(0, 1)] public float highGeneralAffinityMax;
        [Range(0, 1)] public float lowGeneralAffinityMin;
        [Range(0, 1)] public float lowGeneralAffinityMax;
        
        [Space]
        [Range(0,1)]public float highAffCollisionImpactRateMin;
        [Range(0,1)]public float highAffCollisionImpactRateMax;
        public int highAffCollisionImpactMagnitudeMin;
        public int highAffCollisionImpactMagnitudeMax;

        [Space] 
        [Range(0, 1)] public float exPushOthersRateMin;
        [Range(0, 1)] public float exPushOthersRateMax;
        public float exPushOthersMagnitudeMin;
        public float exPushOthersMagnitudeMax;
        [Range(0, 1)] public float inPushOthersRateMin;
        [Range(0, 1)] public float inPushOthersRateMax;
        public float inPushOthersMagnitudeMin;
        public float inPushOthersMagnitudeMax;
        
        [Space] 
        [Range(0, 1)] public float exPullOthersRateMin;
        [Range(0, 1)] public float exPullOthersRateMax;
        public float exPullOthersMagnitudeMin;
        public float exPullOthersMagnitudeMax;
        [Range(0, 1)] public float inPullOthersRateMin;
        [Range(0, 1)] public float inPullOthersRateMax;
        public float inPullOthersMagnitudeMin;
        public float inPullOthersMagnitudeMax;
        
        [Space]
        [Range(0,1)]public float lowAffCollisionImpactRateMin;
        [Range(0,1)]public float lowAffCollisionImpactRateMax;
        public int lowAffCollisionImpactMagnitudeMin;
        public int lowAffCollisionImpactMagnitudeMax;
        
        
        [Title("Reference")] public UnitColorController colorController;

        [Title("Data")] [ShowInInspector] [ShowIf("_initialized")]
        public Dictionary<Color, Affinity> Affinities;

        private Color[] _introverts;
        private Color[] _extroverts;
        private Color _neutral;

        private readonly string[] _classification = new string[]
        {
            "highAff_all",
            "highAff_introverts_lowAff_extroverts",
            "highAff_extroverts_lowAff_introverts",
            "lowAff_all"
        };

        public AffinityData GetAffinity(Color myColor, Color otherColor)
        {
            return Affinities[myColor].Data[otherColor];
        }

        private void Awake()
        {
            Instance = this;
            
            Affinities ??= new Dictionary<Color, Affinity>();
            _introverts ??= new Color[Mathf.FloorToInt(colorController.palette.Count / 2)];
            _extroverts ??= new Color[Mathf.FloorToInt(colorController.palette.Count / 2)];
        }

        public void InitializeAffinities()
        {
            _initialized = true;

            var length = colorController.palette.Count;

            for (var i = 0; i < length; ++i)
            {
                var color = colorController.palette[i];
                var affinity = new Affinity();
                foreach (var c in colorController.palette.Where(c => c != color))
                {
                    affinity.Data.Add(c, new AffinityData());
                }

                Affinities.Add(color, affinity);
            }

            FirstPass();
            SecondPass();
        }

        // separate colors to introverts and extroverts
        private void FirstPass()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            
            for (var i = 0; i < _introverts.Length; ++i)
            {
                var arr = colorController.palette.Where(color => !_introverts.Contains(color)).ToArray();
                _introverts[i] = arr[Random.Range(0, arr.Length)];
            }

            for (var i = 0; i < _extroverts.Length; ++i)
            {
                var arr = colorController.palette
                    .Where(color => !_introverts.Contains(color) && !_extroverts.Contains(color)).ToArray();
                _extroverts[i] = arr[Random.Range(0, arr.Length)];
            }

            if (colorController.palette.Count % 2 != 0)
                _neutral = colorController.palette.Where(color =>
                    !_introverts.Contains(color) && !_extroverts.Contains(color)).ToArray()[0];
        }

        // classify each extroverts
        private void SecondPass()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);

            var randomClassification = _classification.OrderBy(item => Random.value).ToArray();

            for (var i = 0; i < _extroverts.Length; ++i)
            {
                switch (randomClassification[i])
                {
                    case "highAff_all":
                        ThisColorHighAffsAll(_extroverts[i]);
                        ThisColorHighAffsAll(_introverts[i]);
                        break;
                    case "highAff_introverts_lowAff_extroverts":
                        ThisColorHighIntrovertsLowExtroverts(_extroverts[i]);
                        ThisColorHighIntrovertsLowExtroverts(_introverts[i]);
                        break;
                    case "highAff_extroverts_lowAff_introverts":
                        ThisColorHighExtrovertsLowIntroverts(_extroverts[i]);
                        ThisColorHighExtrovertsLowIntroverts(_introverts[i]);
                        break;
                    case "lowAff_all":
                        ThisColorLowAffsAll(_extroverts[i]);
                        ThisColorLowAffsAll(_introverts[i]);
                        break;
                }
            }

            if (_neutral == default) return;
            
            switch (randomClassification[0])
            {
                case "highAff_all":
                    ThisColorHighAffsAll(_neutral);
                    break;
                case "highAff_introverts_lowAff_extroverts":
                    ThisColorHighIntrovertsLowExtroverts(_neutral);
                    break;
                case "highAff_extroverts_lowAff_introverts":
                    ThisColorHighExtrovertsLowIntroverts(_neutral);
                    break;
                case "lowAff_all":
                    ThisColorLowAffsAll(_neutral);
                    break;
            }
        }

        #region MainSetters
        private void SetHighAffinitiesHelper(Color settingColor, IReadOnlyCollection<Color> colors)
        {
            foreach (var c in colors)
            {
                Affinities[settingColor].Data[c].generalAffinity =
                    Random.Range(highGeneralAffinityMin, highGeneralAffinityMax);
                
                Affinities[settingColor].Data[c].SetCollisionImpactRate(Random.Range(highAffCollisionImpactRateMin, highAffCollisionImpactRateMax));
                
                Affinities[settingColor].Data[c].collisionImpactMagnitude = 
                    Random.Range(highAffCollisionImpactMagnitudeMin, highAffCollisionImpactMagnitudeMax);
            }
        }
        private void SetLowAffinitiesHelper(Color settingColor, IReadOnlyCollection<Color> colors)
        {
            foreach (var c in colors)
            {
                Affinities[settingColor].Data[c].generalAffinity =
                    Random.Range(lowGeneralAffinityMin, lowGeneralAffinityMax);
                
                Affinities[settingColor].Data[c].SetCollisionImpactRate(Random.Range(lowAffCollisionImpactRateMin, lowAffCollisionImpactRateMax));
                
                Affinities[settingColor].Data[c].collisionImpactMagnitude = 
                    Random.Range(lowAffCollisionImpactMagnitudeMin, lowAffCollisionImpactMagnitudeMax);
            }
        }
        #endregion

        private void ThisColorHighAffsAll(Color color)
        {
            var colors = colorController.palette.Where(c => c != color).ToArray();
            var highAffs = Mathf.FloorToInt(colors.Length * Random.Range(highAffAllRatioMin, highAffAllRatioMax));
            var highAffColors = colors.OrderBy(i => Random.value).Take(highAffs).ToArray();
            var remainingColors = colors.Where(c => !highAffColors.Contains(c)).ToArray();

            SetHighAffinitiesHelper(color, highAffColors);
            SetLowAffinitiesHelper(color, remainingColors);
        }
        
        private void ThisColorLowAffsAll(Color color)
        {
            var colors = colorController.palette.Where(c => c != color).ToArray();
            var lowAffs = Mathf.FloorToInt(colors.Length * Random.Range(lowAffAllRatioMin, lowAffAllRatioMax));
            var lowAffColors = colors.OrderBy(i => Random.value).Take(lowAffs).ToArray();
            var remainingColors = colors.Where(c => !lowAffColors.Contains(c)).ToArray();

            SetLowAffinitiesHelper(color, lowAffColors);
            SetHighAffinitiesHelper(color, remainingColors);
        }

        private void ThisColorHighIntrovertsLowExtroverts(Color color)
        {
            var introvertsHighAffs = Mathf.FloorToInt(_introverts.Length * Random.Range(highAffRatioMin, highAffRatioMax));
            var extrovertsLowAffs = Mathf.FloorToInt(_extroverts.Length * Random.Range(lowAffRatioMin, lowAffRatioMax));
            var introvertsHighAffColors = _introverts.OrderBy(i => Random.value).Take(introvertsHighAffs).ToArray();
            var introvertsLowAffColors = _introverts.Where(c => !introvertsHighAffColors.Contains(c)).ToArray();
            var extrovertsHighAffColors = _extroverts.OrderBy(i => Random.value).Take(extrovertsLowAffs).ToArray();
            var extrovertsLowAffColors = _extroverts.Where(c => !extrovertsHighAffColors.Contains(c)).ToArray();

            var allHighAffColors = introvertsHighAffColors.Concat(extrovertsHighAffColors).Where(c => c != color);
            var allLowAffColors = extrovertsLowAffColors.Concat(introvertsLowAffColors).Where(c => c != color);
            
            if (_neutral != default && color != _neutral)
            {
                if (Random.value >= .5f)
                    allHighAffColors = allHighAffColors.Append(_neutral);
                else
                    allLowAffColors = allLowAffColors.Append(_neutral);
            }

            SetHighAffinitiesHelper(color, allHighAffColors.ToArray());
            SetLowAffinitiesHelper(color, allLowAffColors.ToArray());
        }
        
        private void ThisColorHighExtrovertsLowIntroverts(Color color)
        {
            var extrovertsHighAffs = Mathf.FloorToInt(_extroverts.Length * Random.Range(highAffRatioMin, highAffRatioMax));
            var introvertsLowAffs = Mathf.FloorToInt(_introverts.Length * Random.Range(lowAffRatioMin, lowAffRatioMax));
            var extrovertsHighAffColors = _extroverts.OrderBy(i => Random.value).Take(extrovertsHighAffs).ToArray();
            var extrovertsLowAffColors = _extroverts.Where(c => !extrovertsHighAffColors.Contains(c)).ToArray();
            var introvertsLowAffColors = _introverts.OrderBy(i => Random.value).Take(introvertsLowAffs).ToArray();
            var introvertsHighAffColors = _introverts.Where(c => !introvertsLowAffColors.Contains(c)).ToArray();
            
            var allHighAffColors = extrovertsHighAffColors.Concat(introvertsHighAffColors).Where(c => c != color);
            var allLowAffColors = introvertsLowAffColors.Concat(extrovertsLowAffColors).Where(c => c != color);
            
            if (_neutral != default && color != _neutral)
            {
                if (Random.value >= .5f)
                    allHighAffColors = allHighAffColors.Append(_neutral);
                else
                    allLowAffColors = allLowAffColors.Append(_neutral);
            }
            
            SetHighAffinitiesHelper(color, allHighAffColors.ToArray());
            SetLowAffinitiesHelper(color, allLowAffColors.ToArray());
        }
        
    }
}
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeAdjuster : MonoBehaviour
{
    [Title("Setting")] 
    public bool enableGammaShifts;
    public float gammaShiftSpeed;
    public Vector3 gammaPositiveSide;
    public Vector3 gammaNegativeSide;
    
    [Title("Reference")]
    public Volume volume;

    private float _gammaShiftFactor;
    private VolumeProfile _volumeProfile;
    private LiftGammaGain _liftGammaGain;

    private void Start()
    {
        _volumeProfile = volume.profile;

        if (enableGammaShifts)
        {
            _liftGammaGain = _volumeProfile.Add<LiftGammaGain>();
            _liftGammaGain.gamma.overrideState = true;
        }
    }

    private void Update()
    {
        if (enableGammaShifts)
        {
            _gammaShiftFactor = Mathf.Sin(Time.realtimeSinceStartup * gammaShiftSpeed);

            _liftGammaGain.gamma.value = _gammaShiftFactor > 0 ? Vector3.Lerp(Vector3.one, gammaPositiveSide, _gammaShiftFactor) : Vector3.Lerp(Vector3.one, gammaNegativeSide, -_gammaShiftFactor);
        }
    }
}

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    public class ProceduralAudioClipsManager : MonoBehaviour
    {
        [Title("Setting")]
        public List<AudioClip> backgroundClips;
        public List<AudioClip> impactClips;

        private void Awake()
        {
            backgroundClips ??= new List<AudioClip>();
            impactClips ??= new List<AudioClip>();
        }
    }
}

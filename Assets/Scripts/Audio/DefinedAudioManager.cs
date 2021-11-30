using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Audio
{
    public enum DefinedAudioType
    {
        Start,
        Transition,
        InClimax,
        Climax,
        OffClimax
    }

    public class DefinedAudioManager : MonoBehaviour
    {
        [Title("Data")]
        public bool isPlaying;
        
        [Title("Setting")]
        public float pauseTimeBtwMin;
        public float pauseTimeBtwMax;
        
        [Title("Reference")]
        public AudioSource player;
        public Animator musicAnim;
        public Text musicName;

        private bool _fadingOut;
        private DefinedAudioType _current;
        private static readonly int FadeIn = Animator.StringToHash("FadeIn");
        private static readonly int FadeOut = Animator.StringToHash("FadeOut");
        private static readonly int PausedFinish = Animator.StringToHash("PausedFinish");
        private static readonly int ClipSet = Animator.StringToHash("ClipSet");


        private List<AudioClip> _allClips;

        private void Awake()
        {
            _allClips = Resources.LoadAll<AudioClip>("Audio").ToList();
        }

        private void Start()
        {
            _current = DefinedAudioType.Start;
        }

        public void SetNewAudioClip()
        {
            var clip = GetRandomAudioClip(_current);
            _current = ((int) _current + 1) > 4 ? DefinedAudioType.Start : (DefinedAudioType) (int) _current + 1;
            player.clip = clip;
            player.Play();
            musicAnim.SetTrigger(ClipSet);
            _fadingOut = false;
            musicName.text = clip.name;
        }

        public void StartPausing()
        {
            StartCoroutine(PausingCoroutine());
        }

        private IEnumerator PausingCoroutine()
        {
            yield return new WaitForSecondsRealtime(Random.Range(pauseTimeBtwMin, pauseTimeBtwMax));
            musicAnim.SetTrigger(PausedFinish);
        }
        
        private AudioClip GetRandomAudioClip(DefinedAudioType type)
        {
            return _allClips[Random.Range(0, _allClips.Count)];
        }

        private void Update()
        {
            if (!isPlaying || !player.clip) return;

            if (_fadingOut || !(player.clip.length - player.time <= 15)) return;
            
            musicAnim.SetTrigger(FadeOut);
            _fadingOut = true;
        }
        
    }
}
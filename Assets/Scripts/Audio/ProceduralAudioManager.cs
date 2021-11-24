using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class ProceduralAudioManager : MonoBehaviour
    {
        private enum TempoType
        {
            Background,
            Impact,
        }

        [Title("Setting")] 
        public int bars;
        public float mainTempo;
        public float backgroundTempo;
        public float impactTempo;

        [Title("Reference")] 
        public List<AudioSource> audioPlayers;

        public ProceduralAudioClipsManager clipsManager;

        private Queue<AudioClip> _mainQueue;
        private List<Queue<AudioClip>> _playQueues;
        private AudioClip[] _prevPattern;
        private AudioClip[] _curPattern;

        private int _curPatternIndexer;
#pragma warning disable 414
        private int _prevPatternIndexer;
#pragma warning restore 414
        private int _backgroundIndexer;
        private int _impactIndexer;

        private float _singleNote;
        private float _noteTimer;
        private float[] _tempos;
        private float _mainTempoTimer;
        private float[] _tempoTimers;
        
        private void Awake()
        {
            audioPlayers ??= new List<AudioSource>();
            _tempos = new float[System.Enum.GetValues(typeof(TempoType)).Length];
            _tempoTimers = new float[System.Enum.GetValues(typeof(TempoType)).Length];
            _playQueues ??= new List<Queue<AudioClip>>();
            _mainQueue ??= new Queue<AudioClip>();
            _prevPattern = new AudioClip[bars];
            _curPattern = new AudioClip[bars];
            for (var i = 0; i < _tempoTimers.Length; i++)
                _playQueues.Add(new Queue<AudioClip>());
            _singleNote = GetTempoInSeconds(mainTempo) / bars;
        }

        private void Start()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);

            _mainTempoTimer = GetTempoInSeconds(mainTempo);
            
            _tempos[0] = backgroundTempo;
            _tempos[1] = impactTempo;
            _noteTimer = _singleNote;

            _curPatternIndexer = 0;
            _prevPatternIndexer = 0;
            _backgroundIndexer = -1;
            _impactIndexer = -1;
            
            for (var i = 0; i < _tempoTimers.Length; ++i)
                _tempoTimers[i] = GetTempoInSeconds(_tempos[i]);

            StartCoroutine(StartBackgroundAudio());
        }

        private void CreateAudioPlayers()
        {
            for (var i = 0; i < bars; ++i)
            {
                var o = new GameObject("player" + i);
                o.AddComponent<Transform>();
                o.transform.SetParent(transform);
                var player = o.AddComponent<AudioSource>();
                audioPlayers.Add(player);
            }
        }

        private void Update()
        {
            _noteTimer -= Time.deltaTime;

            if (_noteTimer <= 0)
            {
                _noteTimer = _singleNote;
                
                
            }
            
            
            _mainTempoTimer -= Time.deltaTime;

            if (_mainTempoTimer <= 0)
            {
                _mainTempoTimer = GetTempoInSeconds(mainTempo);

                if (_mainQueue.Count > 0)
                {
                    var clip = _mainQueue.Dequeue();
                    _curPattern[_curPatternIndexer] = clip;
                    
                    
                }
            }
            
            for (var i = 0; i < _tempoTimers.Length; ++i)
            {
                _tempoTimers[i] -= Time.deltaTime;
                if(_tempoTimers[i] > 0 || _playQueues[i].Count <= 0)
                    continue;
                
                var audioClip = _playQueues[i].Dequeue();
                _mainQueue.Enqueue(audioClip);

                _tempoTimers[i] = GetTempoInSeconds(_tempos[i]);
            }
        }

        private void AddToQueue(AudioClip clip, TempoType type)
        {
            _playQueues[(int)type].Enqueue(clip);
        }

        private static float GetTempoInSeconds(float t)
        {
            return t / 60;
        }

        public void PlayRandomImpactSound()
        {
            _impactIndexer++;
            if (_impactIndexer >= clipsManager.impactClips.Count)
                _impactIndexer = 0;
            var randomClip = clipsManager.impactClips[_impactIndexer];
            AddToQueue(randomClip, TempoType.Impact);
        }

        private IEnumerator StartBackgroundAudio()
        {
            _backgroundIndexer++;
            if (_backgroundIndexer >= clipsManager.backgroundClips.Count)
                _backgroundIndexer = 0;            
            var clip = clipsManager.backgroundClips[_backgroundIndexer];
            AddToQueue(clip, TempoType.Background);
            yield return new WaitForSeconds(GetTempoInSeconds(backgroundTempo));
            StartCoroutine(StartBackgroundAudio());
        }
    }
}

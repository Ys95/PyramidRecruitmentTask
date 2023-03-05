using System.Collections.Generic;
using PyramidRecruitmentTask.Signals;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private Transform         _audioObjectsContainer;
        private List<AudioSource> _audioObjectsPool;
        private AudioSource       _bgmPlayer;

        private AudioClip _currentBgm;

        [Inject] private SignalBus _signalBus;

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayAudioSignal>(OnPlayAudioSignal);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<PlayAudioSignal>(OnPlayAudioSignal);
        }

        public void Initialize()
        {
            _audioObjectsPool           = new List<AudioSource>();
            _audioObjectsContainer      = new GameObject().transform;
            _audioObjectsContainer.name = "AudioObjectsContainer";
            _bgmPlayer                  = GetComponent<AudioSource>();

            for (int i = 0; i < 5; i++)
            {
                AddAudioObjectToPool();
            }
        }

        private void OnPlayAudioSignal(PlayAudioSignal signal)
        {
            PlayAudio(signal.P_AudioClip, signal.P_Position);
        }

        public void PlayBGM(AudioClip clip, bool loop)
        {
            _bgmPlayer.loop = loop;

            if (clip == _currentBgm && !_bgmPlayer.isPlaying)
            {
                return;
            }

            _currentBgm     = clip;
            _bgmPlayer.clip = clip;
            _bgmPlayer.Play();
        }

        public void PlayAudio(AudioClip clip, Vector3 position)
        {
            var source = GetAudioObject();

            source.transform.position = position;
            source.clip               = clip;
            source.Play();
        }

        private AudioSource GetAudioObject()
        {
            for (int i = 0; i < _audioObjectsPool.Count; i++)
            {
                if (!_audioObjectsPool[i].isPlaying)
                {
                    return _audioObjectsPool[i];
                }
            }

            return AddAudioObjectToPool();
        }

        private AudioSource AddAudioObjectToPool()
        {
            var obj = new GameObject();
            obj.name             = "AudioObject";
            obj.transform.parent = _audioObjectsContainer;
            var audio = obj.AddComponent<AudioSource>();
            _audioObjectsPool.Add(audio);

            return audio;
        }
    }
}
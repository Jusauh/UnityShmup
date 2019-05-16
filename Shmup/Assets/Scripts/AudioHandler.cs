using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shmup.Identifiers;
using Shmup.DataStructures;

namespace Shmup
{
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField]
        private AudioDataContainer[] _audioDataContainers;
        private Dictionary<AudioIdentifier, AudioSource> _audioSouces = new Dictionary<AudioIdentifier, AudioSource>();
        private Dictionary<AudioIdentifier, int> _spamFilter = new Dictionary<AudioIdentifier, int>();

        protected void Awake()
        {
            for(int i = 0; i < _audioDataContainers.Length; i++)
            {
                CreateAudioSouce(_audioDataContainers[i].Id, _audioDataContainers[i].AudioClip);
                _spamFilter.Add(_audioDataContainers[i].Id, 0);
            }
        }

        public void PlayAudio(AudioIdentifier id, AudioPlayAction action, float volume = 1, float pan = 0, int priority = 128)
        {
            if (Time.frameCount > _spamFilter[id])
            {
                switch (action)
                {
                    case AudioPlayAction.NEW:
                        Play(id, volume, pan, priority);
                        _spamFilter[id] = Time.frameCount + 3;
                        break;
                    case AudioPlayAction.RESTART:
                        _audioSouces[id].Stop();
                        Play(id, volume, pan, priority);
                        _spamFilter[id] = Time.frameCount + 3;
                        break;
                    case AudioPlayAction.WAIT:
                        if (!_audioSouces[id].isPlaying)
                        {
                            Play(id, volume, pan, priority);
                            _spamFilter[id] = Time.frameCount + 3;
                        }
                        break;
                }
            }
        }

        private void Play(AudioIdentifier id, float volume, float pan, int priority)
        {
            _audioSouces[id].panStereo = pan;
            _audioSouces[id].volume = volume;
            _audioSouces[id].priority = priority;
            _audioSouces[id].Play();
        }

        private void CreateAudioSouce(AudioIdentifier id, AudioClip clip)
        {
            GameObject audioSource = new GameObject(id.ToString());
            audioSource.transform.SetParent(transform);
            AudioSource source = audioSource.AddComponent<AudioSource>();
            source.clip = clip;
            _audioSouces.Add(id, source);
        }
    }
}

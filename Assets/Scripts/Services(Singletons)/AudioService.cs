using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;

namespace Services.Audio {
    /*
        Serializable Class GameAudio to fill gameAudios parameter in AudioService.
    */
    [System.Serializable]
    public class GameAudio {
        public AudioType audioType;
        [HideInInspector]
        public AudioSource audioSrc;
        public AudioClip audioClip;
        public bool loop;
        [Range(0, 1)]
        public float volume;
    }

    /*
        MonoSingleton AudioService class. Handles all the Audio in the project.
    */
    public class AudioService : GenericMonoSingleton<AudioService>
    {
        [SerializeField] GameAudio[] gameAudios;
        protected override void Awake() {
            if (Instance != null) {
                Destroy(this);
            } else {
                instance = (AudioService)this;
                DontDestroyOnLoad(this.gameObject);
            }

            foreach (GameAudio gameAudio in gameAudios) {
                gameAudio.audioSrc = gameObject.AddComponent<AudioSource>();
                gameAudio.audioSrc.volume = gameAudio.volume;
                gameAudio.audioSrc.clip = gameAudio.audioClip;
                gameAudio.audioSrc.loop = gameAudio.loop;
            }

        }

        /*
            Starts Playing the Audio of the specific type. Searches in the gameAudios array.
            Parameters : 
            - audioType : AudioType to be played. 
        */
        public void PlayAudio(AudioType audioType) {
            GameAudio gameAudio = Array.Find(gameAudios, item => item.audioType == audioType);
            gameAudio.audioSrc.Play();
        }

        /*
            Stops Playing the Audio of the specific type. Searches in the gameAudios array.
            Parameters : 
            - audioType : AudioType to be played. 
        */
        public void StopAudio(AudioType audioType) {
            GameAudio gameAudio = Array.Find(gameAudios, item => item.audioType == audioType);
            gameAudio.audioSrc.Stop();
        }
        
        
    }
}
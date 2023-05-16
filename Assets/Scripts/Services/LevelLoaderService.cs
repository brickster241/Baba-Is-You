using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;
using UnityEngine.SceneManagement;
using Services.Audio;

namespace Services.LevelLoader {
    public class LevelLoaderService : GenericMonoSingleton<LevelLoaderService>
    {
        [SerializeField] Animator crossfade;

        public void TriggerSceneStart() {
            crossfade.gameObject.SetActive(true);
            crossfade.SetTrigger("SceneStart");
        }

        public void TriggerSceneEnd() {
            AudioService.Instance.PlayAudio(Enums.AudioType.SCENE_CHANGE);
            crossfade.gameObject.SetActive(true);
            crossfade.SetTrigger("SceneEnd");
        }

        public void DisableGameObject() {
            crossfade.gameObject.SetActive(false);
        }

        public void LoadScene(int buildIndex) {
            crossfade.gameObject.SetActive(true);
            StartCoroutine(LoadSceneCoroutine(buildIndex));
        }

        private IEnumerator LoadSceneCoroutine(int buildIndex) {
            TriggerSceneEnd();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(buildIndex);
        }
    }

}

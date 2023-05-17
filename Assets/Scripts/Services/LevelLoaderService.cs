using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;
using UnityEngine.SceneManagement;
using Services.Audio;

namespace Services.LevelLoader {
    /*
        LevelLoaderService MonoSingleton class. Handles all the scene transition and scene change functions.
    */
    public class LevelLoaderService : GenericMonoSingleton<LevelLoaderService>
    {
        [SerializeField] Animator crossfade;

        /*
            Triggers animation when scene starts.( CrossFade )
        */
        public void TriggerSceneStart() {
            crossfade.gameObject.SetActive(true);
            crossfade.SetTrigger("SceneStart");
        }
        
        /*
            Triggers animation when scene ends.( CrossFade )
        */
        public void TriggerSceneEnd() {
            AudioService.Instance.PlayAudio(Enums.AudioType.SCENE_CHANGE);
            crossfade.gameObject.SetActive(true);
            crossfade.SetTrigger("SceneEnd");
        }

        /*
            Disables Game Object after animation is complete. Added in Animation Events.
        */
        public void DisableGameObject() {
            crossfade.gameObject.SetActive(false);
        }

        /*
            Loads the scene with a specific buildindex.
        */
        public void LoadScene(int buildIndex) {
            crossfade.gameObject.SetActive(true);
            StartCoroutine(LoadSceneCoroutine(buildIndex));
        }

        /*
            Coroutine to Trigger Scene End Transition.
        */
        private IEnumerator LoadSceneCoroutine(int buildIndex) {
            TriggerSceneEnd();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(buildIndex);
        }
    }

}

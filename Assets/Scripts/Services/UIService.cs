using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Generics;
using Services.Audio;
using Services.LevelLoader;

namespace Services.UI {
    public class UIService : GenericMonoSingleton<UIService>
    {
        public bool isUIVisible;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] GameObject levelCompleteUI;
        [SerializeField] GameObject levelFailedUI;
        [SerializeField] GameObject levelPausedUI;

        private void Start() {
            LevelLoaderService.Instance.TriggerSceneStart();
            isUIVisible = false;
        }

        public void FadeIn() {
            isUIVisible = true;
            canvasGroup.transform.position = new Vector3(0, 5f, 0);
            canvasGroup.DOFade(1f, 0.5f);
            canvasGroup.transform.DOMove(Vector3.zero, 0.5f);
        }

        public void FadeOut(TweenCallback callback) {
            canvasGroup.DOFade(0f, 0.5f).onComplete += callback;
            canvasGroup.transform.DOMove(new Vector3(0, -5, 0), 0.5f);
            isUIVisible = false;
        }

        public void OnLevelComplete() {
            AudioService.Instance.PlayAudio(Enums.AudioType.LEVEL_COMPLETE);
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
            int totalLevels = SceneManager.sceneCountInBuildSettings - 1;
            if (currentLevel == unlockedLevels && unlockedLevels < totalLevels) {
                PlayerPrefs.SetInt("UnlockedLevels", currentLevel + 1);
            }
            levelCompleteUI.SetActive(true);
            FadeIn();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void OnLevelFailed() {
            AudioService.Instance.PlayAudio(Enums.AudioType.LEVEL_FAILED);
            levelFailedUI.SetActive(true);
            FadeIn();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void OnLevelPaused() {
            AudioService.Instance.PlayAudio(Enums.AudioType.BUTTON_CLICK);
            levelPausedUI.SetActive(true);
            FadeIn();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void OnRestartButtonClick() {
            LevelLoaderService.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnNextButtonClick() {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            LevelLoaderService.Instance.LoadScene((buildIndex + 1) % SceneManager.sceneCountInBuildSettings);    
        }

        public void OnMainMenuButtonClick() {
            LevelLoaderService.Instance.LoadScene(0);
        }

        public void OnResumeButtonClick() {
            FadeOut(() => {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                levelCompleteUI.SetActive(false);
                levelFailedUI.SetActive(false);
                levelPausedUI.SetActive(false);
            });
            
        }
    }

}

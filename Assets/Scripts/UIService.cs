using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Generics;

public class UIService : GenericMonoSingleton<UIService>
{
    public bool isUIVisible;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject levelCompleteUI;
    [SerializeField] GameObject levelFailedUI;
    [SerializeField] GameObject levelPausedUI;

    private void Start() {
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

        levelCompleteUI.SetActive(true);
        FadeIn();
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnLevelFailed() {
        levelFailedUI.SetActive(true);
        FadeIn();
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnLevelPaused() {
        levelPausedUI.SetActive(true);
        FadeIn();
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnRestartButtonClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnNextButtonClick() {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene((buildIndex + 1) % SceneManager.sceneCountInBuildSettings);    
    }

    public void OnMainMenuButtonClick() {
        SceneManager.LoadScene(0);
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

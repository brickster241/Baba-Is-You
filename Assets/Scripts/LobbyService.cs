using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyService : GenericMonoSingleton<LobbyService>
{
    [SerializeField] CanvasGroup LevelSelectUI;
    [SerializeField] CanvasGroup MainUI;
    [SerializeField] CanvasGroup InstructionUI;
    [SerializeField] TextMeshProUGUI[] LevelButtonTexts;
    LobbyUIType currentLobbyUIType = LobbyUIType.NONE;
    CanvasGroup currentCanvasGroup = null;

    private void Start() {
        LevelLoaderService.Instance.TriggerSceneStart();
        SwitchUI(LobbyUIType.MAIN);
        EnableButtons();
    }

    public void EnableButtons() {
        int unlocked = PlayerPrefs.GetInt("UnlockedLevels", 1);
        for (int i = 1; i <= LevelButtonTexts.Length; i++) {
            Image buttonImg = LevelButtonTexts[i - 1].GetComponentInParent<Image>();
            Outline outlineButton = LevelButtonTexts[i - 1].GetComponentInParent<Outline>();
            Button button = LevelButtonTexts[i - 1].GetComponentInParent<Button>();
            if (i <= unlocked) {
                buttonImg.color = Color.green;
                outlineButton.effectColor = Color.green;
                LevelButtonTexts[i - 1].color = Color.black;
                button.enabled = true;
            } else {
                outlineButton.effectColor = Color.grey;
                buttonImg.color = Color.grey;
                LevelButtonTexts[i - 1].color = Color.white;
                button.enabled = false;
            }
        }
    }

    public void OnMainMenuButtonClick() {
        SwitchUI(LobbyUIType.MAIN);
    }

    public void OnStartButtonClick() {
        SwitchUI(LobbyUIType.LEVEL_SELECT);
    }

    public void LoadLevel(int level) {
        LevelLoaderService.Instance.LoadScene(level);
    }

    public void OnBackButtonClick() {
        SwitchUI(LobbyUIType.MAIN);
    }

    public void OnInstructionButtonClick() {
        SwitchUI(LobbyUIType.INSTRUCTION);
    }

    private void SwitchUI(LobbyUIType lobbyUIType) {
        if (currentLobbyUIType != LobbyUIType.NONE) {
            FadeOut(currentCanvasGroup, () => {
                currentCanvasGroup.interactable = false;
                currentCanvasGroup.blocksRaycasts = false;
                currentLobbyUIType = lobbyUIType;
                currentCanvasGroup = GetCanvasGroupFromUIType(lobbyUIType);
                FadeIn(currentCanvasGroup, () => {
                    currentCanvasGroup.interactable = true;
                    currentCanvasGroup.blocksRaycasts = true;
                });
            });
            
        } else {
            currentLobbyUIType = lobbyUIType;
            currentCanvasGroup = GetCanvasGroupFromUIType(lobbyUIType);
            FadeIn(currentCanvasGroup, () => {
                currentCanvasGroup.interactable = true;
                currentCanvasGroup.blocksRaycasts = true;
            });
        }
        
    }

    public CanvasGroup GetCanvasGroupFromUIType(LobbyUIType lobbyUIType) {
        if (lobbyUIType == LobbyUIType.NONE) {
            return null;
        } else if (lobbyUIType == LobbyUIType.LEVEL_SELECT) {
            return LevelSelectUI;
        } else if (lobbyUIType == LobbyUIType.MAIN) {
            return MainUI;
        } else {
            return InstructionUI;
        }
    }

    public void FadeIn(CanvasGroup canvasGroup, TweenCallback callback) {
        canvasGroup.transform.position = new Vector3(0, 5f, 0);
        canvasGroup.DOFade(1f, 0.75f).onComplete += callback;
        canvasGroup.transform.DOMove(Vector3.zero, 0.75f);
    }

    public void FadeOut(CanvasGroup canvasGroup, TweenCallback callback) {
        canvasGroup.DOFade(0f, 0.25f).onComplete += callback;
        canvasGroup.transform.DOMove(new Vector3(0, -10, 0), 0.25f);
    }
}

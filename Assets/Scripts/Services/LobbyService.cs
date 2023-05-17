using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Services.Audio;
using Enums;
using Services.LevelLoader;

namespace Services.UI {
    /*
        MonoSingleton LobbyService Class. Handles transitions between UI for Lobby Scene. In a way , it acts like a State Machine. 
    */
    public class LobbyService : GenericMonoSingleton<LobbyService>
    {
        [SerializeField] CanvasGroup LevelSelectUI;
        [SerializeField] CanvasGroup MainUI;
        [SerializeField] CanvasGroup InstructionUI;
        [SerializeField] TextMeshProUGUI[] LevelButtonTexts;
        LobbyUIType currentLobbyUIType = LobbyUIType.NONE;
        CanvasGroup currentCanvasGroup = null;

        private void Start() {
            SwitchUI(LobbyUIType.MAIN);
            LevelLoaderService.Instance.TriggerSceneStart();
            EnableButtons();
        }

        /*
            Quits the Application.
        */
        public void QuitApplication() {
            Application.Quit();
        }

        /*
            Enables Buttons based on Unlocked Levels.
        */
        private void EnableButtons() {
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

        /*
            Switches UI To MainMenu UI.
        */
        public void OnMainMenuButtonClick() {
            SwitchUI(LobbyUIType.MAIN);
        }

        /*
            Switches UI To Level Select.
        */
        public void OnStartButtonClick() {
            SwitchUI(LobbyUIType.LEVEL_SELECT);
        }

        /*
            Loads Level of specific BuildIndex.
        */
        public void LoadLevel(int level) {
            AudioService.Instance.PlayAudio(Enums.AudioType.BUTTON_CLICK);
            LevelLoaderService.Instance.LoadScene(level);
        }

        /*
            Switches UI To Main UI.
        */
        public void OnBackButtonClick() {
            SwitchUI(LobbyUIType.MAIN);
        }

        /*
            Switches UI To Instruction UI.
        */
        public void OnInstructionButtonClick() {
            SwitchUI(LobbyUIType.INSTRUCTION);
        }

        /*
            Switches UI to Specific UI Type, also implements Fade In and Fade Out Transition.
        */
        private void SwitchUI(LobbyUIType lobbyUIType) {
            AudioService.Instance.PlayAudio(Enums.AudioType.UI_SWITCH);
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

        /*
            Gets CanvasGroup reference based on UIType.
        */
        private CanvasGroup GetCanvasGroupFromUIType(LobbyUIType lobbyUIType) {
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

        /*
            FadeIn Method. Fades in the UI.
        */
        private void FadeIn(CanvasGroup canvasGroup, TweenCallback callback) {
            canvasGroup.transform.position = new Vector3(0, 5f, 0);
            canvasGroup.DOFade(1f, 0.75f).onComplete += callback;
            canvasGroup.transform.DOMove(Vector3.zero, 0.75f);
        }

        /*
            FadeOut Method. Fades Out the UI.
        */
        private void FadeOut(CanvasGroup canvasGroup, TweenCallback callback) {
            canvasGroup.DOFade(0f, 0.25f).onComplete += callback;
            canvasGroup.transform.DOMove(new Vector3(0, -10, 0), 0.25f);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;
using Services.UI;
using Services.Block;

namespace Services.IO {
    /*
        MonoSingleton InputService class. Handles all the Player Input which is then provided to BlockManager.
    */
    public class InputService : GenericMonoSingleton<InputService>
    {
        Vector2 direction;
        bool isTurnComplete;

        private void Start() {
            direction = Vector2.zero;
            isTurnComplete = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (isTurnComplete && !UIService.Instance.isUIVisible) {
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    direction = Vector2.up;
                } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    direction = Vector2.down;
                } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    direction = Vector2.right;
                } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    direction = Vector2.left;
                } else {
                    direction = Vector2.zero;
                    if (Input.GetKeyDown(KeyCode.Space)) {
                        UIService.Instance.OnLevelPaused();
                    }
                }
                if (direction != Vector2.zero) {
                    TurnExecute(direction);
                }
            }
            
        }

        /*
            Executes Turn for a specific input direction.
        */
        private void TurnExecute(Vector2 dir) {
            StartCoroutine(BlockManager.Instance.ExecuteTurn(dir));
        }

        /*
            Sets the Turn Complete attribute to value. Called inside the coroutine to ensure multiple inputs are not registered.
        */
        public void SetTurnComplete(bool value) {
            isTurnComplete = value;
        }
    } 

}

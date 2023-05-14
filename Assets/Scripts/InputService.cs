using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputService : MonoBehaviour
{
    Vector2 direction;
    bool isTurnComplete;
    [SerializeField] NounService nounService;

    private void Awake() {
        direction = Vector2.zero;
        isTurnComplete = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurnComplete) {
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
            }
            if (direction != Vector2.zero) {
                nounService.StartMovement(direction);
                StartCoroutine(TurnExecute());
            }
        }
        
    }

    IEnumerator TurnExecute() {
        isTurnComplete = false;
        yield return new WaitForSeconds(0.25f);
        isTurnComplete = true;
    }
} 

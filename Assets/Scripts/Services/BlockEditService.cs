using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Scriptables;

namespace Services.Block {
    [ExecuteInEditMode]
    public class BlockEditService : MonoBehaviour
    {
        [SerializeField] BlockType blockType;
        [SerializeField] PropertyType propertyType;
        [SerializeField] NounType nounType;
        [SerializeField] NounType nounTextType;
        [SerializeField] PropertyType propertyTextType;
        [SerializeField] Sprite defaultSprite;
        [SerializeField] BlockScriptableObjectList blockConfigurations;
        SpriteRenderer sr;

        private void Awake() {
            sr = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            BlockScriptableObject blockConfig = Array.Find(blockConfigurations.blockConfigs, item => item.blockType == blockType && item.nounType == nounType && item.propertyType == propertyType && item.nounTextType == nounTextType && item.propertyTextType == propertyTextType);
            if (blockConfig != null) {
                sr.sprite = blockConfig.blockSprite;
            } else {
                sr.sprite = defaultSprite;
            }
        }
    }

}

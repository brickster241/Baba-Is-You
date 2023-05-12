using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlockEditService : MonoBehaviour
{
    [SerializeField] BlockType blockType;
    [SerializeField] PropertyType propertyType;
    [SerializeField] NounType nounType;
    [SerializeField] BlockScriptableObjectList blockConfigurations;
    SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        BlockScriptableObject blockConfig = Array.Find(blockConfigurations.blockConfigs, item => item.blockType == blockType && item.nounType == nounType && item.propertyType == propertyType);
        sr.sprite = blockConfig.blockSprite;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    private SpriteRenderer sr;
    public NounType nounType;
    public BlockType blockType;
    public NounType nounText;
    public PropertyType propertyText;
    public PropertySM propertySM;
    public bool isBlockMovementPossible;
    public bool isMovementCalculated;
    public Animator anim;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        propertySM = new PropertySM(this);
    }

    public void setAnimation(BlockScriptableObjectList blockConfigs) {
        BlockScriptableObject blockConfig = Array.Find(blockConfigs.blockConfigs, item => item.blockType == blockType && item.nounType == nounType && item.nounTextType == nounText && item.propertyTextType == propertyText);
        if (blockConfig != null)
            anim.Play(blockConfig.stateName);
    }

    public void AddProperty(PropertyType property) {
        propertySM.AddProperty(property);
    }

    public void RemoveProperty(PropertyType property) {
        propertySM.RemoveProperty(property);
    }

    public void HighlightBlock() {
        sr.color = Color.white;
    }

    public void DisableHighlightBlock() {
        sr.color = new Color(1f, 1f, 1f, 0.55f);
    }

    public void FlipSprite(Vector2 direction) {
        if (direction == Vector2.right)
            sr.flipX = false;
        else if (direction == Vector2.left)
            sr.flipX = true;
    }

    public void Move(Vector2 direction) {
        transform.DOMove(transform.position + new Vector3(direction.x, direction.y, 0f), 0.25f);
    }

    public void InvokeLevelComplete() {
        BlockManager.Instance.InvokeLevelComplete();
    }
}

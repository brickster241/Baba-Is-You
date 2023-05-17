using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Enums;
using Services.Block;
using Block.StateMachine;
using Scriptables;

namespace Block.Controller {
    /*
        Controller class for Block. Handles all the operations of Block.
        - nounType and blockType are block properties.
        - nounText and propertyText are properties when BlockType is NOUN_TEXT / PROPERTY_TEXT.
        - propertySM is State Machine associated with each block. Used to keep track of all properties BLOCK has.
        - isBlockMovementPossible , isMovementCalculated are boolean variables to track block movement.
    */
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
            transform.position = NearestRoundOff(transform.position);
        }

        /*
            NearestRoundOff Method. Changes Position to nearest int coordinates.
        */
        private Vector3 NearestRoundOff(Vector3 blockPosition) {
            Vector3 result = new Vector3(blockPosition.x, blockPosition.y, 0f);
            result.x = Mathf.RoundToInt(blockPosition.x);
            result.y = Mathf.RoundToInt(blockPosition.y);
            result.z = Mathf.RoundToInt(blockPosition.z);
            return result;
        }

        /*
            Sets the animation based on Block Configurations.
        */
        public void setAnimation(BlockScriptableObjectList blockConfigs) {
            BlockScriptableObject blockConfig = Array.Find(blockConfigs.blockConfigs, item => item.blockType == blockType && item.nounType == nounType && item.nounTextType == nounText && item.propertyTextType == propertyText);
            if (blockConfig != null)
                anim.Play(blockConfig.stateName);
        }

        /*
            Adds a property to the existing set of Properties.
        */
        public void AddProperty(PropertyType property) {
            propertySM.AddProperty(property);
        }

        /*
            Removes a property from existing set of properties.
        */
        public void RemoveProperty(PropertyType property) {
            propertySM.RemoveProperty(property);
        }

        /*
            Highlights a text block when Rule is activated.
        */
        public void HighlightBlock() {
            sr.color = Color.white;
        }

        /*
            Dims the text block when Rule is deactivated.
        */
        public void DisableHighlightBlock() {
            sr.color = new Color(1f, 1f, 1f, 0.55f);
        }

        /*
            Flips the sprite based on User Input direction.
        */
        public void FlipSprite(Vector2 direction) {
            if (direction == Vector2.right)
                sr.flipX = false;
            else if (direction == Vector2.left)
                sr.flipX = true;
        }

        /*
            Moves the block towards the direction. Executed after movement is calculated.
        */
        public void Move(Vector2 direction) {
            transform.DOMove(transform.position + new Vector3(direction.x, direction.y, 0f), 0.25f);
        }

        /*
            InvokesLevelComplete Method. Executed when Level is complete. Is Executed when block is in YOU + WIN state.
        */
        public void InvokeLevelComplete() {
            BlockManager.Instance.InvokeLevelComplete();
        }
    }

}

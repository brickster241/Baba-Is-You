using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Block.Controller;

namespace Services.Block {
    /*
        RuleService class. Not a singleton, but is called from BlockManager which is a singleton.. hence only one instance throughout the game.
        Handles all the Operations for NOUN_TEXT, OPERATOR, PROPERTY_TEXT Blocks.
    */
    public class RuleService
    {
        private BlockManager blockManager;
        private List<BlockController> operatorBlocks;
        private List<BlockController> textBlocks;
        List<List<bool>> activeRules;

        public RuleService(BlockManager blockManager) {
            activeRules = new List<List<bool>>();
            this.blockManager = blockManager;
            operatorBlocks = this.blockManager.GetOperatorBlocks();
            textBlocks = this.blockManager.GetTextBlocks();
            SetActiveRules();
        }

        /*
            Sets Initial Rules for the game. Every block has a NONE Property and Non-NONE BLockType blocks have PUSH property.
        */
        private void SetActiveRules() {
            int rows = Constants.nounTypes.Length;
            int cols = Constants.propertyTypes.Length;
            for (int i = 0; i < rows; i++) {
                List<bool> row = new List<bool>();
                for (int j = 0; j < cols; j++) {
                    row.Add(false);
                }
                activeRules.Add(row);
            }
            
            for (int i = 0; i < rows; i++) {
                activeRules[(int)Constants.nounTypes[i]][(int)PropertyType.NONE] = true;
                AddRule(Constants.nounTypes[i], PropertyType.NONE);
                if ((NounType)i == NounType.NONE) {
                    activeRules[i][(int)PropertyType.PUSH] = true;
                    AddRule(NounType.NONE, PropertyType.PUSH);
                }
            }
        }

        /*
            UpdateRules Method. Fetches Current RuleMatrix and compares with the initial RuleMatrix.
            Based on comparision, Rules are added or removed.
        */
        public void UpdateRules() {
            List<List<bool>> updatedRules = GetRuleMatrix();
            int rows = Constants.nounTypes.Length;
            int cols = Constants.propertyTypes.Length;
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    if (activeRules[i][j] == updatedRules[i][j]) {
                        continue;
                    } else if (activeRules[i][j]) {
                        Debug.Log("RULE REMOVED : " + (NounType)i + " IS " + (PropertyType)j);
                        RemoveRule((NounType)i, (PropertyType)j);
                    } else {
                        Debug.Log("RULE ADDED : " + (NounType)i + " IS " + (PropertyType)j);
                        AddRule((NounType)i, (PropertyType)j);
                    }
                }
            }
            activeRules = updatedRules;
        }

        /*
            GetRuleMatrix Method. Returns a 2d-List where matrix[NOUN][PROPERTY] = true.
        */
        public List<List<bool>> GetRuleMatrix() {
            int rows = Constants.nounTypes.Length;
            int cols = Constants.propertyTypes.Length;
            List<List<bool>> ruleMatrix = new List<List<bool>>();
            for (int i = 0; i < rows; i++) {
                List<bool> row = new List<bool>();
                for (int j = 0; j < cols; j++) {
                    row.Add(false);
                }
                ruleMatrix.Add(row);
            }

            for (int i = 0; i < rows; i++) {
                ruleMatrix[(int)Constants.nounTypes[i]][(int)PropertyType.NONE] = true;
                if ((NounType)i == NounType.NONE) {
                    ruleMatrix[i][(int)PropertyType.PUSH] = true;
                }
            }
            
            for (int i = 0; i < operatorBlocks.Count; i++) {
                Dictionary<Vector2, BlockController> adjBlocks = blockManager.GetAdjacentTextBlocks(operatorBlocks[i]);
                BlockController leftBlock = adjBlocks[Vector2.left];
                BlockController topBlock = adjBlocks[Vector2.up];
                BlockController rightBlock = adjBlocks[Vector2.right];
                BlockController bottomBlock = adjBlocks[Vector2.down];
                if (leftBlock != null && rightBlock != null) {
                    NounType nounType = leftBlock.nounText;
                    PropertyType property = rightBlock.propertyText;
                    ruleMatrix[(int)nounType][(int)property] = true;
                }
                if (topBlock != null && bottomBlock != null) {
                    NounType nounType = topBlock.nounText;
                    PropertyType property = bottomBlock.propertyText;
                    ruleMatrix[(int)nounType][(int)property] = true;
                }
            }
            return ruleMatrix;
        }

        /*
            Adds Rule to existing set of properties forthe specified nountype. Calls BlockManager.
        */
        public void AddRule(NounType nounType, PropertyType property) {
            blockManager.AddRule(nounType, property);
        }

        /*
            Removes Rule from existing set of properties forthe specified nountype. Calls BlockManager.
        */
        public void RemoveRule(NounType nounType, PropertyType property) {
            blockManager.RemoveRule(nounType, property);
        }

        /*
            Updates Text Block Color based on Rule Activation and adjacent Blocks.
        */
        public void UpdateTextBlockColors() {
            for (int i = 0; i < textBlocks.Count; i++) {
                BlockController textBlock = textBlocks[i];
                if (textBlock.blockType == BlockType.NOUN_TEXT) {
                    UpdateTextBlockColor(textBlock, Vector2.right, Vector2.down);
                } else if (textBlock.blockType == BlockType.PROPERTY_TEXT) {
                    UpdateTextBlockColor(textBlock, Vector2.left, Vector2.up);
                }
                
            }
        }

        /*
            Updated TextBlock Color based on direction. Method is called on every NOUN_TEXT, PROPERTY_TEXT Block.
            Highlights color if the block is part of the RULE.
        */
        private void UpdateTextBlockColor(BlockController textBlock, Vector2 direction1, Vector2 direction2) {
            List<BlockController> nearRight = blockManager.GetAdjacentBlocksInDirection(textBlock, direction1);
            List<BlockController> farRight = blockManager.GetAdjacentBlocksInDirection(textBlock, direction1 * 2);
            List<BlockController> nearDown = blockManager.GetAdjacentBlocksInDirection(textBlock, direction2);
            List<BlockController> farDown = blockManager.GetAdjacentBlocksInDirection(textBlock, direction2 * 2);
            BlockController operatorBlockRight = null;
            BlockController operatorBlockDown = null;
            BlockController propertyBlockRight = null;
            BlockController propertyBlockDown = null;
            for (int i = 0; i < nearRight.Count; i++) {
                if (nearRight[i].blockType == BlockType.OPERATOR) {
                    operatorBlockRight = nearRight[i];
                    break;
                }
            }
            for (int i = 0; i < nearDown.Count; i++) {
                if (nearDown[i].blockType == BlockType.OPERATOR) {
                    operatorBlockDown = nearDown[i];
                    break;
                }
            }
            for (int i = 0; i < farRight.Count; i++) {
                if (farRight[i].blockType == BlockType.PROPERTY_TEXT) {
                    propertyBlockRight = farRight[i];
                    break;
                }
            }
            for (int i = 0; i < farDown.Count; i++) {
                if (farDown[i].blockType == BlockType.PROPERTY_TEXT) {
                    propertyBlockDown = farDown[i];
                    break;
                }
            }
            bool isHorizontalHighlighted = false;
            bool isVerticalHighlighted = false;
            if (operatorBlockRight != null && propertyBlockRight != null) {
                textBlock.HighlightBlock();
                propertyBlockRight.HighlightBlock();
                isHorizontalHighlighted = true;
            } else {
                if (propertyBlockRight != null)
                    propertyBlockRight.DisableHighlightBlock();
            }
            if (operatorBlockDown != null && propertyBlockDown != null) {
                textBlock.HighlightBlock();
                propertyBlockDown.HighlightBlock();
                isVerticalHighlighted = true;
            } else {
                if (propertyBlockDown != null)
                    propertyBlockDown.DisableHighlightBlock();
            }
            if (!isHorizontalHighlighted && !isVerticalHighlighted) {
                textBlock.DisableHighlightBlock();
            }
        }
    }

}

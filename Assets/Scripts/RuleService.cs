using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("Set Active Rules.");
    }

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
                leftBlock.HighlightBlock();
                rightBlock.HighlightBlock();
                ruleMatrix[(int)nounType][(int)property] = true;
            }
            if (topBlock != null && bottomBlock != null) {
                NounType nounType = topBlock.nounText;
                PropertyType property = bottomBlock.propertyText;
                topBlock.HighlightBlock();
                bottomBlock.HighlightBlock();
                ruleMatrix[(int)nounType][(int)property] = true;
            }
        }
        return ruleMatrix;
    }

    public void AddRule(NounType nounType, PropertyType property) {
        blockManager.AddRule(nounType, property);
    }

    public void RemoveRule(NounType nounType, PropertyType property) {
        blockManager.RemoveRule(nounType, property);
    }
}

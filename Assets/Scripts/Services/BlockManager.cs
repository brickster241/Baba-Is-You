using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;
using Services.Audio;

public class BlockManager : GenericMonoSingleton<BlockManager>
{
    private List<BlockController> blockControllers;
    private Dictionary<NounType, List<BlockController>> nounBlocks;
    private Dictionary<BlockType, List<BlockController>> blocksWithBlockType;
    private NounService nounService;
    private RuleService ruleService;
    private Dictionary<Vector3, List<BlockController>> blockPositionMatrix;
    [SerializeField] BlockScriptableObjectList blockConfigs;

    private void Start() {
        blockControllers = new List<BlockController>();
        nounBlocks = new Dictionary<NounType, List<BlockController>>();
        blocksWithBlockType = new Dictionary<BlockType, List<BlockController>>();
        blockPositionMatrix = new Dictionary<Vector3, List<BlockController>>();

        // NOUNBLOCKS
        for (int i = 0; i < Constants.nounTypes.Length; i++) {
            nounBlocks[Constants.nounTypes[i]] = new List<BlockController>();
        }
        for (int i = 0; i < Constants.blockTypes.Length; i++) {
            blocksWithBlockType[Constants.blockTypes[i]] = new List<BlockController>();
        }

        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blocks.Length; i++) {
            BlockController blockController = blocks[i].GetComponent<BlockController>();
            blockController.setAnimation(blockConfigs);
            blockControllers.Add(blockController);
            BlockType blockType = blockController.blockType;
            NounType nounType = blockController.nounType;
            blocksWithBlockType[blockType].Add(blockController);
            nounBlocks[nounType].Add(blockController);
        }
        nounService = new NounService(this);
        ruleService = new RuleService(this);
    }

    public List<BlockController> GetAllBlocks() {
        return blockControllers;
    }

    public Dictionary<NounType, List<BlockController>> GetNounBlocks() {
        return nounBlocks;
    }

    private void UpdateRules() {
        ruleService.UpdateRules();
    }

    private void StartMovement(Vector2 direction) {
        nounService.StartMovement(direction);
        UpdateBlockPositionMatrix();
    }

    public void AddRule(NounType nounType, PropertyType property) {
        nounService.AddProperty(nounType, property);
    }

    public void RemoveRule(NounType nounType, PropertyType property) {
        nounService.RemoveProperty(nounType, property);
    }

    public List<BlockController> GetTextBlocks() {
        List<BlockController> propertyTextBlocks = GetBlocksOfBlockType(BlockType.PROPERTY_TEXT);
        List<BlockController> nounTextBlocks = GetBlocksOfBlockType(BlockType.NOUN_TEXT);
        List<BlockController> textBlocks = new List<BlockController>();
        textBlocks.AddRange(propertyTextBlocks);
        textBlocks.AddRange(nounTextBlocks);
        return textBlocks;
    }

    public List<BlockController> GetOperatorBlocks() {
        return GetBlocksOfBlockType(BlockType.OPERATOR);
    }

    private List<BlockController> GetBlocksOfBlockType(BlockType blockType) {
        return blocksWithBlockType[blockType];
    }

    public List<BlockController> GetBlocksOfNounType(NounType nounType) {
        return nounBlocks[nounType];
    }

    public Dictionary<Vector2, BlockController> GetAdjacentTextBlocks(BlockController blockController) {
        Dictionary<Vector2, BlockController> adjBlocks = new Dictionary<Vector2, BlockController>();
        for (int i = 0; i < Constants.directions.Length; i++) {
            adjBlocks[Constants.directions[i]] = null;
        }
        List<BlockController> nearbyBlocks = new List<BlockController>();
        nearbyBlocks.AddRange(GetAdjacentBlocksInDirection(blockController, Vector2.up));
        nearbyBlocks.AddRange(GetAdjacentBlocksInDirection(blockController, Vector2.down));
        nearbyBlocks.AddRange(GetAdjacentBlocksInDirection(blockController, Vector2.left));
        nearbyBlocks.AddRange(GetAdjacentBlocksInDirection(blockController, Vector2.right));
        
        Vector2 blockPos = new Vector2(blockController.transform.position.x, blockController.transform.position.y);
        for (int i = 0; i < nearbyBlocks.Count; i++) {
            BlockController nearbyBlock = nearbyBlocks[i];
            Vector2 pos = new Vector2(nearbyBlock.transform.position.x, nearbyBlock.transform.position.y);
            if (pos - blockPos == Vector2.left && nearbyBlock.blockType == BlockType.NOUN_TEXT) {
                adjBlocks[Vector2.left] = nearbyBlock;
            } else if (pos - blockPos == Vector2.right && nearbyBlock.blockType == BlockType.PROPERTY_TEXT) {
                adjBlocks[Vector2.right] = nearbyBlock;
            } else if (pos - blockPos == Vector2.up && nearbyBlock.blockType == BlockType.NOUN_TEXT) {
                adjBlocks[Vector2.up] = nearbyBlock;
            } else if (pos - blockPos == Vector2.down && nearbyBlock.blockType == BlockType.PROPERTY_TEXT) {
                adjBlocks[Vector2.down] = nearbyBlock;
            }
        }
        return adjBlocks;
    }

    public bool isMovementInsideGrid(BlockController blockController, Vector2 direction) {
        if (direction == Vector2.right && blockController.transform.position.x == 16) {
            return false;
        } else if (direction == Vector2.left && blockController.transform.position.x == -16) {
            return false;
        } else if (direction == Vector2.up && blockController.transform.position.y == 8) {
            return false;
        } else if (direction == Vector2.down && blockController.transform.position.y == -8) {
            return false;
        } else {
            return true;
        }
    }

    public List<BlockController> GetAdjacentBlocksInDirection(BlockController blockController, Vector2 direction) {
        Vector3 targetPosition = blockController.transform.position + new Vector3(direction.x, direction.y, 0f);
        if (blockPositionMatrix.ContainsKey(targetPosition)) {
            return blockPositionMatrix[targetPosition];
        } else {
            blockPositionMatrix[targetPosition] = new List<BlockController>();
            return blockPositionMatrix[targetPosition];
        }
    }

    private bool isCurrentLevelComplete() {
        Dictionary<PropertyType, List<BlockController>> PropertyBlocks = GetBlocksOfPropertyType();
        List<BlockController> youBlocks = PropertyBlocks[PropertyType.YOU];
        List<BlockController> winBlocks = PropertyBlocks[PropertyType.WIN];
        HashSet<Vector3> positionSet = new HashSet<Vector3>();
        foreach (BlockController youBlock in youBlocks)
        {
            positionSet.Add(youBlock.transform.position);
        }
        foreach (BlockController winBlock in winBlocks)
        {
            positionSet.Add(winBlock.transform.position);
        }
        return positionSet.Count != youBlocks.Count + winBlocks.Count;
    }

    private Dictionary<PropertyType, List<BlockController>> GetBlocksOfPropertyType() {
        Dictionary<PropertyType, List<BlockController>> propertyBlocks = new Dictionary<PropertyType, List<BlockController>>();
        for (int i = 0; i < Constants.propertyTypes.Length; i++) {
            propertyBlocks[Constants.propertyTypes[i]] = new List<BlockController>();
        }
        for (int i = 0; i < blockControllers.Count; i++) {
            PropertyType property = blockControllers[i].propertySM.GetCurrentDominantProperty();
            propertyBlocks[property].Add(blockControllers[i]);
        }
        return propertyBlocks;
    }

    private void UpdateBlockPositionMatrix() {
        blockPositionMatrix.Clear();
        for (int i = 0; i < blockControllers.Count; i++) {
            Vector3 position = blockControllers[i].transform.position;
            if (blockPositionMatrix.ContainsKey(position)) {
                blockPositionMatrix[position].Add(blockControllers[i]);
            } else {
                blockPositionMatrix[position] = new List<BlockController>();
                blockPositionMatrix[position].Add(blockControllers[i]);
            }
        }
    }

    private void UpdateBlockColors() {
        ruleService.UpdateTextBlockColors();
    }

    private bool isCurrentLevelFailed() {
        Dictionary<PropertyType, List<BlockController>> PropertyBlocks = GetBlocksOfPropertyType();
        List<BlockController> youBlocks = PropertyBlocks[PropertyType.YOU];
        return youBlocks.Count == 0;
    }

    public void InvokeLevelComplete() {
        UIService.Instance.OnLevelComplete();
    }

    public void InvokeLevelFailed() {
        UIService.Instance.OnLevelFailed();
    }

    public IEnumerator ExecuteTurn(Vector2 dir) {
        AudioService.Instance.PlayAudio(AudioType.TURN);
        InputService.Instance.SetTurnComplete(false);
        UpdateBlockPositionMatrix();
        UpdateRules();
        UpdateBlockColors();
        StartMovement(dir);
        yield return new WaitForSeconds(0.25f);
        UpdateBlockPositionMatrix();
        UpdateRules();
        UpdateBlockColors();
        CheckLevelConditions();
        InputService.Instance.SetTurnComplete(true);
    }



    public void CheckLevelConditions() {
        bool isLevelCompleted = isCurrentLevelComplete();
        bool isLevelFailed = isCurrentLevelFailed();
        if (isLevelCompleted) {
            InvokeLevelComplete();
        } else if (isLevelFailed) {
            InvokeLevelFailed();
        }
    }
}

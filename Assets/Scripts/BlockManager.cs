using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private List<BlockController> blockControllers;
    private Dictionary<NounType, List<BlockController>> nounBlocks;
    private Dictionary<BlockType, List<BlockController>> blocksWithBlockType;
    private NounService nounService;
    private RuleService ruleService;
    [SerializeField] BlockScriptableObjectList blockConfigs;

    private void Awake() {
        blockControllers = new List<BlockController>();
        nounBlocks = new Dictionary<NounType, List<BlockController>>();
        blocksWithBlockType = new Dictionary<BlockType, List<BlockController>>();

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

    public void UpdateRules() {
        ruleService.UpdateRules();
    }

    public void StartMovement(Vector2 direction) {
        nounService.StartMovement(direction);
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

    public List<BlockController> GetBlocksOfBlockType(BlockType blockType) {
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
        Vector2 blockPos = new Vector2(blockController.transform.position.x, blockController.transform.position.y);
        for (int i = 0; i < blockControllers.Count; i++) {
            Vector2 pos = new Vector2(blockControllers[i].transform.position.x, blockControllers[i].transform.position.y);
            if (pos - blockPos == Vector2.left && blockControllers[i].blockType == BlockType.NOUN_TEXT) {
                adjBlocks[Vector2.left] = blockControllers[i];
            } else if (pos - blockPos == Vector2.right && blockControllers[i].blockType == BlockType.PROPERTY_TEXT) {
                adjBlocks[Vector2.right] = blockControllers[i];
            } else if (pos - blockPos == Vector2.up && blockControllers[i].blockType == BlockType.NOUN_TEXT) {
                adjBlocks[Vector2.up] = blockControllers[i];
            } else if (pos - blockPos == Vector2.down && blockControllers[i].blockType == BlockType.PROPERTY_TEXT) {
                adjBlocks[Vector2.down] = blockControllers[i];
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
        Vector2 blockPos = new Vector2(blockController.transform.position.x, blockController.transform.position.y); 
        List<BlockController> adjBlocks = new List<BlockController>();
        for (int i = 0; i < blockControllers.Count; i++) {
            Vector2 position = new Vector2(blockControllers[i].transform.position.x, blockControllers[i].transform.position.y);
            if (position - blockPos == direction) {
                adjBlocks.Add(blockControllers[i]);
            }
        }
        return adjBlocks;
    }

    public bool isLevelComplete() {
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
}

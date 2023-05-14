using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NounService : MonoBehaviour
{
    private List<BlockController> blockControllers;
    [SerializeField] LayerMask blockLayerMask;
    private Dictionary<NounType, List<BlockController>> blocks;
    
    private void Awake() {
        blockControllers = new List<BlockController>();
        blocks = new Dictionary<NounType, List<BlockController>>();
        for (int i = 0; i < Constants.nounTypes.Length; i++) {
            blocks[Constants.nounTypes[i]] = new List<BlockController>();
        }
        GameObject[] blockGameObjects = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blockGameObjects.Length; i++) {
            BlockController blockController = blockGameObjects[i].GetComponent<BlockController>();
            NounType blockNounType = blockController.nounType;
            blocks[blockNounType].Add(blockController);
            blockControllers.Add(blockController);
        }
    }

    public void AddProperty(NounType nounType, PropertyType property) {
        for (int i = 0; i < blocks[nounType].Count; i++) {
            blocks[nounType][i].AddProperty(property);
        }
    }

    public void RemoveProperty(NounType nounType, PropertyType property) {
        for (int i = 0; i < blocks[nounType].Count; i++) {
            blocks[nounType][i].RemoveProperty(property);
        }
    }

    public List<BlockController> GetBlocksOfProperty(PropertyType property) {
        List<BlockController> propertyBlocks = new List<BlockController>();
        for (int i = 0; i < blockControllers.Count; i++) {
            BlockController blockController = blockControllers[i];
            if (blockController.propertySM.GetCurrentDominantProperty() == property) {
                propertyBlocks.Add(blockController);
            }
        }
        return propertyBlocks;
    }

    public List<BlockController> GetBlocksOfNounType(NounType nounType) {
        return blocks[nounType];
    }

    public void StartMovement(Vector2 direction) {
        Debug.Log("Starting Movement.");
        // GET YOU BLOCKS IN OPPOSITE DIRECTION
        // GET YOU + PUSH BLOCKS IN DIRECTION
        for (int i = 0; i < blockControllers.Count; i++) {
            blockControllers[i].isMovementCalculated = false;
            blockControllers[i].isBlockMovementPossible = false;
        }

        for (int i = 0; i < blockControllers.Count; i++) {
            if (blockControllers[i].propertySM.GetCurrentDominantProperty() == PropertyType.YOU) {
                if (!blockControllers[i].isMovementCalculated) {
                    blockControllers[i].isBlockMovementPossible = CalculateMovement(blockControllers[i], direction);
                }
            }
        }

        for (int i = 0; i < blockControllers.Count; i++) {
            if (blockControllers[i].isBlockMovementPossible) {
                blockControllers[i].Move(direction);
            }
        }

    }

    private bool CalculateMovement(BlockController blockController, Vector2 direction) {
        if (blockController.isMovementCalculated)
            return blockController.isBlockMovementPossible;
        if (blockController.propertySM.GetCurrentDominantProperty() == PropertyType.STOP) {
            blockController.isMovementCalculated = true;
            blockController.isBlockMovementPossible = false;
            return false;
        }
        if (!isValid(blockController, direction)) {
            blockController.isMovementCalculated = true;
            blockController.isBlockMovementPossible = false;
            return false;
        }
        List<BlockController> adjBlocks = GetAdjacentBlocksInDirection(blockController, direction);
        bool isMovementPossible = true;
        for (int i = 0; i < adjBlocks.Count; i++) {
            PropertyType property = adjBlocks[i].propertySM.GetCurrentDominantProperty();
            if (property == PropertyType.YOU || property == PropertyType.PUSH || property == PropertyType.STOP) {
                isMovementPossible = isMovementPossible && CalculateMovement(adjBlocks[i], direction);
            }
        }
        blockController.isMovementCalculated = true;
        blockController.isBlockMovementPossible = isMovementPossible;
        return isMovementPossible;
    }

    private bool isValid(BlockController blockController, Vector2 direction) {
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
            //
            Vector2 position = new Vector2(blockControllers[i].transform.position.x, blockControllers[i].transform.position.y);
            if (position - blockPos == direction) {
                adjBlocks.Add(blockControllers[i]);
            }
        }
        return adjBlocks;
    }
}

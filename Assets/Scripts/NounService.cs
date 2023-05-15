using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NounService
{
    private BlockManager blockManager;
    private List<BlockController> blockControllers;
    private Dictionary<NounType, List<BlockController>> nounBlocks;


    public NounService(BlockManager blockManager) {
        this.blockManager = blockManager;
        blockControllers = this.blockManager.GetAllBlocks();
        nounBlocks = this.blockManager.GetNounBlocks();
    }

    public void AddProperty(NounType nounType, PropertyType property) {
        List<BlockController> blocks = blockManager.GetBlocksOfNounType(nounType);
        for (int i = 0; i < blocks.Count; i++) {
            blocks[i].AddProperty(property);
        }
    }

    public void RemoveProperty(NounType nounType, PropertyType property) {
        for (int i = 0; i < nounBlocks[nounType].Count; i++) {
            nounBlocks[nounType][i].RemoveProperty(property);
        }
    }

    public void StartMovement(Vector2 direction) {
        for (int i = 0; i < blockControllers.Count; i++) {
            blockControllers[i].isMovementCalculated = false;
            blockControllers[i].isBlockMovementPossible = false;
        }

        for (int i = 0; i < blockControllers.Count; i++) {
            if (blockControllers[i].propertySM.GetCurrentDominantProperty() == PropertyType.YOU) {
                blockControllers[i].FlipSprite(direction);
                if (!blockControllers[i].isMovementCalculated) {
                    blockControllers[i].isBlockMovementPossible = IsMovementPossible(blockControllers[i], direction);
                }
            }
        }
        for (int i = 0; i < blockControllers.Count; i++) {
            if (blockControllers[i].isBlockMovementPossible) {
                blockControllers[i].Move(direction);
            }
        }
    }

    private bool IsMovementPossible(BlockController blockController, Vector2 direction) {
        if (blockController.isMovementCalculated)
            return blockController.isBlockMovementPossible;
        if (!blockManager.isMovementInsideGrid(blockController, direction)) {
            blockController.isMovementCalculated = true;
            blockController.isBlockMovementPossible = false;
            return false;
        }
        List<BlockController> adjBlocks = blockManager.GetAdjacentBlocksInDirection(blockController, direction);
        bool isMovementPossible = true;
        bool isStopBlockPresent = false;
        for (int i = 0; i < adjBlocks.Count; i++) {
            PropertyType property = adjBlocks[i].propertySM.GetCurrentDominantProperty();
            if (property == PropertyType.STOP) {
                isStopBlockPresent = true;
                break;
            }
        }
        if (isStopBlockPresent) {
            isMovementPossible = false;
            for (int i = 0; i < adjBlocks.Count; i++) {
                adjBlocks[i].isMovementCalculated = true;
                adjBlocks[i].isBlockMovementPossible = false;
            }
        } else {
            for (int i = 0; i < adjBlocks.Count; i++) {
                PropertyType property = adjBlocks[i].propertySM.GetCurrentDominantProperty();
                if (property == PropertyType.YOU || property == PropertyType.PUSH) {
                    isMovementPossible = isMovementPossible && IsMovementPossible(adjBlocks[i], direction);
                }
            }
        }
        blockController.isMovementCalculated = true;
        blockController.isBlockMovementPossible = isMovementPossible;
        return isMovementPossible;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;
using Services.Audio;
using Services.IO;
using Services.UI;
using Scriptables;
using Block.Controller;
using Enums;

namespace Services.Block {
    /*
        MonoSingleton BlockManager class. Handles all the general block operations of the game. 
    */
    public class BlockManager : GenericMonoSingleton<BlockManager>
    {
        private List<BlockController> blockControllers;
        private Dictionary<NounType, List<BlockController>> nounBlocks;
        private Dictionary<BlockType, List<BlockController>> blocksWithBlockType;
        private NounService nounService;
        private RuleService ruleService;
        private Dictionary<Vector3, List<BlockController>> blockPositionMatrix;
        [SerializeField] BlockScriptableObjectList blockConfigs;

        /*
            Initializes all the BlockControllers. Stores and fills dictionary based on NounType, BlockType.
            Initializes RuleService, NounService classes which control Rule Blocks / Noun Blocks respectively.
        */
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

        /*
            Fetches all the BlockControllers present in the scene.
        */
        public List<BlockController> GetAllBlocks() {
            return blockControllers;
        }

        /*
            Returns the dictionary containing BlockControllers as values and nounType as keys.
        */
        public Dictionary<NounType, List<BlockController>> GetNounBlocks() {
            return nounBlocks;
        }

        /*
            Updates rules based on position of rule blocks.
        */
        private void UpdateRules() {
            ruleService.UpdateRules();
        }

        /*
            Starts Movement of Blocks based on Input direction.
        */
        private void StartMovement(Vector2 direction) {
            nounService.StartMovement(direction);
            UpdateBlockPositionMatrix();
        }

        /*
            Adds Rule NOUN IS PROPERTY, calls NounService to add property to each block controlller of specified nounType.
        */
        public void AddRule(NounType nounType, PropertyType property) {
            nounService.AddProperty(nounType, property);
        }

        /*
            Removes Rule NOUN IS PROPERTY, calls NounService to remove property to each block controlller of specified nounType.
        */
        public void RemoveRule(NounType nounType, PropertyType property) {
            nounService.RemoveProperty(nounType, property);
        }

        /*
            Returns all blocks which are not Noun blocks and not operator blocks.
            i.e. Retuns all NOUN_TEXT , PROPERTY_TEXT BlockType blocks.
        */
        public List<BlockController> GetTextBlocks() {
            List<BlockController> propertyTextBlocks = GetBlocksOfBlockType(BlockType.PROPERTY_TEXT);
            List<BlockController> nounTextBlocks = GetBlocksOfBlockType(BlockType.NOUN_TEXT);
            List<BlockController> textBlocks = new List<BlockController>();
            textBlocks.AddRange(propertyTextBlocks);
            textBlocks.AddRange(nounTextBlocks);
            return textBlocks;
        }

        /*
            Returns all IS Blocks, i.e. OPERATOR BlockType.
        */
        public List<BlockController> GetOperatorBlocks() {
            return GetBlocksOfBlockType(BlockType.OPERATOR);
        }

        /*
            Returns all BlockControllers of specified BlockType.
        */
        private List<BlockController> GetBlocksOfBlockType(BlockType blockType) {
            return blocksWithBlockType[blockType];
        }

        /*
            Returns all BlockControllers of specified NounType.
        */
        public List<BlockController> GetBlocksOfNounType(NounType nounType) {
            return nounBlocks[nounType];
        }

        /*
            Returns all the text block controllers which are Adjacent to a block.
            Returns a dictionary as only a single textBlock can be adjacent.
        */
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

        /*
            Checks if Movement is Possible based on position and direction of movement.
        */
        public bool isMovementInsideGrid(BlockController blockController, Vector2 direction) {
            if (direction == Vector2.right && blockController.transform.position.x == Constants.GRID_MAX_X) {
                return false;
            } else if (direction == Vector2.left && blockController.transform.position.x == -Constants.GRID_MAX_X) {
                return false;
            } else if (direction == Vector2.up && blockController.transform.position.y == Constants.GRID_MAX_Y) {
                return false;
            } else if (direction == Vector2.down && blockController.transform.position.y == -Constants.GRID_MAX_Y) {
                return false;
            } else {
                return true;
            }
        }

        /*
            Returns all Block Controller adjacent to a block towards a particular direction. Uses BlockPositionMatrix to quickly fetch values.
        */
        public List<BlockController> GetAdjacentBlocksInDirection(BlockController blockController, Vector2 direction) {
            Vector3 targetPosition = blockController.transform.position + new Vector3(direction.x, direction.y, 0f);
            if (blockPositionMatrix.ContainsKey(targetPosition)) {
                return blockPositionMatrix[targetPosition];
            } else {
                blockPositionMatrix[targetPosition] = new List<BlockController>();
                return blockPositionMatrix[targetPosition];
            }
        }

        /*
            Checks if the current level is complete. Uses YOU and WIN blocks and checks for an intersection.
        */
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

        /*
            Returns a dictionary containing Dominant Property and Blocks Associated with it.
        */
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

        /*
            Updates BlockPositionMatrix by filling the position keys and BlockControllers as values.
        */
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

        /*
            Updates Rule Block Colors based on rule activation. 
        */
        private void UpdateBlockColors() {
            ruleService.UpdateTextBlockColors();
        }

        /*
            Checks if current Level cannot be completed. Is case when no. of YOU blocks are zero.
        */
        private bool isCurrentLevelFailed() {
            Dictionary<PropertyType, List<BlockController>> PropertyBlocks = GetBlocksOfPropertyType();
            List<BlockController> youBlocks = PropertyBlocks[PropertyType.YOU];
            return youBlocks.Count == 0;
        }

        /*
            InvokeLevelComplete Method. Gets Called whenever Level is Complete.
        */
        public void InvokeLevelComplete() {
            UIService.Instance.OnLevelComplete();
        }
        
        /*
            InvokeLevelFailed Method. Gets Called whenever Level is Failed.
        */
        private void InvokeLevelFailed() {
            UIService.Instance.OnLevelFailed();
        }

        /*
            ExecuteTurn Coroutine. Handles Logic for a single input turn.
        */
        public IEnumerator ExecuteTurn(Vector2 dir) {
            AudioService.Instance.PlayAudio(Enums.AudioType.TURN);
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


        /*
            Checks all the conditions for which UI can be triggered.
        */
        private void CheckLevelConditions() {
            bool isLevelCompleted = isCurrentLevelComplete();
            bool isLevelFailed = isCurrentLevelFailed();
            if (isLevelCompleted) {
                InvokeLevelComplete();
            } else if (isLevelFailed) {
                InvokeLevelFailed();
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Block.Controller;
using Enums;

namespace Block.StateMachine {
    /*
        PropertySM Class. State Machine Class to handle different properties of a block.
        Handles 4 properties. Their dominance has order : YOU > STOP > PUSH > WIN > NONE. Can have multiple properties active at the same time but only one will be dominant.
    */
    public class PropertySM
    {
        public BlockController blockController;
        private NoneProperty noneProperty;
        private YouProperty youProperty;
        private PushProperty pushProperty;
        private StopProperty stopProperty;
        private WinProperty winProperty;
        private BaseProperty currentProperty = null;
        private Dictionary<PropertyType, bool> propertyStatus;
        
        /*
            Initializes all the Property Types and sets default property to NONE.
        */
        public PropertySM(BlockController blockController) {
            propertyStatus = new Dictionary<PropertyType, bool>();
            this.blockController = blockController;
            for (int i = 0; i < Constants.propertyTypes.Length; i++) {
                propertyStatus[Constants.propertyTypes[i]] = false;
            }
            noneProperty = new NoneProperty(this);
            youProperty = new YouProperty(this);
            stopProperty = new StopProperty(this);
            winProperty = new WinProperty(this);
            pushProperty = new PushProperty(this);
            SwitchState(PropertyType.NONE);
        }

        /*
            Sets Property'a Active Status to true / false. Used to keep track of all the active properties.
        */
        public void SetPropertyStatus(PropertyType property, bool isActive) {
            propertyStatus[property] = isActive;
        }

        /*
            Invokes Level Completion Method of controller. Which then calls BlockManager to trigger Level Completion.
        */
        public void InvokeLevelComplete() {
            blockController.InvokeLevelComplete();
        }

        /*
            Adds property to existing set of BLOCK properties.
        */
        public void AddProperty(PropertyType property) {
            currentProperty.AddProperty(property);
        }

        /*
            Removes property from existing set of BLOCK Properties.
        */
        public void RemoveProperty(PropertyType property) {
            currentProperty.RemoveProperty(property);
        }

        /*
            Switches Property State based on dominant Property order.
            YOU > STOP > PUSH > WIN > NONE.
        */
        public void SwitchState(PropertyType property) {
            if (currentProperty != null)
                currentProperty.OnStateExit();
            if (property == PropertyType.YOU) {
                currentProperty = youProperty;
            } else if (property == PropertyType.STOP) {
                currentProperty = stopProperty;
            } else if (property == PropertyType.PUSH) {
                currentProperty = pushProperty;
            } else if (property == PropertyType.WIN) {
                currentProperty = winProperty;
            } else if (property == PropertyType.NONE) {
                currentProperty = noneProperty;
            }
            currentProperty.OnStateEnter();
        }

        /*
            Returns the Dominant Property amongst active properties.
        */
        public PropertyType GetCurrentDominantProperty() {
            if (propertyStatus[PropertyType.YOU]) {
                return PropertyType.YOU;
            } else if (propertyStatus[PropertyType.STOP]) {
                return PropertyType.STOP;
            } else if (propertyStatus[PropertyType.PUSH]) {
                return PropertyType.PUSH;
            } else if (propertyStatus[PropertyType.WIN]) {
                return PropertyType.WIN;
            } else {
                return PropertyType.NONE;
            }
        }
    }

}

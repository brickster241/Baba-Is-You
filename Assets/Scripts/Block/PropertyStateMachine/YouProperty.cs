using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Block.StateMachine {
    /*
        YouProperty Class. Handles operations when BLOCK is in YOU state.
        Input Movements control YOU blocks, which in turn push PUSH Blocks.
        While BLOCK has YOU has its dominant property, it can move as per user input. 
    */
    public class YouProperty : BaseProperty
    {
        public YouProperty(PropertySM propertySM) : base(propertySM) {}

        /*
            OnStateEnter Method. Executed when BLOCK enters YOU state.
        */
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 5;
        }

        /*
            AddProperty Method. Adds a property while BLOCK is in YOU state.
            Also checks for Level Completion, if WIN is added.
        */
        public override void AddProperty(PropertyType property)
        {
            if (property == PropertyType.WIN) {
                propertySM.InvokeLevelComplete();
            }
            base.AddProperty(property);
            propertySM.SetPropertyStatus(property, true);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.YOU) {
                propertySM.SwitchState(dominantProperty);
            }
        }

        /*
            RemoveProperty Method. Removes an existing property while BLOCK is in YOU state.
        */
        public override void RemoveProperty(PropertyType property)
        {
            base.RemoveProperty(property);
            propertySM.SetPropertyStatus(property, false);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.YOU) {
                propertySM.SwitchState(dominantProperty);
            }
        }
    }

}

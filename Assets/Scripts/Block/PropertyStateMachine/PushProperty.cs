using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Block.StateMachine {
    /*
        PushProperty Class. Handles operations when BLOCK is in PUSH state.
        While BLOCK has PUSH has its dominant property, it can be pushed around. 
    */
    public class PushProperty : BaseProperty
    {
        public PushProperty(PropertySM propertySM) : base(propertySM) {}

        /*
            OnStateEnter Method. Executed when BLOCK enters PUSH state.
        */
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 3;
        }

        /*
            AddProperty Method. Adds a property while BLOCK is in PUSH state.
        */
        public override void AddProperty(PropertyType property)
        {
            base.AddProperty(property);
            propertySM.SetPropertyStatus(property, true);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.PUSH) {
                propertySM.SwitchState(dominantProperty);
            }
        }

        /*
            RemoveProperty Method. Removes an existing property while BLOCK is in PUSH state.
        */
        public override void RemoveProperty(PropertyType property)
        {
            base.RemoveProperty(property);
            propertySM.SetPropertyStatus(property, false);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.PUSH) {
                propertySM.SwitchState(dominantProperty);
            }
        }
    }

}

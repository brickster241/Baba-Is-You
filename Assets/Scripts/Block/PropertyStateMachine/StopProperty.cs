using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Block.StateMachine {
    /*
        StopProperty Class. Handles operations when BLOCK is in STOP state.
        While BLOCK has STOP has its dominant property, it cannot be pushed & is not pass-through. 
    */
    public class StopProperty : BaseProperty
    {
        public StopProperty(PropertySM propertySM) : base(propertySM) {}

        /*
            OnStateEnter Method. Executed when BLOCK enters STOP state.
        */
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 4;
        }

        /*
            AddProperty Method. Adds a property while BLOCK is in STOP state.
        */
        public override void AddProperty(PropertyType property)
        {
            base.AddProperty(property);
            propertySM.SetPropertyStatus(property, true);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.STOP) {
                propertySM.SwitchState(dominantProperty);
            }
        }

        /*
            RemoveProperty Method. Removes an existing property while BLOCK is in STOP state.
        */
        public override void RemoveProperty(PropertyType property)
        {
            base.RemoveProperty(property);
            propertySM.SetPropertyStatus(property, false);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.STOP) {
                propertySM.SwitchState(dominantProperty);
            }
        }
    }

}

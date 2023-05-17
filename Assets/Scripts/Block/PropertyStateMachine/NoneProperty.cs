using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Block.StateMachine {
    /*
        NoneProperty Class. This is the default state of every BLOCK. 
        While BLOCK has NONE has its dominant property, it is pass-through. 
    */
    public class NoneProperty : BaseProperty
    {
        public NoneProperty(PropertySM propertySM) : base(propertySM){}

        /*
            OnStateEnter Method. Executed when BLOCK enters NONE state.
        */
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 1;
        }

        /*
            AddProperty Method. Adds a property while BLOCK is in NONE state.
        */
        public override void AddProperty(PropertyType property)
        {
            base.AddProperty(property);
            propertySM.SetPropertyStatus(property, true);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.NONE) {
                propertySM.SwitchState(dominantProperty);
            }
        }

        /*
            RemoveProperty Method. Removes an existing property while BLOCK is in NONE state.
        */
        public override void RemoveProperty(PropertyType property)
        {
            base.RemoveProperty(property);
            propertySM.SetPropertyStatus(property, false);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.NONE) {
                propertySM.SwitchState(dominantProperty);
            }
        }
    }

}

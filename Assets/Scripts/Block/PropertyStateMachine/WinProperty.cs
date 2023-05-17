using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Block.StateMachine {
    /*
        WinProperty Class. Handles operations when BLOCK is in WIN state.
        While BLOCK has WIN has its dominant property, it is passthrough but when YOU reaches WIN, triggers level completion. 
    */
    public class WinProperty : BaseProperty
    {
        public WinProperty(PropertySM propertySM) : base(propertySM) {}

        /*
            OnStateEnter Method. Executed when BLOCK enters WIN state.
        */
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 2;
        }

        /*
            AddProperty Method. Adds a property while BLOCK is in WIN state.
        */
        public override void AddProperty(PropertyType property)
        {
            if (property == PropertyType.YOU) {
                propertySM.InvokeLevelComplete();
            }
            base.AddProperty(property);
            propertySM.SetPropertyStatus(property, true);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.WIN) {
                propertySM.SwitchState(dominantProperty);
            }
        }

        /*
            RemoveProperty Method. Removes an existing property while BLOCK is in WIN state.
        */
        public override void RemoveProperty(PropertyType property)
        {
            base.RemoveProperty(property);
            propertySM.SetPropertyStatus(property, false);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.WIN) {
                propertySM.SwitchState(dominantProperty);
            }
        }
    }

}

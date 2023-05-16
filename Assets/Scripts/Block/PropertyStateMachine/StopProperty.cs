using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Block.StateMachine {
    public class StopProperty : BaseProperty
    {
        public StopProperty(PropertySM propertySM) : base(propertySM) {}

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 4;
        }

        public override void AddProperty(PropertyType property)
        {
            base.AddProperty(property);
            propertySM.SetPropertyStatus(property, true);
            PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
            if (dominantProperty != PropertyType.STOP) {
                propertySM.SwitchState(dominantProperty);
            }
        }

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

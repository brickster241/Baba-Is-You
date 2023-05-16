using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinProperty : BaseProperty
{
    public WinProperty(PropertySM propertySM) : base(propertySM) {}

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 2;
    }

    public override void AddProperty(PropertyType property)
    {
        base.AddProperty(property);
        propertySM.SetPropertyStatus(property, true);
        PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
        if (dominantProperty != PropertyType.WIN) {
            propertySM.SwitchState(dominantProperty);
        }
    }

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

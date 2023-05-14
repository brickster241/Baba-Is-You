using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneProperty : BaseProperty
{
    public NoneProperty(PropertySM propertySM) : base(propertySM){}

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
    }

    public override void AddProperty(PropertyType property)
    {
        base.AddProperty(property);
        propertySM.SetPropertyStatus(property, true);
        PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
        if (dominantProperty != PropertyType.NONE) {
            propertySM.SwitchState(dominantProperty);
        }
    }

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

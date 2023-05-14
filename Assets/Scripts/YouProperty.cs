using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouProperty : BaseProperty
{
    public YouProperty(PropertySM propertySM) : base(propertySM) {}

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        SpriteRenderer sr = propertySM.blockController.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 5;
    }

    public override void AddProperty(PropertyType property)
    {
        if (property == PropertyType.WIN) {
            Debug.Log("GAME WIN.");
        }
        base.AddProperty(property);
        propertySM.SetPropertyStatus(property, true);
        PropertyType dominantProperty = propertySM.GetCurrentDominantProperty();
        if (dominantProperty != PropertyType.YOU) {
            propertySM.SwitchState(dominantProperty);
        }
    }

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

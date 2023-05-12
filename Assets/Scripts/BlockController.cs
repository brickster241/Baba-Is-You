using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public NounType nounType;
    public PropertySM propertySM;

    public void AddProperty(PropertyType property) {
        propertySM.AddProperty(property);
    }

    public void RemoveProperty(PropertyType property) {
        propertySM.RemoveProperty(property);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProperty
{
    protected PropertySM propertySM;

    public BaseProperty(PropertySM propertySM) {
        this.propertySM = propertySM;
    }

    public virtual void OnStateEnter() {}

    public virtual void OnStateUpdate() {}

    public virtual void OnStateExit() {}

    public virtual void AddProperty(PropertyType property) {}

    public virtual void RemoveProperty(PropertyType property) {}
}

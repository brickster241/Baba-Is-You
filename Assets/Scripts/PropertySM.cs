using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertySM
{
    public BlockController blockController;
    private NoneProperty noneProperty;
    private YouProperty youProperty;
    private PushProperty pushProperty;
    private StopProperty stopProperty;
    private WinProperty winProperty;
    private BaseProperty currentProperty = null;
    private Dictionary<PropertyType, bool> propertyStatus;
    
    
    public PropertySM(BlockController blockController) {
        propertyStatus = new Dictionary<PropertyType, bool>();
        this.blockController = blockController;
        for (int i = 0; i < Constants.propertyTypes.Length; i++) {
            propertyStatus[Constants.propertyTypes[i]] = false;
        }
        noneProperty = new NoneProperty(this);
        youProperty = new YouProperty(this);
        stopProperty = new StopProperty(this);
        winProperty = new WinProperty(this);
        pushProperty = new PushProperty(this);
        SwitchState(PropertyType.NONE);
    }

    public void SetPropertyStatus(PropertyType property, bool isActive) {
        propertyStatus[property] = isActive;
    }

    public void AddProperty(PropertyType property) {
        currentProperty.AddProperty(property);
    }

    public void RemoveProperty(PropertyType property) {
        currentProperty.RemoveProperty(property);
    }

    public void SwitchState(PropertyType property) {
        if (currentProperty != null)
            currentProperty.OnStateExit();
        if (property == PropertyType.YOU) {
            currentProperty = youProperty;
        } else if (property == PropertyType.STOP) {
            currentProperty = stopProperty;
        } else if (property == PropertyType.PUSH) {
            currentProperty = pushProperty;
        } else if (property == PropertyType.WIN) {
            currentProperty = winProperty;
        } else if (property == PropertyType.NONE) {
            currentProperty = noneProperty;
        }
        currentProperty.OnStateEnter();
    }

    public PropertyType GetCurrentDominantProperty() {
        if (propertyStatus[PropertyType.YOU]) {
            return PropertyType.YOU;
        } else if (propertyStatus[PropertyType.STOP]) {
            return PropertyType.STOP;
        } else if (propertyStatus[PropertyType.PUSH]) {
            return PropertyType.PUSH;
        } else if (propertyStatus[PropertyType.WIN]) {
            return PropertyType.WIN;
        } else {
            return PropertyType.NONE;
        }
    }
}

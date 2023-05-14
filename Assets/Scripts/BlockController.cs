using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    public NounType nounType;
    public BlockType blockType;
    public NounType nounText;
    public PropertyType propertyText;
    public PropertySM propertySM;
    public bool isBlockMovementPossible;
    public bool isMovementCalculated;

    private void Awake() {
        propertySM = new PropertySM(this);
    }

    public void AddProperty(PropertyType property) {
        propertySM.AddProperty(property);
    }

    public void RemoveProperty(PropertyType property) {
        propertySM.RemoveProperty(property);
    }

    public void Move(Vector2 direction) {
        transform.DOMove(transform.position + new Vector3(direction.x, direction.y, 0f), 0.25f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockScriptableObject", menuName = "Scriptable-Objects/BlockScriptableObject")]
public class BlockScriptableObject : ScriptableObject {
    public BlockType blockType;
    public NounType nounType;
    public PropertyType propertyType;
    public NounType nounTextType;
    public PropertyType propertyTextType;
    public Sprite blockSprite;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Scriptables {
    /*
        BlockScriptableObject to define Block Configurations.
    */
    [CreateAssetMenu(fileName = "BlockScriptableObject", menuName = "Scriptable-Objects/BlockScriptableObject")]
    public class BlockScriptableObject : ScriptableObject {
        public BlockType blockType;
        public NounType nounType;
        public PropertyType propertyType;
        public NounType nounTextType;
        public PropertyType propertyTextType;
        public Sprite blockSprite;
        public string stateName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables {
    /*
        Nested Scriptable Object for maintaining multiple Block Configurations and importing them together.
    */
    [CreateAssetMenu(fileName = "BlockScriptableObjectList", menuName = "Scriptable-Objects/BlockScriptableObjectList")]
    public class BlockScriptableObjectList : ScriptableObject {
        public BlockScriptableObject[] blockConfigs;
    }
}

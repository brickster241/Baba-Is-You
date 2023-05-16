using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables {
    [CreateAssetMenu(fileName = "BlockScriptableObjectList", menuName = "Scriptable-Objects/BlockScriptableObjectList")]
    public class BlockScriptableObjectList : ScriptableObject {
        public BlockScriptableObject[] blockConfigs;
    }
}

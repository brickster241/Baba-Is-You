using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generics {

    // Generic Class for MonoBehaviour Singleton - Will be used by Services.
    public class GenericMonoSingleton<T> : MonoBehaviour where T : GenericMonoSingleton<T>
    {
        protected static T instance;
        public static T Instance {get {return instance;}}

        protected virtual void Awake()
        {
            if(instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = (T)this;
            }
        }
    }

}
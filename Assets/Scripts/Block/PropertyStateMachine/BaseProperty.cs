using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Block.StateMachine {
    /*
        BaseProperty Class. Acts as a base class for creating properties. Every Property must inherit this class.
        Sets a reference to StateMachine which is accessible by all property Classes.
    */
    public class BaseProperty
    {
        protected PropertySM propertySM;

        public BaseProperty(PropertySM propertySM) {
            this.propertySM = propertySM;
        }

        /*
            OnStateEnter Method. Is executed whenever BLOCK enters this state.
        */
        public virtual void OnStateEnter() {}

        /*
            OnStateUpdate Method. Is executed evry frame while BLOCK stays this state.
        */
        public virtual void OnStateUpdate() {}

        /*
            OnStateExit Method. Is executed whenever BLOCK exits this state.
        */
        public virtual void OnStateExit() {}

        /*
            Adds new property while the BLOCK is currently in this state.
        */
        public virtual void AddProperty(PropertyType property) {}

        /*
            Removes existing property while the BLOCK is curretly in this state.
        */
        public virtual void RemoveProperty(PropertyType property) {}
    }

}

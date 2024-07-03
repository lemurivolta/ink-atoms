using System;
using UnityAtoms.BaseAtoms;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    [Serializable]
    public class FloatVariableObserver : SimpleTypeVariableObserver<float, FloatVariable, FloatEvent>
    {
        internal override float Cast(object o)
        {
            // allow for an int to be assigned to a float (not the contrary though) 
            return o is int i ? i : base.Cast(o);
        }

        protected override bool IsSameValue(object inkValue, float atomValue)
        {
            // if the value is an int, convert it first to a float
            return base.IsSameValue(inkValue is int i ? (float)i : inkValue, atomValue);
        }
    }
}
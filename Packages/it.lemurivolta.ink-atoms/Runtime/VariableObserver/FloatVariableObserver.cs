using System;
using UnityAtoms.BaseAtoms;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    [Serializable]
    public class FloatVariableObserver : SimpleTypeVariableObserver<float, FloatVariable>
    {
        internal override float Cast(object o)
        {
            // allow for an int to be assigned to a float (not the contrary though) 
            if (o is int i) return i;

            return base.Cast(o);
        }
    }
}
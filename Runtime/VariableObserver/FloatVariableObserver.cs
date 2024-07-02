using System;
using UnityAtoms.BaseAtoms;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    [Serializable]
    public class FloatVariableObserver : SimpleTypeVariableObserver<float, FloatVariable, FloatEvent>
    {
        internal override float Cast(object o) =>
            // allow for an int to be assigned to a float (not the contrary though) 
            o is int i ? i : base.Cast(o);

        protected override bool IsSameValue(object inkValue, float atomValue) =>
            inkValue switch
            {
                // the ink variable is a float: base comparison is ok
                float => base.IsSameValue(inkValue, atomValue),
                // the ink variable is an int: first convert to float
                int i => base.IsSameValue((float)i, atomValue),
                // neither: they are surely different
                _ => false
            };
    }
}
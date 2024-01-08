using UnityEngine;
using System;

namespace DC.Tools
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DrawIfAttribute : PropertyAttribute
    {
        public string comparedPropertyName
        {
            get;
            private set;
        }
        public object comparedValue
        {
            get;
            private set;
        }
        public DisablingType disablingType
        {
            get;
            private set;
        }

        public DrawIfAttribute(string comparedPropertyName, object comparedValue, DisablingType disablingType = DisablingType.DontDraw)
        {
            this.comparedPropertyName = comparedPropertyName;
            this.comparedValue = comparedValue;
            this.disablingType = disablingType;
        }

        public DrawIfAttribute(string comparedPropertyName, DisablingType disablingType = DisablingType.DontDraw, params object[] comparedValues)
        {
            this.comparedPropertyName = comparedPropertyName;
            this.comparedValue = comparedValues;
            this.disablingType = disablingType;
        }
    }

    public enum DisablingType
    {
        ReadOnly,
        DontDraw
    }

}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DC.Tools;

namespace DC.Tools.Editor
{
    [CustomPropertyDrawer(typeof(DrawIfAttribute))]
    public class DrawIfPropertyDrawer : PropertyDrawer
    {

        private int debugCount = 0;

        //Reference to the attribute on the property.
        DrawIfAttribute drawIf;

        //Field that is being compared.
        SerializedProperty comparedField;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShowMe(property) && drawIf.disablingType == DisablingType.DontDraw)
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
        }

        private bool ShowMe(SerializedProperty property)
        {
            drawIf = attribute as DrawIfAttribute;
            // Replace propertyname to the value from the parameter
            string path; //= property.propertyPath.Contains(".") ? System.IO.Path.ChangeExtension(property.propertyPath, drawIf.comparedPropertyName) : drawIf.comparedPropertyName;

            if (property.propertyPath.Contains("."))
            {
                if (property.propertyPath.Contains("Array"))
                {
                    //path = drawIf.comparedPropertyName;
                    if (debugCount < 5)
                    {
                        Debug.LogError("Arrays and Lists are unsupported at this point in time!");
                        debugCount++;
                    }

                    return true;
                }
                else
                {
                    path = System.IO.Path.ChangeExtension(property.propertyPath, drawIf.comparedPropertyName);
                }
            }
            else
            {
                path = drawIf.comparedPropertyName;
            }

            //Debug.Log("Property Name: " + property.displayName + " Property Type: " + property.propertyType + " Path: " + path);

            comparedField = property.serializedObject.FindProperty(path);

            if (debugCount == 0)
            {
                //Debug.Log(comparedField.intValue & (int)drawIf.comparedValue);
                //Debug.Log((int)drawIf.comparedValue);
                //debugCount++;
            }

            if (comparedField == null)
            {
                Debug.LogError("Cannot find property with name: " + path);
                return true;
            }

            //Get the value & compare based on types
            switch (comparedField.type)
            {
                //Possible extend cases to support your own type
                case "bool":
                    return comparedField.boolValue.Equals(drawIf.comparedValue);
                case "Enum":
                    try
                    {
                        var drawIfVals = drawIf.comparedValue as object[];

                        if (drawIfVals == null)
                            return (comparedField.intValue.Equals((int)drawIf.comparedValue));

                        if (drawIfVals.Length == 0)
                            return true;

                        foreach (object val in drawIfVals)
                        {
                            if (comparedField.intValue.Equals((int)val))
                                return true;
                        }
                        return false;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e.GetType() + ": trying to cast drawIf value to enum array when no enum array was passed");
                        return true;
                    }
                    
                default:
                    Debug.LogError("Error: " + comparedField.type + " is not supported from " + path);
                    return true;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //If condition is met, draw the field.
            if (ShowMe(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            //Check if Disabling Type is 'Read Only'. If Yes, draw it Disabled
            else if (drawIf.disablingType == DisablingType.ReadOnly)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }
        }
    }
}
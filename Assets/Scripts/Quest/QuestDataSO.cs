using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestConditions
{
    public string parameterKey;
    public enum Operator
    {
        lessThan,
        lessOrEqual,
        equal,
        notEqual,
        graterOrEqual,
        graterThan
    }
    public Operator conditionOperator;
    public enum ValueType
    {
        Int,
        Float,
        String,
        Boolean,
       // Vector2,
      //  Vector3
    }
    public ValueType valueType;
    public int intValue;
    public float floatValue;
    public string stringValue;
    public bool boolValue;
    //public Vector2 vec2Value;
   // public Vector3 vec3Value;
    public object GetValue()
    {
        switch (valueType)
        {
            case ValueType.Int: return intValue;
            case ValueType.Float: return floatValue;
            case ValueType.String: return stringValue;
            case ValueType.Boolean: return boolValue;
            //case ValueType.Vector2: return vec2Value;
           // case ValueType.Vector3: return vec3Value;
        }
        return null;
    }
    public bool MeetCondition(object value)
    {
        try
        {
            switch (valueType)
            {
                case ValueType.Int: return MeetCondition((int)value);
                case ValueType.Float: return MeetCondition((float)value);
                case ValueType.String: return MeetCondition((string)value);
                case ValueType.Boolean: return MeetCondition((bool)value);
            }
        }
        catch
        {
            Debug.LogError($"Quest condition cast error '{parameterKey}'");
            return false;
        }
        return false;
    }
    bool MeetCondition(int value)
    {
        switch (conditionOperator)
        {
            case Operator.lessThan:return value < intValue;
            case Operator.lessOrEqual:return value <= intValue;
            case Operator.equal:return value == intValue;
            case Operator.notEqual: return value != intValue;
            case Operator.graterOrEqual: return value >= intValue;
            case Operator.graterThan:return value > intValue;
        }
        return false;
    }
    bool MeetCondition(float value)
    {
        switch (conditionOperator)
        {
            case Operator.lessThan: return value < floatValue;
            case Operator.lessOrEqual: return value <= floatValue;
            case Operator.equal: return value == floatValue;
            case Operator.notEqual: return value != floatValue;
            case Operator.graterOrEqual: return value >= floatValue;
            case Operator.graterThan: return value > floatValue;
        }
        return false;
    }

    bool MeetCondition(string value)
    {
        switch (conditionOperator)
        {
            case Operator.lessThan: return string.Compare(value , stringValue)==-1;
            case Operator.lessOrEqual: return string.Compare(value, stringValue) != 1;
            case Operator.equal: return value == stringValue;
            case Operator.notEqual: return value != stringValue;
            case Operator.graterOrEqual: return string.Compare(value, stringValue) != -1;
            case Operator.graterThan: return string.Compare(value, stringValue) == 1;
        }
        return false;
    }
    bool MeetCondition(bool value)
    {
        switch (conditionOperator)
        {
            case Operator.equal: return value == boolValue;
            case Operator.notEqual: return value != boolValue;
        }
        return false;
    }
}


[CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObject/Quest")]
public class QuestDataSO : ScriptableObject
{
    public List<QuestDataSO> subQuests = new List<QuestDataSO>();
    public List<QuestConditions> startConditions = new List<QuestConditions>();
    public List<QuestConditions> endConditions = new List<QuestConditions>();

}

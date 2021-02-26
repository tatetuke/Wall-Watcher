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
    public bool MeetCondition()
    {
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

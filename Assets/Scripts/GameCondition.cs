using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestConditions
{
    /// <summary>
    /// この条件についての説明文(任意)
    /// </summary>
    public string description;
    /// <summary>
    /// GameConditionのうち、すべて成り立っていればOK
    /// </summary>
    public List<GameCondition> conditions;
    /// <summary>
    /// conditionsのうち、すべて成り立っていればtrue(AND条件)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool MeetConditions()
    {
        foreach(var i in conditions)
        {
            if (!i.MeetCondition()) return false;
        }
        return true;
    }

    public void Set(QuestConditions originalConversation, QuestConditions newConversation)
    {
        /*if (originalConversation.id != newConversation.id)
        {
            foreach (var i in items)
            {
                //conversationのidが書き換えられたとき、会話リストにある選択肢も連動してidを書き換える
                var options = i.options;
                for (var j = 0; j < options.Count; j++)
                {
                    if (options[j].targetId == originalConversation.id)
                    {
                        var c = options[j];
                        c.targetId = newConversation.id;
                        options[j] = c;
                    }
                }
            }
        }
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].id == originalConversation.id)
            {
                items[i] = newConversation;
                break;
            }
        }*/
    }
}

[System.Serializable]
public class GameCondition
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
    /// <summary>
    /// 比較対象となる値を返す
    /// </summary>
    /// <returns></returns>
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
        var property = GamePropertyManager.Instance;

        try
        {
            switch (valueType)
            {
                case ValueType.Int: return MeetCondition(property.GetProperty<int>(parameterKey));
                case ValueType.Float: return MeetCondition(property.GetProperty<float>(parameterKey));
                case ValueType.String: return MeetCondition(property.GetProperty<string>(parameterKey));
                case ValueType.Boolean: return MeetCondition(property.GetProperty<bool>(parameterKey));
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
            case Operator.lessThan: return value < intValue;
            case Operator.lessOrEqual: return value <= intValue;
            case Operator.equal: return value == intValue;
            case Operator.notEqual: return value != intValue;
            case Operator.graterOrEqual: return value >= intValue;
            case Operator.graterThan: return value > intValue;
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
            case Operator.lessThan: return string.Compare(value, stringValue) == -1;
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
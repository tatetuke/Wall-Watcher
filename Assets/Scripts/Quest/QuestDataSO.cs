using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObject/Quest")]
public class QuestDataSO : ScriptableObject
{
    public List<QuestDataSO> subQuests = new List<QuestDataSO>();
    public List<QuestConditions> startConditions = new List<QuestConditions>();
    public List<QuestConditions> endConditions = new List<QuestConditions>();

}

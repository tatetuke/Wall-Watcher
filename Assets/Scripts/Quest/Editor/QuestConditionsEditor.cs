using UnityEditor;
using UnityEngine;

// reorderablelistにしたかったけど途中
/*[CustomPropertyDrawer(typeof(QuestConditions))]
public class QuestDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //元は 1 つのプロパティーであることを示すために PropertyScope で囲む
        // using (new EditorGUI.PropertyScope(position, label, property))
        {
            //ラベル領域の幅を調整
            //    EditorGUIUtility.labelWidth = 100;

            //position.height = EditorGUIUtility.singleLineHeight;

            //各プロパティーの Rect を求める
            Rect rect1 = new Rect(position)
            {
                y = position.y,
                height = EditorGUIUtility.singleLineHeight,
                width = position.width / 4 * 3
            };

            Rect rect2 = new Rect(rect1)
            {
                y = rect1.y,
                x = rect1.width + 40,
                width = position.width - rect1.width
            };

            Rect rect3 = new Rect(rect1)
            {
                y = rect2.y + EditorGUIUtility.singleLineHeight + 5,
                x = position.x,
                width = position.width / 5
            };

            Rect rect4 = new Rect(rect3)
            {
                y = rect3.y,
                x = rect3.width + 40,
                width = position.width - rect3.width
            };

            //各プロパティーの SerializedProperty を求める
            var parameterProperty = property.FindPropertyRelative("description");
            var script = property;
            var conditionProperty = property.FindPropertyRelative("conditions");
            //各プロパティーの GUI を描画
            parameterProperty.stringValue = EditorGUI.TextField(rect1, parameterProperty.stringValue);
            if (GUI.Button(rect4, "Edit", EditorStyles.miniButton))
            {
                ConditionWizard.Edit(property.serializedObject, parameterProperty,conditionProperty);
            }
        }
    }
    //戻り値として返した値が GUI の高さとして使用されるようになる
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = base.GetPropertyHeight(property, label);
        return height * 4;
    }
}*/
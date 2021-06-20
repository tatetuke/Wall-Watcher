using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameCondition))]
public class CharacterDrawer : PropertyDrawer
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
                height= EditorGUIUtility.singleLineHeight,
                width=position.width/4*3
            };

            Rect rect2 = new Rect(rect1)
            {
                y = rect1.y,
                x=rect1.width+ 40,
                width =position.width- rect1.width
            };

            Rect rect3 = new Rect(rect1)
            {
                y = rect2.y + EditorGUIUtility.singleLineHeight + 5,
                x= position.x,
                width= position.width/5
            };

            Rect rect4 = new Rect(rect3)
            {
                y = rect3.y,
                x =rect3.width+40,
                width = position.width-rect3.width
            };

            //各プロパティーの SerializedProperty を求める
            var parameterProperty = property.FindPropertyRelative("parameterKey");
            var operatorProperty = property.FindPropertyRelative("conditionOperator");
            var valueTypeProperty = property.FindPropertyRelative("valueType");

            //各プロパティーの GUI を描画
            parameterProperty.stringValue = EditorGUI.TextField(rect1, parameterProperty.stringValue);
            var j = (GameCondition.ValueType)EditorGUI.EnumPopup(rect2, (GameCondition.ValueType)valueTypeProperty.enumValueIndex);
            valueTypeProperty.enumValueIndex = (int)j;
            var i =EditorGUI.Popup(rect3,  operatorProperty.enumValueIndex,new string[] { "<","<=","==","!=",">=",">"});
            operatorProperty.enumValueIndex = (int)i;
            var typeO = (GameCondition.ValueType)valueTypeProperty.enumValueIndex;
            switch (typeO)
            {
                case GameCondition.ValueType.Int:
                    var intProperty = property.FindPropertyRelative("intValue");
                    intProperty.intValue = EditorGUI.IntField(rect4, intProperty.intValue);
                    break;
                case GameCondition.ValueType.Float:
                    var floatProperty = property.FindPropertyRelative("floatValue");
                    floatProperty.floatValue = EditorGUI.FloatField(rect4, floatProperty.floatValue);
                    break;
                case GameCondition.ValueType.Boolean:
                    var boolProperty = property.FindPropertyRelative("boolValue");
                    boolProperty.boolValue = EditorGUI.Toggle(rect4, boolProperty.boolValue);
                    break;
               /* case QuestConditions.ValueType.Vector2:
                    var vec2Property = property.FindPropertyRelative("vec2Value");
                    vec2Property.vector2Value = EditorGUI.Vector2Field(rect4,"value", vec2Property.vector2Value);
                    break;
                case QuestConditions.ValueType.Vector3:
                    var vec3Property = property.FindPropertyRelative("vec3Value");
                    vec3Property.vector3Value = EditorGUI.Vector3Field(rect4, "value", vec3Property.vector3Value);
                    break;*/
                case GameCondition.ValueType.String:
                    var stringProperty = property.FindPropertyRelative("stringValue");
                    stringProperty.stringValue = EditorGUI.TextField(rect4, stringProperty.stringValue);
                    break;
            }
        }
    }
    //戻り値として返した値が GUI の高さとして使用されるようになる
    public override float GetPropertyHeight(SerializedProperty property,GUIContent label)
    {
        var height = base.GetPropertyHeight(property, label);
        return height*4;
    }
}
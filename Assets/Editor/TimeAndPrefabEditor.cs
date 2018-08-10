using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CanEditMultipleObjects]
[CustomPropertyDrawer (typeof (TimeAndPrefabAttribute))]
public class TimeAndPrefabAttribute : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		using (new EditorGUI.PropertyScope (position, label, property))
		{
			position.height = EditorGUIUtility.singleLineHeight;
			var ele1 = property.FindPropertyRelative ("Time");
			var ele2 = property.FindPropertyRelative ("Prefab");
			var ele3 = property.FindPropertyRelative ("Pos");

			var ele1Rect = new Rect (position)
			{
				height = position.height
			};
			var ele2Rect = new Rect (position)
			{
				height = position.height,
					y = ele1Rect.y + EditorGUIUtility.singleLineHeight + 2
			};
			var ele3Rect = new Rect (position)
			{
				height = position.height * 2,
					y = ele2Rect.y + EditorGUIUtility.singleLineHeight + 2
			};

			EditorGUI.PropertyField (ele1Rect, ele1);
			EditorGUI.PropertyField (ele2Rect, ele2);
			EditorGUI.PropertyField (ele3Rect, ele3);
		}
	}
}
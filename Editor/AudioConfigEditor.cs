using UnityEditor;

namespace AudioExpress
{
	[CustomEditor(typeof(AudioClip))]
	public class AudioConfigEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty isUsingClips = serializedObject.FindProperty("isUsingClips");
			EditorGUILayout.PropertyField(isUsingClips);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("mixerGroup"));

			if (isUsingClips.boolValue) EditorGUILayout.PropertyField(serializedObject.FindProperty("clips"));
			else EditorGUILayout.PropertyField(serializedObject.FindProperty("clip"));


			EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("pitchVariation"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
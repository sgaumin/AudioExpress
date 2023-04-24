using UnityEditor;
using UnityEngine;

namespace AudioExpress
{
	[CustomPropertyDrawer(typeof(AudioClip))]
	internal class AudioClipPropertyDrawer : PropertyDrawer
	{
		private const float LABEL_WIDTH = 80f;
		private const float PLAY_BUTTON_WIDTH = 30f;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			AudioClip context = (AudioClip)property.objectReferenceValue;

			Rect labelRect = new Rect(position.x, position.y, LABEL_WIDTH, position.height);
			Rect propertyRect = new Rect(labelRect.x + labelRect.width, position.y, position.width - LABEL_WIDTH - PLAY_BUTTON_WIDTH, position.height);
			Rect buttonRect = new Rect(propertyRect.x + propertyRect.width, position.y, PLAY_BUTTON_WIDTH, position.height);

			EditorGUI.PrefixLabel(labelRect, label);
			EditorGUI.PropertyField(propertyRect, property, GUIContent.none);

			// We are disabling playing button if no Audio Clip reference has been assigned
			EditorGUI.BeginDisabledGroup(context == null);

			if (AudioPool.IsPlaying(context))
			{
				if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("d_PauseButton@2x")))
				{
					context.Stop();
				}
			}
			else
			{
				if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("d_PlayButton@2x")))
				{
					// We are stopping all audio units, only when the game is not running
					if (!Application.isPlaying) AudioPool.StopAll();

					context.Play();
				}
			}

			EditorGUI.EndDisabledGroup();

			EditorGUI.EndProperty();
		}
	}
}
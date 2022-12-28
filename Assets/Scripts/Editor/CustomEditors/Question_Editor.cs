using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(Question))]
    public class QuestionEditor : UnityEditor.Editor {

        #region Variables

        #region Serialized Properties

        SerializedProperty _questionInfoProp;
        SerializedProperty  _answersProp;
        SerializedProperty  _useTimerProp;
        SerializedProperty  _timerProp;
        SerializedProperty  _answerTypeProp;
        SerializedProperty  _addScoreProp;

        SerializedProperty  ArraySizeProp
        {
            get
            {
                return _answersProp.FindPropertyRelative("Array.size");
            }
        }
        #endregion

        private bool _showParameters;

        #endregion

        #region Default Unity methods

        void OnEnable ()
        {
            #region Fetch Properties
            _questionInfoProp    = serializedObject.FindProperty("_info");
            _answersProp         = serializedObject.FindProperty("_answers");
            _useTimerProp        = serializedObject.FindProperty("_useTimer");
            _timerProp           = serializedObject.FindProperty("_timer");
            _answerTypeProp      = serializedObject.FindProperty("_answerType");
            _addScoreProp        = serializedObject.FindProperty("_addScore");
            #endregion

            #region Get Values
            _showParameters = EditorPrefs.GetBool("Question_showParameters_State");
            #endregion
        }

        public override void OnInspectorGUI ()
        {
            GUILayout.Label("Question", EditorStyles.miniLabel);
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea)
            {
                fontSize = 15,
                fixedHeight = 30,
                alignment = TextAnchor.MiddleLeft
            };
            _questionInfoProp.stringValue = EditorGUILayout.TextArea(_questionInfoProp.stringValue, textAreaStyle);
            GUILayout.Space(7.5f);

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontSize = 10
            };
            EditorGUI.BeginChangeCheck();
            _showParameters = EditorGUILayout.Foldout(_showParameters, "Parameters", foldoutStyle);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool("Question_showParameters_State", _showParameters);
            }
            if (_showParameters)
            {
                EditorGUILayout.PropertyField(_useTimerProp, new GUIContent("Use Timer", "Should this question have a timer?"));
                if (_useTimerProp.boolValue)
                {
                    _timerProp.intValue = EditorGUILayout.IntSlider(new GUIContent("Time"), _timerProp.intValue, 1, 120);
                }
                GUILayout.Space(2);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_answerTypeProp, new GUIContent("Answer Type", "Specify this question answer type."));
                if (EditorGUI.EndChangeCheck())
                {
                    if (_answerTypeProp.enumValueIndex == (int)Question.AnswerType.Single)
                    {
                        if (GetCorrectAnswersCount() > 1)
                        {
                            UncheckCorrectAnswers();
                        }
                    }
                }
                _addScoreProp.intValue = EditorGUILayout.IntSlider(new GUIContent("Add Score"), _addScoreProp.intValue, 0, 100);
            }
            GUILayout.Space(7.5f);
            GUILayout.Label("Answers", EditorStyles.miniLabel);
            DrawAnswers();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        void DrawAnswers ()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.PropertyField(ArraySizeProp);
            GUILayout.Space(5);

            EditorGUI.indentLevel++;
            for (int i = 0; i < ArraySizeProp.intValue; i++)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_answersProp.GetArrayElementAtIndex(i));
                if (EditorGUI.EndChangeCheck())
                {
                    if (_answerTypeProp.enumValueIndex == (int)Question.AnswerType.Single)
                    {
                        SerializedProperty isCorrectProp = _answersProp.GetArrayElementAtIndex(i).FindPropertyRelative("_isCorrect");

                        if (isCorrectProp.boolValue)
                        {
                            UncheckCorrectAnswers();
                            _answersProp.GetArrayElementAtIndex(i).FindPropertyRelative("_isCorrect").boolValue = true;

                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
                GUILayout.Space(5);
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        void UncheckCorrectAnswers ()
        {
            for (int i = 0; i < ArraySizeProp.intValue; i++)
            {
                _answersProp.GetArrayElementAtIndex(i).FindPropertyRelative("_isCorrect").boolValue = false;
            }
        }

        int GetCorrectAnswersCount ()
        {
            int count = 0;
            for (int i = 0; i < ArraySizeProp.intValue; i++)
            {
                if (_answersProp.GetArrayElementAtIndex(i).FindPropertyRelative("_isCorrect").boolValue)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
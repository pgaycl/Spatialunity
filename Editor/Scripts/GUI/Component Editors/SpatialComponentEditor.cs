using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpatialSys.UnitySDK
{
    [CustomEditor(typeof(SpatialComponentBase), true), CanEditMultipleObjects]
    public class SpatialComponentDefaultEditor : SpatialComponentEditorBase { }
    public abstract class SpatialComponentEditorBase : UnityEditor.Editor
    {
        private bool _initialized;
        //used to hide the script field
        private static readonly string[] _excludedProperties = new string[] { "m_Script" };
        private Texture2D _backgroundTexture;
        private Texture2D _buttonTexture;
        private Texture2D _warningTexture;
        private Texture2D _iconTexture;
        private Texture2D _docsLinkTexture;

        private GUIStyle _warningStyle;
        private GUIContent _warningContent;
        private GUIStyle _areaStyle;
        private GUIStyle _logoStyle;
        private GUIStyle _titleStyle;
        private GUIStyle _subTitleStyle;
        private GUIStyle _helpButtonStyle;

        private void InitializeIfNecessary(SpatialComponentBase target)
        {
            if (_initialized)
            {
                return;
            }
            GUIContent c = EditorGUIUtility.ObjectContent(target, target.GetType());
            if (c.image == null)
            {
                _iconTexture = Resources.Load("spatialLogo512") as Texture2D;
            }
            else
            {
                _iconTexture = (Texture2D)c.image;
            }

            _backgroundTexture = Resources.Load("TooltipBackground") as Texture2D;
            _buttonTexture = Resources.Load("ButtonBackground") as Texture2D;
            _warningTexture = Resources.Load("WarningBackground") as Texture2D;
            _docsLinkTexture = Resources.Load("icon_docsLink") as Texture2D;

            _warningStyle = new GUIStyle() {
                border = new RectOffset(8, 8, 8, 8),
                padding = new RectOffset(8, 8, 8, 8),
                fontStyle = FontStyle.Bold,
                fontSize = 18,
                wordWrap = true,
                alignment = TextAnchor.MiddleLeft,
            };
            _warningStyle.normal.background = _warningTexture;
            _warningStyle.active.background = _warningTexture;
            _warningStyle.normal.textColor = new Color(1, .66f, 0f);
            _warningStyle.active.textColor = new Color(1, .66f, 0f);

            _warningContent = new GUIContent(" Experimental Feature", Resources.Load("WarningLink") as Texture2D);

            _areaStyle = new GUIStyle() {
                border = new RectOffset(8, 8, 8, 8),
                padding = new RectOffset(8, 8, 8, 8),
            };
            _areaStyle.normal.background = _backgroundTexture;
            _areaStyle.normal.textColor = Color.white;

            _logoStyle = new GUIStyle() {
                fixedHeight = 48,
                fixedWidth = 48,
                padding = new RectOffset(0, 0, 0, 0),
                border = new RectOffset(0, 0, 0, 0),
                contentOffset = new Vector2(0, -3),
            };

            _titleStyle = new GUIStyle() {
                fontStyle = FontStyle.Bold,
                fontSize = 28,
                wordWrap = true,
                contentOffset = new Vector2(0, 3),
            };
            _titleStyle.normal.textColor = Color.white;

            _subTitleStyle = new GUIStyle() {
                fontSize = 12,
                wordWrap = true,
                richText = true,
            };
            _subTitleStyle.normal.textColor = new Color(1, 1, 1, .75f);

            _helpButtonStyle = new GUIStyle() {
                border = new RectOffset(8, 8, 8, 8),
                padding = new RectOffset(4, 4, 4, 4),
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 14,
                fixedWidth = 48,
            };
            _helpButtonStyle.active.background = _buttonTexture;
            _helpButtonStyle.normal.background = _buttonTexture;
        }

        public override void OnInspectorGUI()
        {
            var editorTarget = target as SpatialComponentBase;
            InitializeIfNecessary(editorTarget);
            serializedObject.Update();

            if (editorTarget.isExperimental)
            {
                if (GUILayout.Button("Experimental Feature", _warningStyle))
                {
                    //open a docs link when we have one.
                }
            }
            GUILayout.Space(8);
            GUILayout.BeginVertical(_areaStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Box(_iconTexture, _logoStyle);
            GUILayout.Space(4);
            GUILayout.Label(editorTarget.prettyName, _titleStyle);
            if (!string.IsNullOrEmpty(editorTarget.documentationURL))
            {
                GUILayout.Space(4);
                if (GUILayout.Button(_docsLinkTexture, _helpButtonStyle))
                {
                    Application.OpenURL(editorTarget.documentationURL);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(editorTarget.tooltip, _subTitleStyle);
            GUILayout.EndVertical();

            GUILayout.Space(8);

            DrawPropertiesExcluding(serializedObject, _excludedProperties);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

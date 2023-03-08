using UnityEngine;
using UnityEditor;

namespace SpatialSys.UnitySDK.Editor
{
    public class SpatialGUIUtility
    {
        private const string TEXTURE_PATH = "Packages/io.spatial.unitysdk/Editor/Textures";

        public static Texture2D LoadGUITexture(string name, bool hasLightVariant = false)
        {
            if (!EditorGUIUtility.isProSkin && hasLightVariant)
                name = name.Insert(name.LastIndexOf('.'), "-Light");

            string path = System.IO.Path.Combine(TEXTURE_PATH, name);
            Texture2D tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
            return tex;
        }

        public static Sprite LoadSprite(string name)
        {
            string path = System.IO.Path.Combine(TEXTURE_PATH, name);
            return AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        }

        //--------------------------------------------------------------------------------------------------------------
        // Warning / Error Sections
        //--------------------------------------------------------------------------------------------------------------

        public static class HelpBoxStyles
        {
            public static GUIStyle warningBox;
            public static GUIStyle warningText;
            public static GUIStyle warningSubtext;
            public static GUIStyle errorBox;
            public static GUIStyle errorText;
            public static GUIStyle errorSubtext;
            public static GUIStyle infoBox;
            public static GUIStyle infoText;
            public static GUIStyle infoSubtext;

            static HelpBoxStyles()
            {
                infoBox = new GUIStyle() {
                    border = new RectOffset(8, 8, 8, 8),
                    padding = new RectOffset(8, 8, 8, 8),
                    margin = new RectOffset(0, 0, 4, 4),
                };
                infoBox.normal.background = LoadGUITexture(EditorGUIUtility.isProSkin ? "GUI/InfoBackground.png" : "GUI/InfoBackground_lightmode.png");
                infoText = new GUIStyle();
                infoText.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f) : new Color(0f, 0f, 0f);
                infoText.fontStyle = FontStyle.Bold;
                infoText.wordWrap = true;
                infoSubtext = new GUIStyle();
                infoSubtext.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f) : new Color(0f, 0f, 0f);
                infoSubtext.wordWrap = true;
                infoSubtext.fontStyle = FontStyle.Normal;
                infoSubtext.fontSize = 11;

                warningBox = new GUIStyle() {
                    border = new RectOffset(8, 8, 8, 8),
                    padding = new RectOffset(8, 8, 8, 8),
                    margin = new RectOffset(0, 0, 4, 4),
                };
                warningBox.normal.background = LoadGUITexture("GUI/WarningBackground.png");
                warningText = new GUIStyle();
                warningText.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1f, .9f, .5f) : new Color(.4f, .32f, 0f);
                warningText.fontStyle = FontStyle.Bold;
                warningText.wordWrap = true;
                warningSubtext = new GUIStyle();
                warningSubtext.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1f, .9f, .5f) : new Color(.4f, .32f, 0f);
                warningSubtext.fontStyle = FontStyle.Normal;
                warningSubtext.wordWrap = true;
                warningSubtext.fontSize = 11;

                errorBox = new GUIStyle() {
                    border = new RectOffset(8, 8, 8, 8),
                    padding = new RectOffset(8, 8, 8, 8),
                    margin = new RectOffset(0, 0, 4, 4),
                };
                errorBox.normal.background = LoadGUITexture("GUI/ErrorBackground.png");
                errorText = new GUIStyle();
                errorText.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1f, .26f, .26f) : new Color(.4f, .0f, 0f);
                errorText.fontStyle = FontStyle.Bold;
                errorText.wordWrap = true;
                errorSubtext = new GUIStyle();
                errorSubtext.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1f, .26f, .26f) : new Color(.4f, .0f, 0f);
                errorSubtext.fontStyle = FontStyle.Normal;
                errorSubtext.wordWrap = true;
                errorSubtext.fontSize = 11;
            }
        }

        public static void HelpBox(string title, HelpSectionType type = HelpSectionType.Info)
        {
            HelpBox(title, null, type);
        }
        public static void HelpBox(string title, string subtitle, HelpSectionType type = HelpSectionType.Info)
        {
            GUIStyle boxSyle;
            GUIStyle titeleStyle;
            GUIStyle subtitleStyle;

            switch (type)
            {
                case HelpSectionType.Info:
                    boxSyle = HelpBoxStyles.infoBox;
                    titeleStyle = HelpBoxStyles.infoText;
                    subtitleStyle = HelpBoxStyles.infoSubtext;
                    break;
                case HelpSectionType.Warning:
                    boxSyle = HelpBoxStyles.warningBox;
                    titeleStyle = HelpBoxStyles.warningText;
                    subtitleStyle = HelpBoxStyles.warningSubtext;
                    break;
                case HelpSectionType.Error:
                    boxSyle = HelpBoxStyles.errorBox;
                    titeleStyle = HelpBoxStyles.errorText;
                    subtitleStyle = HelpBoxStyles.errorSubtext;
                    break;
                default:
                    boxSyle = HelpBoxStyles.infoBox;
                    titeleStyle = HelpBoxStyles.infoText;
                    subtitleStyle = HelpBoxStyles.infoSubtext;
                    break;
            }

            GUILayout.BeginVertical(boxSyle);
            {
                if (!string.IsNullOrEmpty(title))
                {
                    GUILayout.Label(title, titeleStyle);
                }
                if (!string.IsNullOrEmpty(subtitle))
                {
                    GUILayout.Label(subtitle, subtitleStyle);
                }
            }
            GUILayout.EndVertical();
        }
        public enum HelpSectionType
        {
            Info,
            Warning,
            Error
        }
    }
}

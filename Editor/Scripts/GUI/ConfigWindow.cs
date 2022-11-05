using UnityEngine;
using UnityEditor;

namespace SpatialSys.UnitySDK.Editor
{
    public class ConfigWindow : EditorWindow
    {
        private static readonly string ACCOUNT_PAGE_URL = $"https://{SpatialAPI.SPATIAL_ORIGIN}/account";
        private const string WINDOW_PREFERENCES_PREFS_KEY = "SpatialSDK_ConfigWindowPrefs";

        public enum TabType
        {
            GettingStarted,
            Authentication,
            Configuration
        }
        private static readonly string[] _tabNames = new string[] {
            "Getting Started",
            "Authentication",
            "Configuration"
        };

        [System.Serializable]
        public class WindowPreferences
        {
            public TabType selectedTab = TabType.GettingStarted;
        }

        private bool _initialized = false;
        private GUIStyle _headerTextStyle;
        private string _token;
        private WindowPreferences _preferences;
        private EditorConfig _config;
        private UnityEditor.Editor _configEditor;

        public static ConfigWindow Open()
        {
            ConfigWindow window = GetWindow<ConfigWindow>("Spatial SDK Configuration");
            window.Show();
            return window;
        }

        public static void Open(TabType startingTab)
        {
            ConfigWindow window = Open();
            window.InitializeIfNecessary();
            window._preferences.selectedTab = startingTab;
        }

        private void OnDisable()
        {
            if (_configEditor != null)
                UnityEngine.Object.DestroyImmediate(_configEditor);

            if (_config != null)
                AssetDatabase.SaveAssetIfDirty(_config);

            SaveWindowPreferences();
        }

        private void OnGUI()
        {
            InitializeIfNecessary();

            DrawTabsToolbar();

            switch (_preferences.selectedTab)
            {
                case TabType.GettingStarted:
                    DrawGettingStarted();
                    break;
                case TabType.Authentication:
                    DrawAuthentication();
                    break;
                case TabType.Configuration:
                    DrawConfiguration();
                    break;
            }
        }

        private void DrawTabsToolbar()
        {
            int tabIndex = (int)_preferences.selectedTab;
            tabIndex = GUILayout.Toolbar(tabIndex, _tabNames);
            _preferences.selectedTab = (TabType)tabIndex;
        }

        private void DrawGettingStarted()
        {
            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Welcome to Spatial Unity SDK!", _headerTextStyle);
            GUILayout.Space(10f);

            EditorGUILayout.LabelField("To gain the full potential of this toolset, it is recommended to follow all of these steps in the checklist!");

            if (GUILayout.Button("Read Documentation", GUILayout.MaxWidth(300f)))
            {
                EditorUtility.OpenDocumentationPage();
            }

            GUILayout.Space(10f);

            EditorGUILayout.LabelField($"1. Ensure you are on Unity {EditorUtility.CLIENT_UNITY_VERSION}, otherwise testing may not work (you are on {Application.unityVersion})");

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("2. Navigate to", GUILayout.Width(80f));

                if (EditorGUILayout.LinkButton("this link"))
                    Help.BrowseURL(ACCOUNT_PAGE_URL);

                EditorGUILayout.LabelField(" to retrieve an access token and paste it into the field below.");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            {
                DrawAuthTokenInputField();
            }
            EditorGUI.indentLevel--;

            GUILayout.Space(10f);

            EditorGUILayout.LabelField("3. Read up on documentation using the button above.");
            EditorGUILayout.LabelField("4. Build your environment scene.");
            EditorGUILayout.LabelField("5. Test out your creation inside of Spatial using the 'Test Current Space' button at the top right corner of the editor!");
        }

        private void DrawAuthentication()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField(EditorGUIUtility.IconContent("console.infoicon.sml"), GUILayout.Width(20f));
                EditorGUILayout.LabelField("You can retrieve an access token", GUILayout.Width(188f));

                if (EditorGUILayout.LinkButton("here"))
                    Help.BrowseURL(ACCOUNT_PAGE_URL);
            }
            EditorGUILayout.EndHorizontal();

            DrawAuthTokenInputField();
        }

        private void DrawAuthTokenInputField()
        {
            EditorGUILayout.HelpBox("IMPORTANT: Do NOT share this token with anyone!", MessageType.Warning);

            _token = _token.Trim();
            string prevToken = _token;
            _token = EditorGUILayout.PasswordField(new GUIContent("SDK Token:", "This token is used to authenticate with Spatial and upload your content"), _token);

            if (_token != prevToken)
            {
                EditorUtility.SaveAuthToken(_token);

                // There's a weird issue where the pasted value wouldn't show up unless you deselected the input field, and it only seems to
                // happen when the pasted value is very long.
                // We don't want to deselect if the user is typing in the value by hand (not really a good idea in the first place), so we can
                // kind of check that by seeing if at least 3 characters changed in a single GUI frame.
                if (!string.IsNullOrEmpty(_token) && EditorUtility.CheckStringDistance(prevToken, _token, distanceThreshold: 3))
                    GUI.FocusControl(null);
            }
        }

        private void DrawConfiguration()
        {
            if (_config == null || _configEditor == null)
            {
                EditorGUILayout.HelpBox("Failed to find configuration file. You may create a new configuration using the button below!", MessageType.Info);

                if (GUILayout.Button("Create Configuration File"))
                {
                    EditorConfig newConfig = EditorUtility.CreateDefaultConfigurationFile();
                    SetConfig(newConfig);
                }
                return;
            }

            _configEditor.OnInspectorGUI();

            GUILayout.Space(10f);

            GUI.backgroundColor = new Color(0.6f, 1f, 0.4f);
            if (GUILayout.Button("Save Changes", GUILayout.Height(30f)))
                AssetDatabase.SaveAssetIfDirty(_config);
            GUI.backgroundColor = Color.white;
        }

        private void InitializeIfNecessary()
        {
            if (_initialized)
                return;

            _initialized = true;
            _headerTextStyle = new GUIStyle(EditorStyles.boldLabel);
            _headerTextStyle.fontSize = 22;
            _headerTextStyle.padding = new RectOffset(0, 0, -8, -8);
            _headerTextStyle.alignment = TextAnchor.MiddleCenter;

            // Set up defaults then load in saved values.
            _preferences = new WindowPreferences();
            LoadWindowPreferences();

            _token = EditorUtility.GetSavedAuthToken();
            SetConfig(EditorConfig.instance);
        }

        private void LoadWindowPreferences()
        {
            string prefsString = EditorPrefs.GetString(WINDOW_PREFERENCES_PREFS_KEY, "{}");
            JsonUtility.FromJsonOverwrite(prefsString, _preferences);
        }

        private void SaveWindowPreferences()
        {
            EditorPrefs.SetString(WINDOW_PREFERENCES_PREFS_KEY, JsonUtility.ToJson(_preferences));
        }

        private void SetConfig(EditorConfig config)
        {
            if (_configEditor != null)
            {
                DestroyImmediate(_configEditor);
                _configEditor = null;
            }

            _config = config;

            if (_config != null)
                _configEditor = UnityEditor.Editor.CreateEditor(_config);
        }
    }
}
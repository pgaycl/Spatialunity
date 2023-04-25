using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SpatialSys.UnitySDK.Editor
{
    [CustomPropertyDrawer(typeof(SpatialQuest.Reward))]
    public class QuestRewardInspector : UnityEditor.PropertyDrawer
    {
        private Dictionary<string, float> _heights = new Dictionary<string, float>();

        private static string[] _badgeNames;
        private static string[] _badgeIds;
        private bool _fetchingBadges;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeProp = property.FindPropertyRelative(nameof(SpatialQuest.Reward.type));
            SerializedProperty idProp = property.FindPropertyRelative(nameof(SpatialQuest.Reward.id));

            EditorGUI.BeginProperty(rect, label, property);

            float startY = rect.y;

            // Type
            Rect typeRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(typeRect, typeProp);
            rect.y += typeRect.height + EditorGUIUtility.standardVerticalSpacing;

            // Reward type dropdown
            if (typeProp.enumValueIndex == (int)SpatialQuest.RewardType.Badge)
            {
                Rect idRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);

                if (_badgeNames == null || BadgeManager.GetCachedBadges().Count != _badgeNames.Length)
                {
                    RetrieveBadgeOptions();
                }
                else
                {
                    EditorGUI.BeginChangeCheck();
                    string idValue = idProp.stringValue;
                    int currentIndex = Array.IndexOf(_badgeIds, idValue);
                    int index = EditorGUI.Popup(
                        new Rect(rect.x, rect.y, rect.width, 20), "Badge", currentIndex, _badgeNames);
                    if (EditorGUI.EndChangeCheck())
                    {
                        idProp.stringValue = _badgeIds[index];
                    }
                    rect.y += idRect.height + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 50;
        }

        private void RetrieveBadgeOptions()
        {
            if (_fetchingBadges)
                return;

            List<SpatialAPI.Badge> badges = BadgeManager.GetCachedBadges();
            if (badges != null)
            {
                SetBadgeOptions(badges);
                return;
            }

            _fetchingBadges = true;
            BadgeManager.FetchBadges().Then(badges => {
                _fetchingBadges = false;
                SetBadgeOptions(badges);
            });
        }

        private void SetBadgeOptions(List<SpatialAPI.Badge> badges)
        {
            _badgeIds = badges.Select(b => b.id).ToArray();
            _badgeNames = badges.Select(b => b.name).ToArray();
        }
    }
}
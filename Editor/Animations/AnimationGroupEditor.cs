using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Brainamics.Core
{
    [CustomEditor(typeof(AnimationGroup), true)]
    public class AnimationGroupEditor : Editor
    {
        private readonly List<Type> _clipTypes = new();
        private SerializedProperty _clipsProperty, _playOnEnableProperty, _loopProperty;

        private AnimationGroup Target => target as AnimationGroup;

        private void OnEnable()
        {
            _clipsProperty = serializedObject.FindProperty("_clips");
            _playOnEnableProperty = serializedObject.FindProperty(nameof(AnimationGroup.PlayOnEnable));
            _loopProperty = serializedObject.FindProperty(nameof(AnimationGroup.Loop));

            InitializeClipTypes();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_playOnEnableProperty);
            EditorGUILayout.PropertyField(_loopProperty);

            EditorGUILayout.Space();
            GUILayout.Label("Clips", EditorStyles.boldLabel);
            for (var i = 0; i < Target.Clips.Count; i++)
            {
                var clip = Target.Clips[i];
                var indexProperty = _clipsProperty.GetArrayElementAtIndex(i);
                DrawClipInspector(clip, indexProperty, i);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Clip"))
                ShowAddClipMenu();
            if (!string.IsNullOrEmpty(GUIUtility.systemCopyBuffer))
            {
                if (GUILayout.Button("Paste Clip as New"))
                {
                    var clip = AnimationClipBase.DeserializeNew(GUIUtility.systemCopyBuffer);
                    if (clip != null)
                    {
                        Undo.RecordObject(Target, $"Paste New Clip");
                        Target.Clips.Add(clip);
                    }
                }
            }

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Play"))
                    Target.Play();
                if (GUILayout.Button("Stop"))
                    Target.Stop();
                if (GUILayout.Button("Kill"))
                    Target.Kill();
            }

            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawClipInspector(AnimationClipBase clip, SerializedProperty indexProperty, int index)
        {
            var enabledProperty = indexProperty.FindPropertyRelative(nameof(AnimationClipBase.Enabled));

            var backgroundTexture = clip.Fold ? Texture2D.normalTexture : Texture2D.grayTexture;
            var guiStyle = new GUIStyle
            {
                normal =
                {
                    background = backgroundTexture,
                    textColor = Color.white
                },
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(10, 10, 0, 0),
                border = new RectOffset(0, 0, 0, 0),
                fixedHeight = 20,
            };

            var clipName = ResolveClipName(clip.GetType());

            EditorGUILayout.BeginHorizontal();

            // Checkbox for Enabled property
            var bgRect = EditorGUILayout.GetControlRect(GUILayout.Width(30));
            bgRect.width -= 11;
            bgRect.height += 2;
            bgRect.y -= 2;
            EditorGUI.DrawTextureTransparent(bgRect, backgroundTexture, ScaleMode.StretchToFill);

            // Draw toggle on top of the colored rect
            var toggleRect = bgRect;
            toggleRect.width = 20;
            toggleRect.x += 3 + (bgRect.width - toggleRect.width) / 2;
            EditorGUI.PropertyField(toggleRect, enabledProperty, GUIContent.none);

            clip.Fold = EditorGUILayout.BeginFoldoutHeaderGroup(clip.Fold, clipName, guiStyle, rect =>
            {
                var menu = new GenericMenu();
                if (Application.isPlaying)
                {
                    menu.AddItem(new GUIContent("Play"), false, () =>
                    {
                        clip.Play(Target);
                    });
                    menu.AddItem(new GUIContent("Stop"), false, () =>
                    {
                        clip.Stop();
                    });
                    menu.AddItem(new GUIContent("Kill"), false, () =>
                    {
                        clip.Kill(Target);
                    });
                    menu.AddSeparator(string.Empty);
                }

                menu.AddItem(new GUIContent("Delete"), false, () =>
                {
                    Undo.RecordObject(Target, $"Delete Clip: {clipName}");
                    Target.Clips.Remove(clip);
                });
                menu.AddItem(new GUIContent("Duplicate"), false, () =>
                {
                    Undo.RecordObject(Target, $"Duplicate Clip: {clipName}");
                    var json = JsonUtility.ToJson(clip);
                    clip = JsonUtility.FromJson(json, clip.GetType()) as AnimationClipBase;
                    Target.Clips.Insert(index + 1, clip);
                });
                menu.AddSeparator(null);
                menu.AddItem(new GUIContent("Copy Clip"), false, () =>
                {
                    GUIUtility.systemCopyBuffer = clip.AsSerializedString();
                });
                var content = new GUIContent("Paste Clip Values");
                if (string.IsNullOrEmpty(GUIUtility.systemCopyBuffer))
                {
                    menu.AddDisabledItem(content);
                }
                else
                {
                    menu.AddItem(content, false, () =>
                    {
                        clip.DeserializeFromString(GUIUtility.systemCopyBuffer);
                    });
                }

                menu.AddSeparator(null);
                menu.AddItem(new GUIContent("Move Up"), false, () =>
                {
                    Undo.RecordObject(Target, $"Move Clip Up");
                    var index = Target.Clips.IndexOf(clip);
                    if (index > 0)
                        Target.Clips.Move(clip, index - 1);
                });
                menu.AddItem(new GUIContent("Move Down"), false, () =>
                {
                    Undo.RecordObject(Target, $"Move Clip Down");
                    var index = Target.Clips.IndexOf(clip);
                    if (index < Target.Clips.Count)
                        Target.Clips.Move(clip, index + 2);
                });
                menu.DropDown(rect);
            });
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndHorizontal();
            if (clip.Fold)
            {
                indexProperty.DrawAllProperties();
            }

            EditorGUILayout.Space();
        }

        private void ShowAddClipMenu()
        {
            var menu = new GenericMenu();
            foreach (var type in _clipTypes)
            {
                var clipName = ResolveClipName(type);
                menu.AddItem(new GUIContent(clipName), false, () =>
                {
                    Undo.RecordObject(Target, $"Add Clip: {clipName}");
                    var clip = Activator.CreateInstance(type) as AnimationClipBase;
                    clip.Fold = true;
                    Target.Clips.Add(clip);
                });
            }

            menu.ShowAsContext();
        }

        private void InitializeClipTypes()
        {
            _clipTypes.Clear();

            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(AnimationClipBase)));
            _clipTypes.AddRange(types);
        }

        private string ResolveClipName(Type type)
        {
            var attr = type.GetCustomAttribute<AnimationClipAttribute>();
            if (attr == null)
                return type.Name;
            return attr.ResolveMenuName(type);
        }
    }
}
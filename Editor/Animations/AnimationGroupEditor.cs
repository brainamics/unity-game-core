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
        private SerializedProperty _clipsProperty, _playOnEnableProperty;

        private AnimationGroup Target => target as AnimationGroup;

        private void OnEnable()
        {
            _clipsProperty = serializedObject.FindProperty("_clips");
            _playOnEnableProperty = serializedObject.FindProperty(nameof(AnimationGroup.PlayOnEnable));

            InitializeClipTypes();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_playOnEnableProperty);

            EditorGUILayout.Space();
            GUILayout.Label("Clips", EditorStyles.boldLabel);
            for (int i = 0; i < Target.Clips.Count; i++)
            {
                var clip = Target.Clips[i];
                var indexProperty = _clipsProperty.GetArrayElementAtIndex(i);
                DrawClipInspector(clip, indexProperty, i);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Clip"))
                ShowAddClipMenu();
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
            var guiStyle = new GUIStyle
            {
                normal =
                {
                    background = clip.Fold ? Texture2D.normalTexture : Texture2D.grayTexture,
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
                menu.DropDown(rect);
            });
            EditorGUILayout.EndFoldoutHeaderGroup();
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
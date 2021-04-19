using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AnimationSystem;
#if UNITY_EDITOR
[CustomEditor(typeof(AnimationController))]
public class AnimationControllerEditor : Editor
{
    private SerializedProperty animations;

    private List<AnimationSystem.Animation> animationList = new List<AnimationSystem.Animation>();
    private List<TransformAnimation> transformAnimations = new List<TransformAnimation>();
    private List<FadeAnimation> fadeAnimations = new List<FadeAnimation>();
    private List<ScaleAnimation> scaleAnimations = new List<ScaleAnimation>();


    private int typeSelectionTransform = 0;
    private int typeSelectionFade = 0;
    private int typeSelectionScale = 0;
    private void OnEnable()
    {
        animations = serializedObject.FindProperty("animations");
    }
    public override void OnInspectorGUI()
    {
        AnimationController animationController = (AnimationController)target;
        animationList = animationController.Animations;
        FillData();

        serializedObject.Update();

        GUIStyle labelStyle = new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        labelStyle.normal.textColor = Color.white;

        GUILayout.Space(10);
        GUILayout.Label("ADD ANIMATION", labelStyle);
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Transform") && transformAnimations.Count == 0)
        {
            typeSelectionTransform = 0;
            TransformAnimation transformAnimation = new TransformAnimation();
            transformAnimations.Add(transformAnimation);
            animationController.Animations.Add(transformAnimation);
        }

        if (GUILayout.Button("Scale") && scaleAnimations.Count == 0)
        {
            typeSelectionFade = 0;
            ScaleAnimation scaleAnimation = new ScaleAnimation();
            scaleAnimations.Add(scaleAnimation);
            animationController.Animations.Add(scaleAnimation);
        }

        if (GUILayout.Button("Fade") && fadeAnimations.Count == 0)
        {
            typeSelectionScale = 0;
            FadeAnimation fadeAnimation = new FadeAnimation();
            fadeAnimations.Add(fadeAnimation);
            animationController.Animations.Add(fadeAnimation);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(animations);

        UpdateAnimationFields();

        serializedObject.ApplyModifiedProperties();
    }

    private void FillData()
    {
        transformAnimations.Clear();
        fadeAnimations.Clear();
        scaleAnimations.Clear();

        foreach (AnimationSystem.Animation animation in animationList)
        {
            if (animation is TransformAnimation)
            {
                transformAnimations.Add(animation as TransformAnimation);
            }
            else if (animation is FadeAnimation)
            {
                fadeAnimations.Add(animation as FadeAnimation);
            }
            else if (animation is ScaleAnimation)
            {
                scaleAnimations.Add(animation as ScaleAnimation);
            }
        }
    }
    private void UpdateAnimationFields()
    {
        GUIStyle labelStyle = new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        labelStyle.normal.textColor = Color.white;

        GUIStyle animationLabelStyle = new GUIStyle() { alignment = TextAnchor.LowerCenter };
        animationLabelStyle.normal.textColor = Color.white;

        if (animationList.Count == 0) return;

        GUILayout.Space(10);
        GUILayout.Label("ANIMATIONS", labelStyle);
        GUILayout.Space(10);

        foreach (TransformAnimation transformAnimation in transformAnimations)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Transform", animationLabelStyle, GUILayout.Width(100));
            AnimationTypeSelection(transformAnimation);
            RemoveAnimationButton(transformAnimation);

            EditorGUILayout.EndHorizontal();
        }
        foreach (FadeAnimation fadeAnimation in fadeAnimations)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Fade", animationLabelStyle, GUILayout.Width(100));
            AnimationTypeSelection(fadeAnimation);
            RemoveAnimationButton(fadeAnimation);

            EditorGUILayout.EndHorizontal();
        }
        foreach (ScaleAnimation scaleAnimation in scaleAnimations)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Scale", animationLabelStyle, GUILayout.Width(100));
            AnimationTypeSelection(scaleAnimation);
            RemoveAnimationButton(scaleAnimation);

            EditorGUILayout.EndHorizontal();
        }

    }

    private void RemoveAnimationButton(AnimationSystem.Animation animation)
    {
        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            ((AnimationController)target).Animations.Remove(animation);
        }

    }
    private void AnimationTypeSelection(AnimationSystem.Animation animation)
    {
        string[] options;
        if (animation is TransformAnimation)
        {
            options = new string[]
            {
            "Type", "Transform", "RectTransform",
            };
            typeSelectionTransform = EditorGUILayout.Popup("", typeSelectionTransform, options);
        }
        else if(animation is FadeAnimation)
        {
            options = new string[]
            {
            "Type", "SpriteRenderer", "Image",
            };
            typeSelectionFade = EditorGUILayout.Popup("", typeSelectionFade, options);
        }
        else if(animation is ScaleAnimation)
        {
            options = new string[]
            {
            "Type", "Transform", "RectTransform",
            };
            typeSelectionScale = EditorGUILayout.Popup("", typeSelectionScale, options);
        }
    }
}
#endif
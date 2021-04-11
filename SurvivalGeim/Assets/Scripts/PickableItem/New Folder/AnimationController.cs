using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationSystem;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private List<AnimationSystem.Animation> animations = new List<AnimationSystem.Animation>();

    public List<AnimationSystem.Animation> Animations { get => animations; set => animations = value; }
}

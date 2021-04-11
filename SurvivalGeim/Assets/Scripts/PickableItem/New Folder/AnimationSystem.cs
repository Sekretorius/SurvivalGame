using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

namespace AnimationSystem
{
    public interface IAnimate
    {
        public UnityEvent OnAnimationEnd { get; }
        public bool IsAnimating { get; }
        public bool IsAnimationPaused { get; }
        public void Init();
        public void Init(Action<IEnumerator> startCoroutine);
        public bool StartAnimation();
        public bool StopAnimation();
        public bool PauseAnimation();
        public bool UnPauseAnimation();
    }
    [Serializable]
    public class Animation : IAnimate
    {
        protected bool isAnimating = false;
        protected bool isAnimationPaused = false;
        protected bool isStopingAnimation = false;

        protected UnityEvent onAnimationEnd;
        protected Action<IEnumerator> startCoroutine;

        public bool IsAnimationPaused => isAnimationPaused;

        public bool IsAnimating => isAnimating;

        public UnityEvent OnAnimationEnd => onAnimationEnd;

        public virtual void Init()
        {

        }
        public virtual void Init(Action<IEnumerator> startCoroutine)
        {
            this.startCoroutine = startCoroutine;
            onAnimationEnd = new UnityEvent();
        }

        public virtual bool StartAnimation()
        {
            return false;
        }
        public virtual bool PauseAnimation()
        {
            if (isAnimating)
            {
                isAnimationPaused = true;
                return true;
            }
            return false;
        }
        public virtual bool UnPauseAnimation()
        {
            if (isAnimating)
            {
                isAnimationPaused = false;
                return true;
            }
            return false;
        }
        public virtual bool StopAnimation()
        {
            if (isAnimating)
            {
                isStopingAnimation = true;
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class FadeAnimation : Animation
    {
        public Image Image { get { return image; } set { image = value; objectColor = value.color; } }
        public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } set { spriteRenderer = value; objectColor = value.color; } }

        private Image image;
        private SpriteRenderer spriteRenderer;


        private Color objectColor;
        [SerializeField]
        private float animationDuration = 1f;
        [SerializeField]
        [Range(0, 1)]
        private float fadePercentage = 1f;
        [SerializeField]
        private bool isFadingIn = true;

        public FadeAnimation() { }
        public FadeAnimation(Image image, float duration, bool isFadingIn = true, float percentage = 1)
        {
            Image = image;
            this.isFadingIn = isFadingIn;
            animationDuration = duration;
            fadePercentage = percentage;

        }
        public FadeAnimation(SpriteRenderer spriteRenderer, float duration, bool isFadingIn = true, float percentage = 1)
        {
            SpriteRenderer = spriteRenderer;
            this.isFadingIn = isFadingIn;
            animationDuration = duration;
            fadePercentage = percentage;
        }

        public override bool StartAnimation()
        {
            if (startCoroutine != null)
            {
                startCoroutine(Fade());
                return true;
            }
            return false;
        }

        private IEnumerator Fade()
        {
            while (isAnimating)
            {
                isStopingAnimation = true;
                yield return new WaitForEndOfFrame();
            }

            float time = 0;
            Color startColor = objectColor;
            Color endColor = isFadingIn ? new Color(objectColor.r, objectColor.g, objectColor.b, fadePercentage) : new Color(objectColor.r, objectColor.g, objectColor.b, 1 - fadePercentage);

            if (startColor == endColor)
            {
                yield break;
            }

            isAnimating = true;
            while (time < animationDuration)
            {
                if (isAnimationPaused)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                if (isStopingAnimation)
                {
                    OnAnimationEnd.Invoke();
                    isStopingAnimation = false;
                    yield break;
                }
                objectColor = Color.Lerp(startColor, endColor, Mathf.InverseLerp(0, animationDuration, time));

                ChangeColor(objectColor);

                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            ChangeColor(endColor);
            OnAnimationEnd.Invoke();
            isAnimating = false;
        }
        private void ChangeColor(Color color)
        {
            if (image != null)
            {
                image.color = color;
            }
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }
    }

    [Serializable]
    public class ScaleAnimation : Animation
    {
        private RectTransform rectTransform;
        private Transform transform;
        public RectTransform RectTransform { get { return rectTransform; } set { rectTransform = value; } }
        public Transform Transform { get { return transform; } set { transform = value; } }

        [SerializeField]
        private float animationDuration = 1f;
        [SerializeField]
        private Vector3 targetScale = Vector3.zero;

        public ScaleAnimation() { }
        public ScaleAnimation(RectTransform rectTransform, float duration, Vector3 targetScale)
        {
            this.rectTransform = rectTransform;
            animationDuration = duration;
            this.targetScale = targetScale != null ? targetScale : (Vector3)rectTransform.rect.size;
        }
        public ScaleAnimation(Transform transform, float duration, Vector3 targetScale)
        {
            this.transform = transform;
            animationDuration = duration;
            this.targetScale = targetScale != null ? targetScale : (Vector3)rectTransform.rect.size;
        }

        public override bool StartAnimation()
        {
            if (startCoroutine != null)
            {
                startCoroutine(Scale());
                return true;
            }
            return false;
        }
        private IEnumerator Scale()
        {
            while (isAnimating)
            {
                isStopingAnimation = true;
                yield return new WaitForEndOfFrame();
            }

            float time = 0;
            Vector3 originScale = Vector3.zero;

            if (transform != null && transform.localScale == targetScale)
            {
                yield break;
            }
            else if (transform != null)
            {
                originScale = transform.localScale;
            }
            if (rectTransform != null && rectTransform.rect.size == (Vector2)targetScale)
            {
                yield break;
            }
            else if (rectTransform != null)
            {
                originScale = rectTransform.rect.size;
            }

            isAnimating = true;
            while (time < animationDuration)
            {
                if (isAnimationPaused)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                if (isStopingAnimation)
                {
                    OnAnimationEnd.Invoke();
                    isStopingAnimation = false;
                    yield break;
                }

                Vector3 newScale = Vector2.Lerp(originScale, targetScale, Mathf.InverseLerp(0, animationDuration, time));
                ChangeScale(newScale);

                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            ChangeScale(targetScale);
            OnAnimationEnd.Invoke();
            isAnimating = false;
        }

        private void ChangeScale(Vector3 newScale)
        {
            if (transform != null)
            {
                transform.localScale = newScale;
            }
            if (rectTransform != null)
            {
                if (rectTransform.anchorMin == rectTransform.anchorMax)
                {
                    rectTransform.sizeDelta = newScale;
                }
                else
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newScale.x);
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newScale.y);
                }
            }
        }
    }

    [Serializable]
    public class TransformAnimation : Animation
    {
        public enum TransformType { TargetPosition, Direction }

        private RectTransform rectTransform;
        private Transform transform;
        public RectTransform RectTransform { get { return rectTransform; } set { rectTransform = value; } }
        public Transform Transform { get { return transform; } set { transform = value; } }


        [SerializeField]
        private TransformType transformType;

        [SerializeField]
        private float animationDuration = 1f;
        [SerializeField]
        private Vector3 targetPosition = Vector3.zero;
        [SerializeField]
        private float degrees = 0;
        [SerializeField]
        private float distance = 0;

        public TransformAnimation() { }
        public TransformAnimation(RectTransform rectTransform, float duration, Vector3 targetPosition)
        {
            this.rectTransform = rectTransform;
            animationDuration = duration;
            this.targetPosition = targetPosition != null ? targetPosition : rectTransform.position;
            transformType = TransformType.TargetPosition;
        }
        public TransformAnimation(Transform transform, float duration, Vector3 targetPosition)
        {
            this.transform = transform;
            animationDuration = duration;
            this.targetPosition = targetPosition != null ? targetPosition : transform.position;
            transformType = TransformType.TargetPosition;
        }
        public TransformAnimation(RectTransform rectTransform, float duration, float degrees, float distance)
        {
            this.rectTransform = rectTransform;
            animationDuration = duration;
            this.degrees = degrees;
            this.distance = distance;
            transformType = TransformType.Direction;
        }
        public TransformAnimation(Transform transform, float duration, float degrees, float distance)
        {
            this.transform = transform;
            animationDuration = duration;
            this.degrees = degrees;
            this.distance = distance;
            transformType = TransformType.Direction;
        }
        public override bool StartAnimation()
        {
            if (startCoroutine != null)
            {
                startCoroutine(Move());
                return true;
            }
            return false;
        }
        private IEnumerator Move()
        {
            while (isAnimating)
            {
                isStopingAnimation = true;
                yield return new WaitForEndOfFrame();
            }

            float time = 0;
            Vector3 originPosition = Vector3.zero;

            if (transform != null && transform.position == targetPosition)
            {
                yield break;
            }
            else if (transform != null)
            {
                originPosition = transform.position;
            }
            if (rectTransform != null && rectTransform.position == targetPosition)
            {
                yield break;
            }
            else if (rectTransform != null)
            {
                originPosition = rectTransform.position;
            }

            if (transformType == TransformType.Direction)
            {
                Vector3 direction = Quaternion.AngleAxis(degrees, -Vector3.forward) * Vector3.up;
                targetPosition = originPosition + direction * distance;
            }

            isAnimating = true;
            while (time < animationDuration)
            {
                if (isAnimationPaused)
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                if (isStopingAnimation)
                {
                    OnAnimationEnd.Invoke();
                    isStopingAnimation = false;
                    yield break;
                }

                Vector3 newPosition = Vector2.Lerp(originPosition, targetPosition, Mathf.InverseLerp(0, animationDuration, time));
                ChangePosition(newPosition);

                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            ChangePosition(targetPosition);
            OnAnimationEnd.Invoke();
            isAnimating = false;
        }
        private void ChangePosition(Vector3 newPosition)
        {
            if (transform != null)
            {
                transform.position = newPosition;
            }
            if (rectTransform != null)
            {
                rectTransform.position = newPosition;
            }
        }
    }

    public static class RectranformExtension
    {
        public static void CalculateAnchors(this RectTransform rectTransform, RectTransform parent = null, Canvas canvas = null)
        {
            if (parent == null)
            {
                parent = rectTransform.parent.GetComponent<RectTransform>();
            }
            if (canvas == null)
            {
                canvas = rectTransform.GetComponentInParent<Canvas>();
            }
            if (parent != null && canvas != null)
            {
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                Vector2 screenDif = new Vector2(canvasRect.rect.width / canvas.pixelRect.width, canvasRect.rect.height / canvas.pixelRect.height);
                Vector2 parentDownLeftCorner = new Vector2(parent.position.x - parent.rect.width / 2 / screenDif.x, parent.position.y - parent.rect.height / 2 / screenDif.y);

                //position in parent
                Vector2 topLeftCorner = new Vector2(rectTransform.position.x - rectTransform.rect.width / 2 / screenDif.x, rectTransform.position.y + rectTransform.rect.height / 2 / screenDif.y) - parentDownLeftCorner;
                Vector2 downRightCorner = new Vector2(rectTransform.position.x + rectTransform.rect.width / 2 / screenDif.x, rectTransform.position.y - rectTransform.rect.height / 2 / screenDif.y) - parentDownLeftCorner;

                Vector2 minAnchor = new Vector2(topLeftCorner.x / parent.rect.width * screenDif.x, downRightCorner.y / parent.rect.height * screenDif.y);
                Vector2 maxAnchor = new Vector2(downRightCorner.x / parent.rect.width * screenDif.x, topLeftCorner.y / parent.rect.height * screenDif.y);

                rectTransform.anchorMin = new Vector2(Mathf.Clamp(minAnchor.x, 0, 1), Mathf.Clamp(minAnchor.y, 0, 1));
                rectTransform.anchorMax = new Vector2(Mathf.Clamp(maxAnchor.x, 0, 1), Mathf.Clamp(maxAnchor.y, 0, 1));

                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = Vector2.zero;
            }
        }
        /// <summary>
        /// Returns rect corners position in screen space: 0 - down left, 1 - top left, 2 - top right, 3 - top down - corners
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static Vector3[] GetScreenPointCornerPositions(this RectTransform rectTransform, Canvas canvas = null)
        {
            if (canvas == null)
            {
                canvas = rectTransform.GetComponentInParent<Canvas>();
            }
            if (canvas != null)
            {
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                Vector2 screenDif = new Vector2(canvasRect.rect.width / canvas.pixelRect.width, canvasRect.rect.height / canvas.pixelRect.height);

                Vector2 topLeftCorner = new Vector2(rectTransform.position.x - rectTransform.rect.width / 2 / screenDif.x, rectTransform.position.y + rectTransform.rect.height / 2 / screenDif.y);
                Vector2 downRightCorner = new Vector2(rectTransform.position.x + rectTransform.rect.width / 2 / screenDif.x, rectTransform.position.y - rectTransform.rect.height / 2 / screenDif.y);


                return new Vector3[]
                {
                    new Vector3(topLeftCorner.x, downRightCorner.y),
                    topLeftCorner,
                    new Vector3(downRightCorner.x, topLeftCorner.y),
                    downRightCorner
                };
            }
            return null;
        }
    }
}
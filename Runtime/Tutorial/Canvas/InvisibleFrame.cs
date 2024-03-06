using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    public class InvisibleFrame : MonoBehaviour
    {
        private Canvas _canvas;
        private Coroutine _dynamicCoroutine;

        [Header("Elements")]
        [SerializeField]
        private Image _left;

        [SerializeField]
        private Image _right;

        [SerializeField]
        private Image _top;

        [SerializeField]
        private Image _bottom;

        public void Hide()
            => SetVisibility(false);

        public void SetVisibility(bool visible)
        {
            if (!visible)
                StopDynamicCoroutine();
            _left.gameObject.SetActive(visible);
            _right.gameObject.SetActive(visible);
            _top.gameObject.SetActive(visible);
            _bottom.gameObject.SetActive(visible);
        }

        public void SetFrameDynamic(GameObject target, Camera camera = null)
        {
            if (camera == null)
                camera = Camera.main;

            StopDynamicCoroutine();
            _dynamicCoroutine = StartCoroutine(SetFrameCoroutine());

            IEnumerator SetFrameCoroutine()
            {
                var delay = new WaitForSeconds(0.1f);
                var lastBounds = default(SpaceBounds);
                var bounds = BoundsUtils.GetBounds(target);

                while (target != null)
                {
                    bounds = BoundsUtils.GetBounds(target);
                    if (lastBounds != bounds)
                    {
                        SetFrameInternal(bounds, camera);
                    }
                    yield return delay;
                }
                SetVisibility(false);
                _dynamicCoroutine = null;
            }
        }

        public void SetFrame(GameObject target, Camera camera = null)
        {
            var bounds = BoundsUtils.GetBounds(target);
            SetFrame(bounds, camera);
        }

        public void SetFrame(SpaceBounds bounds, Camera camera = null)
        {
            StopDynamicCoroutine();
            SetFrameInternal(bounds, camera);
        }

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>(true);
        }

        private void SetFrameInternal(SpaceBounds bounds, Camera camera = null)
        {
            var rect = BoundsUtils.BoundsToScreenRect(bounds, camera);
            SetScreenFrame(rect);
        }

        private void SetScreenFrame(Rect rect)
        {
            var left = rect.min.x / _canvas.scaleFactor;
            var right = (Screen.width - rect.max.x) / _canvas.scaleFactor;
            var bottom = rect.min.y / _canvas.scaleFactor;
            var top = (Screen.height - rect.max.y) / _canvas.scaleFactor;
            SetVisibility(true);
            _left.rectTransform.sizeDelta = new Vector2(left, _left.rectTransform.sizeDelta.y);
            _right.rectTransform.sizeDelta = new Vector2(right, _right.rectTransform.sizeDelta.y);
            _top.rectTransform.sizeDelta = new Vector2(_top.rectTransform.sizeDelta.x, top);
            _bottom.rectTransform.sizeDelta = new Vector2(_bottom.rectTransform.sizeDelta.x, bottom);
        }

        private void StopDynamicCoroutine()
        {
            if (_dynamicCoroutine == null)
                return;
            StopCoroutine(_dynamicCoroutine);
            _dynamicCoroutine = null;
        }
    }

}

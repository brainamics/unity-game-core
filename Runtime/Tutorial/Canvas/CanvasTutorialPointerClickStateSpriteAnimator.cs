using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [RequireComponent(typeof(TutorialPointerBase))]
    public class CanvasTutorialPointerClickStateSpriteAnimator : MonoBehaviour
    {
        private ITutorialPointer _pointer;
        private Coroutine _animation;
        private Sprite _originalSprite;

        public Image CanvasImage;
        public SpriteRoutine[] Routines;

        private void Awake()
        {
            if (!TryGetComponent(out _pointer))
                throw new System.InvalidOperationException($"A component implementing {nameof(ITutorialPointer)} is required.");
        }

        private void OnEnable()
        {
            _pointer.OnClickStateChanged.AddListener(HandleClickStateChanged);
            UpdateState();
        }

        private void OnDisable()
        {
            _pointer.OnClickStateChanged.RemoveListener(HandleClickStateChanged);
            _animation = null;
        }

        private void HandleClickStateChanged(ITutorialPointer pointer)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            var active = _animation != null;
            if (_pointer.IsClickDown == active || CanvasImage == null)
                return;

            if (!_pointer.IsClickDown)
            {
                if (_animation != null)
                    StopCoroutine(_animation);
                _animation = null;
                ResetState();
                return;
            }

            _animation = StartCoroutine(Animate());
        }

        private void ResetState()
        {
            if (_originalSprite == null || CanvasImage == null)
                return;
            CanvasImage.sprite = _originalSprite;
        }

        private IEnumerator Animate()
        {
            _originalSprite = CanvasImage.sprite;

            while (true)
            {
                foreach (var routine in Routines)
                {
                    CanvasImage.sprite = routine.SpriteOrDefault(_originalSprite);
                    yield return new WaitForSeconds(routine.Duration);
                }

                yield return null;
            }
        }

        [System.Serializable]
        public sealed class SpriteRoutine
        {
            public Sprite Sprite;
            public float Duration = 1f;

            public Sprite SpriteOrDefault(Sprite @default)
                => Sprite == null ? @default : Sprite;
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Brainamics.Core
{
    public class ButtonActionRepeater : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private int _repetitionCount;
        private Coroutine _repeaterCoroutine;

        public float RepetitionStartupDelay = 0.5f;
        public float RepetitionInterval = 0.2f;
        public UnityEvent<int> OnPerformed;

        public void OnPointerDown(PointerEventData eventData)
        {
            _repetitionCount = 0;

            if (_repeaterCoroutine != null)
            {
                StopCoroutine(_repeaterCoroutine);
                _repeaterCoroutine = null;
            }
            _repeaterCoroutine = StartCoroutine(RunRepeaterCoroutine());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_repeaterCoroutine != null)
                StopCoroutine(_repeaterCoroutine);
            _repeaterCoroutine = null;

            if (_repetitionCount == 0)
                OnPerformed.Invoke(-1);
            _repetitionCount = 0;
        }

        private IEnumerator RunRepeaterCoroutine()
        {
            yield return new WaitForSeconds(RepetitionStartupDelay);

            while (true)
            {
                OnPerformed.Invoke(_repetitionCount++);
                yield return new WaitForSeconds(RepetitionInterval);
            }
        }
    }
}
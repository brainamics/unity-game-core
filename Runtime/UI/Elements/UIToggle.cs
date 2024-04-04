using Brainamics.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Brinamics.Core
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggle : MonoBehaviour
    {
        private Toggle _toggle;
        private bool _checked;

        public AnimationGroup OnFeedback;
        public AnimationGroup OffFeedback;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(HandleValueChanged);
            UpdateState(true);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(HandleValueChanged);
        }

        private void HandleValueChanged(bool _)
        {
            UpdateState();
        }

        private void UpdateState(bool force = false)
        {
            if (!force && _checked == _toggle.isOn)
                return;
            _checked = _toggle.isOn;

            var enabled = _checked ? OnFeedback : OffFeedback;
            var disabled = _checked ? OffFeedback : OnFeedback;
            if (disabled != null)
                disabled.Kill();
            if (enabled != null)
                enabled.Play();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [RequireComponent(typeof(TutorialStep))]
    public abstract class TutorialStepBehaviorBase : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        protected TutorialStep Step { get; private set; }

        protected virtual void Awake()
        {
            Step = GetComponent<TutorialStep>();
        }

        protected virtual void OnEnable()
        {
            Step.OnActivated.AddListener(HandleStepActivated);
            Step.OnDeactivated.AddListener(HandleStepDeactivated);
        }

        protected virtual void OnDisable() { }

        protected abstract void OnActivate();

        protected abstract void OnDeactivate();

        private void Activate(bool activate)
        {
            if (IsActive == activate)
                return;
            IsActive = activate;

            if (activate)
                OnActivate();
            else
                OnDeactivate();
        }

        private void HandleStepActivated(TutorialStep _)
        {
            Activate(true);
        }

        private void HandleStepDeactivated(TutorialStep _)
        {
            Activate(false);
        }
    }
}

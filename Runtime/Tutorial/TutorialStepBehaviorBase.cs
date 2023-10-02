using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [RequireComponent(typeof(TutorialStep))]
    [DefaultExecutionOrder(99)]
    public abstract class TutorialStepBehaviorBase : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        protected TutorialStep Step { get; private set; }

        protected virtual void Awake()
        {
            Step = GetComponent<TutorialStep>();
            if (Step.State == TutorialStepState.Active)
                HandleStepActivated(Step);
        }

        protected virtual void OnEnable()
        {
            Step.OnActivated.AddListener(HandleStepActivated);
            Step.OnDeactivated.AddListener(HandleStepDeactivated);
        }

        protected virtual void OnDisable()
        {
            Step.OnActivated.RemoveListener(HandleStepActivated);
            Step.OnDeactivated.RemoveListener(HandleStepDeactivated);
        }

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

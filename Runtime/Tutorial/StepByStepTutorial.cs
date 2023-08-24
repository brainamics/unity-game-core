using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    [DefaultExecutionOrder(1)]
    public class StepByStepTutorial : MonoBehaviour
    {
        protected TutorialStep[] _steps;
        protected TutorialStep _activeStep;

        public bool ActivateOnEnable = true;
        public UnityEvent<TutorialStep, TutorialStep> OnActiveStepChanged;

        public TutorialStep ActiveStep
        {
            get => _activeStep;
            set
            {
                if (_activeStep == value)
                    return;
                var oldStep = _activeStep;
                _activeStep = value;
                if (value != null && value.State != TutorialStepState.Active)
                    value.State = TutorialStepState.Active;
                OnActiveStepChanged.Invoke(oldStep, value);
            }
        }

        protected virtual void Awake() { }

        protected virtual void OnEnable()
        {
            _steps = GetComponentsInChildren<TutorialStep>();
            BindSteps();
            if (ActivateOnEnable)
                UpdateActiveStep();
        }

        protected virtual void OnDisable()
        {
            UnbindSteps();
            _steps = System.Array.Empty<TutorialStep>();
        }

        private void BindSteps()
        {
            foreach (var step in _steps)
                step.OnStateChanged.AddListener(HandleStepStateChanged);
        }

        private void UnbindSteps()
        {
            DeactivateCurrentStep();
            foreach (var step in _steps)
                step.OnStateChanged.RemoveListener(HandleStepStateChanged);
        }

        private void HandleStepStateChanged(TutorialStep step)
        {
            UpdateActiveStep();
        }

        private void DeactivateCurrentStep()
        {
            if (_activeStep == null)
                return;

            if (_activeStep.IsActive)
                _activeStep.State = TutorialStepState.NotStarted;
            _activeStep = null;
        }

        private void UpdateActiveStep()
        {
            if (_activeStep != null && !_activeStep.IsActive)
            {
                ActiveStep = null;
            }

            ActivateFirstIncompleteStep();
        }

        private void ActivateFirstIncompleteStep()
        {
            foreach (var step in _steps)
            {
                if (step.State is not TutorialStepState.NotStarted and not TutorialStepState.Failed)
                    continue;
                ActiveStep = step;
                break;
            }
        }
    }
}

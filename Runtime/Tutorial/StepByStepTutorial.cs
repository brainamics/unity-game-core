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
        protected bool _preventStepUpdates;
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
                _preventStepUpdates = true;
                try
                {
                    if (oldStep != null && oldStep.State == TutorialStepState.Active)
                        oldStep.State = IsPriorStep(value, oldStep) ? TutorialStepState.NotStarted : TutorialStepState.Failed;
                    _activeStep = value;
                    if (value != null && value.State != TutorialStepState.Active)
                        value.State = TutorialStepState.Active;
                }
                finally
                {
                    _preventStepUpdates = false;
                }
                OnActiveStepChanged.Invoke(oldStep, value);
            }
        }

        public int GetStepIndex(TutorialStep step)
            => Array.IndexOf(_steps, step);

        public TutorialStep GetNextStep(TutorialStep step)
        {
            var index = GetStepIndex(step);
            if (index++ < 0 || index >= _steps.Length)
                return null;
            return _steps[index];
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

        protected virtual void HandleStepStateChanged(TutorialStep step)
        {
            UpdateActiveStep();
        }

        private bool IsPriorStep(TutorialStep step, TutorialStep priorToStep)
        {
            var index = Array.IndexOf(_steps, step);
            var priorIndex = Array.IndexOf(_steps, priorToStep);
            return index < priorIndex;
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

        private void DeactivateCurrentStep()
        {
            if (_activeStep == null)
                return;

            if (_activeStep.IsActive)
                _activeStep.State = TutorialStepState.NotStarted;
            _activeStep = null;
        }

        protected void UpdateActiveStep()
        {
            if (_preventStepUpdates)
                return;

            if (_activeStep != null && !_activeStep.IsActive)
            {
                ActiveStep = null;
            }

            if (_activeStep != null)
                return;
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

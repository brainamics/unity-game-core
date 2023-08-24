using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public class StepByStepTutorial : MonoBehaviour
    {
        private TutorialStep[] _steps;
        private TutorialStep _activeStep;

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
                OnActiveStepChanged.Invoke(oldStep, value);
            }
        }

        private void OnEnable()
        {
            _steps = GetComponentsInChildren<TutorialStep>();
            BindSteps();
            UpdateActiveStep();
        }

        private void OnDisable()
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

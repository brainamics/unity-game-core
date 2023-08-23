using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class StepByStepTutorial : MonoBehaviour
    {
        private TutorialStep[] _steps;
        private TutorialStep _activeStep;

        public TutorialStep ActiveStep { get; private set; }

        private void OnEnable()
        {
            _steps = GetComponentsInChildren<TutorialStep>();
            BindSteps();
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
            if (ActiveStep != null && !ActiveStep.IsActive)
            {
                ActiveStep = null;
            }
        }
    }
}

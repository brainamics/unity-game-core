using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public class TutorialStep : MonoBehaviour
    {
        private TutorialStepState _state = TutorialStepState.NotStarted;

        public UnityEvent<TutorialStep> OnActivated;
        public UnityEvent<TutorialStep> OnDeactivated;
        public UnityEvent<TutorialStep> OnStateChanged;

        public bool IsActive => _state == TutorialStepState.Active;

        public TutorialStepState State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;
                var wasActive = IsActive;
                _state = value;
                HandleStateChanged(value);
                OnStateChanged.Invoke(this);
                if (wasActive != IsActive)
                {
                    HandleActivationChanged();
                    if (IsActive)
                        OnActivated.Invoke(this);
                    else
                        OnDeactivated.Invoke(this);
                }
            }
        }

        private void HandleStateChanged(TutorialStepState state)
        {
        }

        private void HandleActivationChanged()
        {
        }
    }
}

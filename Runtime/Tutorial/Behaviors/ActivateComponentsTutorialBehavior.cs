using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class ActivateComponentsTutorialBehavior : TutorialStepBehaviorBase
    {
        public Behaviour[] Components;
        public bool Deactivate;
        public bool RestoreOnExit = true;

        protected override void OnActivate()
        {
            foreach (var component in Components)
                component.enabled = !Deactivate;
        }

        protected override void OnDeactivate()
        {
            if (!RestoreOnExit)
                return;
            foreach (var component in Components)
                component.enabled = Deactivate;
        }
    }
}

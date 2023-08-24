using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class ActivateComponentsTutorialBehavior : TutorialStepBehaviorBase
    {
        public Behaviour[] Components;

        protected override void OnActivate()
        {
            foreach (var component in Components)
                component.enabled = true;
        }

        protected override void OnDeactivate()
        {
            foreach (var component in Components)
                component.enabled = false;
        }
    }
}

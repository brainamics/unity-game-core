using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class HidePointerTutorialBehavior : TutorialStepBehaviorBase
    {
        public TutorialPointerBase Pointer;
        public bool HideOnActivation = true;
        public bool HideOnDeactivation = false;
        public bool Immediate;

        protected override void OnActivate()
        {
            if (HideOnActivation)
                Hide();
        }

        protected override void OnDeactivate()
        {
            if (HideOnDeactivation)
                Hide();
        }

        private void Hide()
        {
            Pointer.PointAt(Immediate ? TutorialPointAtRequest.InvisibleImmediate : TutorialPointAtRequest.Invisible);
        }
    }
}

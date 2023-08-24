using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class ActivateObjectsTutorialBehavior : TutorialStepBehaviorBase
    {
        public GameObject[] Objects;

        protected override void OnActivate()
        {
            foreach (var obj in Objects)
                obj.SetActive(true);
        }

        protected override void OnDeactivate()
        {
            foreach (var obj in Objects)
                obj.SetActive(false);
        }
    }
}

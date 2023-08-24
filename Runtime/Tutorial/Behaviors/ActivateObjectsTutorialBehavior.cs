using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class ActivateObjectsTutorialBehavior : TutorialStepBehaviorBase
    {
        public GameObject[] Objects;
        public bool Deactivate;
        public bool RestoreOnExit = true;

        protected override void OnActivate()
        {
            foreach (var obj in Objects)
                obj.SetActive(!Deactivate);
        }

        protected override void OnDeactivate()
        {
            if (!RestoreOnExit)
                return;
            foreach (var obj in Objects)
                obj.SetActive(Deactivate);
        }
    }
}

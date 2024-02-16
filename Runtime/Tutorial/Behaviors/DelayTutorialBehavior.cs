using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class DelayTutorialBehavior : TutorialStepBehaviorBase
    {
        private Coroutine _coroutine;

        [Min(0)]
        public float Delay = 1f;

        protected override void OnActivate()
        {
            this.StartMonoCoroutine(ref _coroutine, DelayAndPass());
        }

        protected override void OnDeactivate()
        {
        }

        private IEnumerator DelayAndPass()
        {
            yield return new WaitForSeconds(Delay);

            Step.State = TutorialStepState.Completed;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class AnimatorExtensions
{
    public static IEnumerable WaitForState(this Animator animator, string stateName, int layerIndex = 0)
    {
        while (true)
        {
            var state = animator.GetCurrentAnimatorStateInfo(layerIndex);
            if (state.IsName(stateName))
                yield break;
            yield return null;
        }
    }

    public static IEnumerable WaitForStateDuration(this Animator animator, string stateName, int layerIndex = 0)
    {
        var waitTime = 0f;

        while (true)
        {
            var state = animator.GetCurrentAnimatorStateInfo(layerIndex);
            if (state.IsName(stateName))
            {
                waitTime = state.length;
                break;
            }
            yield return null;
        }

        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);
    }

    public static IEnumerable WaitForStateEnd(this Animator animator, string stateName, int layerIndex = 0)
    {
        yield return null;
        while (true)
        {
            var state = animator.GetNextAnimatorStateInfo(layerIndex);
            if (!state.IsName(stateName))
                break;
            yield return null;
        }
        while (true)
        {
            var state = animator.GetCurrentAnimatorStateInfo(layerIndex);
            if (!state.IsName(stateName))
                break;
            yield return null;
        }
    }
}

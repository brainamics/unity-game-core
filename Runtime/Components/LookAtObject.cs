using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class LookAtObject : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        private void Update()
        {
            if (_target == null)
                return;
            transform.LookAt(_target);
        }
    }
}
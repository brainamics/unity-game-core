using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class LinearPipelineHost : MonoBehaviour
    {
        public LinearPipeline Pipeline { get; } = new();

        private void Update()
        {
            Pipeline.TriggerProcess();
        }
    }
}

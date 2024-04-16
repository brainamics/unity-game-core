using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class LinearPipelineHost : MonoBehaviour
    {
        public bool RespectGamePause = true;

        public LinearPipeline Pipeline { get; } = new();

        protected virtual void Awake()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void Update()
        {
            if (RespectGamePause && Time.timeScale == 0)
                return;
            Pipeline.TriggerProcess();
        }
    }
}

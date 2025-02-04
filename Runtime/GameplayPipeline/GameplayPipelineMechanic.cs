using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class GameplayPipelineMechanic : MonoBehaviour
    {
        [SerializeField]
        private GameplayPipelineBase _pipeline;

        public GameplayPipelineBase Pipeline
        {
            get => _pipeline;
            set => _pipeline = value;
        }

        public virtual void Execute(GameplayPipelineContext context)
        {
        }

        protected virtual void Awake()
        {
            if (_pipeline == null)
                _pipeline = GetComponentInParent<GameplayPipelineBase>();
        }

        protected virtual void OnEnable()
        {
            _pipeline.Register(Execute);
        }

        protected virtual void OnDisable()
        {
            _pipeline.Unregister(Execute);
        }
    }
}

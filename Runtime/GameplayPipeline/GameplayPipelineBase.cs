using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Brainamics.Core
{
	[DefaultExecutionOrder(-100)]
	public abstract class GameplayPipelineBase : MonoBehaviour
	{
		private readonly List<GameplayPipelineMechanicDelegate> _mechanics = new();
		private readonly GameplayPipelineContext _context = new();

		public IReadOnlyList<GameplayPipelineMechanicDelegate> Mechanics => _mechanics;

  		public virtual int MaxLoopExecutionTimes => 10000;

		protected abstract object PipelineActionId { get; }

		protected virtual int PipelinePriority => 0;

		protected abstract LinearPipeline LinearPipeline { get; }

		public void Register(GameplayPipelineMechanicDelegate mechanic)
		{
			_mechanics.Add(mechanic);
		}

		public void Unregister(GameplayPipelineMechanicDelegate mechanic)
		{
			_mechanics.Remove(mechanic);
		}

		public virtual T GetMechanic<T>()
  			where T : GameplayPipelineMechanic
		{
		    return gameObject.GetComponentInChildren<T>();
		}

		public bool ExecuteStep()
		{
			_context.Reset();

			var anyWork = false;
			foreach (var mechanic in _mechanics)
			{
				_context.PrepareForCall();
				mechanic.Invoke(_context);
				if (_context.IsSuccess == true)
					anyWork = true;
				if (_context.IsCanceled)
					break;
				if (LinearPipeline.AnyOngoingFeedbacks)
					break;
			}

			return anyWork;
		}

		public bool Execute()
		{
			if (!CanExecute())
   				return false;
  
			Assert.IsFalse(LinearPipeline.AnyOngoingFeedbacks);
			var any = false;

			var times = 0;
			while (ExecuteStep())
			{
				any = true;

				if (LinearPipeline.AnyOngoingFeedbacks)
					break;

     				if (++times > MaxLoopExecutionTimes)
	 			{
     					_mechanics.Clear();
	 				throw new System.InvalidOperationException($"Gameplay pipeline executed in a loop {times} times. This could be an indicator of a possible infinite loop in the pipeline. To prevent this problem from happening in the same play session, all mechanics have been removed from the pipeline.");
      				}
			}

			if (any)
				Schedule();
			return any;
		}

		[ContextMenu("Schedule")]
		public virtual void Schedule()
		{
			LinearPipeline.EnqueueAction(PipelineActionId, () => Execute(), priority: PipelinePriority);
		}

		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
  			Schedule();
		}

		protected virtual void OnDisable()
		{
			LinearPipeline.CancelAction(PipelineActionId);
		}

		protected virtual bool CanExecute()
  			=> true;
	}
}

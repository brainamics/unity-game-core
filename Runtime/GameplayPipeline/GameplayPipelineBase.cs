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
			Assert.IsFalse(LinearPipeline.AnyOngoingFeedbacks);
			var any = false;

			while (ExecuteStep())
			{
				any = true;

				if (LinearPipeline.AnyOngoingFeedbacks)
					break;
			}

			if (any)
				Schedule();
			return any;
		}

		[ContextMenu("Schedule")]
		public void Schedule()
		{
			LinearPipeline.EnqueueAction(PipelineActionId, () => Execute(), priority: PipelinePriority);
		}

		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void OnDisable()
		{
			LinearPipeline.CancelAction(PipelineActionId);
		}
	}
}

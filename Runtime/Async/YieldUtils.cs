﻿using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


namespace TWizard.Core.Async
{
    public static class Yield
    {
        public static bool ShouldKeepWaiting(this Task task) => !task.IsCompleted || !task.IsFaulted || !task.IsCanceled;


        public static YieldableTask AsYieldable(this Task task) => new YieldableTask(task);
        public static YieldableTask<T> AsYieldable<T>(this Task<T> task) => new YieldableTask<T>(task);


        public static IEnumerator Async(Func<Task> asyncFunction)
        {
            if (asyncFunction == null)
                throw new ArgumentNullException(nameof(asyncFunction));

            var task = asyncFunction();
            while (task.ShouldKeepWaiting())
                yield return null;

            if (task.Exception != null)
                throw task.Exception;
        }
    }

    public class YieldableTask : CustomYieldInstruction
    {
        public Task Task { get; }

        public override bool keepWaiting => Task.ShouldKeepWaiting();


        public YieldableTask(Task task) => Task = task ?? throw new ArgumentNullException(nameof(task));
    }

    public class YieldableTask<T> : CustomYieldInstruction
    {
        public Task<T> Task { get; }
        public T Result => Task.Result;

        public override bool keepWaiting => Task.ShouldKeepWaiting();


        public YieldableTask(Task<T> task) => Task = task ?? throw new ArgumentNullException(nameof(task));
    }

    /// <summary>
    /// Util yieldable class to yield until <see cref="Finish"/> is called, can be reused with <see cref="Reset"/>.
    /// </summary>
    public class YieldSignal : CustomYieldInstruction
    {
        private bool wait = true;
        public override bool keepWaiting => wait;


        public void Finish() => wait = false;
        public new void Reset() => wait = true;
    }
}
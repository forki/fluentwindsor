using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using Nito.AsyncEx.Synchronous;

namespace FluentlyWindsor.EndersJson.AsyncEx
{
    public sealed class BoundActionField<T>
    {
        private BoundAction field;

        public BoundActionField(Action<T> action, T context)
        {
            field = new BoundAction(action, context);
        }

        public bool IsEmpty => Interlocked.CompareExchange(ref field, null, null) == null;

        public IBoundAction TryGetAndUnset()
        {
            return Interlocked.Exchange(ref field, null);
        }

        public bool TryUpdateContext(Func<T, T> contextUpdater)
        {
            while (true)
            {
                var original = Interlocked.CompareExchange(ref field, field, field);
                if (original == null)
                    return false;
                var updatedContext = new BoundAction(original, contextUpdater);
                var result = Interlocked.CompareExchange(ref field, updatedContext, original);
                if (ReferenceEquals(original, result))
                    return true;
            }
        }

        public interface IBoundAction
        {
            void Invoke();
        }

        private sealed class BoundAction : IBoundAction
        {
            private readonly Action<T> _action;
            private readonly T _context;

            public BoundAction(Action<T> action, T context)
            {
                _action = action;
                _context = context;
            }

            public BoundAction(BoundAction originalBoundAction, Func<T, T> contextUpdater)
            {
                _action = originalBoundAction._action;
                _context = contextUpdater(originalBoundAction._context);
            }

            public void Invoke() => _action?.Invoke(_context);
        }
    }

    public abstract class SingleDisposable<T> : IDisposable
    {
        private readonly BoundActionField<T> _context;

        private readonly ManualResetEventSlim _mre = new ManualResetEventSlim();

        protected SingleDisposable(T context)
        {
            _context = new BoundActionField<T>(Dispose, context);
        }

        public bool IsDisposeStarted => _context.IsEmpty;

        public bool IsDispoed => _mre.IsSet;

        public bool IsDisposing => IsDisposeStarted && !IsDispoed;

        protected abstract void Dispose(T context);

        public void Dispose()
        {
            var context = _context.TryGetAndUnset();
            if (context == null)
            {
                _mre.Wait();
                return;
            }
            try
            {
                context.Invoke();
            }
            finally
            {
                _mre.Set();
            }
        }

        protected bool TryUpdateContext(Func<T, T> contextUpdater) => _context.TryUpdateContext(contextUpdater);
    }

    public sealed partial class AsyncContext
    {
        private sealed class AsyncContextTaskScheduler : TaskScheduler
        {
            private readonly AsyncContext _context;

            public AsyncContextTaskScheduler(AsyncContext context)
            {
                _context = context;
            }

            [System.Diagnostics.DebuggerNonUserCode]
            protected override IEnumerable<Task> GetScheduledTasks()
            {
                return _context._queue.GetScheduledTasks();
            }

            protected override void QueueTask(Task task)
            {
                _context.Enqueue(task, false);
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                return (AsyncContext.Current == _context) && TryExecuteTask(task);
            }

            public override int MaximumConcurrencyLevel
            {
                get { return 1; }
            }

            public void DoTryExecuteTask(Task task)
            {
                TryExecuteTask(task);
            }
        }
    }



    public sealed partial class AsyncContext
    {
        private sealed class TaskQueue : IDisposable
        {
            private readonly BlockingCollection<Tuple<Task, bool>> _queue;

            public TaskQueue()
            {
                _queue = new BlockingCollection<Tuple<Task, bool>>();
            }

            public IEnumerable<Tuple<Task, bool>> GetConsumingEnumerable()
            {
                return _queue.GetConsumingEnumerable();
            }

            [System.Diagnostics.DebuggerNonUserCode]
            internal IEnumerable<Task> GetScheduledTasks()
            {
                foreach (var item in _queue)
                    yield return item.Item1;
            }

            public bool TryAdd(Task item, bool propagateExceptions)
            {
                try
                {
                    return _queue.TryAdd(Tuple.Create(item, propagateExceptions));
                }
                catch (InvalidOperationException)
                {
                    // vexing exception
                    return false;
                }
            }

            public void CompleteAdding()
            {
                _queue.CompleteAdding();
            }

            public void Dispose()
            {
                _queue.Dispose();
            }
        }
    }


    public sealed partial class AsyncContext
    {
        private sealed class AsyncContextSynchronizationContext : SynchronizationContext
        {
            private readonly AsyncContext _context;

            public AsyncContextSynchronizationContext(AsyncContext context)
            {
                _context = context;
            }

            public AsyncContext Context
            {
                get
                {
                    return _context;
                }
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                _context.Enqueue(_context._taskFactory.Run(() => d(state)), true);
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                if (AsyncContext.Current == _context)
                {
                    d(state);
                }
                else
                {
                    var task = _context._taskFactory.Run(() => d(state));
                    task.WaitAndUnwrapException();
                }
            }

            public override void OperationStarted()
            {
                _context.OperationStarted();
            }

            public override void OperationCompleted()
            {
                _context.OperationCompleted();
            }

            public override SynchronizationContext CreateCopy()
            {
                return new AsyncContextSynchronizationContext(_context);
            }

            public override int GetHashCode()
            {
                return _context.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = obj as AsyncContextSynchronizationContext;
                if (other == null)
                    return false;
                return (_context == other._context);
            }
        }
    }


    [DebuggerDisplay("Id = {Id}, OperationCount = {_outstandingOperations}")]
    [DebuggerTypeProxy(typeof(DebugView))]
    public sealed partial class AsyncContext : IDisposable
    {
        private readonly TaskQueue _queue;

        private readonly AsyncContextSynchronizationContext _synchronizationContext;

        private readonly AsyncContextTaskScheduler _taskScheduler;

        private readonly TaskFactory _taskFactory;

        private int _outstandingOperations;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public AsyncContext()
        {
            _queue = new TaskQueue();
            _synchronizationContext = new AsyncContextSynchronizationContext(this);
            _taskScheduler = new AsyncContextTaskScheduler(this);
            _taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.HideScheduler, TaskContinuationOptions.HideScheduler, _taskScheduler);
        }

        public int Id => _taskScheduler.Id;

        private void OperationStarted()
        {
            var newCount = Interlocked.Increment(ref _outstandingOperations);
        }

        private void OperationCompleted()
        {
            var newCount = Interlocked.Decrement(ref _outstandingOperations);
            if (newCount == 0)
                _queue.CompleteAdding();
        }

        private void Enqueue(Task task, bool propagateExceptions)
        {
            OperationStarted();
            task.ContinueWith(_ => OperationCompleted(), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, _taskScheduler);
            _queue.TryAdd(task, propagateExceptions);
        }

        public void Dispose()
        {
            _queue.Dispose();
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Execute()
        {
            using (new SynchronizationContextHelpers.SynchronizationContextSwitcher(_synchronizationContext))
            {
                var tasks = _queue.GetConsumingEnumerable();
                foreach (var task in tasks)
                {
                    _taskScheduler.DoTryExecuteTask(task.Item1);

                    // Propagate exception if necessary.
                    if (task.Item2)
                        task.Item1.WaitAndUnwrapException();
                }
            }
        }

        public static void Run(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (var context = new AsyncContext())
            {
                var task = context._taskFactory.Run(action);
                context.Execute();
                task.WaitAndUnwrapException();
            }
        }

        public static TResult Run<TResult>(Func<TResult> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (var context = new AsyncContext())
            {
                var task = context._taskFactory.Run(action);
                context.Execute();
                return task.WaitAndUnwrapException();
            }
        }

        public static void Run(Func<Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            // ReSharper disable AccessToDisposedClosure
            using (var context = new AsyncContext())
            {
                context.OperationStarted();
                var task = context._taskFactory.Run(action).ContinueWith(t =>
                {
                    context.OperationCompleted();
                    t.WaitAndUnwrapException();
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, context._taskScheduler);
                context.Execute();
                task.WaitAndUnwrapException();
            }
            // ReSharper restore AccessToDisposedClosure
        }

        public static TResult Run<TResult>(Func<Task<TResult>> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            // ReSharper disable AccessToDisposedClosure
            using (var context = new AsyncContext())
            {
                context.OperationStarted();
                var task = context._taskFactory.Run(action).ContinueWith(t =>
                {
                    context.OperationCompleted();
                    return t.WaitAndUnwrapException();
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, context._taskScheduler);
                context.Execute();
                return task.WaitAndUnwrapException();
            }
            // ReSharper restore AccessToDisposedClosure
        }

        public static AsyncContext Current
        {
            get
            {
                var syncContext = SynchronizationContext.Current as AsyncContextSynchronizationContext;
                if (syncContext == null)
                {
                    return null;
                }

                return syncContext.Context;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public SynchronizationContext SynchronizationContext => _synchronizationContext;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public TaskScheduler Scheduler => _taskScheduler;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public TaskFactory Factory => _taskFactory;

        [DebuggerNonUserCode]
        internal sealed class DebugView
        {
            private readonly AsyncContext _context;

            public DebugView(AsyncContext context)
            {
                _context = context;
            }

            public TaskScheduler TaskScheduler => _context._taskScheduler;
        }
    }
}
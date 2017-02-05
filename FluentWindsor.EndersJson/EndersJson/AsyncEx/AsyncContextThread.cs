using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx.Synchronous;

namespace FluentlyWindsor.EndersJson.AsyncEx
{
    [DebuggerTypeProxy(typeof(DebugView))]
    public sealed class AsyncContextThread : SingleDisposable<AsyncContext>
    {
        private readonly Task _thread;

        private static AsyncContext CreateAsyncContext()
        {
            var result = new AsyncContext();
            result.SynchronizationContext.OperationStarted();
            return result;
        }

        private AsyncContextThread(AsyncContext context)
            : base(context)
        {
            Context = context;
            _thread = Task.Factory.StartNew(Execute, CancellationToken.None, TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        public AsyncContextThread()
            : this(CreateAsyncContext())
        {
        }

        public AsyncContext Context { get; }

        private void Execute()
        {
            using (Context)
            {
                Context.Execute();
            }
        }

        private void AllowThreadToExit()
        {
            Context.SynchronizationContext.OperationCompleted();
        }

        public Task JoinAsync()
        {
            Dispose();
            return _thread;
        }

        public void Join()
        {
            JoinAsync().WaitAndUnwrapException();
        }

        protected override void Dispose(AsyncContext context)
        {
            AllowThreadToExit();
        }

        public TaskFactory Factory => Context.Factory;

        [DebuggerNonUserCode]
        internal sealed class DebugView
        {
            private readonly AsyncContextThread _thread;

            public DebugView(AsyncContextThread thread)
            {
                _thread = thread;
            }

            public AsyncContext Context => _thread.Context;

            public object Thread => _thread._thread;
        }
    }
}
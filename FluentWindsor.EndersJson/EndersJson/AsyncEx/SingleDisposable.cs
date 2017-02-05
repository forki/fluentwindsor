using System;
using System.Threading;

namespace FluentlyWindsor.EndersJson.AsyncEx
{
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
}
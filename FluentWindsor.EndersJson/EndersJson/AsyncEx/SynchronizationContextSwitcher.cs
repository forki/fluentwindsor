using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentlyWindsor.EndersJson.AsyncEx
{
    public sealed class SynchronizationContextSwitcher : SingleDisposable<object>
    {
        private readonly SynchronizationContext _oldContext;

        public SynchronizationContextSwitcher(SynchronizationContext newContext)
            : base(new object())
        {
            _oldContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(newContext);
        }

        protected override void Dispose(object context)
        {
            SynchronizationContext.SetSynchronizationContext(_oldContext);
        }

        public static void NoContext(Action action)
        {
            using (new SynchronizationContextSwitcher(null))
                action();
        }

        public static Task NoContextAsync(Func<Task> action)
        {
            using (new SynchronizationContextSwitcher(null))
                return action();
        }
    }
}
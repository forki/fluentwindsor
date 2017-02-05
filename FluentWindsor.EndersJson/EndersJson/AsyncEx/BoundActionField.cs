using System;
using System.Threading;

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
}
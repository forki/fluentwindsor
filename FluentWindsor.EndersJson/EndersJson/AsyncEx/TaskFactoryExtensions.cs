using System;
using System.Threading.Tasks;

namespace FluentlyWindsor.EndersJson.AsyncEx
{
    public static class TaskFactoryExtensions
    {
        public static Task Run(this TaskFactory @this, Action action)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return @this.StartNew(action, @this.CancellationToken, @this.CreationOptions | TaskCreationOptions.DenyChildAttach, @this.Scheduler ?? TaskScheduler.Default);
        }

        public static Task<TResult> Run<TResult>(this TaskFactory @this, Func<TResult> action)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return @this.StartNew(action, @this.CancellationToken, @this.CreationOptions | TaskCreationOptions.DenyChildAttach, @this.Scheduler ?? TaskScheduler.Default);
        }

        public static Task Run(this TaskFactory @this, Func<Task> action)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return @this.StartNew(action, @this.CancellationToken, @this.CreationOptions | TaskCreationOptions.DenyChildAttach, @this.Scheduler ?? TaskScheduler.Default).Unwrap();
        }

        public static Task<TResult> Run<TResult>(this TaskFactory @this, Func<Task<TResult>> action)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return @this.StartNew(action, @this.CancellationToken, @this.CreationOptions | TaskCreationOptions.DenyChildAttach, @this.Scheduler ?? TaskScheduler.Default).Unwrap();
        }
    }
}
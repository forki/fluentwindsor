using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentlyWindsor.EndersJson.AsyncEx
{
    public static class TaskExtensions
    {
        public static void WaitAndUnwrapException(this Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            task.GetAwaiter().GetResult();
        }

        public static void WaitAndUnwrapException(this Task task, CancellationToken cancellationToken)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            try
            {
                task.Wait(cancellationToken);
            }
            catch (AggregateException ex)
            {
                throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
            }
        }

        public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            return task.GetAwaiter().GetResult();
        }

        public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task, CancellationToken cancellationToken)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            try
            {
                task.Wait(cancellationToken);
                return task.Result;
            }
            catch (AggregateException ex)
            {
                throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
            }
        }

        public static void WaitWithoutException(this Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            try
            {
                task.Wait();
            }
            catch (AggregateException)
            {
            }
        }

        public static void WaitWithoutException(this Task task, CancellationToken cancellationToken)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            try
            {
                task.Wait(cancellationToken);
            }
            catch (AggregateException)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
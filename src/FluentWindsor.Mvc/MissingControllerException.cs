using System;

namespace FluentlyWindsor.Mvc
{
    internal class MissingControllerException : Exception
    {
        public MissingControllerException(string message) : base(message)
        {
        }
    }
}
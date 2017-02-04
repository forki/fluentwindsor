using System;
using Castle.Core;
using FluentWindsor.Hawkeye;

namespace Hawkeye.Tests.Dummies
{
	[Interceptor(typeof(FluentWindsor.Hawkeye.Hawkeye))]
	public class TestLoggingTarget
	{
		[Log(LogLevel.Debug)]
		public virtual void DebugMethod()
		{
			Console.WriteLine("DebugMethod called ... ");
		}

		[Log(LogLevel.Info)]
		public virtual void InfoMethod()
		{
			Console.WriteLine("InfoMethod called ... ");
		}

		[Log(LogLevel.Debug)]
		public virtual void ExceptionMethod()
		{
			throw new Exception("A test error that should be visible to the logging framework ... ");
		}
	}
}
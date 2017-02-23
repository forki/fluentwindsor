<img align="left" src="https://avatars0.githubusercontent.com/u/7360948?v=3" />

&nbsp;Hawkeye<br /><br />
=============

| Version | Build |
|---------|---------|
|  <a href= "https://www.nuget.org/packages/Hawkeye.Windsor/"><img src="https://img.shields.io/nuget/v/FluentWindsor.svg" /></a> |  <a href= "https://ci.appveyor.com/project/fir3pho3nixx/fluentwindsor"><img src="https://ci.appveyor.com/api/projects/status/8nj9cgfnw9spqbpr/branch/master?svg=true" /></a> |

Need a quick and dirty logging solution that services both development and production scenarios then you have come to the right place!
This project uses deep Castle Windsor integration with log4net via FluentWindsor.

##How it works

A whirlwind tour of how you get this going.

###Log2Console

First go and grab yourself the latest copy of log2console.

https://log2console.codeplex.com/

This is quite a mature log viewing application which you can use to see log messages. Once you have this installed, you are ready to
begin tweaking your applications configuration.

###Tweaking your configuration

First start by embedding the following section header in your config file:

``` xml
<configSections>
	<section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
</configSections>
```

Next we will implement the configuration for log4net, which should look something like so:

``` xml
<log4net>
  <appender name="udp" type="FluentlyWindsor.Hawkeye.Appenders.UdpAppender, FluentWindsor.Hawkeye">
    <remoteAddress value="localhost" />
    <remotePort value="7071" />
    <layout type="log4net.Layout.XmlLayoutSchemaLog4j" />
  </appender>
  <appender name="eventLog" type="log4net.Appender.EventLogAppender">
    <applicationName value="FluentWindsor.Hawkeye.Examples" />
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
    </layout>
    <filter type="log4net.Filter.LevelRangeFilter">
      <levelMin value="ERROR" />
      <levelMax value="FATAL" />
    </filter>
  </appender>
  <root>
    <level value="ALL" />
    <appender-ref ref="udp" />
    <appender-ref ref="eventLog" />
  </root>
</log4net>
```

It is important to note that there are 2 appenders which are configured with different log levels. The first is the UDP appender
which has the most verbose log level set and will show you everything for any log viewer that is setup with this appender. The 
benefit of this approach is that you dont have a log file filling up the hard disk in case you mis-configured it accidentally. The 
log messages will simply be dropped off the network stack. 

The second appender is the windows event log appender which is here to support production deployment scenarios. It is also less verbose
and will only log items that are deemed to be errors(exceptions) or fatal errors that cause the application to crash.

###Implementing logging in your code

Let's walkthrough a quick example of how you would go about implementing logging in your code. First you need to make sure that you 
have registered hawkeye with your castle windsor container. There is a WindsorInstaller in the root of this library. If you are 
using FluentWindsor then you do not have to do anything, installation is handled for you :)

Next let's look at how we implement logging on a simple service method. Here is an example of one that does not do very much: 

``` csharp
public class MyService {
	public void MyMethod(){ /* ... does stuff ... */ }
}
```

This would be registered with your castle windsor container by using the following registration code: 

``` csharp
container.Register(Component.For<MyService>().Lifestyle.Transient);
```

If you want to trace or trap errors for when MyMethod executes, the first change you would have to make is
by wrapping your service with the Hawkeye interceptor like so:

``` csharp
[Interceptor(typeof(Hawkeye))]
public class MyService {
	public void MyMethod(){ /* ... does stuff ... */ }
}
```

This then wraps the service in what is called a proxy. Next you make the method visible to logging making the method virtual, this
allows the interceptor to do it's job of observing method calls to `MyMethod`. Finally you finish up by attaching a log level to 
the method. 

``` csharp
[Interceptor(typeof(Hawkeye))]
public class MyService {
	[Log(LogLevel.Info)]
	public void MyMethod(){ /* ... does stuff ... */ }
}
```

Now each time the method is hit, it will emit a log via log4net with the category of INFO. If the method throws an uncaught exception, 
it will escalate the log level to error and also be logged into the windows event log. 

###Viewing log events in Log2Console

Once you fire up log2console for the first time it will ask you to setup an appender. Simply choose a UDP appender and be sure that 
you have the IPv6 flag set to true and you should be good to go! Please screen shot below for how to configure the appender. 

![Receiver Config](https://raw.githubusercontent.com/cryosharp/fluentwindsor/master/Images/log2console-receiver.png "Receiver Config")

If you open the solution I have also included an example where a console app makes some requests to a WebApi implementation. Another
screenshot of the log2console output is included below. 

![Log2Console](https://raw.githubusercontent.com/cryosharp/fluentwindsor/master/Images/log2console.png "Log2Console")

##A Castle Windsor version

There is also a castle windsor version of this cache which can be auto loaded using FluentWindsor. For more about how you can use
this please see https://github.com/cryosharp/fluentwindsor.

##Problems

For any problems please sign into github and raise issues.



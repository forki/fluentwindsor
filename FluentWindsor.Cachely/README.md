<img align="left" src="https://avatars0.githubusercontent.com/u/7360948?v=3" />

&nbsp;Cachely<br /><br />
=============

| Version | Build |
|---------|---------|
| <a href= "https://www.nuget.org/packages/Cachely.Windsor/"><img src="https://img.shields.io/nuget/v/FluentWindsor.svg" /></a> | <a href= "https://ci.appveyor.com/project/fir3pho3nixx/fluentwindsor"><img src="https://ci.appveyor.com/api/projects/status/8nj9cgfnw9spqbpr/branch/master?svg=true" /></a> |

This is a naive sliding expiration cache. If you need something quick and dirty that is easy to learn and
battle tested then hopefully this will solve your problem.

## How it works

First start by pulling the latest package from NuGet. After doing so you can create your cache by using the following code

``` csharp
var cache = new Cache<string>();
```

This example will create a new cache that stores `string` values. In reality you could store any class or native type of 
your choosing. So if for arguments sake we wanted to cache a user class:

``` csharp
public class User 
{ 
	public string Username { get;set; } 
	public string Password { get;set; } 
}
```

Then you would simply create the cache by using the following code:

``` csharp
var userCache = new Cache<User>();
```

## Sliding expiration of items

There is an additional constructor which allows you to pass a TimeSpan as a parameter so you can set the expiry or alternatively
once the cache is created you could also use the `SetExpiry` member to vary the sliding expiration like so:

``` csharp
var userCache = new Cache<User>(TimeSpan.FromMinutes(1));
```

This will expire items that have not been read or written for longer than a minute. 

## Reading, writing and expiry

Given you have constructed your cache, you can use the following method to write objects into the cache. 

``` csharp
userCache.SetItem("1", new User(){ Username = "Gav", Password = "t0ps3cr3t" });
```

Getting them backout is as simple as:

``` csharp
var user = userCache.GetItem("1");
```

If you decide you no longer want the item to be in the cache, then you can expire it like so:

``` csharp
userCache.ExpireItem("1");
```

Subsequent requests for the same key will yield a default(T) value. 

## More about the expiry

To give you a bit more detail about the expiry mechanism of the cache, let's say we have the following example: 

``` csharp
var userCache = new Cache<User>();
userCache.SetExpiry(TimeSpan.FromMinutes(1));
```

A user cache which will expire any item that has not been read or written for longer than a minute. See the code below
for a further breakdown how the expiry mechanism works.

``` csharp
// This happens at 09:00:00, with expiry set to 09:01:00
userCache.SetItem("1", new User(){ Username = "Gav", Password = "t0ps3cr3t" }); 

// This happens at 09:00:30 and pushes expiry to 09:01:30
var user = userCache.GetItem("1"); 

// This happens at 09:01:00 and pushes expiry to 09:02:00
userCache.SetItem("1", new User(){ Username = "Gav", Password = "t0ps3cr3t" }); 

// This happens at 09:03:00 and produces null
var user = userCache.GetItem("1"); 
```

## Why do I care?

Well if you care about performance then you might want to switch out. Cachely vs Microsoft.Runtime.Caching.MemoryCache and ~~System.Web.Caching.Cache~~

	Cachely -> Reads: 1728, Creates: 3057, Deletes: 3034 in 5 second(s)
	MemoryCache -> Reads: 503, Creates: 2269, Deletes: 2164 in 5 second(s)
	ASP.NET Web Cache -> Awful, dont bother ... 

Please see performance tests.

## A Castle Windsor version

There is also a castle windsor version of this cache which can be auto loaded using FluentWindsor. For more about how you can use
this please see https://github.com/cryosharp/fluentwindsor.

## Problems?

For any problems please sign into github and raise issues.

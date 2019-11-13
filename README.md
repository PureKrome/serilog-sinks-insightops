# Serilog.Sinks.InsightOps

A sink for Serilog that writes events to insightOps by Rapid7.
 

A Serilog sink that writes log events to insightOps via TCP or HTTPS. This sink is also configured for the most common scenario's - an easy way to get started for most people. As such some advanced features are (by design) left out of this sink.

## Getting started

To use the console sink, first install the [NuGet package](https://nuget.org/packages/serilog.sinks.insightops):

```powershell
Install-Package Serilog.Sinks.InsightOps
```

Next, define you insightOps account settings:

```csharp
var settings = new InsightOpsSettings
{
    Region = "<to fill in by you>",
    Token = "<to fill in by you>",
    UseSsl = false // or True for sending via HTTPS
};
```

Then enable the sink using `WriteTo.InsightOps()`:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.InsightOps(settings)
    .CreateLogger();
```

And now log something. Here's an example of some semantic logging:

```csharp
var position = new { Latitude = 25, Longitude = 134 };
var elapsedMs = 34;
log.Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
```

Log events will look like this at insightOps:

```
13 Nov 2019 00:59:47.645 Processed { Latitude: 25, Longitude: 134 } in 034 ms.
```

### Log view
![](https://i.imgur.com/DxlKNA4.jpg)

### Table view
![](https://imgur.com/h0x5el3.jpg)

---


Copyright &copy; 2019 Serilog Contributors - Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html).

See also: [Serilog Documentation](https://github.com/serilog/serilog/wiki)

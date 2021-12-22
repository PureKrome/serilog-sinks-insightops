# Serilog.Sinks.InsightOps

A sink for Serilog that writes events to insightOps by Rapid7.
 

A Serilog sink that writes log events to insightOps via TCP or HTTPS. This sink is also configured for the most common scenario's - an easy way to get started for most people. As such some advanced features are (by design) left out of this sink.

## Getting started (simple, text based logging)

To use the console sink, first install the [NuGet package](https://nuget.org/packages/serilog.sinks.insightops):

```powershell
Install-Package Serilog.Sinks.InsightOps
```

Next, define you insightOps account settings:

```csharp
var settings = new InsightOpsSinkSettings
{
    Region = "<to fill in by you>", // au, eu, jp or us
    Token = "<to fill in by you>", // Guid, taken from your InsightOps log account
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


## Structured Logging

To get structured logging with insightOps, we will need to send the data (over the wire) as JSON.  
To do that, we need to do the following:

```csharp
Log.Logger = new LoggerConfiguration()
    // 🤘🏻 Notice how we've defined the JSON formatter! 🤘🏻
    .WriteTo.InsightOps(settings, new RenderedCompactJsonFormatter())
    .CreateLogger();
```

and this will now send the data up to insightOps as Structed Logging:

### Log view
![image](https://user-images.githubusercontent.com/899878/147096634-9fb3a69a-7784-44ba-8918-287d6536ff82.png)

### Table View
![image](https://user-images.githubusercontent.com/899878/147096758-e1868a51-6b14-499c-9049-7503c98e4c47.png)

For the record, there are the types of JSON data formats you can use:

- `JsonFormatter()`
- `CompactJsonFormatter()`
- `RenderedCompactJsonFormatter()`

For more detailed explanation of these, [this is a blog post](https://nblumhardt.com/2016/07/serilog-2-0-json-improvements/) from the Serilog author.

---


Copyright &copy; 2019 Serilog Contributors - Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html).

See also: [Serilog Documentation](https://github.com/serilog/serilog/wiki)

<h1 align="center">Serilog Sinks: InsightOps</h1>

<div align="center">
  A sink for Serilog that writes events to insightOps by Rapid7.
</div>

<br />

<div align="center">
    <!-- License -->
    <a href="https://choosealicense.com/licenses/apache-2.0/">
    <img src="https://img.shields.io/github/license/PureKrome/serilog-sinks-insightops" alt="License - Apache 2.0" />
    </a>
    <!-- NuGet -->
    <a href="https://www.nuget.org/packages/Serilog.Sinks.InsightOps/">
    <img src="https://buildstats.info/nuget/Serilog.Sinks.InsightOps" alt="NuGet" />
    </a>
    <!-- Github Actions -->
    <a href="https://github.com/PureKrome/serilog-sinks-insightops/actions/workflows/MergeToMain.yml">
    <img src="https://github.com/PureKrome/serilog-sinks-insightops/actions/workflows/MergeToMain.yml/badge.svg" alt="Merge Pull Requests into 'main'" />
    </a>
</div>


A Serilog sink that writes log events to insightOps via TCP or HTTPS.  
  
This sink is also configured for the most common scenario's - an easy way to get started for most people. As such some advanced features are (by design) left out of this sink.

<hr/>

### Table of Contents
- [Getting started (simple, text based logging)](#getting-started-simple-text-based-logging)
- [Advanced: loading InsightOps settings via configuration file](#more-advanced-getting-started-loading-settings-via-configuration-file)
- [Structured Logging](#structured-logging)
- [Advanced Structured Logging: loading InsightOps settings via configuration file](#structured-logging-advanced-settings-via-configuration-file)

<hr/>

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

## More advanced getting started (loading settings via configuration file)

Probably the best way to load the configuration settings is via your `appSettings.config` file(s).

Here's a lovely example:

1. ⚠ Make sure you install the `Serilog.Sinks.InsightOps` nuget package, otherwise the Serilog won't be able to load the configuration settings.
2. Add the relevant section to your `appSettings.config` file(s)
    - `Using` section
    - `WriteTo` section
    - `Name` and `Args` key/values.

![image](https://user-images.githubusercontent.com/899878/148729173-f2a6f368-15fa-4c61-930a-9432bcf34957.png)

<details>
<summary>Example appSettings.json code (copy/paste friendly)</summary>

```
{
    "Serilog": {
        "Using": [ "Serilog.Sinks.InsightOps" ],
        "MinimumLevel": {
            "Default": "Debug",
            "Override": {
                "System": "Debug",
                "Microsoft": "Debug"
            }
        },
        "WriteTo": [
            {
                "Name": "Console"
            },
            {
                "Name": "InsightOps",
                "Args": {
                    "Token": "<to be manually set>",
                    "Region": "au",
                    "UseSsl": "true"
                }
            }
        ]
    }
}
```

</details>

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
- `CompactJsonFormatter()` [This has no `@m` "message" property. Only the `@mt` "message template"]
- `RenderedCompactJsonFormatter()` [This has an `@m` message property, plus other values]

## Structured Logging (Advanced) : Settings via configuration file

Here's an example section of loading the settings via the `appSettings.config` file:

Note the `Formatter` arg.

```
{
    "Serilog": {
        "Using": [ "Serilog.Sinks.InsightOps" ],
        "MinimumLevel": {
            "Default": "Debug",
            "Override": {
                "System": "Debug",
                "Microsoft": "Debug"
            }
        },
        "WriteTo": [
            {
                "Name": "Console"
            },
            {
                "Name": "InsightOps",
                "Args": {
                    "Token": "<to be manually set>",
                    "Region": "au",
                    "UseSsl": "true",
                    "Formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter, Serilog.Formatting.Compact"
                }
            }
        ]
    }
}
```

For more detailed explanation of these, [this is a blog post](https://nblumhardt.com/2016/07/serilog-2-0-json-improvements/) from the Serilog author.

---


Copyright &copy; 2019 Serilog Contributors - Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html).

See also: [Serilog Documentation](https://github.com/serilog/serilog/wiki)

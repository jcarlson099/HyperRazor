# HyperRazor
Tool to assist with rendering Razor components directly to HTML.

This is intended to be used with an API to return HTML content suitable for use with HTMX.

Requires .NET 8 for the RenderComponentAsync method.

## Example Usage
Example uses a simple minimal API to return HTML for a counter component.

### Program.cs
```csharp 
using <ComponentNamespace>;
using Microsoft.AspNetCore.Components.Web;
using static HyperRazor.ComponentToHtml;
...

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();

//Is there a better way to get the ILoggerFactory?
ILoggerFactory loggerFactory = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();

var app = builder.Build();

await using var htmlRenderer = new HtmlRenderer(app.Services, loggerFactory);

var counter = 0;
app.MapGet("/counter", async () =>
{
    var paramsDict = new Dictionary<string, object?>
    {
        { "CurrentCount", counter }
    };
    return await ComponentToHtmlAsync<Counter, MainLayout>(htmlRenderer, paramsDict);
});
app.MapPost("/counter", () => {
    counter++; return counter;
});

app.UseStaticFiles();
app.Run();

```


### Counter
```razor
<h1>Counter</h1>

<p>Current count: <span id="count">@currentCount</span></p>

<button hx-post="/counter"
    hx-target="#count"
    >Click me</button>

@code {
    [Parameter]
    public int currentCount { get; set; }
}
```
namespace HyperRazor;
public static class ComponentToHtml
{
    public static async Task<IResult> ComponentToHtmlAsync<T>(HtmlRenderer htmlRenderer, Dictionary<string, object?>? parameters = null) 
        where T : IComponent
    {
        return await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var paramView = parameters == null ? ParameterView.Empty : ParameterView.FromDictionary(parameters);
            var output = await htmlRenderer.RenderComponentAsync<T>(paramView);
            return Results.Extensions.Html(output.ToHtmlString());
        });
    }

    public static async Task<IResult> ComponentToHtmlAsync<T, TLayout>(HtmlRenderer htmlRenderer, Dictionary<string, object?>? parameters = null)
        where T : ComponentBase
        where TLayout : LayoutComponentBase
    {
        return await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var paramsDict = new Dictionary<string, object?>
            {
                { "Component", typeof(T) },
                { "Layout", typeof(TLayout) },
                { "Parameters", parameters }
            };
            var paramView = ParameterView.FromDictionary(paramsDict);
            var output = await htmlRenderer.RenderComponentAsync<App>(paramView);
            return Results.Extensions.Html(output.ToHtmlString());
        });
    }
}

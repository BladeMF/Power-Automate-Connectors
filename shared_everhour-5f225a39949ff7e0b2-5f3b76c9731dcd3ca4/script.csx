public class Script : ScriptBase
{
public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        if (response.IsSuccessStatusCode)
        {
            if (this.Context.OperationId == "Trigger")
            {
                return await this.AddLocationHeader(response).ConfigureAwait(false);
            }
        }
        return response;
    }
    private async Task<HttpResponseMessage> AddLocationHeader(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        var result = JObject.Parse(responseString);
        response.Headers.Add("Location", "https://api.everhour.com/hooks/" + result["id"]);
        return response;
    }
}
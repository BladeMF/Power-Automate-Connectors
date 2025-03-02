
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PowerAutomateScript;

public class Script : ScriptBase
{
	public override async Task<HttpResponseMessage> ExecuteAsync()
	{
		if (this.Context.OperationId == "FindWithinRange")
		{
			return await this.FindWithinRangeOperation().ConfigureAwait(false);
		}
		else if (this.Context.OperationId == "AlignValuesToColumns")
		{
			return await this.AlignValuesToColumns().ConfigureAwait(false);
		}

		// Handle an invalid operation ID
		return GetBadRequestResponse(
			$"Unknown operation ID '{this.Context.OperationId}'"
		);
	}

	private static HttpResponseMessage GetBadRequestResponse(string message)
	{
		HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
		response.Content = CreateJsonContent(message);
		return response;
	}

	private async Task<HttpResponseMessage> AlignValuesToColumns()
	{
		HttpResponseMessage response;
		if (this.Context.Request.Content == null)
		{
			return GetBadRequestResponse(
					$"Request content is null."
			);
		}
		var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
		var contentAsJson = JObject.Parse(contentAsString);
		if (contentAsJson["values"] is not JObject values)
		{
			return GetBadRequestResponse($"'values' is missing or null.");
		}
		if (contentAsJson["columns"]?["value"] is not JArray columns)
		{
			return GetBadRequestResponse($"'columns' or 'columns.value' is missing or null.");
		}

		var result = new JArray();
		foreach (var column in columns.Cast<JObject>())
		{
			string? columnName = column["name"]?.ToString();
			if (columnName == null)
			{
				return GetBadRequestResponse($"A column is missing a name.");
			}
			result.Add(values[columnName] ?? JValue.CreateNull());
		}
		response = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = CreateJsonContent(result.ToString())
		};
		return response;
	}

	private async Task<HttpResponseMessage> FindWithinRangeOperation()
	{
		if (this.Context.Request.Content == null)
		{
			return GetBadRequestResponse(
				$"Request content is null."
			);
		}
		var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
		var contentAsJson = JObject.Parse(contentAsString);
		var search = (string?)contentAsJson["search"];
		if (search == null)
		{
			return GetBadRequestResponse($"'search' is missing or null.");
		}
		if (contentAsJson["range"]?["values"] is not JArray range)
		{
			return GetBadRequestResponse($"'range' or 'range/values' is missing or null.");
		}

		var index = -1;
		var indices = new List<int>();
		foreach (var value in range.Cast<JArray>())
		{
			index++;
			if (search == value[0].ToString())
			{
				indices.Add(index);
			}
		}
		return new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = CreateJsonContent(new JArray(indices.ToArray()).ToString())
		};
	}
}

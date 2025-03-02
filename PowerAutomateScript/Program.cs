// See https://aka.ms/new-console-template for more information
// 45614.16:18:19.2183589
// 1900-01-01T00:00:00.0
// 2024-11-20T16:18:19.2183589
using Newtonsoft.Json.Linq;

var now = new DateTime(2024, 11, 20);
var excelBase = new DateTime(1899, 12, 30);
var diff = now.Subtract(excelBase);
// Excel incorrectly treats 1900 as a leap year, 
// so we add 1 to the total days for dates after 28th Feb 1900
Console.WriteLine(diff.TotalDays);
Console.WriteLine(now.ToOADate());

// JObject obj = new JObject();
// obj["key1"] = 123;
// obj["key2"] = "val2";
// Console.WriteLine(obj["key3"]);

// var contentAsJson = JObject.Parse(@"{
//     ""search"": ""1739614567.906519"",
//     ""range"": {
//         ""address"": ""Разходи!N2:N144"",
//         ""addressLocal"": ""Разходи!N2:N144"",
//         ""columnCount"": 1,
//         ""cellCount"": 143,
//         ""columnHidden"": false,
//         ""rowHidden"": false,
//         ""columnIndex"": 13,
//         ""hidden"": false,
//         ""rowCount"": 143,
//         ""rowIndex"": 1,
//         ""Values"": [
//             [
//                 ""1739614567.906519""
//             ]
//         ]
//     }
// }");
// var search = (string?)contentAsJson["search"];
// if (search == null)
// {
//     Console.WriteLine($"'search' is missing or null.");
// }
// else
// {
//     Console.Write(search);
// }
// if (contentAsJson["range"]?["values"] is not JArray range)
// {
//     Console.WriteLine($"'range' or 'range.values' is missing or null.");
// }
// else
// {
//     Console.Write(range);
// }

var indices = new List<int>();
indices.Add(1);
indices.Add(2);
Console.WriteLine(new JArray(indices.ToArray()));

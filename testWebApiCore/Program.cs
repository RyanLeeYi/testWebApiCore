using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

string url = $"http://10.20.16.153:2234/";
string signInJsonString = readJsonFile(System.Environment.CurrentDirectory + @"\json2.json");
string sendMessageJsonString = readJsonFile(System.Environment.CurrentDirectory + @"\json1.json");

string response = "";
response = await GetToken(signInJsonString);
Console.WriteLine(response);

JWT_TokenResponse tokenResponse = JsonConvert.DeserializeObject<JWT_TokenResponse>(response);

response = await SendLineNotifyMassage(tokenResponse.token, sendMessageJsonString);

Console.WriteLine(response);

async Task<string> GetToken(string Data)
{
    var httpClient = new HttpClient();

    HttpContent contentPost = new StringContent(Data, Encoding.UTF8, "application/json");

    HttpResponseMessage result = await httpClient.PostAsync(url + "signin", contentPost);

    string res = await result.Content.ReadAsStringAsync();

    return res;
}

async Task<string> QueryJwtID(string token)
{
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

    HttpResponseMessage result = await httpClient.GetAsync($"http://10.20.16.153:8888/jwtid");

    string res = await result.Content.ReadAsStringAsync();

    return res;
}
async Task<string> SendLineNotifyMassage(string token,string data)
{
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

    HttpContent contentPost = new StringContent(data, Encoding.UTF8, "application/json");

    HttpResponseMessage result = await httpClient.PostAsync(url + $"LineNotify/SendMessage/NoProxy", contentPost);

    string res = await result.Content.ReadAsStringAsync();

    return res;
}

string readJsonFile(string filePath)
{
    StreamReader r = new StreamReader(filePath, Encoding.UTF8);
    string jsonString = r.ReadToEnd();
    r.Dispose();
    return jsonString;
}
class JWT_TokenResponse
{
    public string token { get; set; } 
}
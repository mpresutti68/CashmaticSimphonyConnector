// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.Source.CashmaticApi
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using CashmaticSimphonyConnector.Exceptions;
using CashmaticSimphonyConnector.Models;
using Micros.PosCore.Extensibility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace CashmaticSimphonyConnector.Source;

internal class CashmaticApi
{
  private readonly ExtensibilityAppLogger Logger;

  public string IpAddress { get; set; }

  public string Username { get; set; }

  public string Password { get; set; }

  public CashmaticApi(
    string ipAddress,
    string username,
    string password,
    ExtensibilityAppLogger logger)
  {
    this.IpAddress = ipAddress;
    this.Username = username;
    this.Password = password;
    this.Logger = logger;
    System.Threading.Timer timer = new System.Threading.Timer(new TimerCallback(this.TimerRenewTokenCallback), (object) null, 0, 60000);
  }

  private string TokenAccess { get; set; }

  private string TokenRenew { get; set; }

  private void TimerRenewTokenCallback(object state)
  {
    try
    {
      this.RenewToken();
    }
    catch (Exception ex)
    {
      this.Logger.Log((ELogLevel)(-3), $"Cashmatic API: Renew token failed {ex}");
    }
  }

  private async Task<JObject> ExecuteHttpCommand(string cmd, JObject data = null, bool canRetry = true)
  {
    string requestUri = $"https://{this.IpAddress}:50301/api/{cmd}";
    HttpClient httpClient = new HttpClient((HttpMessageHandler) new HttpClientHandler()
    {
      ServerCertificateCustomValidationCallback = (Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>) ((message, cert, chain, errors) => true)
    })
    {
      Timeout = TimeSpan.FromSeconds(5.0)
    };
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.TokenAccess);
    HttpResponseMessage httpResponseMessage;
    try
    {
      if (data == null)
        data = new JObject();
      StringContent content = new StringContent(((JToken) data).ToString((Formatting) 0, Array.Empty<JsonConverter>()), Encoding.UTF8, "application/json");
      httpResponseMessage = await httpClient.PostAsync(requestUri, (HttpContent) content);
    }
    catch (HttpRequestException ex)
    {
      if (ex.InnerException is WebException innerException && innerException.Status == WebExceptionStatus.ConnectFailure)
        throw new DeviceNotConnectedException();
      throw;
    }
    catch (TaskCanceledException ex)
    {
      throw new DeviceTimeoutException();
    }
    if ((httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized || httpResponseMessage.StatusCode == HttpStatusCode.Forbidden) && canRetry)
    {
      await this.Login();
      return await this.ExecuteHttpCommand(cmd, data, false);
    }
    JObject jobject = httpResponseMessage.StatusCode == HttpStatusCode.OK ? JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync()) : throw new CashmaticApiException((int) (1000 + httpResponseMessage.StatusCode), httpResponseMessage.ReasonPhrase);
    if (jobject["code"].ToObject<int>() != 0)
      throw new CashmaticApiException(jobject["code"].ToObject<int>(), jobject["message"].ToObject<string>());
    return jobject.ContainsKey(nameof (data)) ? jobject[nameof (data)].ToObject<JObject>() : new JObject();
  }

  private async Task Login()
  {
    JObject data = new JObject();
    data.Add("username", (JToken)this.Username);
    data.Add("password", (JToken)this.Password);
    data.Add("token", (JToken)this.TokenRenew);
    this.TokenAccess = (await this.ExecuteHttpCommand("user/Login", data, false))["token"].ToObject<string>();
  }

  private JObject ExecuteHttpCommandSync(string cmd, JObject data = null, bool canRetry = true)
  {
    Exception raisedEx = (Exception) null;
    JObject response = (JObject) null;
    Thread thread = new Thread((ThreadStart) (() =>
    {
      try
      {
        response = this.ExecuteHttpCommand(cmd, data, canRetry).Result;
      }
      catch (Exception ex)
      {
        raisedEx = ex;
      }
    }));
    thread.Start();
    thread.Join();
    if (raisedEx != null)
      throw raisedEx.InnerException;
    return response;
  }

  private void RenewToken()
  {
    this.TokenAccess = this.ExecuteHttpCommandSync("user/RenewToken")["token"].ToObject<string>();
  }

  public void StartPayment(int amount)
  {
    JObject data = new JObject();
    data.Add(nameof (amount), (JToken)amount);
    this.ExecuteHttpCommandSync("transaction/StartPayment", data);
  }

  public void CancelPayment() => this.ExecuteHttpCommandSync("transaction/CancelPayment");

  public Transaction ActiveTransaction()
  {
    return ((JToken) this.ExecuteHttpCommandSync("device/ActiveTransaction")).ToObject<Transaction>();
  }

  public Transaction LastTransaction()
  {
    return ((JToken) this.ExecuteHttpCommandSync("device/LastTransaction")).ToObject<Transaction>();
  }
}

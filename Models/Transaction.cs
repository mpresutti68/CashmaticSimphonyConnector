// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.Models.Transaction
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using Newtonsoft.Json;

#nullable disable
namespace CashmaticSimphonyConnector.Models;

internal class Transaction
{
  [JsonProperty("operation")]
  public string Operation { get; set; } = string.Empty;

  [JsonProperty("requested")]
  public int Requested { get; set; }

  [JsonProperty("inserted")]
  public int Inserted { get; set; }

  [JsonProperty("dispensed")]
  public int Dispensed { get; set; }

  [JsonProperty("currency")]
  public string Currency { get; set; }

  [JsonProperty("notDispensed")]
  public int NotDispensed { get; set; }

  [JsonProperty("end")]
  public string End { get; set; }
}

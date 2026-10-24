// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.Models.Settings
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace CashmaticSimphonyConnector.Models;

internal class Settings
{
  public string Username { get; set; } = string.Empty;

  public string Password { get; set; } = string.Empty;

  public string IpAddress { get; set; } = string.Empty;

  public int PaymentNumber { get; set; } = 101;

  public int RoundingPoint { get; set; }

  public string CultureRequestedAmount { get; set; } = string.Empty;

  public string CultureInsertedAmount { get; set; } = string.Empty;

  public void Parse(string settings)
  {
    string[] source = settings.Split(new string[1]
    {
      Environment.NewLine
    }, StringSplitOptions.None);
    string str1 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.Contains("username")));
    string str2;
    if (str1 == null)
      str2 = (string) null;
    else
      str2 = ((IEnumerable<string>) str1.Split('=')).Last<string>();
    if (str2 == null)
      str2 = string.Empty;
    this.Username = str2;
    string str3 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.Contains("password")));
    string str4;
    if (str3 == null)
      str4 = (string) null;
    else
      str4 = ((IEnumerable<string>) str3.Split('=')).Last<string>();
    if (str4 == null)
      str4 = string.Empty;
    this.Password = str4;
    string str5 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.Contains("ip_address")));
    string str6;
    if (str5 == null)
      str6 = (string) null;
    else
      str6 = ((IEnumerable<string>) str5.Split('=')).Last<string>();
    if (str6 == null)
      str6 = string.Empty;
    this.IpAddress = str6;
    string str7 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.Contains("payment_number")));
    string s1;
    if (str7 == null)
      s1 = (string) null;
    else
      s1 = ((IEnumerable<string>) str7.Split('=')).Last<string>();
    int num1 =0 ;
    ref int local1 = ref num1;
    if (int.TryParse(s1, out local1))
      this.PaymentNumber = num1;
    string str8 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.Contains("rounding_point")));
    string s2;
    if (str8 == null)
      s2 = (string) null;
    else
      s2 = ((IEnumerable<string>) str8.Split('=')).Last<string>();
    int num2 = 0;
    ref int local2 = ref num2;
    if (int.TryParse(s2, out local2))
      this.RoundingPoint = num2;
    string str9 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.Contains("culture_requested_amount")));
    string str10;
    if (str9 == null)
      str10 = (string) null;
    else
      str10 = ((IEnumerable<string>) str9.Split('=')).Last<string>().Trim();
    if (str10 == null)
      str10 = string.Empty;
    this.CultureRequestedAmount = str10;
    string str11 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.Contains("culture_inserted_amount")));
    string str12;
    if (str11 == null)
      str12 = (string) null;
    else
      str12 = ((IEnumerable<string>) str11.Split('=')).Last<string>().Trim();
    if (str12 == null)
      str12 = string.Empty;
    this.CultureInsertedAmount = str12;
  }
}

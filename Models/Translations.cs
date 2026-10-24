// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.Models.Translations
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace CashmaticSimphonyConnector.Models;

internal class Translations
{
  public string LblRequested { get; set; }

  public string LblInserted { get; set; }

  public string LblDispensed { get; set; }

  public string LblNotDispensed { get; set; }

  public string LblCancel { get; set; }

  public string LblOk { get; set; }

  public void Parse(string translations)
  {
    if (translations == null)
      return;
    string[] source = translations.Split(new string[1]
    {
      Environment.NewLine
    }, StringSplitOptions.None);
    string str1 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.StartsWith("lbl_requested")));
    string str2;
    if (str1 == null)
      str2 = (string) null;
    else
      str2 = ((IEnumerable<string>) str1.Split('=')).Last<string>();
    if (str2 == null)
      str2 = string.Empty;
    this.LblRequested = str2;
    string str3 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.StartsWith("lbl_inserted")));
    string str4;
    if (str3 == null)
      str4 = (string) null;
    else
      str4 = ((IEnumerable<string>) str3.Split('=')).Last<string>();
    if (str4 == null)
      str4 = string.Empty;
    this.LblInserted = str4;
    string str5 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.StartsWith("lbl_dispensed")));
    string str6;
    if (str5 == null)
      str6 = (string) null;
    else
      str6 = ((IEnumerable<string>) str5.Split('=')).Last<string>();
    if (str6 == null)
      str6 = string.Empty;
    this.LblDispensed = str6;
    string str7 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.StartsWith("lbl_not_dispensed")));
    string str8;
    if (str7 == null)
      str8 = (string) null;
    else
      str8 = ((IEnumerable<string>) str7.Split('=')).Last<string>();
    if (str8 == null)
      str8 = string.Empty;
    this.LblNotDispensed = str8;
    string str9 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.StartsWith("lbl_cancel")));
    string str10;
    if (str9 == null)
      str10 = (string) null;
    else
      str10 = ((IEnumerable<string>) str9.Split('=')).Last<string>();
    if (str10 == null)
      str10 = string.Empty;
    this.LblCancel = str10;
    string str11 = ((IEnumerable<string>) source).FirstOrDefault<string>((Func<string, bool>) (x => x.StartsWith("lbl_ok")));
    string str12;
    if (str11 == null)
      str12 = (string) null;
    else
      str12 = ((IEnumerable<string>) str11.Split('=')).Last<string>();
    if (str12 == null)
      str12 = string.Empty;
    this.LblOk = str12;
  }

  public void Initialize(string lang)
  {
    switch (lang)
    {
      case "it":
        this.LblRequested = string.IsNullOrEmpty(this.LblRequested) ? "Richiesto:" : this.LblRequested;
        this.LblInserted = string.IsNullOrEmpty(this.LblInserted) ? "Inserito:" : this.LblInserted;
        this.LblDispensed = string.IsNullOrEmpty(this.LblDispensed) ? "Erogato:" : this.LblDispensed;
        this.LblNotDispensed = string.IsNullOrEmpty(this.LblNotDispensed) ? "Non erogato:" : this.LblNotDispensed;
        this.LblCancel = string.IsNullOrEmpty(this.LblCancel) ? "Annulla" : this.LblCancel;
        this.LblOk = string.IsNullOrEmpty(this.LblOk) ? "OK" : this.LblOk;
        break;
      case "es":
        this.LblRequested = string.IsNullOrEmpty(this.LblRequested) ? "Solicitado:" : this.LblRequested;
        this.LblInserted = string.IsNullOrEmpty(this.LblInserted) ? "Insertado:" : this.LblInserted;
        this.LblDispensed = string.IsNullOrEmpty(this.LblDispensed) ? "Dispensado:" : this.LblDispensed;
        this.LblNotDispensed = string.IsNullOrEmpty(this.LblNotDispensed) ? "No dispensado:" : this.LblNotDispensed;
        this.LblCancel = string.IsNullOrEmpty(this.LblCancel) ? "Cancelar" : this.LblCancel;
        this.LblOk = string.IsNullOrEmpty(this.LblOk) ? "OK" : this.LblOk;
        break;
      default:
        this.LblRequested = string.IsNullOrEmpty(this.LblRequested) ? "Requested:" : this.LblRequested;
        this.LblInserted = string.IsNullOrEmpty(this.LblInserted) ? "Inserted:" : this.LblInserted;
        this.LblDispensed = string.IsNullOrEmpty(this.LblDispensed) ? "Dispensed:" : this.LblDispensed;
        this.LblNotDispensed = string.IsNullOrEmpty(this.LblNotDispensed) ? "Not dispensed:" : this.LblNotDispensed;
        this.LblCancel = string.IsNullOrEmpty(this.LblCancel) ? "Cancel" : this.LblCancel;
        this.LblOk = string.IsNullOrEmpty(this.LblOk) ? "OK" : this.LblOk;
        break;
    }
  }
}

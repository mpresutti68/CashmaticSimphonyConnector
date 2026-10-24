// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.Application
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CashmaticSimphonyConnector.DialogsContext;
using CashmaticSimphonyConnector.Models;
using CashmaticSimphonyConnector.Source;
using Micros.Ops;
using Micros.Ops.Extensibility;
using Micros.PosCore.Extensibility;
using Micros.PosCore.Extensibility.UserInterface;
using static System.Collections.Specialized.BitVector32;

#nullable disable
namespace CashmaticSimphonyConnector;

public class Application : OpsExtensibilityApplication
{
  private readonly Settings settings = new Settings();
  private CashmaticApi api;
  private readonly Translations translations = new Translations();

  public Application(IExecutionContext context)
    : base(context)
  {
    this.Logger.LogAlways("DLL init");
    this.settings.Parse(this.DataStore.ReadExtensionApplicationContentTextByNameKey(this.OpsContext.RvcID, this.ApplicationName, "Settings"));
    ReadDataExtension();
    this.api = new CashmaticApi(this.settings.IpAddress, this.settings.Username, this.settings.Password, this.Logger);
    this.translations.Parse(this.DataStore.ReadExtensionApplicationContentTextByNameKey(this.OpsContext.RvcID, this.ApplicationName, "Translations"));
    this.translations.Initialize(this.GetCurrentLanguage());
  }

    private void ReadDataExtension()
    {
        int wksid = OpsContext.WorkstationID;
        string nomecolip = "CashMatic_ipaddrress";
        string nomecoluser = "CashMatic_user";
        string nomecolpwd = "CashMatic_pwd";
        string table = "Workstation";
        string[] campi = DataStore.ReadExtensionDataValues(table);
        if (campi.Contains(nomecolip)) settings.IpAddress = DataStore.ReadExtensionDataValue(table, nomecolip, wksid);
        if (campi.Contains(nomecoluser)) settings.Username = DataStore.ReadExtensionDataValue(table, nomecoluser, wksid);
        if (campi.Contains(nomecolpwd)) settings.Password = DataStore.ReadExtensionDataValue(table, nomecolpwd, wksid);
        
    }

  [ExtensibilityMethod]
  public void DoPaymentExtensionMethod()
  {
    try
    {
      this.Logger.LogAlways($"DoPaymentExtensionMethod with culture {this.OpsContext.CurrentCultureInfo} with total due {this.OpsContext.CheckTotalDue}");
      double result;
      if (!double.TryParse(this.OpsContext.CheckTotalDue, NumberStyles.Any, (IFormatProvider) new CultureInfo(string.IsNullOrEmpty(this.settings.CultureRequestedAmount) ? this.OpsContext.CurrentCultureInfo : this.settings.CultureRequestedAmount), out result))
        this.Logger.Log((ELogLevel) (-1), "Parsing amount failed");
      else if (result <= 0.0)
      {
        this.Logger.Log((ELogLevel) (-1), "No amount provided " + this.OpsContext.CheckTotalDue);
      }
      else
      {
        int amount = (int) (result * 100.0);
        if (this.settings.RoundingPoint > 0)
        {
          int num = amount;
          amount = (int) (Math.Round((double) amount / (double) this.settings.RoundingPoint) * (double) this.settings.RoundingPoint);
          if (num != amount)
            this.Logger.Log((ELogLevel) (-1), $"Payment amount rounded from {num} to {amount}");
        }
        this.Logger.Log((ELogLevel) (-1), $"Starting payment for {amount}");
        try
        {
          this.api.StartPayment(amount);
        }
        catch (Exception ex)
        {
          this.Logger.Log((ELogLevel) (-3), ex.Message);
          this.OpsContext.ShowError(ex.Message);
          return;
        }
        PaymentDialogContext paymentDialogContext = new PaymentDialogContext(ref this.api, this.Logger, this.translations);
        ExtensibilityInPlaceHtmlDialogParameters dialogParameters = new ExtensibilityInPlaceHtmlDialogParameters();
        dialogParameters.HTML = this.GetHtmlDialogFromResource("PaymentDialog");
        ((ExtensibilityHtmlDialogParameters) dialogParameters).Context = (object) paymentDialogContext;
        this.OpsContext.ShowExtensibilityHtmlDialog((ExtensibilityHtmlDialogParameters) dialogParameters);
        if (paymentDialogContext.TransactionDetails == null)
        {
          this.OpsContext.ShowError("Transaction window closed forcefully");
        }
        else
        {
          if (!(paymentDialogContext.TransactionDetails.End == "normal"))
            return;
          CultureInfo provider = string.IsNullOrEmpty(this.settings.CultureInsertedAmount) ? CultureInfo.CurrentCulture : new CultureInfo(this.settings.CultureInsertedAmount);
          string str = ((double) paymentDialogContext.TransactionDetails.Inserted / 100.0).ToString("F2", (IFormatProvider) provider) ?? "";
          this.Logger.Log((ELogLevel) (-1), "Closed payment with " + str);
          new OpsChangeDueRequest().AmountTendered = str;
          this.OpsContext.ProcessCommand(new OpsCommand()
          {
            Command = (OpsCommandType) 2008,
            Number = (long) this.settings.PaymentNumber,
            Arguments = "Cash:Cash $ " + str
          });
        }
      }
    }
    catch (Exception ex)
    {
      this.Logger.Log((ELogLevel) (-3), $"Unhandled exception {ex.Message} - {ex}");
      throw;
    }
  }

  private string GetCurrentLanguage()
  {
    try
    {
      object container = this.OpsContext.Container;
      if (container != null)
      {
        string fullName = container.GetType().FullName;
      }
      string str = container?.GetType().GetProperty("Language")?.GetValue(container)?.ToString();
      this.Logger.LogAlways("Language: " + str);
      return str.Split('-')[0].ToString() ?? "en";
    }
    catch
    {
      return "en";
    }
  }

  private string GetHtmlDialogFromResource(string dialogName)
  {
    string name = $"CashmaticSimphonyConnector.Dialogs.{dialogName}.html";
    using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
    {
      using (StreamReader streamReader = new StreamReader(manifestResourceStream))
        return streamReader.ReadToEnd();
    }
  }
}

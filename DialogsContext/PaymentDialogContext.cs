// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.DialogsContext.PaymentDialogContext
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using CashmaticSimphonyConnector.Models;
using CashmaticSimphonyConnector.Source;
using Micros.PosCore.Extensibility;
using System;
using System.Threading;

#nullable disable
namespace CashmaticSimphonyConnector.DialogsContext;

internal class PaymentDialogContext
{
  private bool _CloseWindow;
  private int SafeExitCnt;
  private readonly CashmaticApi api;
  private readonly ExtensibilityAppLogger logger;
  private readonly Thread bgUiThread;
  private readonly string dialogLanguage;
  private readonly Translations translations;

  public int Requested { get; set; }

  public int Inserted { get; set; }

  public int Dispensed { get; set; }

  public int NotDispensed { get; set; }

  public string Currency { get; set; }

  public bool CloseWindow
  {
    get => this._CloseWindow;
    set
    {
      this._CloseWindow = value;
      this.bgUiThread.Abort();
    }
  }

  public bool CancelEnabled
  {
    get => this.Inserted < this.Requested && this.Dispensed <= 0 && this.NotDispensed <= 0;
  }

  public Transaction TransactionDetails { get; set; }

  public bool ShowNotDispensedWarning => this.NotDispensed > 0;

  public PaymentDialogContext(
    ref CashmaticApi api,
    ExtensibilityAppLogger logger,
    Translations translations)
  {
    this.api = api;
    this.logger = logger;
    this.dialogLanguage = string.IsNullOrWhiteSpace(this.dialogLanguage) ? "en" : this.dialogLanguage;
    this.translations = translations;
    this.bgUiThread = new Thread(new ThreadStart(this.BgUiThreadSrc));
    this.bgUiThread.Start();
  }

  public string GetRequested() => this.FormatAmount(this.Requested);

  public string GetInserted() => this.FormatAmount(this.Inserted);

  public string GetDispensed() => this.FormatAmount(this.Dispensed);

  public string GetNotDispensed() => this.FormatAmount(this.NotDispensed);

  public string GetCurrency() => this.Currency;

  public bool GetCloseWindow() => this.CloseWindow;

  public bool CanCancel() => this.CancelEnabled;

  public bool GetNotDispensedWarning() => this.ShowNotDispensedWarning;

  public string GetLblRequested() => this.translations.LblRequested;

  public string GetLblInserted() => this.translations.LblInserted;

  public string GetLblDispensed() => this.translations.LblDispensed;

  public string GetLblNotDispensed() => this.translations.LblNotDispensed;

  public string GetLblCancel() => this.translations.LblCancel;

  public string GetLblOk() => this.translations.LblOk;

  public void BtnCancelClick()
  {
    try
    {
      this.api.CancelPayment();
    }
    catch (Exception ex)
    {
      this.logger.Log((ELogLevel) (-3), $"Payment dialog context {ex}");
    }
  }

  public void BtnSafeExitClick()
  {
    if (++this.SafeExitCnt <= 10)
      return;
    this.logger.LogAlways("Payment window closed by safe exit button");
    this.CloseWindow = true;
  }

  public void BtnNotDispensedOkClick()
  {
    this.logger.Log((ELogLevel) (-1), "Payment dialog: Not dispensed OK pressed");
    this.CloseWindow = true;
  }

  private string FormatAmount(int amount)
  {
    string str = amount.ToString().PadLeft(3, '0');
    return str.Insert(str.Length - 2, ".");
  }

  private void BgUiThreadSrc()
  {
    try
    {
      while (true)
      {
        try
        {
          Transaction transaction = this.api.ActiveTransaction();
          if (transaction.Operation == "idle")
          {
            this.TransactionDetails = this.api.LastTransaction();
            this.Requested = this.TransactionDetails.Requested;
            this.Inserted = this.TransactionDetails.Inserted;
            this.Dispensed = this.TransactionDetails.Dispensed;
            this.NotDispensed = this.TransactionDetails.NotDispensed;
            break;
          }
          this.Requested = transaction.Requested;
          this.Inserted = transaction.Inserted;
          this.Dispensed = transaction.Dispensed;
          this.NotDispensed = transaction.NotDispensed;
          this.Currency = transaction.Currency;
        }
        catch (Exception ex)
        {
          this.logger.Log((ELogLevel) (-3), $"Payment dialog background ui thread: {ex}");
        }
        Thread.Sleep(200);
      }
      if (this.NotDispensed != 0)
        return;
      this.CloseWindow = true;
    }
    catch (ThreadAbortException ex)
    {
    }
  }
}

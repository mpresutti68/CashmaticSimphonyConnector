// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.Exceptions.CashmaticApiException
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using System;

#nullable disable
namespace CashmaticSimphonyConnector.Exceptions;

internal class CashmaticApiException : Exception
{
  public int ErrorCode { get; set; }

  public string ErrorMessage { get; set; } = string.Empty;

  public CashmaticApiException(int errorCode, string errorMessage)
    : base($"Cashmatic API exception: ({errorCode}) {errorMessage}")
  {
    this.ErrorCode = errorCode;
    this.ErrorMessage = errorMessage;
  }
}

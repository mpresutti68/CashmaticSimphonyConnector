// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.Exceptions.DeviceTimeoutException
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using System;

#nullable disable
namespace CashmaticSimphonyConnector.Exceptions;

internal class DeviceTimeoutException : Exception
{
  public DeviceTimeoutException()
    : base("Timeout exception")
  {
  }
}

// Decompiled with JetBrains decompiler
// Type: CashmaticSimphonyConnector.ApplicationFactory
// Assembly: CashmaticSimphonyConnector, Version=1.0.0.10, Culture=neutral, PublicKeyToken=null
// MVID: 1B2BEFA3-0A5C-40E6-8C89-B2C04B72419E
// Assembly location: C:\Progetti\CashMatic\CashmaticSimphonyConnector.dll

using Micros.PosCore.Extensibility;

#nullable disable
namespace CashmaticSimphonyConnector;

public class ApplicationFactory : IExtensibilityAssemblyFactory
{
  public ExtensibilityAssemblyBase Create(IExecutionContext context)
  {
    return (ExtensibilityAssemblyBase) new Application(context);
  }

  public void Destroy(ExtensibilityAssemblyBase app) => app.Destroy();
}

# RequestR with Autofac

## Getting Started

### 1) Setup Services

**Note**: Include `RequestR.Extensions.Autofac` nuget package.

```csharp
ContainerBuilder containerBuilder = new ContainerBuilder();
containerBuilder.AddRequestBus();
```

### 2) Setup the Request Bus

Retrieve the `RequestBus` instance and call the `RegisterAllHandlers()` method.

```csharp
RequestBus requestBus = container.Resolve<RequestBus>();
requestBus.RegisterAllHandlers();
```

### 3) Send request

```csharp
PresentProductsRequest request = new PresentProductsRequest();
MyResponse response = requestBus.Send<MyRequest, MyResponse>(request);
```

# bronto
:sauropod:Bronto is a Progressive Web Application (PWA) that accesses stock market data using .NET 8 and Blazor.

### Demo
The image shows Blazor WASM application execution.\
![Blazor WASM Candlestick Image](https://github.com/rdw100/bronto/blob/main/Bronto/Bronto.Stocks.Pwa/wwwroot/img/Candlestick.JPG)

### Design - Overview
```mermaid
---
title: Integrating a Third-Party API in ASP.NET Web API Project
---

flowchart TD
    User([User])-->|Calls|PWA
    PWA-->|Integrates|Consumer([Consumer])
    Consumer-->|Requests|API
    API-->|Accesses|API_Provider([API Provider])

    style User stroke:Blue,stroke-width:4px
    style PWA stroke:Indigo,stroke-width:4px
    style Consumer stroke:Green,stroke-width:4px
    style API stroke:Orange,stroke-width:4px
    style API_Provider stroke:Red,stroke-width:4px

```
 ### Design - Structure
    Solution
    │     
    ├── Presentation PWA               <- The project UI logic.
    │   └── Pages
    │       └── Stock.cs
    │
    ├── Presentation API               <- The project API logic.
    │   └── Controllers
    │       └── StockController.cs
    │
    ├── Application                    <- The project services (use cases/features).
    │   └── Services
    │       └── StockServices.cs
    │
    ├── Domain                         <- The project business logic.
    │   ├── Entities/Models
    │   │   └── Stock.cs
    │   ├── Interfaces
    │   └── Shared
    │
    ├── Infrastucture                  <- The project infrastructure.
    │   ├── Data
    │   │   └── StockData.cs
    │   ├── HttpClients
    │   │   └── StockCient.cs
    │   └── Caching
    │
    ├── Tests             	           <- The project Unit tests for each layer.
    │   ├── ApplicationTests
    │   ├── DomainTests
    │   └── InfrastructureTests    
    │
    ├── Bronto.sln
    └── README.md


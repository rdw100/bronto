# bronto
:sauropod:Bronto is a Progressive Web Application (PWA) that accesses stock market data using .NET 8 and Blazor.

```mermaid
---
title: Integrating a Third-Party API in ASP.NET Web API Project
---
flowchart LR
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
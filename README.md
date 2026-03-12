# Vault - .NET Console Accounting & Inventory Engine

![.NET](https://img.shields.io/badge/.NET-10.0-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)

Vault is a monolithic console application built on **.NET 10.0**. It acts as a lightweight Enterprise Resource Planning (ERP) engine, focusing on precise financial calculations, batch-based inventory state management, and strict audit trailing without relying on an external RDBMS.

## 🏗️ Architecture & State Management

The application follows a simplified Service-Oriented Architecture (SOA) pattern, separating Domain Entities, Business Logic (Services), and Data State.

* **In-Memory State (`DataBase.cs`):** Acts as the persistence layer using static `List<T>` collections. It simulates Primary Key (PK) auto-incrementation and Foreign Key (FK) relationships.
* **Precision Financials:** All monetary properties utilize the C# `decimal` structure (128-bit) to prevent floating-point rounding errors common in `double` or `float` during financial aggregations.
* **Culture-Agnostic Parsing:** Date inputs are handled via `DateTime.TryParseExact` with `DateTimeStyles.None` to ensure strict parsing (e.g., `dd.MM.yyyy`) regardless of the host OS regional settings.

## 🗄️ Domain Models & Schema

The system uses a 1:N relational mapping concept between `Product` and `PurchaseBatch` to isolate historical cost data from current market catalog prices.

| Entity | PK / FK | Key Properties | Data Types | Business Purpose |
| :--- | :--- | :--- | :--- | :--- |
| `Product` | `ProductId` (PK) | `StockCode`, `BaseSalePrice` | `string`, `decimal` | Master catalog and current retail pricing. |
| `PurchaseBatch`| `BatchId` (PK), `ProductId` (FK) | `QuantityRemaining`, `PurchasePrice`, `ExpirationDate` | `int`, `decimal`, `DateTime` | Tracks historical cost-basis and expiry per shipment. |
| `SalesTransaction`| `SaleId` (PK), `ProductId` (FK) | `QuantitySold`, `TotalCost`, `SalePrice`, `PerformedBy` | `int`, `decimal`, `decimal`, `string` | Immutable ledger entry for revenue, COGS, and audit. |
| `User` | `UserId` (PK) | `Username`, `Title` | `string`, `string` | Session context for audit logging (`PerformedBy`). |

## ⚙️ Core Algorithms: FEFO & COGS

The most critical component is the `AccountingService.SellProduct()` method, which implements the **First-Expired, First-Out (FEFO)** algorithm combined with dynamic **Cost of Goods Sold (COGS)** calculation.

### Execution Pipeline:
1.  **LINQ Filtering & Sorting:**
    ```csharp
    var batchesToUse = DataBase.Purchases
        .Where(b => b.ProductId == prodId && b.QuantityRemaining > 0)
        .OrderBy(b => b.ExpirationDate)
        .ToList();
    ```
    *Filters out depleted stock (O(N)) and sorts available batches chronologically by expiration (O(N log N)).*

2.  **State Mutation (The Fulfillment Loop):**
    Iterates through `batchesToUse`. It mutates the `QuantityRemaining` state of each batch. If a single batch cannot fulfill the requested `qtyToSell`, the loop zeroes out the current batch, aggregates its historical `PurchasePrice` into the `TotalCost` accumulator, and shifts the remaining deficit to the next chronological batch.

3.  **Profit Margin Integrity:** Because `TotalCost` is aggregated *per original batch cost* rather than the current catalog average, the calculated Net Profit (`Revenue - TotalCost`) remains 100% accurate even under hyper-inflationary data entries.

## 📊 Reporting & Data Aggregation

The `ReportingService` leverages complex LINQ pipelines to generate financial statements in real-time.

* **P&L Statement (Date-Range):** Uses `Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)` to isolate transactions. Aggregates data using `.Sum()` for metrics like Total Revenue, Total COGS, and Net Profit.
* **Balance Sheet (Net Worth):** Dynamically calculates Total Assets by evaluating the active inventory at its *historical purchase cost*, not its retail value:
    ```csharp
    decimal warehouseValue = DataBase.Purchases.Sum(b => b.QuantityRemaining * b.PurchasePrice);
    ```
* **Console UI Formatting:** Implements fixed-width string interpolation (e.g., `String.Format("{0,-5} | {1,-20}...", ...)` to maintain strict tabular alignment in standard standard output (STDOUT).

## 🚀 CLI Compilation & Execution

To compile and run the project via the .NET CLI:

```bash
# Navigate to the project directory containing Vault.csproj
cd Vault

# Build the project
dotnet build

# Run the compiled executable
dotnet run

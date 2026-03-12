# 🏦 Vault - Enterprise Console Accounting & Inventory System

![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Architecture](https://img.shields.io/badge/Architecture-FEFO%20%2F%20FIFO-blue?style=for-the-badge)
[LICENSE](LICENSE)
Vault is a highly robust, console-based Accounting and Inventory Management application developed in C#. It is designed to simulate real-world enterprise Resource Planning (ERP) mechanics, moving beyond basic CRUD operations by implementing advanced financial algorithms, batch-based inventory tracking, and dynamic reporting.

## 📑 Table of Contents
1. [System Architecture & Business Logic](#-system-architecture--business-logic)
2. [The FEFO Algorithm](#-the-fefo-algorithm)
3. [Entity Relationships](#-entity-relationships)
4. [Financial Calculations](#-financial-calculations)
5. [Core Features](#-core-features)
6. [Installation & Usage](#-installation--usage)

---

## 🧠 System Architecture & Business Logic

Most beginner inventory applications use a single integer to track stock. **Vault uses a Batch-Based (Parti) tracking system.** This is crucial for handling real-world economic variables like inflation and varying expiration dates.

| Feature | Standard Beginner App | Vault Architecture |
| :--- | :--- | :--- |
| **Stock Tracking** | Single `StockCount` integer per product. | Array of `PurchaseBatch` objects per product. |
| **Cost Basis** | Averages the cost or loses historical data. | Remembers the exact `PurchasePrice` of every batch. |
| **Expiration** | Cannot track multiple expiration dates. | Tracks `ExpirationDate` for every single batch independently. |
| **Profit Math** | Gross estimation based on current prices. | Exact calculation (COGS) matching the sold item to its original batch cost. |

---

## ⚙️ The FEFO Algorithm 
**(First-Expired, First-Out)**

Vault implements a dynamic FEFO algorithm in its `AccountingService`. When a user initiates a sale, the system doesn't just subtract a number; it intelligently sources the items.

**Step-by-Step Execution:**
1. **Query:** Retrieves all active batches for the requested Product ID where `QuantityRemaining > 0`.
2. **Sort:** Uses LINQ to `.OrderBy(batch => batch.ExpirationDate)`.
3. **Fulfill & Deduct:** - Deducts the requested quantity from the oldest batch.
   - If the oldest batch doesn't have enough stock, it zeroes out that batch, carries over the remaining required quantity, and moves to the next oldest batch.
4. **Log:** Records the exact historical cost of those specific items to generate 100% accurate profit margins.

---

## 🗄️ Entity Relationships

The in-memory database simulates a relational database structure. Here is how the core entities interact:

| Entity | Key Properties | Role in System |
| :--- | :--- | :--- |
| **User** | `UserId`, `Username`, `Title` | Handles authentication and Audit Logging (who did what). |
| **Product** | `ProductId`, `BaseSalePrice` | The master catalog. Holds the *current* market sale price. |
| **PurchaseBatch**| `BatchId`, `QuantityRemaining`, `PurchasePrice`, `ExpirationDate` | Represents a specific delivery from a supplier. Holds *historical* cost data. |
| **SalesTransaction**| `SaleId`, `QuantitySold`, `TotalCost`, `PerformedBy` | The permanent record of a sale, including the exact profit margin and the user's audit signature. |

---

## 📊 Financial Calculations

Vault acts as a mini-accountant, calculating real-time financial health using the following logic:

* **COGS (Cost of Goods Sold):** Calculated dynamically during the FEFO loop. `Sum of (Sold Batch Qty * Batch Original Purchase Price)`
* **Net Profit per Sale:** `(Total Sale Revenue) - (COGS)`
* **Total Inventory Value:** Evaluated at cost, not retail. `Sum of (QuantityRemaining * PurchasePrice)` for all active batches.
* **Company Net Worth:** `(Cash In Register + Total Inventory Value) - Total Debt`

---

## 🚀 Core Features

* **📦 Catalog & Inventory Management:** Define products, receive new shipments, and automatically update sale prices across the board.
* **🛒 Smart Sales Engine:** Sell products with automatic FEFO stock deduction and real-time cash register updates.
* **💳 Debt Management:** Pay off supplier debt directly from the cash register.
* **📅 Date-Range Reporting:** Generate Profit & Loss (P&L) reports for specific months/years using advanced LINQ filtering.
* **🏢 Dynamic Balance Sheet:** View total assets, liabilities, and real-time company net worth.
* **🔐 Audit Logging:** Every purchase and sale transaction logs the `PerformedBy` tag (e.g., *Zeynepnur Gold (Admin)*) for security and accountability.

---

## 💻 Installation & Usage

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/yourusername/Vault.git](https://github.com/yourusername/Vault.git)

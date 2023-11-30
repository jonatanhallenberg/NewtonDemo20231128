# Övning - UML / Transactions / Trigger

### Steg 1: Skapa Databasen och Tabellerna

Först skapar vi en ny databas och två tabeller: `Konto` och `Transaktion`.

```sql
CREATE DATABASE BankDB;
GO
USE BankDB;
GO

CREATE TABLE Account (
    AccountID INT PRIMARY KEY,
    CustomerName NVARCHAR(100),
    Balance DECIMAL(18, 2)
);
GO

CREATE TABLE Transaction (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    AccountID INT,
    Type NVARCHAR(50),
    Amount DECIMAL(18, 2),
    TransactionDate DATETIME DEFAULT GETDATE()
);
GO

```

### Steg 2: Skapa ett UML-diagram över datamodellen

- Använd https://app.diagrams.net/
- Skapa ett diagram som illustrerar tabellerna Account och Transaction

### Steg 3: Lägga till testdata

Lägg till några testkonton i `Account`-tabellen.

```sql
INSERT INTO Account (AccountID, CustomerName, Balance) VALUES (1, 'Jonatan Hallenberg', 1000);
INSERT INTO Account (AccountID, CustomerName, Balance) VALUES (2, 'Anna Svensson', 500);
GO
```

### Steg 4: Skapa Stored Procedure för Transaktioner

Skapa en stored procedure för att hantera överföringar mellan konton.

```sql
CREATE PROCEDURE PerformTransaction
    @FromAccountID INT,
    @ToAccountID INT,
    @Amount DECIMAL(18, 2)
AS
BEGIN
    BEGIN TRANSACTION;

    UPDATE Account SET Balance = Balance - @Amount WHERE AccountID = @FromAccountID;
    UPDATE Account SET Balance = Balance + @Amount WHERE AccountID = @ToAccountID;

    INSERT INTO Transaction (AccountID, Type, Amount) VALUES (@FromAccountID, 'Withdrawal', @Amount);
    INSERT INTO Transaction (AccountID, Type, Amount) VALUES (@ToAccountID, 'Deposit', @Amount);

    COMMIT;
END;
GO
```

### Steg 5: Testa Stored Procedure

Testa den skapade stored proceduren genom att utföra en överföring.

```sql
EXEC PerformTransaction @FromAccountID = 1, @ToAccountID = 2, @Amount = 200;
GO
```

### Steg 6: Kontrollera Resultatet

Kontrollera att saldot har uppdaterats korrekt och att transaktionsposterna har lagts till.

```sql
SELECT * FROM Account;
SELECT * FROM Transaction;
GO
```

### Steg 7: Simulera en Felaktig Transaktion

Simulera en situation där transaktionen bör misslyckas och därför behöver återställas (rollback).

```sql
CREATE PROCEDURE TestFailedTransaction
    @FromAccountID INT,
    @ToAccountID INT,
    @Amount DECIMAL(18, 2)
AS
BEGIN
    BEGIN TRANSACTION;

    UPDATE Account SET Balance = Balance - @Amount WHERE AccountID = @FromAccountID;

    -- Simulate an error here
    DECLARE @FakeError INT = 1 / 0;

    UPDATE Account SET Balance = Balance + @Amount WHERE AccountID = @ToAccountID;

    IF @@ERROR <> 0
        ROLLBACK;
    ELSE COMMIT;
END;
GO
```

### Steg 8: Testa om rollback fungerar

- Upprepa Steg 5 och 6 fast med *TestFailedTransaction* och kontrollera att ingen data uppdateras i Account.

### Steg 9: Skapa `TransactionLog` Tabell och Trigger

Skapa en `TransactionLog` tabell och en Trigger som loggar varje transaktion.

```sql
CREATE TABLE TransactionLog (
    LogID INT PRIMARY KEY IDENTITY,
    TransactionID INT,
    LogDate DATETIME DEFAULT GETDATE()
);
GO

CREATE TRIGGER LogTransaction ON Transaction
AFTER INSERT
AS
BEGIN
    INSERT INTO TransactionLog (TransactionID)
    SELECT TransactionID FROM inserted;
END;
GO
```

- Testa igen Steg 4 och 5 (med `PerformTransaction`) och kontrollera att en rad skapas i TransactioLog-tabellen med hjälp av den nya TRIGGER:n

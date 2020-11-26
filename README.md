# BDSA2020

## Setup secret database connection string

* First move to the ``BDSA2020.Entities`` directory
* Then run the following commands:

``` PowerShell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ConnectionString" " $connectionString"
```

To view your user-secret run the command: ``dotnet user-secrets list``

## Update database

* Move to the ``BDSA2020.Entities`` directory
* First add the new migration: ``dotnet ef migrations add <NameOfMigration> --startup-project ../BDSA2020.Api/``
* Update database: ``dotnet ef database update --startup-project ../BDSA2020.Api/``

## How to run the App

1. Follow the above steps first to make sure you have setup a database
2. Start the API
    * Go to the ``BDSA2020.Api`` directory
    * Run the command ``dotnet run``
    * The Api will then be avaliable at [https://localhost:5000](https://localhost:5000).

3. Start up the GUI
    * In a different terminal go to the ``BDSA2020.View`` directory
    * Run the command ``dotnet run``
    * The GUI can be seen at [https://localhost:4000](https://localhost:4000).

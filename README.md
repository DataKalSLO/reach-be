# hourglass-be

## Requirements
[.NET Core SDK](https://dotnet.microsoft.com/download)

## Recommended
[Visual Studio 2019 Community](https://visualstudio.microsoft.com/downloads/)

## To Run
- Ensure that you have two untracked files in the top of the `HourglassServer` directory: `appsettings.json` and `appsettings.Development.json` (Visual Studio should add these by default). In `appsettings.Development.json`, add the following in the top level of the json object.
```
"ConnectionStrings": {
   "HourglassDatabase": "our Postgres connection string here"
}
```

### Note
- If you see syntax that looks like SQL in API controllers like this:
```
return (from degrees in _context.Degrees
        where degrees.University == university
        group degrees by degrees.Year into groupedDegrees
        select new XYGraphData
```
It is probably [LINQ query langauge](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/basic-linq-query-operations) (syntactic sugar for LINQ library). When using LINQ, feel free to use [method syntax](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/method-based-query-syntax-examples-projection). An example of this in our code can be found [here](https://github.com/CPSECapstone/hourglass-be/blob/0a6779bdfbbb84c1f521f2e4fb3d74c51619e5b8/HourglassServer/Controllers/DummyController.cs#L25).

- When updating the database schema, run the following dotnet command: `dotnet ef dbcontext scaffold "our connection string here" Npgsql.EntityFrameworkCore.PostgreSQL -o Data`. Note: you will most likely need to install `dotnet-ef` by running `dotnet tool install --global dotnet-ef` This command looks at the database schema and updates the corresponding classes in our code. You should then commit the changes. More information can be found [here](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet).

## Reverse Engineer/Scaffoling  Script

These instruction use the command line interface provided by .NET to run the scaffold tool.
More detailed instructions can be found here: https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding

#### Preliminary Step: Specify Tables to Reverse Engineer
1. Make sure that HourglassServer/TablesToReverseEngineer.txt has a list of all the tables that 
you want created into classes. This file should have a table name per line. Trailing newline should not matter.

#### 1. Install the Command Line Interface for .net (CLI Tools)

https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

#### 2. Install Reverse Engineering Tool, Scaffold:

'dotnet tool install --global dotnet-ef'

#### 3. Install Postgress NuGet Driver (should be installed if project is able to run)

https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL

#### 4. Run script reverse_engineer_database_table

python reverse_engineer_database_table.py

or

python3 reverse_engineer_database_table.py

## Resources
https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio

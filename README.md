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

## Note
- If you see syntax that looks like SQL in API controllers like this:
```
return (from degrees in _context.Degrees
        where degrees.University == university
        group degrees by degrees.Year into groupedDegrees
        select new XYGraphData
```
It is probably [LINQ query langauge](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/basic-linq-query-operations) (syntactic sugar for LINQ library). When using LINQ, feel free to use [method syntax](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/method-based-query-syntax-examples-projection). An example of this in our code can be found [here](https://github.com/CPSECapstone/hourglass-be/blob/0a6779bdfbbb84c1f521f2e4fb3d74c51619e5b8/HourglassServer/Controllers/DummyController.cs#L25).

- When updating the database schema, run the following in Visual Studio's Nuget package manager console: `Scaffold-DBContext "our connection string" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Data`. This command looks at the database schema and updates the corresponding classes in our code. You should then commit the changes. More information can be found [here](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell).

## Resources
https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio

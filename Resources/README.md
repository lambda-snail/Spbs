
# Running Locally

Before running locally we need to start the local server. This is done by navigating to the ```resources``` folder and executing:

```
docker-compose up -d
```

When done the container can be left running or stopped:

```
docker-compose down
```

# Migrating and Updating the Database

## Migrations

To perform a migration, go to the ```Spbs.Ui``` project and execute the following two commands:

```
dotnet ef migrations add Initial-Migration -o Data/Migrations/Expenses --context ExpensesDbContext
dotnet ef migrations add Initial-Migration -o Data/Migrations/RecurringExpenses --context RecurringExpensesDbContext 
```

## Updating

To apply the migrations execute the following commands:

```
dotnet ef database update --context ExpensesDbContext -- --environment Development
dotnet ef database update --context RecurringExpensesDbContext -- --environment Development
```

For production we will pass the flag ```Production``` to ef core. In the future there may be environment specific configurations added so it is best to have this practice from the start.
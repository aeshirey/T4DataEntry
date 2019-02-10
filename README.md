# T4DataEntry

T4DataEntry is a WPF project that uses [T4 templates](https://docs.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2017) to generate XAML and backing C# for data entry purposes. Currently, it uses a SQLite database wrapped with [sqlite-net](https://www.nuget.org/packages/sqlite-net).

To use, clone the project and add text files into the `DOM` folder to represent the database schema you want. Each text file represents a single table. Within each file, each row represents a column and currently takes the format `{column name}:{c# type}`. For example, the contents of `Company.txt` is:

    CompanyId:Guid
    Name:string
    StockSymbol:string
    Founded:DateTime

![Solution explorer with text files in the DOM folder](https://github.com/aeshirey/T4DataEntry/blob/images/_images/solution_explorer.png)

After adding all relevant files, invoke the T4 template engine. Currently, this is most easily done by opening `MainWindow.tt` and `MainWindowCs.tt` and re-saving.

This will generate a functional `MainWindow.xaml` and `MainWindowCs.cs` that work together. You can then run the app and see one tab per table, each of which has a `DataGrid` to represent records in the database and data entry elements for each column:

![Automatically generated WPF app](https://github.com/aeshirey/T4DataEntry/blob/images/_images/main_window.png)

## Schema Notes
Relevant notes on building your schema:

* Columns ending with "Id" are considered to be identifier columns that reference another table, `{table name}Id`
* In the case where an ID column matches its own table (eg, `CompanyId` within the `Company` table), it is treated as the primary key and is not currently editable.
* Valid column types are `string`, `int`, `int?`, `Guid`, `Guid?`, `bool`, `bool?`, `DateTime`, and `DateTime?`.
* `FOREIGN KEY` constraints are not currently added.

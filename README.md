# T4DataEntry

T4DataEntry is a WPF project that uses [T4 templates](https://docs.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2017) to generate XAML and backing C# for data entry purposes. Currently, it uses a SQLite database wrapped with [sqlite-net](https://www.nuget.org/packages/sqlite-net).

To use, clone the project and add text files into the `DOM` folder to represent the database schema you want. Each text file represents a single table. Within each file, each row represents a column and currently takes the format `{column name}:{c# type}[:optional modifiers]` or `#{meta tag}:{value}`. For example, the contents of [`Company.txt`](https://github.com/aeshirey/T4DataEntry/blob/master/T4DataEntry/DOM/Company.txt) is:

    #order:2
    CompanyId:Guid
    *Name:string!
    *StockSymbol:string?
    Founded:DateTime

These values are explained as follows:

| Column Specification | SQL Type | Notes |
| -------------------- | -------- | ----- |
| `#order:2`           | n/a      | Makes this table appear as the second in the list. If unspecified, tables are listed in the order returned by `Directory.GetFiles`. |
| `CompanyId:Guid`     | `CompanyId TEXT NOT NULL PRIMARY KEY` | A non-nullable `Guid` C# type; the generated application will create new values using `Guid.NewGuid()` automatically, just as would an `AUTOINCREMENT` column within the database. |
| `*Name:string!`      | `Name TEXT NOT NULL` | The <kbd>*</kbd> indicates that this column should be used as a display column in the application for any Company record. The <kbd>!</kbd> modifier of the type is an input validation that indicates the values are not only non-nullable, but also _must_ have a non-blank value. (This applies only to strings.) In other words, any `string!` "type" is considered invalid if it is the empty string. |
| `*StockSymbol:string?` | `StockSymbol TEXT` | Similarly included in the display. The <kbd>?</kbd> type modifier means that it is a nullable column; input validation does not restrict an empty input element. |
| `Founded:DateTime` | `Founded DATETIME NOT NULL` | |

The composite of `Name, StockSymbol` is displayed in the user interface. For example, `Microsoft, MSFT`. See [Display of Records](#display-of-records).

![Solution explorer with text files in the DOM folder](https://github.com/aeshirey/T4DataEntry/blob/images/_images/solution_explorer.png)

After adding all relevant files, invoke the T4 template engine. Currently, this is most easily done by opening `MainWindow.tt` and `MainWindowCs.tt` and re-saving.

This will generate a functional `MainWindow.xaml` and `MainWindowCs.cs` that work together. You can then run the app and see one tab per table, each of which has a `DataGrid` to represent records in the database and data entry elements for each column:

![Automatically generated WPF app](https://github.com/aeshirey/T4DataEntry/blob/images/_images/main_window.png)

## Modifiers
A column definition can include optional additional parameters. Currently, the only such definition is <kbd>PK</kbd> to explicitly identify a primary key. This can be useful when a table uses a composite key. For example:

    #order:3
    PersonId:Guid:PK
    CompanyId:Guid:PK
    Title:string
    OfficeNumber:int?

An employee, therefore, is defined by the composite primary key `{PersonId, CompanyId}`. As of now, [sqlite-net](https://github.com/praeclarum/sqlite-net/) does not currently [support multi-column primary keys](https://github.com/praeclarum/sqlite-net/issues/280) in its ORM design. T4DataEntry will, therefore, generate an appropriate SQL query for these scenarios:

```c#
public UserDB(string databaseFile = DatabaseFile) : base(databaseFile)
{
    CreateTable<Person>();
    CreateTable<Company>();
    Execute("create table if not exists \"Employee\" (\"PersonId\" varchar(36) not null, \"CompanyId\" varchar(36) not null, \"Title\" varchar not null, \"OfficeNumber\" integer, primary key (PersonId, CompanyId))");
    Execute("create table if not exists \"Tenure\" (\"PersonId\" varchar(36) not null, \"CompanyId\" varchar(36) not null, \"StartDate\" datetime not null, \"EndDate\" datetime, primary key (PersonId, CompanyId))");
    CreateTable<Car>();
    Commit();
}
```

## Meta Tags
Any row starting with <kbd>#</kbd> is considered a meta tag that defines some behavior for the table instead of specifying a column. The list of tags is currently:

* `#order:{number}` - Specifies the order in which the table appears. Conflicts are not resolved. Default is <kbd>int.MaxValue</kbd>, so tables with any order specified come before unspecified.
* `#heights:{datagrid height},{input elements height}` - Specifies the height of this table's DataGrid (listing existing rows) and the input elements (for editing existing/creating new records). The values are specified in integers and correspond to [the WPF convention for such](https://stackoverflow.com/a/717314/1191181). Default is <kbd>1,1</kbd>.
* `#tostring:{simple|detailed}` - Specifies how a table will be displayed when it is referenced by another. See [Display of Records](#display-of-records). Default is <kbd>simple</kbd>

## Schema Notes
Relevant notes on building your schema:

* Columns ending with "Id" are considered to be identifier columns that reference another table, `{table name}Id`
* In the case where an ID column matches its own table (eg, `CompanyId` within the `Company` table), it is treated as the primary key and is not currently editable. Primary keys can also be explicitly specified by adding the <kbd>PK</kbd> modifier to a column definition.
* Valid column types are `string`, `string?` (the same as `TEXT NOT NULL`), `string!` (a column with application-level validation enforcing non-empty text), `int`, `int?`, `double`, `double?`, `Guid`, `Guid?`, `bool`, `bool?`, `DateTime`, and `DateTime?`.
* `FOREIGN KEY` constraints are not currently added.

## Display of records
When displaying a record (typically in a DataGrid when one column references another), column names starting with <kbd>*</kbd> are used in the UI to give an object reference a displayable name. For example, if the Company table is defined as follows:

	CompanyId:Guid
	*Name:string
	*StockSymbol:string
	Founded:DateTime

Then any references to a Company in the UI will display the combination of these columns instead of the (likely incomprehensible) primary key itself. The display can be switched between "simple" (default) and "detailed" by using the meta tag ([described above](#meta-tags)). For example, a Company may be displayed as `Microsoft, MSFT` (simple) or `Company(Name=Microsoft, StockSymbol=MSFT)` (detailed).

If no display columns are explicitly specified and a <kbd>Name</kbd> column is present, it will be used. If no <kbd>Name</kbd> column exists, then the primary key will be used (ie, `Employee.CompanyId`). If no primary key exists, then the underlying object representation (eg, `T4DataEntry.Company`) is used.

![UI display of records](https://github.com/aeshirey/T4DataEntry/blob/images/_images/isDisplayable.png)

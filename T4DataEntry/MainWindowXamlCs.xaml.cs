
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SQLite;

namespace T4DataEntry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public UserDB UserDB;

        public MainWindow()
        {
            InitializeComponent();

			UserDB = new UserDB();

			// load all records
			    LoadCompany();
    LoadEmployee();
    LoadPerson();
        }

		#region Insert records
        private void InsertCompany(object sender, RoutedEventArgs e)
        {
            Guid g;
            // read each Company column from its individual controls
            Guid CompanyId = Guid.Parse(tbCompany_CompanyId.Text);
            string Name = tbCompany_Name.Text;
            string StockSymbol = tbCompany_StockSymbol.Text;
            DateTime Founded = dtCompany_Founded.SelectedDate ?? DateTime.MinValue;

            var record = new Company
            {
                CompanyId = CompanyId,
                Name = Name,
                StockSymbol = StockSymbol,
                Founded = Founded,
            };

            UserDB.InsertOrReplace(record);
            LoadCompany();
        }

        private void InsertEmployee(object sender, RoutedEventArgs e)
        {
            int i;
            Guid g;
            // read each Employee column from its individual controls
            Guid EmployeeId = Guid.Parse(tbEmployee_EmployeeId.Text);
            var PersonId = (tbEmployee_PersonId.SelectedItem as Person).PersonId;
            var CompanyId = (tbEmployee_CompanyId.SelectedItem as Company).CompanyId;
            string Title = tbEmployee_Title.Text;
            int? OfficeNumber = int.TryParse(tbEmployee_OfficeNumber.Text, out i) ? i : (int?)null;

            var record = new Employee
            {
                EmployeeId = EmployeeId,
                PersonId = PersonId,
                CompanyId = CompanyId,
                Title = Title,
                OfficeNumber = OfficeNumber,
            };

            UserDB.InsertOrReplace(record);
            LoadEmployee();
        }

        private void InsertPerson(object sender, RoutedEventArgs e)
        {
            int i;
            Guid g;
            // read each Person column from its individual controls
            Guid PersonId = Guid.Parse(tbPerson_PersonId.Text);
            string Name = tbPerson_Name.Text;
            int? Age = int.TryParse(tbPerson_Age.Text, out i) ? i : (int?)null;
            string Hometown = tbPerson_Hometown.Text;

            var record = new Person
            {
                PersonId = PersonId,
                Name = Name,
                Age = Age,
                Hometown = Hometown,
            };

            UserDB.InsertOrReplace(record);
            LoadPerson();
        }

    		#endregion
    
        
    	
    	#region Load data methods
    	private void LoadCompany()
        {
            dgCompany.Items.Clear();
            var records = UserDB.Table<Company>().ToList();
            records.ForEach(r => dgCompany.Items.Add(r));
            tbEmployee_CompanyId.ItemsSource = records;
            tbCompany_Name.ItemsSource = records.Select(r => r.Name).ToList();
            tbCompany_StockSymbol.ItemsSource = records.Select(r => r.StockSymbol).ToList();
        }
        private void LoadEmployee()
        {
            dgEmployee.Items.Clear();
            var records = UserDB.Table<Employee>().ToList();
            records.ForEach(r => dgEmployee.Items.Add(r));
            tbPerson_PersonId.ItemsSource = records;
            tbCompany_CompanyId.ItemsSource = records;
            tbEmployee_Title.ItemsSource = records.Select(r => r.Title).ToList();
            tbEmployee_OfficeNumber.ItemsSource = records.Select(r => r.OfficeNumber).ToList();
        }
        private void LoadPerson()
        {
            dgPerson.Items.Clear();
            var records = UserDB.Table<Person>().ToList();
            records.ForEach(r => dgPerson.Items.Add(r));
            tbEmployee_PersonId.ItemsSource = records;
            tbPerson_Name.ItemsSource = records.Select(r => r.Name).ToList();
            tbPerson_Age.ItemsSource = records.Select(r => r.Age).ToList();
            tbPerson_Hometown.ItemsSource = records.Select(r => r.Hometown).ToList();
        }
    	#endregion
    
    
    
    	
    	#region Events
    	// https://stackoverflow.com/questions/10667002/
    	private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    	{
    		if (sender != null)
    		{
    			DataGrid grid = sender as DataGrid;
    			if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
    			{
    				DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
    				if (!dgr.IsMouseOver)
    				{
    					(dgr as DataGridRow).IsSelected = false;
    				}
    			}
    		}        
    	}
    
    	private void Company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the Company object
            if (e.AddedItems.Count == 0)
            {
                tbCompany_CompanyId.Text = string.Empty;
                tbCompany_Name.Text = default(string);
                tbCompany_StockSymbol.Text = default(string);
                dtCompany_Founded.SelectedDate = DateTime.Today;
            }
            else
            {
                Company company = e.AddedItems[0] as Company;
                tbCompany_CompanyId.Text = company.CompanyId.ToString();
                tbCompany_Name.Text = company.Name;
                tbCompany_StockSymbol.Text = company.StockSymbol;
                dtCompany_Founded.SelectedDate = company.Founded;
            }
        }
        private void Employee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the Employee object
            if (e.AddedItems.Count == 0)
            {
                tbEmployee_EmployeeId.Text = string.Empty;
                tbEmployee_PersonId.Text = string.Empty;
                tbEmployee_CompanyId.Text = string.Empty;
                tbEmployee_Title.Text = default(string);
                tbEmployee_OfficeNumber.Text = string.Empty;
            }
            else
            {
                Employee employee = e.AddedItems[0] as Employee;
                tbEmployee_EmployeeId.Text = employee.EmployeeId.ToString();
                tbEmployee_PersonId.Text = employee.PersonId.ToString();
                tbEmployee_CompanyId.Text = employee.CompanyId.ToString();
                tbEmployee_Title.Text = employee.Title;
                tbEmployee_OfficeNumber.Text = employee.OfficeNumber.ToString();
            }
        }
        private void Person_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the Person object
            if (e.AddedItems.Count == 0)
            {
                tbPerson_PersonId.Text = string.Empty;
                tbPerson_Name.Text = default(string);
                tbPerson_Age.Text = string.Empty;
                tbPerson_Hometown.Text = default(string);
            }
            else
            {
                Person person = e.AddedItems[0] as Person;
                tbPerson_PersonId.Text = person.PersonId.ToString();
                tbPerson_Name.Text = person.Name;
                tbPerson_Age.Text = person.Age.ToString();
                tbPerson_Hometown.Text = person.Hometown;
            }
        }
    	#endregion
    
    	}
    
    	#region Generated classes for the object model
    	public class Company
        {
            [PrimaryKey]
            public Guid CompanyId { get; set; }
            public string Name { get; set; }
            public string StockSymbol { get; set; }
            public DateTime? Founded { get; set; }
            public override string ToString() => Name;
        }

        public class Employee
        {
            [PrimaryKey]
            public Guid EmployeeId { get; set; }
            public Guid? PersonId { get; set; }
            public Guid? CompanyId { get; set; }
            public string Title { get; set; }
            public int? OfficeNumber { get; set; }
        }

        public class Person
        {
            [PrimaryKey]
            public Guid PersonId { get; set; }
            public string Name { get; set; }
            public int? Age { get; set; }
            public string Hometown { get; set; }
            public override string ToString() => Name;
        }

	#endregion

	

	public class UserDB : SQLiteConnection
    {
        public const string DatabaseFile = @"data.sqlite";
        public static SQLiteConnection DbConnection;
        public UserDB(string databaseFile = DatabaseFile) : base(databaseFile)
        {
            CreateTable<Company>();
            CreateTable<Employee>();
            CreateTable<Person>();
            Commit();
        }
	}
}

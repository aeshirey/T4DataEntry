
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
			LoadCar();
            LoadCompany();
            LoadEmployee();
            LoadPerson();

			// input element validation
			// Car:
            cbCar_Year.KeyUp += (sender, e) => { btnCar_Save.IsEnabled = IsCarValid(); };

            // Company:
            cbCompany_CompanyId.KeyUp += (sender, e) => { btnCompany_Save.IsEnabled = IsCompanyValid(); };
            cbCompany_Name.KeyUp += (sender, e) => { btnCompany_Save.IsEnabled = IsCompanyValid(); };

            // Employee:
            cbEmployee_EmployeeId.KeyUp += (sender, e) => { btnEmployee_Save.IsEnabled = IsEmployeeValid(); };
            cbEmployee_PersonId.KeyUp += (sender, e) => { btnEmployee_Save.IsEnabled = IsEmployeeValid(); };
            cbEmployee_CompanyId.KeyUp += (sender, e) => { btnEmployee_Save.IsEnabled = IsEmployeeValid(); };

            // Person:
            cbPerson_PersonId.KeyUp += (sender, e) => { btnPerson_Save.IsEnabled = IsPersonValid(); };
            cbPerson_Age.KeyUp += (sender, e) => { btnPerson_Save.IsEnabled = IsPersonValid(); };

        }
		    
    	
    	#region Load data methods
    	private void LoadCar()
        {
            // remove all Car entries from its DataGrid and pull the new set from the database
            dgCar.Items.Clear();
            var records = UserDB.Table<Car>().ToList();

            // add all records back into the DataGrid
            records.ForEach(r => dgCar.Items.Add(r));
            cbCar_Make.ItemsSource = records.Select(r => r.Make).ToList();
            cbCar_Model.ItemsSource = records.Select(r => r.Model).ToList();
            cbCar_Year.ItemsSource = records.Select(r => r.Year).ToList();
            btnCar_Save.IsEnabled = false;
        }
        private void LoadCompany()
        {
            // remove all Company entries from its DataGrid and pull the new set from the database
            dgCompany.Items.Clear();
            var records = UserDB.Table<Company>().ToList();

            // add all records back into the DataGrid
            records.ForEach(r => dgCompany.Items.Add(r));
            cbCar_CompanyId.ItemsSource = records;
            cbEmployee_CompanyId.ItemsSource = records;
            cbCompany_Name.ItemsSource = records.Select(r => r.Name).ToList();
            cbCompany_StockSymbol.ItemsSource = records.Select(r => r.StockSymbol).ToList();
            btnCompany_Save.IsEnabled = false;
        }
        private void LoadEmployee()
        {
            // remove all Employee entries from its DataGrid and pull the new set from the database
            dgEmployee.Items.Clear();
            var records = UserDB.Table<Employee>().ToList();

            // add all records back into the DataGrid
            records.ForEach(r => dgEmployee.Items.Add(r));
            cbEmployee_Title.ItemsSource = records.Select(r => r.Title).ToList();
            cbEmployee_OfficeNumber.ItemsSource = records.Select(r => r.OfficeNumber).ToList();
            btnEmployee_Save.IsEnabled = false;
        }
        private void LoadPerson()
        {
            // remove all Person entries from its DataGrid and pull the new set from the database
            dgPerson.Items.Clear();
            var records = UserDB.Table<Person>().ToList();

            // add all records back into the DataGrid
            records.ForEach(r => dgPerson.Items.Add(r));
            cbCar_PersonId.ItemsSource = records;
            cbEmployee_PersonId.ItemsSource = records;
            cbPerson_Name.ItemsSource = records.Select(r => r.Name).ToList();
            cbPerson_Age.ItemsSource = records.Select(r => r.Age).ToList();
            cbPerson_Hometown.ItemsSource = records.Select(r => r.Hometown).ToList();
            btnPerson_Save.IsEnabled = false;
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
        
        #region Input validation events
        private bool IsCarValid()
        {
            int i;
            if (!int.TryParse(cbCar_Year.Text, out i)) return false;
            return true;
        }

        private bool IsCompanyValid()
        {
            Guid g;
            if (!Guid.TryParse(cbCompany_CompanyId.Text, out g)) return false;
            if (string.IsNullOrEmpty(cbCompany_Name.Text)) return false;
            return true;
        }

        private bool IsEmployeeValid()
        {
            Guid g;
            if (!Guid.TryParse(cbEmployee_EmployeeId.Text, out g)) return false;
            if (!Guid.TryParse(cbEmployee_PersonId.Text, out g)) return false;
            if (!Guid.TryParse(cbEmployee_CompanyId.Text, out g)) return false;
            return true;
        }

        private bool IsPersonValid()
        {
            int i;
            Guid g;
            if (!Guid.TryParse(cbPerson_PersonId.Text, out g)) return false;
            if (!int.TryParse(cbPerson_Age.Text, out i)) return false;
            return true;
        }

        #endregion
        
        
        #region DataGrid selection changed
        private void Car_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the Car object
            if (e.AddedItems.Count == 0)
            {
                cbCar_CarId.Text = default(string);
                cbCar_Make.Text = default(string);
                cbCar_Model.Text = default(string);
                cbCar_Year.Text = string.Empty;
                cbCar_PersonId.Text = string.Empty;
                cbCar_CompanyId.Text = string.Empty;
            }
            else
            {
                Car car = e.AddedItems[0] as Car;
                cbCar_CarId.Text = car.CarId;
                cbCar_Make.Text = car.Make;
                cbCar_Model.Text = car.Model;
                cbCar_Year.Text = car.Year.ToString();
                foreach (var item in cbCar_PersonId.Items)
                {
                   if ((item as Person).PersonId == car.PersonId)
                      cbCar_PersonId.SelectedItem = item;
                }
                foreach (var item in cbCar_CompanyId.Items)
                {
                   if ((item as Company).CompanyId == car.CompanyId)
                      cbCar_CompanyId.SelectedItem = item;
                }
            }
        }
        private void Company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the Company object
            if (e.AddedItems.Count == 0)
            {
                cbCompany_CompanyId.Text = string.Empty;
                cbCompany_Name.Text = default(string);
                cbCompany_StockSymbol.Text = default(string);
                dtCompany_Founded.SelectedDate = DateTime.Today;
            }
            else
            {
                Company company = e.AddedItems[0] as Company;
                cbCompany_CompanyId.Text = company.CompanyId.ToString();
                cbCompany_Name.Text = company.Name;
                cbCompany_StockSymbol.Text = company.StockSymbol;
                dtCompany_Founded.SelectedDate = company.Founded;
            }
        }
        private void Employee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the Employee object
            if (e.AddedItems.Count == 0)
            {
                cbEmployee_EmployeeId.Text = string.Empty;
                cbEmployee_PersonId.Text = string.Empty;
                cbEmployee_CompanyId.Text = string.Empty;
                cbEmployee_Title.Text = default(string);
                cbEmployee_OfficeNumber.Text = string.Empty;
            }
            else
            {
                Employee employee = e.AddedItems[0] as Employee;
                cbEmployee_EmployeeId.Text = employee.EmployeeId.ToString();
                foreach (var item in cbEmployee_PersonId.Items)
                {
                   if ((item as Person).PersonId == employee.PersonId)
                      cbEmployee_PersonId.SelectedItem = item;
                }
                foreach (var item in cbEmployee_CompanyId.Items)
                {
                   if ((item as Company).CompanyId == employee.CompanyId)
                      cbEmployee_CompanyId.SelectedItem = item;
                }
                cbEmployee_Title.Text = employee.Title;
                cbEmployee_OfficeNumber.Text = employee.OfficeNumber.ToString();
            }
        }
        private void Person_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the Person object
            if (e.AddedItems.Count == 0)
            {
                cbPerson_PersonId.Text = string.Empty;
                cbPerson_Name.Text = default(string);
                cbPerson_Age.Text = string.Empty;
                cbPerson_Hometown.Text = default(string);
            }
            else
            {
                Person person = e.AddedItems[0] as Person;
                cbPerson_PersonId.Text = person.PersonId.ToString();
                cbPerson_Name.Text = person.Name;
                cbPerson_Age.Text = person.Age.ToString();
                cbPerson_Hometown.Text = person.Hometown;
            }
        }
        #endregion
        
        
        
        #region Save button clicked (update/insert records)
        private void Car_Click(object sender, RoutedEventArgs e)
        {
            Guid g;
            Car selected = (Car)dgCar.SelectedItem;
            // read each Car column from its individual controls
            string _CarId = selected?.CarId ?? cbCar_CarId.Text;
            string _Make = cbCar_Make.Text;
            string _Model = cbCar_Model.Text;
            int _Year = int.Parse(cbCar_Year.Text);
            var _PersonId = (cbCar_PersonId.SelectedItem as Person)?.PersonId;
            var _CompanyId = (cbCar_CompanyId.SelectedItem as Company)?.CompanyId;

            var record = new Car
            {
                CarId = _CarId,
                Make = _Make,
                Model = _Model,
                Year = _Year,
                PersonId = _PersonId,
                CompanyId = _CompanyId,
            };

            UserDB.InsertOrReplace(record);
            LoadCar();
        }

        private void Company_Click(object sender, RoutedEventArgs e)
        {
            Company selected = (Company)dgCompany.SelectedItem;
            // read each Company column from its individual controls
            Guid _CompanyId = selected?.CompanyId ?? Guid.NewGuid();
            string _Name = cbCompany_Name.Text;
            string _StockSymbol = cbCompany_StockSymbol.Text;
            DateTime _Founded = dtCompany_Founded.SelectedDate ?? DateTime.MinValue;

            var record = new Company
            {
                CompanyId = _CompanyId,
                Name = _Name,
                StockSymbol = _StockSymbol,
                Founded = _Founded,
            };

            UserDB.InsertOrReplace(record);
            LoadCompany();
        }

        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            int i;
            Employee selected = (Employee)dgEmployee.SelectedItem;
            // read each Employee column from its individual controls
            Guid _EmployeeId = selected?.EmployeeId ?? Guid.NewGuid();
            var _PersonId = (cbEmployee_PersonId.SelectedItem as Person).PersonId;
            var _CompanyId = (cbEmployee_CompanyId.SelectedItem as Company).CompanyId;
            string _Title = cbEmployee_Title.Text;
            int? _OfficeNumber = int.TryParse(cbEmployee_OfficeNumber.Text, out i) ? i : (int?)null;

            var record = new Employee
            {
                EmployeeId = _EmployeeId,
                PersonId = _PersonId,
                CompanyId = _CompanyId,
                Title = _Title,
                OfficeNumber = _OfficeNumber,
            };

            UserDB.InsertOrReplace(record);
            LoadEmployee();
        }

        private void Person_Click(object sender, RoutedEventArgs e)
        {
            Person selected = (Person)dgPerson.SelectedItem;
            // read each Person column from its individual controls
            Guid _PersonId = selected?.PersonId ?? Guid.NewGuid();
            string _Name = cbPerson_Name.Text;
            int _Age = int.Parse(cbPerson_Age.Text);
            string _Hometown = cbPerson_Hometown.Text;

            var record = new Person
            {
                PersonId = _PersonId,
                Name = _Name,
                Age = _Age,
                Hometown = _Hometown,
            };

            UserDB.InsertOrReplace(record);
            LoadPerson();
        }

    	#endregion
    	#endregion
    }
    
    #region Generated classes for the object model
    public class Car
    {
        [PrimaryKey]
        public string CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? CompanyId { get; set; }
        public override string ToString() => $"Car(Make={Make}, Model={Model})";
    }

    public class Company
    {
        [PrimaryKey]
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string StockSymbol { get; set; }
        public DateTime Founded { get; set; }
        public override string ToString() => $"Company(Name={Name}, StockSymbol={StockSymbol})";
    }

    public class Employee
    {
        [PrimaryKey]
        public Guid EmployeeId { get; set; }
        public Guid PersonId { get; set; }
        public Guid CompanyId { get; set; }
        public string Title { get; set; }
        public int? OfficeNumber { get; set; }
        public override string ToString() => EmployeeId.ToString();
    }

    public class Person
    {
        [PrimaryKey]
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
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
            CreateTable<Car>();
            CreateTable<Company>();
            CreateTable<Employee>();
            CreateTable<Person>();
			Commit();
		}
	}
}

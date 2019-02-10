
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
        }
		    
        	
        	#region Load data methods
        	private void LoadCar()
            {
                // remove all Car entries from its DataGrid and pull the new set from the database
                dgCar.Items.Clear();
                var records = UserDB.Table<Car>().ToList();

                // add all records back into the DataGrid
                records.ForEach(r => dgCar.Items.Add(r));
                tbCar_Make.ItemsSource = records.Select(r => r.Make).ToList();
                tbCar_Model.ItemsSource = records.Select(r => r.Model).ToList();
                tbCar_Year.ItemsSource = records.Select(r => r.Year).ToList();
                tbCar_PersonId.ItemsSource = records.Select(r => r.PersonId).ToList();
                tbCar_CompanyId.ItemsSource = records.Select(r => r.CompanyId).ToList();
            }
            private void LoadCompany()
            {
                // remove all Company entries from its DataGrid and pull the new set from the database
                dgCompany.Items.Clear();
                var records = UserDB.Table<Company>().ToList();

                // add all records back into the DataGrid
                records.ForEach(r => dgCompany.Items.Add(r));
                tbCar_CompanyId.ItemsSource = records;
                tbEmployee_CompanyId.ItemsSource = records;
                tbCompany_Name.ItemsSource = records.Select(r => r.Name).ToList();
                tbCompany_StockSymbol.ItemsSource = records.Select(r => r.StockSymbol).ToList();
            }
            private void LoadEmployee()
            {
                // remove all Employee entries from its DataGrid and pull the new set from the database
                dgEmployee.Items.Clear();
                var records = UserDB.Table<Employee>().ToList();

                // add all records back into the DataGrid
                records.ForEach(r => dgEmployee.Items.Add(r));
                tbEmployee_PersonId.ItemsSource = records.Select(r => r.PersonId).ToList();
                tbEmployee_CompanyId.ItemsSource = records.Select(r => r.CompanyId).ToList();
                tbEmployee_Title.ItemsSource = records.Select(r => r.Title).ToList();
                tbEmployee_OfficeNumber.ItemsSource = records.Select(r => r.OfficeNumber).ToList();
            }
            private void LoadPerson()
            {
                // remove all Person entries from its DataGrid and pull the new set from the database
                dgPerson.Items.Clear();
                var records = UserDB.Table<Person>().ToList();

                // add all records back into the DataGrid
                records.ForEach(r => dgPerson.Items.Add(r));
                tbCar_PersonId.ItemsSource = records;
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
        
        	#region DataGrid selection changed
        	private void Car_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // get the Car object
                if (e.AddedItems.Count == 0)
                {
                    tbCar_CarId.Text = default(string);
                    tbCar_Make.Text = default(string);
                    tbCar_Model.Text = default(string);
                    tbCar_Year.Text = string.Empty;
                    tbCar_PersonId.Text = string.Empty;
                    tbCar_CompanyId.Text = string.Empty;
                }
                else
                {
                    Car car = e.AddedItems[0] as Car;
                    tbCar_CarId.Text = car.CarId;
                    tbCar_Make.Text = car.Make;
                    tbCar_Model.Text = car.Model;
                    tbCar_Year.Text = car.Year.ToString();
                    foreach (var item in tbCar_PersonId.Items)
                    {
                       if ((item as Person).PersonId == car.PersonId)
                          tbCar_PersonId.SelectedItem = item;
                    }
                    foreach (var item in tbCar_CompanyId.Items)
                    {
                       if ((item as Company).CompanyId == car.CompanyId)
                          tbCar_CompanyId.SelectedItem = item;
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
                    foreach (var item in tbEmployee_PersonId.Items)
                    {
                       if ((item as Person).PersonId == employee.PersonId)
                          tbEmployee_PersonId.SelectedItem = item;
                    }
                    foreach (var item in tbEmployee_CompanyId.Items)
                    {
                       if ((item as Company).CompanyId == employee.CompanyId)
                          tbEmployee_CompanyId.SelectedItem = item;
                    }
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
        
        
        
        
        	#region Save button clicked (update/insert records)
        private void Car_Click(object sender, RoutedEventArgs e)
        {
            Guid g;
            // read each Car column from its individual controls
            string _CarId = tbCar_CarId.Text;
            string _Make = tbCar_Make.Text;
            string _Model = tbCar_Model.Text;
            int _Year = int.Parse(tbCar_Year.Text);
            var _PersonId = (tbCar_PersonId.SelectedItem as Person)?.PersonId;
            var _CompanyId = (tbCar_CompanyId.SelectedItem as Company)?.CompanyId;

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
            // read each Company column from its individual controls
            Guid _CompanyId = Guid.NewGuid();
            string _Name = tbCompany_Name.Text;
            string _StockSymbol = tbCompany_StockSymbol.Text;
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
            // read each Employee column from its individual controls
            Guid _EmployeeId = Guid.NewGuid();
            var _PersonId = (tbEmployee_PersonId.SelectedItem as Person).PersonId;
            var _CompanyId = (tbEmployee_CompanyId.SelectedItem as Company).CompanyId;
            string _Title = tbEmployee_Title.Text;
            int _OfficeNumber = int.Parse(tbEmployee_OfficeNumber.Text);

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
            // read each Person column from its individual controls
            Guid _PersonId = Guid.NewGuid();
            string _Name = tbPerson_Name.Text;
            int _Age = int.Parse(tbPerson_Age.Text);
            string _Hometown = tbPerson_Hometown.Text;

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
            public override string ToString() => Name;
        }

        public class Employee
        {
            [PrimaryKey]
            public Guid EmployeeId { get; set; }
            public Guid PersonId { get; set; }
            public Guid CompanyId { get; set; }
            public string Title { get; set; }
            public int OfficeNumber { get; set; }
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

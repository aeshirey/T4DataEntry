
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
            LoadTenure();
// Car: CarId:string, Make:string, Model:string, Year:int, PersonId:Guid?, CompanyId:Guid?, IsManualTransmission:bool
// Company: CompanyId:Guid, Name:string, StockSymbol:string, Founded:DateTime
// Employee: EmployeeId:Guid, PersonId:Guid, CompanyId:Guid, Title:string, OfficeNumber:int?
// Person: PersonId:Guid, Name:string, Age:int, Hometown:string, HeightCm:double
// Tenure: PersonId:Guid, CompanyId:Guid?, StartDate:DateTime, EndDate:DateTime?

			// input element validation
			// Car:
            cbCar_CarId.KeyUp += (sender, e) => { btnCar_Save.IsEnabled = IsCarValid(); };
            cbCar_Make.KeyUp += (sender, e) => { btnCar_Save.IsEnabled = IsCarValid(); };
            cbCar_Model.KeyUp += (sender, e) => { btnCar_Save.IsEnabled = IsCarValid(); };
            cbCar_Year.KeyUp += (sender, e) => { btnCar_Save.IsEnabled = IsCarValid(); };
            cbCar_PersonId.SelectionChanged += (sender, e) => { btnCar_Save.IsEnabled = IsCarValid(); };
            cbCar_CompanyId.SelectionChanged += (sender, e) => { btnCar_Save.IsEnabled = IsCarValid(); };

            // Company:
            cbCompany_CompanyId.SelectionChanged += (sender, e) => { btnCompany_Save.IsEnabled = IsCompanyValid(); };
            cbCompany_Name.KeyUp += (sender, e) => { btnCompany_Save.IsEnabled = IsCompanyValid(); };
            cbCompany_StockSymbol.KeyUp += (sender, e) => { btnCompany_Save.IsEnabled = IsCompanyValid(); };
            dtCompany_Founded.SelectedDateChanged += (sender, e) => { btnCompany_Save.IsEnabled = IsCompanyValid(); };

            // Employee:
            cbEmployee_EmployeeId.SelectionChanged += (sender, e) => { btnEmployee_Save.IsEnabled = IsEmployeeValid(); };
            cbEmployee_PersonId.SelectionChanged += (sender, e) => { btnEmployee_Save.IsEnabled = IsEmployeeValid(); };
            cbEmployee_CompanyId.SelectionChanged += (sender, e) => { btnEmployee_Save.IsEnabled = IsEmployeeValid(); };
            cbEmployee_Title.KeyUp += (sender, e) => { btnEmployee_Save.IsEnabled = IsEmployeeValid(); };
            cbEmployee_OfficeNumber.KeyUp += (sender, e) => { btnEmployee_Save.IsEnabled = IsEmployeeValid(); };

            // Person:
            cbPerson_PersonId.SelectionChanged += (sender, e) => { btnPerson_Save.IsEnabled = IsPersonValid(); };
            cbPerson_Name.KeyUp += (sender, e) => { btnPerson_Save.IsEnabled = IsPersonValid(); };
            cbPerson_Age.KeyUp += (sender, e) => { btnPerson_Save.IsEnabled = IsPersonValid(); };
            cbPerson_Hometown.KeyUp += (sender, e) => { btnPerson_Save.IsEnabled = IsPersonValid(); };
            cbPerson_HeightCm.KeyUp += (sender, e) => { btnPerson_Save.IsEnabled = IsPersonValid(); };

            // Tenure:
            cbTenure_PersonId.SelectionChanged += (sender, e) => { btnTenure_Save.IsEnabled = IsTenureValid(); };
            cbTenure_CompanyId.SelectionChanged += (sender, e) => { btnTenure_Save.IsEnabled = IsTenureValid(); };
            dtTenure_StartDate.SelectedDateChanged += (sender, e) => { btnTenure_Save.IsEnabled = IsTenureValid(); };
            dtTenure_EndDate.SelectedDateChanged += (sender, e) => { btnTenure_Save.IsEnabled = IsTenureValid(); };

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
            cbCar_CompanyId.ItemsSource = (new Company[] { null }).Concat(records);
            cbEmployee_CompanyId.ItemsSource = records;
            cbTenure_CompanyId.ItemsSource = (new Company[] { null }).Concat(records);
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
            cbCar_PersonId.ItemsSource = (new Person[] { null }).Concat(records);
            cbEmployee_PersonId.ItemsSource = records;
            cbTenure_PersonId.ItemsSource = records;
            cbPerson_Name.ItemsSource = records.Select(r => r.Name).ToList();
            cbPerson_Age.ItemsSource = records.Select(r => r.Age).ToList();
            cbPerson_Hometown.ItemsSource = records.Select(r => r.Hometown).ToList();
            cbPerson_HeightCm.ItemsSource = records.Select(r => r.HeightCm).ToList();
            btnPerson_Save.IsEnabled = false;
        }
        private void LoadTenure()
        {
            // remove all Tenure entries from its DataGrid and pull the new set from the database
            dgTenure.Items.Clear();
            var records = UserDB.Table<Tenure>().ToList();

            // add all records back into the DataGrid
            records.ForEach(r => dgTenure.Items.Add(r));
            btnTenure_Save.IsEnabled = false;
        }
        #endregion
        
        
        
        	
        #region Events
        // https://stackoverflow.com/questions/10667002/
        private void DataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        	DataGrid grid = sender as DataGrid;
        	if (grid == null) return;
        
        	if (grid.SelectedItems != null && grid.SelectedItems.Count == 1)
        	{
        		DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
        		if (!dgr.IsMouseOver)
        		{
        			(dgr as DataGridRow).IsSelected = false;
        		}
        	}    
        }
        
        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
        	// currently, we're only handling delete events
            if (e.Key != Key.Delete)
        		return;
        
        	DataGrid grid = sender as DataGrid;
        	if (grid == null) return;
        
        	// can't delete if we don't have a selected item
        	if (grid.SelectedItems == null || grid.SelectedItems.Count != 1)
        		return;
        
            var response = MessageBox.Show("Are you sure you want to delete this record? This cannot be undone, and relationships are not deleted.", "Delete record?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (response != MessageBoxResult.Yes)
        		return;
        
        	object item = grid.SelectedItem;
        
        	// reload the appropriate table
            if (item is Car)
            {
                UserDB.Delete((Car)item);
                LoadCar();
            }
            else if (item is Company)
            {
                UserDB.Delete((Company)item);
                LoadCompany();
            }
            else if (item is Employee)
            {
                UserDB.Delete((Employee)item);
                LoadEmployee();
            }
            else if (item is Person)
            {
                UserDB.Delete((Person)item);
                LoadPerson();
            }
            else if (item is Tenure)
            {
                UserDB.Delete((Tenure)item);
                LoadTenure();
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
            if (string.IsNullOrEmpty(cbCompany_Name.Text)) return false;
            return true;
        }

        private bool IsEmployeeValid()
        {
            int i;
            if (cbEmployee_PersonId.SelectedItem == null) return false;
            if (cbEmployee_CompanyId.SelectedItem == null) return false;
            if (!string.IsNullOrEmpty(cbEmployee_OfficeNumber.Text) && !int.TryParse(cbEmployee_OfficeNumber.Text, out i)) return false;
            return true;
        }

        private bool IsPersonValid()
        {
            int i;
            double d;
            if (!int.TryParse(cbPerson_Age.Text, out i)) return false;
            if (!double.TryParse(cbPerson_HeightCm.Text, out d)) return false;
            return true;
        }

        private bool IsTenureValid()
        {
            if (cbTenure_PersonId.SelectedItem == null) return false;
            return true;
        }

        #endregion

        #region Handle 'Enter' key for saving records
        private void Car_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (btnCar_Save.IsEnabled && IsCarValid())
                Car_Click(btnCar_Save, null);
        }
        private void Company_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (btnCompany_Save.IsEnabled && IsCompanyValid())
                Company_Click(btnCompany_Save, null);
        }
        private void Employee_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (btnEmployee_Save.IsEnabled && IsEmployeeValid())
                Employee_Click(btnEmployee_Save, null);
        }
        private void Person_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (btnPerson_Save.IsEnabled && IsPersonValid())
                Person_Click(btnPerson_Save, null);
        }
        private void Tenure_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (btnTenure_Save.IsEnabled && IsTenureValid())
                Tenure_Click(btnTenure_Save, null);
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
                cbCar_IsManualTransmission.IsChecked = default(bool);
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
                cbCar_IsManualTransmission.IsChecked = car.IsManualTransmission;
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
                cbPerson_HeightCm.Text = string.Empty;
            }
            else
            {
                Person person = e.AddedItems[0] as Person;
                cbPerson_PersonId.Text = person.PersonId.ToString();
                cbPerson_Name.Text = person.Name;
                cbPerson_Age.Text = person.Age.ToString();
                cbPerson_Hometown.Text = person.Hometown;
                cbPerson_HeightCm.Text = person.HeightCm.ToString();
            }
        }
        private void Tenure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the Tenure object
            if (e.AddedItems.Count == 0)
            {
                cbTenure_PersonId.Text = string.Empty;
                cbTenure_CompanyId.Text = string.Empty;
                dtTenure_StartDate.SelectedDate = DateTime.Today;
                dtTenure_EndDate.SelectedDate = null;
            }
            else
            {
                Tenure tenure = e.AddedItems[0] as Tenure;
                foreach (var item in cbTenure_PersonId.Items)
                {
                   if ((item as Person).PersonId == tenure.PersonId)
                      cbTenure_PersonId.SelectedItem = item;
                }
                foreach (var item in cbTenure_CompanyId.Items)
                {
                   if ((item as Company).CompanyId == tenure.CompanyId)
                      cbTenure_CompanyId.SelectedItem = item;
                }
                dtTenure_StartDate.SelectedDate = tenure.StartDate;
                dtTenure_EndDate.SelectedDate = tenure.EndDate;
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
            bool _IsManualTransmission = cbCar_IsManualTransmission.IsChecked ?? false;

            var record = new Car
            {
                CarId = _CarId,
                Make = _Make,
                Model = _Model,
                Year = _Year,
                PersonId = _PersonId,
                CompanyId = _CompanyId,
                IsManualTransmission = _IsManualTransmission,
            };

            UserDB.InsertOrReplace(record);
            LoadCar();

            // clear input elements
            cbCar_CarId.Text = default(string);
            cbCar_Make.Text = default(string);
            cbCar_Model.Text = default(string);
            cbCar_Year.Text = string.Empty;
            cbCar_PersonId.Text = string.Empty;
            cbCar_CompanyId.Text = string.Empty;
            cbCar_IsManualTransmission.IsChecked = default(bool);
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

            // clear input elements
            cbCompany_CompanyId.Text = string.Empty;
            cbCompany_Name.Text = default(string);
            cbCompany_StockSymbol.Text = default(string);
            dtCompany_Founded.SelectedDate = DateTime.Today;
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

            // clear input elements
            cbEmployee_EmployeeId.Text = string.Empty;
            cbEmployee_PersonId.Text = string.Empty;
            cbEmployee_CompanyId.Text = string.Empty;
            cbEmployee_Title.Text = default(string);
            cbEmployee_OfficeNumber.Text = string.Empty;
        }

        private void Person_Click(object sender, RoutedEventArgs e)
        {
            Person selected = (Person)dgPerson.SelectedItem;
            // read each Person column from its individual controls
            Guid _PersonId = selected?.PersonId ?? Guid.NewGuid();
            string _Name = cbPerson_Name.Text;
            int _Age = int.Parse(cbPerson_Age.Text);
            string _Hometown = cbPerson_Hometown.Text;
            double _HeightCm = double.Parse(cbPerson_HeightCm.Text);

            var record = new Person
            {
                PersonId = _PersonId,
                Name = _Name,
                Age = _Age,
                Hometown = _Hometown,
                HeightCm = _HeightCm,
            };

            UserDB.InsertOrReplace(record);
            LoadPerson();

            // clear input elements
            cbPerson_PersonId.Text = string.Empty;
            cbPerson_Name.Text = default(string);
            cbPerson_Age.Text = string.Empty;
            cbPerson_Hometown.Text = default(string);
            cbPerson_HeightCm.Text = string.Empty;
        }

        private void Tenure_Click(object sender, RoutedEventArgs e)
        {
            Guid g;
            Tenure selected = (Tenure)dgTenure.SelectedItem;
            // read each Tenure column from its individual controls
            var _PersonId = (cbTenure_PersonId.SelectedItem as Person).PersonId;
            var _CompanyId = (cbTenure_CompanyId.SelectedItem as Company)?.CompanyId;
            DateTime _StartDate = dtTenure_StartDate.SelectedDate ?? DateTime.MinValue;
            DateTime? _EndDate = dtTenure_EndDate.SelectedDate;

            var record = new Tenure
            {
                PersonId = _PersonId,
                CompanyId = _CompanyId,
                StartDate = _StartDate,
                EndDate = _EndDate,
            };

            UserDB.InsertOrReplace(record);
            LoadTenure();

            // clear input elements
            cbTenure_PersonId.Text = string.Empty;
            cbTenure_CompanyId.Text = string.Empty;
            dtTenure_StartDate.SelectedDate = DateTime.Today;
            dtTenure_EndDate.SelectedDate = null;
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
        public bool IsManualTransmission { get; set; }
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
        public double HeightCm { get; set; }
        public override string ToString() => Name;
    }

    public class Tenure
    {
        public Guid PersonId { get; set; }
        public Guid? CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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
            CreateTable<Tenure>();
			Commit();
		}
	}
}

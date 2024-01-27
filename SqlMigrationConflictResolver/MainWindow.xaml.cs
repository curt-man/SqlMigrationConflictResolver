using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

namespace SqlMigrationConflictResolver
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region hidden region
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private string _selectedItem;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<string> _items = new ObservableCollection<string>()
        {
            "One",
            "Two",
            "Three",
            "Four",
            "Five",
        };


        public IEnumerable Items
        {
            get { return _items; }
        }

        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public string NewItem
        {
            set
            {
                if (SelectedItem != null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(value))
                {
                    _items.Add(value);
                    SelectedItem = value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public string ScriptPath { get; set; }

        string[] commandStartKeywords = { "PRINT N'Creating", "PRINT N'Rebuilding", "PRINT N'Adding" };

        List<string> CommandsList = new List<string>();


        private void ConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".sql";
            dlg.Filter = "SQL (*.sql)|*.sql";


            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                SelectedFileLabel.Text = dlg.SafeFileName;
                ScriptPath = dlg.FileName;
                CodeBox.Text = UseFileReadAllLines(ScriptPath);
            }

        }

        public static string UseFileReadAllLines(string path)
        {
            var lines = File.ReadAllLines(path);
            var stringBuilder = new StringBuilder();

            foreach (var line in lines)
            {
                stringBuilder.AppendLine(line);
            }
            stringBuilder.Length -= Environment.NewLine.Length;

            return stringBuilder.ToString();
        }




    }
}

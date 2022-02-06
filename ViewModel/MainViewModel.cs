using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Services;

namespace WpfApp2.ViewModel
{
    class MainViewModel : Observer
    {
        /*private string _TextBlock = "0";

        public string TextBlock
        {
            get { return _TextBlock; }
            set { _TextBlock = value;
                OnPropertyChanged();}
        }
        public int Number = 0;
        public RelayCommand ClickCommand
        { get; set; }
        public MainViewModel()
        {
            ClickCommand = new RelayCommand(o => { Number++; TextBlock = Number.ToString(); });
        }*/

        private string _Login = String.Empty;

        public string Login
        {
            get { return _Login; }
            set { _Login = value;
                OnPropertyChanged();
            }
        }

        private string _Password = String.Empty;

        public string Password
        {
            get { return _Password; }
            set { _Password = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand CheckErrorRegisterCommand
        { get; set; }

        public RelayCommand LoginCommand
        { get; set; }

        public RelayCommand CloseRegisterCommand
        { get; set; }

        public RelayCommand CloseErrorCommand
        { get; set; }

        public RelayCommand CloseMainCommand
        { get; set; }

        public MainViewModel()
        {
            CloseMainCommand = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "BANK")
                    {
                        window.Close();
                    }
                }
            });
            CloseErrorCommand = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "errorWindow")
                    {
                        window.Close();
                    }
                }
            });
            CloseRegisterCommand = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "Регистрация")
                    {
                        window.Close();
                    }
                }
            });
            CheckErrorRegisterCommand = new RelayCommand(o => 
            {
                string FileDirectPath = $"{Environment.CurrentDirectory}\\{Login}";
                if (Password == String.Empty || Login == String.Empty || Directory.Exists(FileDirectPath) || Login.Contains(" "))
                {
                    ErrorWindowOpen();
                }
                else
                {
                    Directory.CreateDirectory(FileDirectPath);
                    File.Create(FileDirectPath + "\\Login.txt").Close();
                    File.Create(FileDirectPath + "\\Password.txt").Close();
                    File.Create(FileDirectPath + "\\MainCash.txt").Close();
                    File.Create(FileDirectPath + "\\History.txt").Close();
                    File.Create(FileDirectPath + "\\Credit.txt").Close();
                    File.WriteAllText(FileDirectPath + "\\Login.txt", Login);
                    File.WriteAllText(FileDirectPath + "\\Password.txt", Password);
                    File.WriteAllText(FileDirectPath + "\\MainCash.txt", "0");
                    File.WriteAllText(FileDirectPath + "\\Credit.txt", "0");
                    RegisterComplete registerWindow = new RegisterComplete();
                    registerWindow.Owner = Application.Current.MainWindow;
                    registerWindow.Show();
                    Login = String.Empty;
                    Password = String.Empty;
                }
            });
            LoginCommand = new RelayCommand(o =>
            {
                string FileDirectPath = $"{Environment.CurrentDirectory}\\{Login}";
                if (!Directory.Exists(FileDirectPath) || Login != File.ReadAllText(FileDirectPath + "\\Login.txt") || Password != File.ReadAllText(FileDirectPath + "\\Password.txt"))
                {
                    ErrorWindowOpen();
                }
                else
                {
                    File.Create($"{Environment.CurrentDirectory}\\TempLogin.txt").Close();
                    File.WriteAllText($"{Environment.CurrentDirectory}\\TempLogin.txt", Login);
                    BankWindow bankWindow = new BankWindow();
                    bankWindow.Owner = Application.Current.MainWindow;
                    bankWindow.Show();
                    Application.Current.MainWindow.Hide();
                }
            });
        }

        public void ErrorWindowOpen()
        {
            ErrorWindow errorWindow = new ErrorWindow();
            errorWindow.Owner = Application.Current.MainWindow;
            errorWindow.Show();
        }

    }
}

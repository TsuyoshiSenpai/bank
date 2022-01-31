using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Services;
using System.Text.RegularExpressions;

namespace WpfApp2.ViewModel
{
    class BankViewModel : Observer
    {
        private string _MainCash;

        public string MainCash
        {
            get { return _MainCash; }
            set { _MainCash = value;
                OnPropertyChanged();}
        }

        private string _Sum;

        public string Sum
        {
            get { return _Sum; }
            set { _Sum = value;
                OnPropertyChanged();
            }
        }

        private string _MSum;

        public string MSum
        {
            get { return _MSum; }
            set
            {
                _MSum = value;
                OnPropertyChanged();
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value;
                OnPropertyChanged();
            }
        }

        private string _CreditAmount = String.Empty;

        public string CreditAmount
        {
            get { return _CreditAmount; }
            set { _CreditAmount = value;
                OnPropertyChanged();
            }
        }


        private string _CreditAmountGeneral = String.Empty;

        public string CreditAmountGeneral
        {
            get { return _CreditAmountGeneral; }
            set { _CreditAmountGeneral = value;
                OnPropertyChanged();
            }
        }

        bool isDelayed;

        private string _History;

        public string History
        {
            get { return _History; }
            set { _History = value;
                OnPropertyChanged();
            }
        }


        #region commands
        public RelayCommand MyBillOpenCommand
        { get; set; }

        public RelayCommand ExitToMainCommand
        { get; set; }

        public RelayCommand BackToBaseCommand
        { get; set; }

        public RelayCommand ReturnCreditCommand
        { get; set; }

        public RelayCommand DelayCreditCommand
        { get; set; }

        public RelayCommand CountCreditCommand
        { get; set; }

        public RelayCommand OpenCreditWindow
        { get; set; }

        public RelayCommand CloseCreditWindow
        { get; set; }

        public RelayCommand AddCashCommand
        { get; set; }

        public RelayCommand CloseAddCashCommand
        { get; set; }

        public RelayCommand OpenAddCashCommand
        { get; set; }

        public RelayCommand OpenTakeTransferCommand
        { get; set; }

        public RelayCommand TakeCashCommand
        { get; set; }

        public RelayCommand CloseTakeTransferCommand
        { get; set; }

        public RelayCommand TransferCommand
        { get; set; }

        public RelayCommand OpenHistoryCommand
        { get; set; }

        public RelayCommand CloseHistoryCommand
        { get; set; }

        #endregion

        private string _Login;

        public string Login
        {
            get { return _Login; }
            set { _Login = value;
                OnPropertyChanged();
            }
        }

        public BankViewModel()
        {
            string TempFileDirectPath = $"{Environment.CurrentDirectory}\\TempLogin.txt";
            Login = File.ReadAllText(TempFileDirectPath);
            string HistoryPath = $"{Environment.CurrentDirectory}\\{Login}\\History.txt";
            MainCash = File.ReadAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt");
            History = File.ReadAllText(HistoryPath);
            string CreditPath = $"{Environment.CurrentDirectory}\\{Login}\\Credit.txt";
            CreditAmountGeneral = File.ReadAllText(CreditPath);
            CheckCredit();
            MyBillOpenCommand = new RelayCommand(o =>
            {
                MyBills myBill = new MyBills();
                myBill.Owner = Application.Current.MainWindow;
                myBill.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "MAIN MENU")
                    {
                        window.Close();
                    }
                }
            });
            ExitToMainCommand = new RelayCommand(o =>
            {
                BankWindow bankWindow = new BankWindow();
                bankWindow.Owner = Application.Current.MainWindow;
                bankWindow.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "MY BILLS")
                    {
                        window.Close();
                    }
                }
            });
            BackToBaseCommand = new RelayCommand(o =>
            {
                File.Delete($"{Environment.CurrentDirectory}\\TempLogin.txt");
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "BANK")
                    {
                        window.Show();
                    }
                    else if (window.Title == "MAIN MENU")
                    {
                        window.Close();
                    }
                }
            });
            OpenCreditWindow = new RelayCommand(o =>
            {
                Credit credit = new Credit();
                credit.Owner = Application.Current.MainWindow;
                credit.Show();
            });
            CloseCreditWindow = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "Credit")
                    {
                        window.Close();
                    }
                }
            });
            OpenAddCashCommand = new RelayCommand(o =>
            {
                AddCash addCash = new AddCash();
                addCash.Owner = Application.Current.MainWindow;
                addCash.Show();
            });
            CloseAddCashCommand = new RelayCommand(o =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "AddCash")
                    {
                        window.Close();
                    }
                }
            });
            AddCashCommand = new RelayCommand(o =>
            {
                PlusCash(Sum);
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "AddCash")
                    {
                        window.Close();
                    }
                }
            });
            OpenTakeTransferCommand = new RelayCommand(o =>
            {
                TakeTransferCash takeTransferCash = new TakeTransferCash();
                takeTransferCash.Owner = Application.Current.MainWindow;
                takeTransferCash.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "MAIN MENU")
                    {
                        window.Close();
                    }
                }
            });
            CloseTakeTransferCommand = new RelayCommand(o =>
            {
                BankWindow bankWindow = new BankWindow();
                bankWindow.Owner = Application.Current.MainWindow;
                bankWindow.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "TakeTransferCash")
                    {
                        window.Close();
                    }
                }
            });
            TakeCashCommand = new RelayCommand(o =>
            {
                MinusCash(MSum);
                MSum = String.Empty;
            });
            TransferCommand = new RelayCommand(o =>
            {
                string DirectPath = $"{Environment.CurrentDirectory}\\{UserName}";
                if (UserName != String.Empty && Directory.Exists(DirectPath))
                {
                    TransferCash(MSum);
                }
                else ErrorWindowOpen();
            });
            OpenHistoryCommand = new RelayCommand(o =>
            {
                HistoryWindow historyWindow = new HistoryWindow();
                historyWindow.Owner = Application.Current.MainWindow;
                historyWindow.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "MAIN MENU")
                    {
                        window.Close();
                    }
                }
            });
            CloseHistoryCommand = new RelayCommand(o =>
            {
                BankWindow bankWindow = new BankWindow();
                bankWindow.Owner = Application.Current.MainWindow;
                bankWindow.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "HistoryWindow")
                    {
                        window.Close();
                    }
                }
            });
            CountCreditCommand = new RelayCommand(o =>
            {
                if (CreditAmount.Contains(" ") || Regex.IsMatch(CreditAmount.Trim(), "^\\D+$") || CreditAmount.Contains("-") || double.Parse(CreditAmountGeneral) > 0)
                {
                    ErrorWindowOpen();
                }
                else
                {
                    double DCreditAmount = double.Parse(CreditAmount);
                    File.WriteAllText(CreditPath, (DCreditAmount * 1.05).ToString());
                    CreditAmountGeneral = (DCreditAmount * 1.05).ToString();
                    File.AppendAllText(HistoryPath, $"Вы взяли кредит на {CreditAmountGeneral} ₽ \n");
                    PlusCash(CreditAmountGeneral);
                    isDelayed = true;
                    CreditAmount = String.Empty;
                    ReturnCredit();
                }
            });
            ReturnCreditCommand = new RelayCommand(o =>
            {
                if (decimal.Parse(MainCash) >= decimal.Parse(CreditAmountGeneral))
                {
                    File.AppendAllText(HistoryPath, $"Вы вернули кредит на {CreditAmountGeneral} ₽ \n");
                    MinusCash(CreditAmountGeneral);
                    isDelayed = false;
                    File.WriteAllText(CreditPath,"0");
                    CreditAmountGeneral = "0";
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.Title == "TakeCredit")
                        {
                            window.Close();
                        }
                    }
                }
                else
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.Title == "TakeCredit")
                        {
                            window.Close();
                        }
                    }
                    ErrorWindowOpen();
                    isDelayed = true;
                }
            });
            DelayCreditCommand = new RelayCommand(o =>
            {
                isDelayed = true;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Title == "TakeCredit")
                    {
                        window.Close();
                    }
                }
            });
        }


        #region methods

        public void PlusCash(string Sum)
        {
            if (Sum.Contains(" ") || Regex.IsMatch(Sum.Trim(),"^\\D+$") || Sum.Contains("-"))
            {
                ErrorWindowOpen();
            }
            else
            {
                string HistoryPath = $"{Environment.CurrentDirectory}\\{Login}\\History.txt";
                decimal OldCash = decimal.Parse(File.ReadAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt"));
                decimal NewCash;
                NewCash = decimal.Parse(Sum) + OldCash;
                File.WriteAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt", NewCash.ToString());
                File.AppendAllText(HistoryPath, $"Счет пополнен на {Sum} ₽ \n");
                History = File.ReadAllText(HistoryPath);
                MainCash = File.ReadAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt");
            }
        }

        public void MinusCash(string Sum)
        {
            if (Sum.Contains(" ") || Regex.IsMatch(Sum.Trim(), "^\\D+$") || Sum.Contains("-"))
            {
                ErrorWindowOpen();
            }
            else
            {
                string HistoryPath = $"{Environment.CurrentDirectory}\\{Login}\\History.txt";
                decimal OldCash = decimal.Parse(File.ReadAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt"));
                decimal NewCash;
                if (OldCash >= decimal.Parse(Sum))
                {
                    NewCash = OldCash - decimal.Parse(Sum);
                    File.WriteAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt", NewCash.ToString());
                    MainCash = File.ReadAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt");
                    File.AppendAllText(HistoryPath, $"Со счета было снято {Sum} ₽ \n");
                    History = File.ReadAllText(HistoryPath);
                }
            }
        }

        public void TransferCash(string Sum)
        {
            if (Sum.Contains(" ") || Regex.IsMatch(Sum.Trim(), "^\\D+$") || Sum.Contains("-"))
            {
                ErrorWindowOpen();
            }
            else
            {
                string SenderHistoryPath = $"{Environment.CurrentDirectory}\\{Login}\\History.txt";
                string RecieverHistoryPath = $"{Environment.CurrentDirectory}\\{UserName}\\History.txt";
                decimal SenderCash = decimal.Parse(File.ReadAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt"));
                decimal RecieverCash = decimal.Parse(File.ReadAllText($"{Environment.CurrentDirectory}\\{UserName}\\MainCash.txt")); ;
                if (SenderCash >= decimal.Parse(Sum))
                {
                    RecieverCash += decimal.Parse(Sum);
                    SenderCash -= decimal.Parse(Sum);
                    File.WriteAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt", SenderCash.ToString());
                    File.AppendAllText(SenderHistoryPath, $"{Sum} ₽ было отправлено пользователю {UserName} \n");
                    MainCash = File.ReadAllText($"{Environment.CurrentDirectory}\\{Login}\\MainCash.txt");
                    File.WriteAllText($"{Environment.CurrentDirectory}\\{UserName}\\MainCash.txt", RecieverCash.ToString());
                    File.AppendAllText(RecieverHistoryPath, $"{Sum} ₽ было получено от пользователя {Login}");
                }
            }
        }

        public void ErrorWindowOpen()
        {
            ErrorWindow errorWindow = new ErrorWindow();
            errorWindow.Owner = Application.Current.MainWindow;
            errorWindow.Show();
        }

        public async void ReturnCredit()
        {
            while (isDelayed)
            {
                await Task.Delay(20000);
                TakeCredit takeCredit = new TakeCredit();
                takeCredit.Owner = Application.Current.MainWindow;
                takeCredit.Show();
            }
        }

        public void CheckCredit()
        {
            if (double.Parse(CreditAmountGeneral) > 0)
            {
                isDelayed = true;
                ReturnCredit();
            }
        }

        #endregion
    }
}

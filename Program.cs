using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;

namespace MiniBankSystem
{
    internal class Program
    {
        // Constants
        const double MinimumBalance = 50.0; // Minimum balance for accounts

        // File paths
        static string AccountsFilePath = "accounts.txt"; // File to store account details
        static string ReviewsFilePath = "reviews.txt"; // File to store reviews
        static string ReportFilePath = "report.txt"; // File to store daily report
        static string RequestFilePath = "requests.txt"; // File to store Request
        static string CustomersFilePath = "Customers.txt"; // File to store Customers
        static string AdminFilePath = "Admins.txt"; // File to store Admins

        // Global lists
        static List<int> accountNumbers = new List<int>(); // List to store account numbers
        static List<string> accountNames = new List<string>(); // List to store account names
        static List<string> NationalId = new List<string>(); // List to store account names
        static List<string> Customers = new List<string>(); // List to store users
        static List<string> Admins = new List<string>(); // List to store admin
        static List<double> balances = new List<double>(); // List to store account balances
        static List<double> loans = new List<double>(); // List to store loan amounts
        static List<string> Passwords = new List<string>(); // List to store passwords
        static List<string> PinCodeCustomers = new List<string>(); // List to store Customers Pins
        static List<string> PinCodeAdmins = new List<string>(); // List to store Admin Pins
        static List<string> transactions = new List<string>(); // List to store transaction history
        static List<string> transactionsLoan = new List<string>(); // List to store loan transaction history
        static List<string> reportLines = new List<string>(); // List to store daily report lines
        static List<string> reportMonth = new List<string>(); // List to store monthly report lines
        // Queue and Stack for account requests and reviews
        static Queue<string> createAccountRequests = new Queue<string>(); // Queue to store account requests
        static Stack<string> reviewsStack = new Stack<string>(); // Stack to store reviews


        // Account number generator
        static int lastAccountNumber;

        // Main method
        public static void Main(string[] args)
        {
            // Run the bank system
            LoadAllData();
            RunBankSystem();
        }
        // System Utilities
        public static void DisplaySystemUtilitiesMenu()
        {
            try
            {
               
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("╔══════════════════════════════╗");
                    Console.WriteLine("║      SYSTEM UTILITIES        ║");
                    Console.WriteLine("╠══════════════════════════════╣");
                    Console.WriteLine("║                              ║");
                    Console.WriteLine("║  1. Initialize System        ║");
                    Console.WriteLine("║  2. Shutdown System          ║");
                    Console.WriteLine("║  3. Display Main Menu        ║");
                    Console.WriteLine("║  4. Exit                     ║");
                    Console.WriteLine("║                              ║");
                    Console.WriteLine("╚══════════════════════════════╝");
                    Console.ResetColor();
                    Console.Write("\n  Select an option (1-4): ");
                    string ch = Console.ReadLine();

                    switch (ch)
                    {
                        case "1":
                            InitializeBankSystem(); // Initialize the bank system
                            break;
                        case "2":
                            ShutdownBankSystem(); // Shutdown the bank system
                            break;
                        case "3":
                            DisplayLoginScreen(); // Display the main menu
                            break;
                        case "4":
                            Console.WriteLine("Good Bye !!!! ");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to:" + ex.Message);
            }

        }

        // Initialize the Bank System
        public static void InitializeBankSystem()
        {
            try
            {
                Console.WriteLine("Initializing Mini Bank System...");

                // Ensure account and review files exist
                if (!File.Exists(AccountsFilePath))
                {
                    File.Create(AccountsFilePath).Close();
                    Console.WriteLine("Accounts file created.");
                }

                if (!File.Exists(ReviewsFilePath))
                {
                    File.Create(ReviewsFilePath).Close();
                    Console.WriteLine("Reviews file created.");
                }

                // Clear in-memory data
                accountNumbers.Clear();
                accountNames.Clear();
                NationalId.Clear();
                PinCodeAdmins.Clear();
                PinCodeCustomers.Clear();
                Admins.Clear();
                Customers.Clear();
                balances.Clear();
                loans.Clear();
                transactions.Clear();
                createAccountRequests.Clear();
                reviewsStack.Clear();

                // Reset account number tracking
                lastAccountNumber = 0;

                // Load data from files
                LoadAllData();

                Console.WriteLine("Bank system initialized successfully.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to:" + ex.Message);
            }
        }

        // Run the Bank System
        public static void RunBankSystem()
        {
            try
            {
                DisplaySystemUtilitiesMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error due to :" + ex.Message);
            }
        }

        // Shutdown the Bank System
        public static void ShutdownBankSystem()
        {
            try
            {
                Console.WriteLine("Shutting down the Mini Bank System...");

                // Save data before exiting
                SaveAllData();

                Console.WriteLine("All data saved successfully.");
                Console.WriteLine("System has been shut down safely. Press any key to exit.");
                Console.ReadLine();

                Environment.Exit(0); // Terminate the application
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error due to:" + ex.Message);
            }
        }
        // Login Screen
        public static void DisplayLoginScreen()
        {
            try
            {
                // Display the login screen
                Console.Clear();
                bool GO = true; // Flag to control the loop

                while (GO)
                {
                    // Clear the console and set the color
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow; // Set the color to dark blue

                    Console.WriteLine("\n\n");
                    Console.WriteLine(@"              ____________________________");
                    Console.WriteLine(@"              |       OMAN    BANK       |");
                    Console.WriteLine(@"             /___________________________/");
                    Console.WriteLine(@"            | []  []  []  []  []  []  [] |");
                    Console.WriteLine(@"            | []  []  []  []  []  []  [] |");
                    Console.WriteLine(@"            | []  []  []  []  []  []  [] |");
                    Console.WriteLine(@"         ___| []  []  []  []  []  []  [] |___");
                    Console.WriteLine(@"        | _ |____________________________| _ |");
                    Console.WriteLine(@"        [ATM]                            [ATM]");
                    Console.WriteLine(@"        |( )|           _____            |( )|");
                    Console.WriteLine(@"        | _ |           |   |            | _ |");
                    Console.WriteLine(@"        |   |           |_  |            |   |");
                    Console.WriteLine(@"        |___|___________|___|____________|___|");

                    Console.ForegroundColor = ConsoleColor.White; // Reset the color to white
                    Console.WriteLine("\n                                                ");
                    Console.WriteLine("    ╔════════════════════════════════════════════╗");
                    Console.WriteLine("    ║                                            ║");
                    Console.WriteLine("    ║            ACCOUNT ACCESS MENU             ║");
                    Console.WriteLine("    ║                                            ║");
                    Console.WriteLine("    ╠════════════════════════════════════════════╣");
                    Console.WriteLine("    ║                                            ║");
                    Console.WriteLine("    ║  1.   Customer Login                       ║");
                    Console.WriteLine("    ║  2.   Admin Login                          ║");
                    Console.WriteLine("    ║  3.   New Sign Up Registration             ║");
                    Console.WriteLine("    ║  4.   Save Data                            ║");
                    Console.WriteLine("    ║  5.   Back                                 ║");
                    Console.WriteLine("    ║                                            ║");
                    Console.WriteLine("    ║     Please select an option (1-5)          ║");
                    Console.WriteLine("    ╚════════════════════════════════════════════╝");

                    Console.ResetColor(); // Reset the color to default
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            CustomerLogin(); // Customer login
                            break;
                        case "2":
                            AdminLogin(); // Admin login
                            break;
                        case "3":
                            Console.WriteLine("Choose you are: 1. Admin || 2. Customer ");
                            string Check = Console.ReadLine();

                            if (Check == "1")
                            {
                                AdminSignUp();
                                SaveAllData();
                                break;
                            }
                            else if (Check == "2")
                            {
                                CustomerSignUp(); // Customer sign up
                                SaveAllData();
                                break;
                            }
                            break;
                        case "4":
                            SaveAllData();
                            GO = false; // Exit the loop
                            break;
                        case "5":
                            DisplaySystemUtilitiesMenu();
                            break;
                        default:
                            Console.WriteLine("Invalid option. Press any key to continue...");
                            Console.ReadLine();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error due to:" + ex.Message);
            }
        }
        
        // Sigin Up for Customer and admin
        public static void CustomerSignUp()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Customer Sign Up ===");
                Console.WriteLine("Please fill in the following details to create a new account.");
                Console.WriteLine("===================================");
                // Get user details with name validation loop
                string name;
                while (true)
                {
                    Console.Write("Enter Your ID Name: ");
                    name = Console.ReadLine();

                    if (Customers.Contains(name))
                    {
                        Console.WriteLine("Name already exists. Please choose a different name.");
                    }
                    else
                    {
                        break; // Exit the loop if name is unique
                    }
                }

                Customers.Add(name); // Add the account name to the list

                // Choose password
                Console.Write("Enter a New Pin: ");
                string pin = Console.ReadLine();
                PinCodeCustomers.Add(pin);
                SaveAllData();
                Console.WriteLine($"Customer Account created successfully! Your Name ID is: {name}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        }

        public static void AdminSignUp()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Admin Sign Up ===");
                Console.WriteLine("Please fill in the following details to create a new account.");
                Console.WriteLine("===================================");
                // Get user details with name validation loop
                string admin;
                while (true)
                {
                    Console.Write("Enter Your Admin ID: ");
                    admin = Console.ReadLine();

                    if (Admins.Contains(admin))
                    {
                        Console.WriteLine("Name already exists. Please choose a different name.");
                    }
                    else
                    {
                        break; // Exit the loop if name is unique
                    }
                }

                Admins.Add(admin); // Add the account name to the list

                // Choose password
                Console.Write("Enter a New Pin: ");
                string pin = Console.ReadLine();
                PinCodeAdmins.Add(pin);
                SaveAllData();
                Console.WriteLine($"Admin Account created successfully! Your Name ID is: {admin}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        }


        // Customer Login and Menu
        public static void CustomerLogin()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Customer Login ===");
                Console.WriteLine("Please enter your account number and password to log in.");
                Console.WriteLine("===================================");
                // Get user Info
                Console.Write("Enter User ID: ");
                string name = Console.ReadLine();
                Console.Write("Enter Pin: ");
                string pin = Console.ReadLine();
                // Check if the User ID and Pin are valid
                if (Customers.Contains(name) && PinCodeCustomers.Contains(pin))
                {
                    DisplayCustomerMenu(); // Display the customer menu
                }
                else
                {
                    Console.WriteLine("Invalid Account!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }

        }

        public static void DisplayCustomerMenu()
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White; // Set the color to dark blue

                    Console.WriteLine("\n");
                    Console.WriteLine("    ╔══════════════════════════════════════╗");
                    Console.WriteLine("    ║   █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█   ║");
                    Console.WriteLine("    ║   █         CUSTOMER MENU        █   ║");
                    Console.WriteLine("    ║   █▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█   ║");
                    Console.WriteLine("    ╠══════════════════════════════════════╣");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ║  [0]   Check Balance                 ║");
                    Console.WriteLine("    ║  [1]   Deposit                       ║");
                    Console.WriteLine("    ║  [2]   Withdraw                      ║");
                    Console.WriteLine("    ║  [3]   Transfer                      ║");
                    Console.WriteLine("    ║  [4]   Transaction History           ║");
                    Console.WriteLine("    ║  [5]   Account Details               ║");
                    Console.WriteLine("    ║  [6]   Apply for Loan                ║");
                    Console.WriteLine("    ║  [7]   Submit Review                 ║");
                    Console.WriteLine("    ║  [8]   Request New Account           ║");
                    Console.WriteLine("    ║  [9]   Change Password               ║");
                    Console.WriteLine("    ║  [10]   Account Statement            ║");
                    Console.WriteLine("    ║  [11]   Currency Converter           ║");
                    Console.WriteLine("    ║  [12]   Change Pin                   ║");
                    Console.WriteLine("    ║  [13]   Logout                       ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.Write("\n    Select an option (0-13): ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "0":
                            CheckMoney(); // Check balance
                            break;
                        case "1":
                            ProcessDeposit(); // Deposit funds
                            break;
                        case "2":
                            ProcessWithdrawal(); // Withdraw funds
                            break;
                        case "3":
                            ProcessTransfer(); // Transfer funds
                            break;
                        case "4":
                            ViewTransactionHistory(); // View transaction history
                            break;
                        case "5":
                            ViewAccountDetails(); // View account details
                            break;
                        case "6":
                            ApplyForLoan(); // Apply for a loan 
                            break;
                        case "7":
                            SubmitReview(); // Submit a review
                            break;
                        case "8":
                            RequestNewAccounts(); // Request a new account
                            break;
                        case "9":
                            ChangePassowrd(); // Change password
                            break;
                        case "10":
                            GenerateAccountStatement(); // Generate account statement
                            break;
                        case "11":
                            CurrencyConverter(); // Currency converter 
                            break;
                        case "12":
                            ChangePinCustomers();
                            break;
                        case "13":
                            Console.WriteLine("Login Out .........");
                            return;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        // Admin Login and Menu
        public static void AdminLogin()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Admin Login ===");
                Console.WriteLine("Please enter your admin username and password to log in.");
                Console.WriteLine("===================================");
                // Get admin info
                Console.Write("Enter Admin Username: ");
                string Admin = Console.ReadLine();
                Console.Write("Enter Admin Pin: ");
                string AdminPin = Console.ReadLine();
                // Check if the Addmin Name and Pin are valid
                if (Admins.Contains(Admin) && PinCodeAdmins.Contains(AdminPin))
                {
                    DisplayAdminMenu(); // Display the admin menu
                }
                else
                {
                    Console.WriteLine("Invalid Admin Info!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }

        }

        public static void DisplayAdminMenu()
        {
            try
            {
                // Display the admin menu
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White; // Set the color to cyan

                    Console.WriteLine("\n");
                    Console.WriteLine("    ╔══════════════════════════════════════╗");
                    Console.WriteLine("    ║   █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█   ║");
                    Console.WriteLine("    ║   █          ADMIN MENU          █   ║");
                    Console.WriteLine("    ║   █▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█   ║");
                    Console.WriteLine("    ╠══════════════════════════════════════╣");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ║  [1]   Account Management            ║");
                    Console.WriteLine("    ║  [2]   Transaction Processing        ║");
                    Console.WriteLine("    ║  [3]   Loan Management               ║");
                    Console.WriteLine("    ║  [4]   Reviews                       ║");
                    Console.WriteLine("    ║  [5]   Reporting                     ║");
                    Console.WriteLine("    ║  [6]   Requests                      ║");
                    Console.WriteLine("    ║  [7]   Process Account Requests      ║");
                    Console.WriteLine("    ║  [8]   Change Password               ║");
                    Console.WriteLine("    ║  [9]   Find Account                  ║");
                    Console.WriteLine("    ║  [10]   Change Pin                   ║");
                    Console.WriteLine("    ║  [11]   Show Top Three Richest       ║");
                    Console.WriteLine("    ║  [12]   Logout                       ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.Write("\n    Select an option (1-12): ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            DisplayAccountManagementMenu();// Account management menu
                            break;
                        case "2":
                            DisplayTransactionManagementMenu(); // Transaction management menu
                            break;
                        case "3":
                            DisplayLoanManagementMenu(); // Loan management menu
                            break;
                        case "4":
                            ViewReviews(); // View reviews
                            break;
                        case "5":
                            DisplayReportingMenu(); // Reporting menu
                            break;
                        case "6":
                            ViewRequsets(); // View requests
                            break;
                        case "7":
                            ProcessAccountRequests(); // Process account requests
                            break;
                        case "8":
                            ChangePassowrd(); // Change password
                            break;
                        // Find Account
                        case "9":
                            Console.WriteLine("Choose You Want Find Account By Name Or By Account Number");
                            string choose = Console.ReadLine();
                            if (choose == "Name")
                            {
                                SearchAccountByNameOrID();
                                break;
                            }
                            else if (choose == "Acoount Number")
                            {
                                Console.Write("Enter Account Number: ");
                                int accountNumber = int.Parse(Console.ReadLine());
                                FindAccount(accountNumber); // Find account
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Try Again Choice (Name or Account Number)");
                                break;
                            }
                        case "10":
                            ChangePinAdmin(); // Change Pin
                            break;
                        case "11":
                            ShowTopRichestCustomers();
                            break;
                        case "12":
                            Console.WriteLine("Logging out...");
                            return;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
           
        }

        // Sub-menus for Admin

        // Account Management Menu
        public static void DisplayAccountManagementMenu()
        {
            try
            {
                // Display the account management menu
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine("\n");
                    Console.WriteLine("    ╔══════════════════════════════════════╗");
                    Console.WriteLine("    ║   █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█   ║");
                    Console.WriteLine("    ║   █   ACCOUNT MANAGEMENT SYSTEM  █   ║");
                    Console.WriteLine("    ║   █▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█   ║");
                    Console.WriteLine("    ╠══════════════════════════════════════╣");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ║  [1]   Create New Account            ║");
                    Console.WriteLine("    ║  [2]   Close Account                 ║");
                    Console.WriteLine("    ║  [3]   View Account Details          ║");
                    Console.WriteLine("    ║  [4]   List All Accounts             ║");
                    Console.WriteLine("    ║  [5]   Export All Accounts To File   ║");
                    Console.WriteLine("    ║  [6]   Back to Admin Menu            ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.Write("\n    Select an option (1-5): ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            CreateNewAccount(); // Create new account
                            break;
                        case "2":
                            DeleteAccount(); // Delete account
                            break;
                        case "3":
                            ViewAccountDetails(); // View account details
                            break;
                        case "4":
                            ListAllAccounts(); // List all accounts
                            break;
                        case "5":
                            ExportAllAccountsToFile();
                            break;
                        case "6":
                            return;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        
        }

        // Transaction Management Menu
        public static void DisplayTransactionManagementMenu()
        {
            try
            {
                // Display the transaction management menu
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White; // Set the color to dark blue

                    Console.WriteLine("\n");
                    Console.WriteLine("    ╔══════════════════════════════════════╗");
                    Console.WriteLine("    ║   █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█   ║");
                    Console.WriteLine("    ║   █    TRANSACTION MANAGEMENT    █   ║");
                    Console.WriteLine("    ║   █▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█   ║");
                    Console.WriteLine("    ╠══════════════════════════════════════╣");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ║  [1]   Deposit                       ║");
                    Console.WriteLine("    ║  [2]   Withdrawal                    ║");
                    Console.WriteLine("    ║  [3]   Transfer                      ║");
                    Console.WriteLine("    ║  [4]   Check Balance                 ║");
                    Console.WriteLine("    ║  [5]   Transaction History           ║");
                    Console.WriteLine("    ║  [6]   Recurring Deposit             ║");
                    Console.WriteLine("    ║  [7]   Back to Admin Menu            ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.WriteLine("\n    Select an option (1-7): ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ProcessDeposit(); // Deposit funds
                            break;
                        case "2":
                            ProcessWithdrawal(); // Withdraw funds
                            break;
                        case "3":
                            ProcessTransfer();  // Transfer funds
                            break;
                        case "4":
                            CheckMoney(); // Check balance
                            break;
                        case "5":
                            ViewTransactionHistory(); // View transaction history
                            break;
                        case "6":
                            SetupRecurringDepositOnMonth(); // Setup recurring deposit
                            break;
                        case "7":
                            Console.WriteLine("Returning to Admin Menu...");
                            return;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        // Loan Management Menu
        public static void DisplayLoanManagementMenu()
        {
            try
            {
                // Display the loan management menu
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White; // Set the color to dark blue

                    Console.WriteLine("\n");
                    Console.WriteLine("    ╔══════════════════════════════════════╗");
                    Console.WriteLine("    ║   █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█   ║");
                    Console.WriteLine("    ║   █       LOAN MANAGEMENT        █   ║");
                    Console.WriteLine("    ║   █▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█   ║");
                    Console.WriteLine("    ╠══════════════════════════════════════╣");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ║  [1]   Approve Loan Application      ║");
                    Console.WriteLine("    ║  [2]   Process Loan Payment          ║");
                    Console.WriteLine("    ║  [3]   Back to Admin Menu            ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.Write("\n    Select an option (1-3): ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ApproveLoanApplication(); // Approve loan application
                            break;
                        case "2":
                            ProcessLoanPayment(); // Process loan payment
                            break;
                        case "3":
                            Console.WriteLine("Returning to Admin Menu...");
                            return;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
    
        }

        // Reporting Menu
        public static void DisplayReportingMenu()
        {
            try
            {
                // Display the reporting menu
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White; // Set the color to dark blue

                    Console.WriteLine("\n");
                    Console.WriteLine("    ╔══════════════════════════════════════╗");
                    Console.WriteLine("    ║   █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█   ║");
                    Console.WriteLine("    ║   █        REPORTING MENU        █   ║");
                    Console.WriteLine("    ║   █▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█   ║");
                    Console.WriteLine("    ╠══════════════════════════════════════╣");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ║  [1]   Generate Daily Report         ║");
                    Console.WriteLine("    ║  [2]   Generate Monthly Statement    ║");
                    Console.WriteLine("    ║  [3]   Back to Admin Menu            ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.Write("\n    Select report type (1-3): ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            GenerateDailyReport(); // Generate daily report
                            break;
                        case "2":
                            GenerateMonthlyStatement(); // Generate monthly statement
                            break;
                        case "3":
                            Console.WriteLine("Returning to Admin Menu...");
                            return;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
         
        }

        // Review Management
        public static void ViewRequsets()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== View Requests ===");
                Console.WriteLine("===================================");
                // Display pending requests
                if (createAccountRequests.Count == 0) // Check if there are any requests
                {
                    Console.WriteLine("No requests pending.");
                    return;
                }
                // Display each request in the queue
                Console.WriteLine("Pending Requests:");
                Console.WriteLine("===================================");
                foreach (var request in createAccountRequests) // loop through the requests
                {
                    Console.WriteLine(request);
                }
                Console.WriteLine("===================================");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
   
        }

        // Account Operations

        // Request New Account
        public static void RequestNewAccounts()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Request New Account ===");
                Console.WriteLine("Please fill in the following details to request a new account.");
                Console.WriteLine("===================================");

                Console.Write("Enter your full name: ");
                string name = Console.ReadLine();

                Console.Write("Enter your National ID: ");
                string nationalID = Console.ReadLine();

                // Check if the National ID already exists in approved accounts
                if (NationalId.Contains(nationalID))
                {
                    Console.WriteLine("An account with this National ID already exists.");
                    return;
                }

                // Check if the National ID already exists in pending requests
                foreach (string request in createAccountRequests)
                {
                    string[] parts = request.Split("|");

                    if (parts.Length > 1 && parts[1].Trim() == nationalID)
                    {
                        Console.WriteLine("A request with this National ID is already pending.");
                        return;
                    }
                }

                // If passed both checks, enqueue the request
                string newRequest = $"{name}|{nationalID}";
                createAccountRequests.Enqueue(newRequest);
                Console.WriteLine("Your request has been submitted successfully.");
                SaveAllData();
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
         
        }

        // Process Account Requests
        public static void ProcessAccountRequests()
        {
            try
            {
                // Display the process account requests screen
                Console.Clear();
                Console.WriteLine("=== Account Requests ===");
                Console.WriteLine("===================================");
                // Check if there are any requests
                if (createAccountRequests.Count == 0) // Check if there are any requests
                {
                    Console.WriteLine("No account requests pending.");
                    return;
                }
                // Process each request in the queue
                while (createAccountRequests.Count > 0)
                {
                    Console.Clear();
                    Console.WriteLine("╔════════════════════════════════════════════╗");
                    Console.WriteLine("║       PENDING ACCOUNT CREATION REQUESTS    ║");
                    Console.WriteLine("╠════════════════════════════════════════════╣");

                    // Display all pending requests with numbers
                    int requestNumber = 1;
                    List<string> requestsList = createAccountRequests.ToList();

                    foreach (string request in createAccountRequests)
                    {
                        string[] parts = request.Split("|");
                        Console.WriteLine($"║ {requestNumber}. {parts[0]} (ID: {parts[1]})              ║");
                        requestNumber++;
                    }

                    Console.WriteLine("╚════════════════════════════════════════════╝");

                    // Let admin choose which request to process
                    Console.Write("\nEnter request number to process (0 to exit): ", createAccountRequests.Count);
                    int selected = int.Parse(Console.ReadLine());

                    if (selected == 0) break; // Exit option
                    if (selected < 1 || selected > createAccountRequests.Count) continue;
                    // Process selected request
                    string selectRequest = createAccountRequests.ElementAt(selected - 1);
                    string[] selectedParts = selectRequest.Split("|");
                    string name = selectedParts[0];
                    string nationalID = selectedParts[1];

                    Console.Clear();
                    Console.WriteLine($"╔════════════════════════════════════════════╗");
                    Console.WriteLine($"║ PROCESSING: {name} (ID: {nationalID})      ║");
                    Console.WriteLine($"╚════════════════════════════════════════════╝");

                    Console.Write("Approve this request? (y/n): ");
                    string response = Console.ReadLine();

                    if (response == "y")
                    {
                        // Create new account
                        int newAccountNumber = GenerateAccountNumber();
                        accountNumbers.Add(newAccountNumber); // Add account number to list
                        accountNames.Add(name); // add name to list
                        NationalId.Add(nationalID);
                        balances.Add(MinimumBalance); // init balance
                        loans.Add(0.0); // init loan
                        transactions.Add("");     
                        transactionsLoan.Add(""); 

                        Console.Write("Enter a password: ");
                        string password = Console.ReadLine();
                        Passwords.Add(password); // Set password
                        transactions.Add(""); // reset transactions

                        Console.WriteLine($"\nAccount created successfully!");
                        Console.WriteLine($"Account Number: {newAccountNumber}");
                        Console.WriteLine($"Account Holder: {name}");
                        Console.WriteLine($"National Id: {nationalID}");
                    }

                    else
                    {
                        Console.WriteLine($"Request for {name} has been rejected.");
                    }

                    // remove the selected request
                    requestsList.RemoveAt(selected - 1);
                    createAccountRequests = new Queue<string>(requestsList);
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadLine();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
       

        }

        // Create New Account
        public static void CreateNewAccount()
        {
            try
            {
                // Display the create new account screen
                Console.Clear();
                Console.WriteLine("=== Create New Account ===");
                Console.WriteLine("Please fill in the following details to create a new account.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter your full name: ");
                string name = Console.ReadLine();
                Console.Write("Enter your National ID: ");
                string nationalID = Console.ReadLine();
                int newAccountNumber = GenerateAccountNumber(); // Generate a unique account number
                accountNumbers.Add(newAccountNumber); // Add the new account number to the list
                accountNames.Add(name); // Add the account name to the list
                balances.Add(MinimumBalance); // Initial balance
                loans.Add(0.0); // Initial loan amount
                transactions.Add("");
                transactionsLoan.Add("");

                // Choose password
                Console.Write("Enter a password: ");
                string password = Console.ReadLine();
                Passwords.Add(password);
                transactions.Add(""); // Empty transaction history
                Console.WriteLine($"Account created successfully! Your Account Number is: {newAccountNumber}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
           
        }

        // Delete Account
        public static void DeleteAccount()
        {
            try
            {
                // Display the delete account screen
                Console.Clear();
                Console.WriteLine("=== Delete Account ===");
                Console.WriteLine("Please fill in the following details to delete your account.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                string input = Console.ReadLine();

                // Check if the account number is valid
                if (!int.TryParse(input, out int accountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != password) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }

                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                if (index != -1) // Check if the account number exists
                {
                    Console.WriteLine($"Account Number: {accountNumber}");
                    Console.WriteLine($"Account Name: {accountNames[index]}");
                    Console.WriteLine($"Current Balance: {balances[index]}");
                    Console.WriteLine($"Loan Amount: {loans[index]}");
                    Console.WriteLine($"Transaction History: {transactions[index]}");
                    Console.WriteLine($"Loan Transaction History: {transactionsLoan[index]}");
                    // Remove the account from the lists
                    accountNumbers.RemoveAt(index); // Remove the account number from the list
                    accountNames.RemoveAt(index); // Remove the account name from the list
                    balances.RemoveAt(index); // Remove the balance from the list
                    loans.RemoveAt(index); // Remove the loan amount from the list
                    transactionsLoan.RemoveAt(index); // Remove the loan transaction history from the list
                    Passwords.RemoveAt(index); // Remove the password from the list 
                    transactions.RemoveAt(index); // Remove the transaction history from the list 
                    Console.WriteLine($"Account {accountNumber} deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
      
        }

        // View Account Details
        public static void ViewAccountDetails()
        {
            try
            {
                // Display the view account details screen
                Console.Clear();
                Console.WriteLine("=== View Account Details ===");
                Console.WriteLine("Please enter your account number to view your details.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                // print account details
                Console.WriteLine("===================================");
                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                Console.WriteLine($"Account Number: {accountNumber}");
                Console.WriteLine($"Account Name: {accountNames[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine($"Current Balance: {balances[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine($"Loan Amount: {loans[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine($"Transaction History: {transactions[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine($"Loan Transaction History: {transactionsLoan[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine("===================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
       
        }

        // List All Accounts
        public static void ListAllAccounts()
        {
            try
            {
                // Display the list of all accounts
                Console.Clear();
                Console.WriteLine("=== List of All Accounts ===");
                Console.WriteLine("===================================");

                // Check if there are any accounts
                if (accountNumbers.Count == 0) // Check if there are any accounts
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }

                // Display each account in the list
                for (int i = 0; i < accountNumbers.Count; i++) // loop through the accounts
                {
                    Console.WriteLine("------------------------------");
                    Console.WriteLine($"Account ID: {i + 1}");
                    Console.WriteLine($"Account Number: {accountNumbers[i]}");
                    Console.WriteLine($"Account Name: {accountNames[i]}");
                    Console.WriteLine($"Balance: {balances[i]}");
                    Console.WriteLine($"Loan Amount: {loans[i]}");
                    Console.WriteLine($"Transaction History: {transactions[i]}");
                    Console.WriteLine($"Loan Transaction History: {transactionsLoan[i]}");
                    Console.WriteLine("------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
         
        }

        // Transaction Processing

        // Process Deposit
        public static void ProcessDeposit()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Deposit Funds ===");
                Console.WriteLine("Please enter your account number and password to deposit funds.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != password) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }

                // Check if the account number exists
                CheckAccountExists(accountNumber);

                // print account details
                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                Console.WriteLine($"Your current balance is: {balances[accountNumbers.IndexOf(accountNumber)]}");

                // Enter for deposit amount
                Console.WriteLine("Please enter the amount you want to deposit.");
                Console.Write("Enter Deposit Amount: ");
                double amount = double.Parse(Console.ReadLine());
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                if (index != -1) // Check if the account number exists
                {
                    // Check if the deposit amount is valid
                    if (amount <= 0)
                    {
                        Console.WriteLine("Invalid deposit amount.");
                        return;
                    }
                    // Update the balance and transaction history
                    balances[index] += amount;
                    transactions[index] += $"Deposit: {amount} | {DateTime.Now} "; // Add the deposit transaction to the history
                    Console.WriteLine($"Deposit successful! New balance: {balances[index]}"); // Display the new balance
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        // Process Withdrawal
        public static void ProcessWithdrawal()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Withdraw Funds ===");
                Console.WriteLine("Please enter your account number and password to withdraw funds.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != password) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }

                // Check if the account number exists
                CheckAccountExists(accountNumber);
                // print account details
                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                Console.WriteLine($"Your current balance is: {balances[accountNumbers.IndexOf(accountNumber)]}");

                // Enter for withdrawal amount
                Console.WriteLine("Please enter the amount you want to withdraw.");
                Console.Write("Enter Withdrawal Amount: ");
                double amount = double.Parse(Console.ReadLine());
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                if (index != -1) // Check if the account number exists
                {
                    // Check if the withdrawal amount is valid
                    if (amount <= 0)
                    {
                        Console.WriteLine("Invalid withdrawal amount.");
                        return;
                    }
                    // Check if there are sufficient funds for withdrawal
                    if (balances[index] < MinimumBalance)
                    {
                        Console.WriteLine("Insufficient funds for withdrawal.");
                        return;
                    }

                    // Check if the withdrawal amount is greater than the balance
                    if (amount > balances[index] - MinimumBalance)
                    {
                        Console.WriteLine("Withdrawal amount exceeds balance.");
                        return;
                    }
                    // Check if the withdrawal amount exceeds the balance
                    if (amount > balances[index])
                    {
                        Console.WriteLine("Withdrawal amount exceeds balance.");
                        return;
                    }
                    // Update the balance and transaction history
                    if (balances[index] >= amount + MinimumBalance) // Check if there are sufficient funds for withdrawal
                    {
                        balances[index] -= amount;
                        transactions[index] += $"Withdrawal: {amount} | {DateTime.Now} "; // Add the withdrawal transaction to the history
                        Console.WriteLine($"Withdrawal successful! New balance: {balances[index]}"); // Display the new balance
                    }
                    else
                    {
                        Console.WriteLine("Insufficient funds for withdrawal.");
                    }
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }

        }

        // Process Transfer
        public static void ProcessTransfer()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Transfer Funds ===");
                Console.WriteLine("Please enter your account number and password to transfer funds.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int fromAccountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(fromAccountNumber);
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(fromAccountNumber)] != password) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(fromAccountNumber);
                // print account details
                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(fromAccountNumber)]}.");
                Console.WriteLine($"Your current balance is: {balances[accountNumbers.IndexOf(fromAccountNumber)]}");

                // Enter Recipient Account Number
                Console.Write("Enter Account Number: ");
                string input2 = Console.ReadLine();
                if (!int.TryParse(input2, out int toAccountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }
                // Check if the recipient account number exists
                CheckAccountExists(toAccountNumber);

                // print recipient account details
                Console.Write("Enter Transfer Amount: ");
                double amount = double.Parse(Console.ReadLine());
                int indexFrom = accountNumbers.IndexOf(fromAccountNumber); // Get the index of the sender account number
                int indexTo = accountNumbers.IndexOf(toAccountNumber); // Get the index of the recipient account number 
                if (indexFrom != -1 && indexTo != -1) // Check if the account numbers exist
                {
                    // Check if the transfer amount is valid
                    if (amount <= 0)
                    {
                        Console.WriteLine("Invalid transfer amount.");
                        return;
                    }
                    // Check if there are sufficient funds for transfer
                    if (balances[indexFrom] < MinimumBalance)
                    {
                        Console.WriteLine("Insufficient funds for transfer.");
                        return;
                    }
                    // Update the balance and transaction history

                    if (balances[indexFrom] >= amount + MinimumBalance) // Check if there are sufficient funds for transfer
                    {
                        balances[indexFrom] -= amount; // Reduce the transfer amount from the sender's balance
                        balances[indexTo] += amount; // Add the transfer amount to the recipient's balance
                        transactions[indexFrom] += $"Transfer to {toAccountNumber}: {amount} | {DateTime.Now} "; // Add the transfer transaction to the history
                        transactions[indexTo] += $"Transfer from {fromAccountNumber}: {amount} | {DateTime.Now} "; // Add the transfer transaction to the history 
                        Console.WriteLine($"Transfer successful! New balance: {balances[indexFrom]}");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient funds for transfer.");
                    }
                }
                else
                {
                    Console.WriteLine("One or both accounts not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
 
        }

        // Check Money
        public static void CheckMoney()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Check Balance ===");
                Console.WriteLine("Please enter your account number and password to check your balance.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(accountNumber);
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != password) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }
                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                Console.WriteLine($"Your current balance is: {balances[accountNumbers.IndexOf(accountNumber)]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: ", ex.Message);
            }
         
        }

        // View Transaction History
        public static void ViewTransactionHistory()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Transaction History ===");
                Console.WriteLine("Please enter your account number and password to view your transaction history.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(accountNumber);

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != password) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }
                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                Console.WriteLine($"Transaction History: {transactions[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine($"Loan Transaction History: {transactionsLoan[accountNumbers.IndexOf(accountNumber)]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
         
        }

        // Loan Operations

        // Apply for Loan
        public static void ApplyForLoan()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Apply for Loan ===");
                Console.WriteLine("Please enter your account number and password to apply for a loan.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(accountNumber);
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != password) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }
                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                Console.Write("Enter Loan Amount: ");
                double loanAmount = double.Parse(Console.ReadLine());
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                if (index != -1) // Check if the account number exists
                {
                    // Check if the loan amount is valid
                    if (loanAmount <= 0)
                    {
                        Console.WriteLine("Invalid loan amount.");
                        return;
                    }
                    // Check if the loan amount exceeds the balance
                    if (loanAmount > balances[index])
                    {
                        Console.WriteLine("Loan amount exceeds balance.");
                        return;
                    }
                    // Update the loan amount and transaction history
                    loans[index] += loanAmount; // Add the loan amount to the account
                    transactionsLoan[index] += $"Loan Applied: {loanAmount} | {DateTime.Now} "; // Add the loan application to the transaction history
                    Console.WriteLine($"Loan application successful! Loan Amount: {loanAmount}"); // Display the loan amount
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        // Approve Loan Application
        public static void ApproveLoanApplication()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Approve Loan Application ===");
                Console.WriteLine("Please enter your account number and password to approve a loan application.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(accountNumber);

                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                // Admin to approve the loan application Yes/No
                Console.Write("Approve this loan application? (y/n): ");
                string response = Console.ReadLine();
                if (response.ToLower() == "y") // Check if the response is yes
                {
                    // Process loan application
                    double loanAmount = loans[accountNumbers.IndexOf(accountNumber)]; // Get the loan amount
                    int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                    if (index != -1) // Check if the account number exists
                    {
                        // Check if the loan amount is valid
                        if (loanAmount <= 0)
                        {
                            Console.WriteLine("Invalid loan amount.");
                            return;
                        }
                        // Update the loan amount and transaction history
                        loans[index] += loanAmount; // Add the loan amount to the account
                        transactionsLoan[index] += $"Loan Approved: {loanAmount} | {DateTime.Now} "; // Add the loan approval to the transaction history
                        Console.WriteLine($"Loan application approved! Loan Amount: {loanAmount}"); // Display the loan amount
                    }
                    else
                    {
                        Console.WriteLine("Account not found.");
                    }
                }
                else
                {
                    loans[accountNumbers.IndexOf(accountNumber)] = 0; // Reset the loan amount
                                                                      // empty transaction history
                    transactionsLoan[accountNumbers.IndexOf(accountNumber)] = ""; // Reset the transaction history
                    transactionsLoan[accountNumbers.IndexOf(accountNumber)] += $"Loan Rejected | {DateTime.Now} "; // Add the loan rejection to the transaction history
                    Console.WriteLine($"Loan application rejected! Loan Amount: {loans[accountNumbers.IndexOf(accountNumber)]}"); // Display the loan amount
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
       
        }

        // Process Loan Payment
        public static void ProcessLoanPayment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Loan Payment ===");
                Console.WriteLine("Please enter your account number and password to make a loan payment.");
                Console.WriteLine("===================================");
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber))
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(accountNumber);

                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                // Get the loan amount
                Console.Write("Enter Loan Payment Amount: ");
                double paymentAmount = double.Parse(Console.ReadLine());
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                if (index != -1)
                {
                    if (loans[index] >= paymentAmount) // Check if the payment amount is valid
                    {
                        // Update the loan amount and transaction history
                        loans[index] -= paymentAmount; // Reduce the payment amount from the loan
                        transactionsLoan[index] += $"Loan Payment: {paymentAmount} | {DateTime.Now} "; // Add the loan payment to the transaction history
                        Console.WriteLine($"Payment successful! Remaining loan amount: {loans[index]}"); // Display the remaining loan amount
                    }
                    else
                    {
                        Console.WriteLine("Payment exceeds remaining loan amount.");
                    }
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        // Reporting

        // Generate Account Statement Daily
        public static void GenerateDailyReport()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Daily Report ===");
                Console.WriteLine("Generating daily report...");
                // Generate daily report
                reportLines.Clear();
                reportLines.Add("Daily Report");
                reportLines.Add("Date: " + DateTime.Now.ToString("yyyy-MM-dd"));
                reportLines.Add("===================================");
                reportLines.Add("| Account Number | Account Name | Balance | Loan | Transactions | Transactions Loan |");
                reportLines.Add("-----------------------------------");

                // print account details
                for (int i = 0; i < accountNumbers.Count; i++)
                {
                    reportLines.Add($"| {accountNumbers[i]} | {accountNames[i]} | {balances[i]} | {loans[i]} | {transactions[i]} | {transactionsLoan[i]} ");
                }
                reportLines.Add("-----------------------------------");
                reportLines.Add("End of Report");
                // Display the report
                Console.WriteLine("Daily Report:");
                foreach (var line in reportLines)
                {
                    Console.WriteLine(line);
                }
                // Save the report to a file
                try
                {
                    using (StreamWriter writer = new StreamWriter(ReportFilePath))
                    {
                        foreach (var line in reportLines)
                        {
                            writer.WriteLine(line);
                        }
                    }
                    Console.WriteLine("Report saved successfully.");
                }
                catch
                {
                    Console.WriteLine("Error saving report.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
 
        }

        // Generate Monthly Statement
        public static void GenerateMonthlyStatement()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Monthly Statement ===");
                Console.WriteLine("Generating monthly statement...");
                // Generate monthly statement
                reportMonth.Clear();
                reportMonth.Add("Monthly Statement");
                reportMonth.Add("Date: " + DateTime.Now.ToString("yyyy-MM-dd"));
                reportMonth.Add("===================================");
                reportMonth.Add("| Account Number | Account Name | Balance | Loan | Transactions | Transactions Loan |");
                reportMonth.Add("-----------------------------------");
                for (int i = 0; i < accountNumbers.Count; i++)
                {
                    reportMonth.Add($"| {accountNumbers[i]} | {accountNames[i]} | {balances[i]} | {loans[i]} | {transactions[i]} | {transactionsLoan[i]}");
                }
                reportMonth.Add("-----------------------------------");
                reportMonth.Add("End of Statement");
                // Display the statement
                Console.WriteLine("Monthly Statement:");
                foreach (var line in reportMonth)
                {
                    Console.WriteLine(line);
                }
                // Save the statement to a file
                try
                {
                    using (StreamWriter writer = new StreamWriter(ReportFilePath))
                    {
                        foreach (var line in reportMonth)
                        {
                            writer.WriteLine(line);
                        }
                    }
                    Console.WriteLine("Statement saved successfully.");
                }
                catch
                {
                    Console.WriteLine("Error saving statement.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
           
        }

        // Save and Load Accounts in Files
        public static void SaveAccountsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        // Safely get each item or default if missing
                        string accountName = (i < accountNames.Count) ? accountNames[i] : "Unknown";
                        string nationalId = (i < NationalId.Count) ? NationalId[i] : "Unknown";
                        double balance = (i < balances.Count) ? balances[i] : 0.0;
                        double loan = (i < loans.Count) ? loans[i] : 0.0;
                        string password = (i < Passwords.Count) ? Passwords[i] : "password123";
                        string transaction = (i < transactions.Count) ? transactions[i] : "";
                        string transactionLoan = (i < transactionsLoan.Count) ? transactionsLoan[i] : "";

                        writer.WriteLine($"{accountNumbers[i]}|{accountName}|{nationalId}|{balance}|{loan}|{password}|{transaction}|{transactionLoan}");
                    }
                }
                Console.WriteLine("Accounts saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving accounts: " + ex.Message);
            }
        }

        public static void LoadAccountsFromFile()
        {
            try
            {
                if (File.Exists(AccountsFilePath))
                {
                    foreach (var line in File.ReadAllLines(AccountsFilePath))
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 7)
                        {
                            accountNumbers.Add(int.Parse(parts[0]));
                            accountNames.Add(parts[1]);
                            NationalId.Add(parts[2]);
                            balances.Add(double.Parse(parts[3]));
                            loans.Add(double.Parse(parts[4]));
                            Passwords.Add(parts[5]);
                            transactions.Add(parts[6]);
                            transactionsLoan.Add(parts.Length > 7 ? parts[7] : "");
                        }
                    }
                }
                Console.WriteLine("Accounts loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading accounts: " + ex.Message);
            }

        }

        // Save and Load Customers and Admin Accounts and Requests
        public static void SaveAdminAccountsToFile()
        {
            try
            {        
                using (StreamWriter writer = new StreamWriter(AdminFilePath))
                {
                    for (int i = 0; i < Admins.Count; i++)
                    {
                        // Write account details to the file
                        writer.WriteLine($"{Admins[i]}|{PinCodeAdmins[i]}");
                    }
                }
                  Console.WriteLine("Admin Accounts saved successfully.");
                                              
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
    
        }

        public static void LoadAdminAccountsFromFile()
        {
            try
            {
                if (File.Exists(AdminFilePath))
                {
                    foreach (var line in File.ReadAllLines(AdminFilePath))
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 2)
                        {
                            Admins.Add(parts[0]);
                            PinCodeAdmins.Add(parts[1]);
                        }
                    }
                }

                Console.WriteLine("Admin Accounts loaded successfully.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
         
        }

        public static void SaveCustomerAccountsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(CustomersFilePath))
                {
                    for (int i = 0; i < Customers.Count; i++)
                    {
                       // Write account details to the file
                       writer.WriteLine($"{Customers[i]}|{PinCodeCustomers[i]}");
                    }
                }
                    Console.WriteLine("Customers Accounts saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        public static void LoadCustomerAccountsFromFile()
        {
            try
            {
                if (File.Exists(CustomersFilePath))
                {
                    foreach (var line in File.ReadAllLines(CustomersFilePath))
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 2)
                        {
                            Customers.Add(parts[0]);
                            PinCodeCustomers.Add(parts[1]);
                        }
                    }
                }
                Console.WriteLine("Customer Accounts loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
     
        }

        public static void SaveRequeststoFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(RequestFilePath))
                {
                    foreach (var request in createAccountRequests)
                    {
                        if (string.IsNullOrWhiteSpace(request))
                            continue; // Skip empty requests

                        string[] parts = request.Split(new string[] { "||" }, StringSplitOptions.None);

                        if (parts.Length >= 2)
                        {
                            writer.WriteLine(request); // Save only properly formatted requests
                        }
                        else
                        {
                            Console.WriteLine($"Skipped saving invalid request: {request}");
                        }
                    }
                }

                Console.WriteLine("Account requests saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }

        }

        public static void LoadRequestsFromFile()
        {
            try
            {
                if (File.Exists(RequestFilePath))
                {
                    foreach (var line in File.ReadAllLines(RequestFilePath))
                    {
                        createAccountRequests.Enqueue(line);
                    }
                }
                Console.WriteLine($"Loaded {createAccountRequests.Count} account requests.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
  
        }

        public static void ExportAllAccountsToFile()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Export All Account Info ===");

                string exportFilePath = "AllAccountsExport.txt"; // You can change this to .txt if preferred

                using (StreamWriter writer = new StreamWriter(exportFilePath))
                {
                    // Write headers
                    writer.WriteLine("AccountNumber,Name,NationalID,Balance,Loan,Password,TransactionHistory,LoanTransactionHistory");

                        // Write each account's info
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        string nationalId = (i < NationalId.Count) ? NationalId[i] : "N/A"; // If NationalId not available
                        string transactionHistory = transactions[i]?.Replace(",", ";");     // Replace commas inside history
                        string loanTransactionHistory = transactionsLoan[i]?.Replace(",", ";");

                        string line = $"{accountNumbers[i]}," +
                                     $"\"{accountNames[i]}\"," +
                                     $"\"{NationalId[i]}\"," +
                                     $"{balances[i]}," +
                                     $"{loans[i]}," +
                                     $"\"{Passwords[i]}\"," +
                                     $"\"{transactions[i]}\"," +
                                     $"\"{transactionsLoan[i]}\"";

                        writer.WriteLine(line);
                    }
                }

                  Console.WriteLine($"All account data successfully exported to: {exportFilePath}");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
         
        }

        public static void SaveReportsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ReportFilePath))
                {
                    for (int i = 0; i < reportLines.Count; i++)
                    {
                        writer.WriteLine($"{reportLines[i]}|{reportMonth[i]}");
                    }
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Error due to :" + ex.Message);
            }
        }

        public static void LoadReportsFromFile()
        {
            try
            {
                if (File.Exists(ReportFilePath))
                {
                    foreach (var line in File.ReadAllLines(ReportFilePath))
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 2)
                        {
                            reportLines.Add(parts[0]);
                            reportMonth.Add(parts[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error due to :" + ex.Message);
            }
        }

        // Reviews

        // Submit Review
        public static void SubmitReview()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Submit Review ===");
                Console.WriteLine("Please enter your review.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter your name: ");
                string name = Console.ReadLine();
                Console.Write("Enter your email: ");
                string email = Console.ReadLine();
                Console.Write("Enter your phone number: ");
                string phoneNumber = Console.ReadLine();
                Console.Write("Enter your account number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(accountNumber);
                Console.Write("Enter your review: ");
                string review = Console.ReadLine();
                // Add the review to the stack
                reviewsStack.Push(review); // Add the review to the stack
                SaveAllData();
                Console.WriteLine("Review submitted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
           
        }

        // View Reviews
        public static void ViewReviews()
        {
            try
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan; //  // Set accent color as Cyan
                Console.WriteLine("╔══════════════════════════════════╗");
                Console.WriteLine("║            REVIEWS               ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.ResetColor();

                if (reviewsStack.Count == 0) // if count = 0 ( no review )
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n  No reviews available.\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("┌──────────────────────────────┐");
                    Console.ResetColor();

                    bool alternate = false;
                    foreach (string review in reviewsStack)
                    {
                        // Left border of row
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("│ ");
                        Console.ResetColor();

                        Console.Write($"{review,-28}");

                        // Right border of row
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(" │");
                        Console.ResetColor();

                    }

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("└──────────────────────────────┘");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }     
        }

        // Save and Load Reviews in Files
        public static void SaveReviews()
        {
            try
            {
                // Write the file line by line
                 using (StreamWriter writer = new StreamWriter(ReviewsFilePath)) // Open the file for writing                
                 {
                        foreach (string review in reviewsStack) // loop through the reviews
                        {
                            writer.WriteLine(review); // Write the review to the file
                        }
                 }
      
                Console.WriteLine("Reviews saved successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        public static void LoadReviews()
        {
            try
            {
                // Read the file line by line
                using (StreamReader reader = new StreamReader(ReviewsFilePath)) // Open the file for reading
                {
                   string line; // Declare a variable to hold each line
                   while ((line = reader.ReadLine()) != null) // Read each line until the end of the file
                   {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            reviewsStack.Push(line);
                        }
                    }
                }
                    Console.WriteLine("Reviews loaded successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
    
        }

        // Utility Functions

        // Generate a unique account number
        public static int GenerateAccountNumber()
        {
            try
            {
                // Check if the account number already exists
                if (accountNumbers.Count == 0) // Check if there are any accounts
                {
                    return 100000; // Return the first account number
                }

                // Get the last account number
                if (accountNumbers.Count > 0) // Check if there are any accounts
                {
                    lastAccountNumber = accountNumbers[accountNumbers.Count - 1]; // Update the last account number
                }
                else
                {
                    lastAccountNumber = 0; // Set to 0 if no accounts exist
                }
                // Generate a unique account number
                int newAccountNumber;
                do
                {
                    newAccountNumber = new Random().Next(100000, 999999); // Generate a random number between 100000 and 999999
                } while (accountNumbers.Contains(newAccountNumber));

                // Check if the generated account number already exists
                if (accountNumbers.Contains(newAccountNumber))
                {
                    Console.WriteLine("Account number already exists. Generating a new one...");
                }
                return newAccountNumber; // Return the generated account number
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
                return 000000;
            }
    
        }

        // Find an account by account number or by Name
        public static string FindAccount(int accountNumber)
        {
            try
            {
                // Check if the account number is valid
                CheckAccountExists(accountNumber);
                // Find the account by account number
                int index = accountNumbers.IndexOf(accountNumber);
                if (index != -1)
                {
                    return $"Account Number: {accountNumbers[index]}, Name: {accountNames[index]}, Balance: {balances[index]}";
                }
                else
                {
                    return $"Account {accountNumber} not found.";
                }
            }
            catch (Exception ex)
            {
                return $"Error Due to: {ex.Message}" ;
               
            }
        }

        public static void SearchAccountByNameOrID()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Search Account ===");
                Console.Write("Enter Name or National ID to search: ");
                string searchInput = Console.ReadLine();

                bool found = false;
                for (int i = 0; i < accountNames.Count; i++)
                {
                    if (accountNames[i] == searchInput || NationalId[i] == searchInput)
                    {
                        Console.WriteLine("===================================");
                        Console.WriteLine($"Account Number: {accountNumbers[i]}");
                        Console.WriteLine($"Account Name: {accountNames[i]}");
                        Console.WriteLine($"National ID: {NationalId[i]}");
                        Console.WriteLine($"Balance: {balances[i]}");
                        Console.WriteLine($"Loan Amount: {loans[i]}");
                        Console.WriteLine($"Transaction History: {transactions[i]}");
                        Console.WriteLine($"Loan Transaction History: {transactionsLoan[i]}");
                        Console.WriteLine("===================================");
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No account found with the provided name or national ID.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          

        }

        // Change Password and Pin
        public static void ChangePinAdmin()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Change Admin Pin ===");
                Console.WriteLine("Please enter your Admin Name and old Pin to change your New Pin.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Admin Name: ");
                string name = Console.ReadLine();
                if (name == null) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid name format.");
                    return;
                }
                Console.Write("Enter Old Pin: ");
                string oldPin = Console.ReadLine();
                if (PinCodeAdmins[Admins.IndexOf(name)] != oldPin) // Check if the old password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }

                Console.Write("Enter New Password: ");
                string newPin = Console.ReadLine(); // Get the new password
                int index = Admins.IndexOf(name); // Get the index of the account number
                if (index != -1)
                {
                    PinCodeAdmins[index] = newPin; // Update the password
                    Console.WriteLine($"Pin changed successfully for Admin: {name}");
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
      
        }

        public static void ChangePinCustomers()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Change Customers Pin ===");
                Console.WriteLine("Please enter your User Name and old Pin to change your New Pin.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter User Name: ");
                string name = Console.ReadLine();
                if (name == null) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid name format.");
                    return;
                }
                Console.Write("Enter Old Pin: ");
                string oldPin = Console.ReadLine();
                if (PinCodeCustomers[Customers.IndexOf(name)] != oldPin) // Check if the old password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }

                Console.Write("Enter New Password: ");
                string newPin = Console.ReadLine(); // Get the new password
                int index = Customers.IndexOf(name); // Get the index of the account number
                if (index != -1)
                {
                    PinCodeCustomers[index] = newPin; // Update the password
                    Console.WriteLine($"Pin changed successfully for name: {name}");
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
       
        }

        public static void ChangePassowrd()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Change Password ===");
                Console.WriteLine("Please enter your account number and old password to change your password.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }
                Console.Write("Enter Old Password: ");
                string oldPassword = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != oldPassword) // Check if the old password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }

                // Check if the account number exists
                CheckAccountExists(accountNumber);

                Console.Write("Enter New Password: ");
                string newPassword = Console.ReadLine(); // Get the new password
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                if (index != -1)
                {
                    Passwords[index] = newPassword; // Update the password
                    Console.WriteLine($"Password changed successfully for Account Number: {accountNumber}");
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        // Generate Account Statement
        public static void GenerateAccountStatement()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Account Statement ===");
                Console.WriteLine("Please enter your account number and password to generate your account statement.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber))
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }

                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Account Number: {accountNumber}");
                Console.WriteLine($"Account Name: {accountNames[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine($"Date: {DateTime.Now}");
                Console.WriteLine($"Current Balance: {balances[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine("----------------------------------------");

                Console.WriteLine("Transaction History:");
                // Display the transaction history
                if (transactions.Count == 0) // Check if there are any transactions
                {
                    Console.WriteLine("No transactions available.");
                }
                else if (transactions[accountNumbers.IndexOf(accountNumber)] == "") // Check if the transaction history is empty
                {
                    Console.WriteLine("No transactions available.");
                }
                else
                {
                    foreach (string t in transactions) // loop through the transactions
                    {
                        Console.WriteLine(t);
                    }
                }

                Console.WriteLine("========================================\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        
        }

        // Setup Recurring Deposit
        public static void SetupRecurringDepositOnMonth()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Setup Recurring Deposit ===");
                Console.WriteLine("Please enter your account number and password to set up a recurring deposit.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }

                // Check if the account number exists
                CheckAccountExists(accountNumber);

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != password) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }
                // Display account details
                Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");

                // Enter the recurring deposit amount
                Console.Write("Enter Recurring Deposit Amount: ");
                double amount = double.Parse(Console.ReadLine());
                // choose When to start the recurring deposit now or after some time
                Console.WriteLine("Choose when the first deposit (Now/ Another time): ");
                string date = Console.ReadLine();
                if (date.ToLower() == "now")
                {
                    // Set the date to now
                    Console.WriteLine("First deposit date is set to now.");
                    date = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    // Ask for the date
                    Console.WriteLine("Enter the date for the first deposit (dd/MM/yyyy): ");
                    date = Console.ReadLine();
                }

                DateTime firstDepositDate; // Parse the date
                                           // Check if the date is valid
                if (!DateTime.TryParse(date, out firstDepositDate))
                {
                    Console.WriteLine("Invalid date format.");
                    return;
                }
                Console.WriteLine("Please enter the interval for the recurring deposit (in months): ");
                int interval = int.Parse(Console.ReadLine());
                DateTime nextDepositDate = firstDepositDate.AddMonths(interval); // Calculate the next deposit date
                Console.WriteLine($"Next deposit date is: {nextDepositDate.ToString("dd/MM/yyyy")}");
                Console.WriteLine($"Recurring deposit of {amount} set up successfully. Next deposit on {nextDepositDate.ToString("dd/MM/yyyy")}");
                int index = accountNumbers.IndexOf(accountNumber);
                if (index != -1)
                {
                    // Add the recurring deposit to the balance
                    balances[index] += amount;
                    transactions[index] += $"Recurring Deposit: {amount} | {DateTime.Now} ";
                    Console.WriteLine($"Recurring deposit successful! New balance: {balances[index]}");
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        
        }

        // Currency Converter
        public static void CurrencyConverter()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Currency Converter ===");
                Console.WriteLine("Please enter your account number to convert currency.");
                Console.WriteLine("===================================");
                // Get user details
                Console.Write("Enter Account Number: ");
                int accountNumber = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }

                Console.WriteLine("Convert Balance To:");
                Console.WriteLine("=================");
                Console.WriteLine("||   1. USD      ||");
                Console.WriteLine("||   2. EUR      ||");
                Console.WriteLine("=================");

                Console.Write("Choose option: ");
                string option = Console.ReadLine();

                double converted = 0; // Declare a variable to hold the converted amount 
                switch (option)
                {
                    case "1":
                        converted = balances[accountNumbers.IndexOf(accountNumber)] * 2.6; Console.WriteLine($"Balance in USD: {converted}"); // Convert to USD
                        break;
                    case "2":
                        converted = balances[accountNumbers.IndexOf(accountNumber)] * 2.4; Console.WriteLine($"Balance in EUR: {converted}"); // Convert to EUR
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        
        }

        // Check if account exists
        public static void CheckAccountExists(int accountNumber)
        {
            try
            {
                if (accountNumbers.Contains(accountNumber))
                {
                    Console.WriteLine("Account Found.");
                }
                else
                {
                    Console.WriteLine("Account Not Found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        
        }

        // Shot Top 3 Richest
        public static void ShowTopRichestCustomers()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Top 3 Richest Customers ===");

                // Prepare list of indices
                List<int> BalanceTemp = new List<int>();
                for (int i = 0; i < balances.Count; i++)
                {
                    BalanceTemp.Add(i);
                }

                // sort by balance descending
                for (int i = 0; i < BalanceTemp.Count - 1; i++)
                {
                    for (int j = i + 1; j < BalanceTemp.Count; j++)
                    {
                        if (balances[BalanceTemp[j]] > balances[BalanceTemp[i]])
                        {
                            // Swap Balance
                            int temp = BalanceTemp[i];
                            BalanceTemp[i] = BalanceTemp[j];
                            BalanceTemp[j] = temp;
                        }
                    }
                }

                // Display top 3 customers
                int topCount = Math.Min(3, BalanceTemp.Count);
                for (int i = 0; i < topCount; i++)
                {
                    int index = BalanceTemp[i];
                    Console.WriteLine("===================================");
                    Console.WriteLine($"Rank #{i + 1}");
                    Console.WriteLine($"Account Number: {accountNumbers[index]}");
                    Console.WriteLine($"Name: {accountNames[index]}");
                    Console.WriteLine($"Balance: {balances[index]}");
                    Console.WriteLine($"Loan: {loans[index]}");
                    Console.WriteLine("===================================");
                }

                if (topCount == 0)
                {
                    Console.WriteLine("No accounts available.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
       
        }

        // Save and Load All Data
        static void LoadAllData()
        {
            try
            {
                Console.WriteLine("Loading data...");
                LoadAccountsFromFile();
                LoadCustomerAccountsFromFile();
                LoadAdminAccountsFromFile();
                LoadRequestsFromFile();
                LoadReviews();
                LoadReportsFromFile();
                Console.WriteLine("All data loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data: " + ex.Message);
            }
        }

        static void SaveAllData()
        {
            try
            {
                SaveAccountsToFile();
                SaveCustomerAccountsToFile();
                SaveAdminAccountsToFile();
                SaveRequeststoFile();
                SaveReviews();
                SaveReportsToFile();
                Console.WriteLine("All data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving data: " + ex.Message);
            }
        }

    }
}

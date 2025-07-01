using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


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
        static List<string> phoneNumbers = new List<string>();   // Store phone number
        static List<string> addresses = new List<string>();      // Store address
        static List<int> ratings = new List<int>(); // Store ratings feedback
        static List<bool> isAccountLocked = new List<bool>();     // Track locked status
        static List<int> failedLoginAttempts = new List<int>();   // Track failed login counts
        // Queue and Stack for account requests and reviews
        static Queue<string> createAccountRequests = new Queue<string>(); // Queue to store account requests
        static Stack<string> reviewsStack = new Stack<string>(); // Stack to store reviews
        static Queue<string> appointments = new Queue<string>(); // Queue to store appointments



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
                            Environment.Exit(0); // Exit the application
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
                reportLines.Clear();
                reportMonth.Clear();
                phoneNumbers.Clear();
                addresses.Clear();
                ratings.Clear();
                isAccountLocked.Clear();
                failedLoginAttempts.Clear();
                appointments.Clear();

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
                // Ask user if they want to create a backup
                Console.Write("Would you like to save a backup of all data? (y/n): ");
                string input = Console.ReadLine().ToLower();

                // If user confirms, create a backup file
                if (input == "y")
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm"); // Timestamp for backup file

                    string dir = "Backups"; // Directory to store backups
                    if (!Directory.Exists(dir)) // Check if the directory exists
                        Directory.CreateDirectory(dir); // Create the directory if it doesn't exist

                    string backupFileName = Path.Combine(dir, $"Backup_{timestamp}.txt"); // Create backup file name with timestamp

                    // Write backup data to the file
                    using (StreamWriter writer = new StreamWriter(backupFileName))
                    {
                        writer.WriteLine("Backup File Created: " + DateTime.Now);
                        writer.WriteLine("======================================");
                        for (int i = 0; i < accountNumbers.Count; i++) // Loop through all accounts
                        {
                            // Write account details to the backup file
                            writer.WriteLine($"Account Number: {accountNumbers[i]}");
                            writer.WriteLine($"Name: {accountNames[i]}");
                            writer.WriteLine($"National ID: {NationalId[i]}");
                            writer.WriteLine($"Balance: {balances[i]}");
                            writer.WriteLine($"Loan: {loans[i]}");
                            writer.WriteLine($"Phone: {phoneNumbers[i]}");
                            writer.WriteLine($"Address: {addresses[i]}");
                            writer.WriteLine($"Transactions: {transactions[i]}");
                            writer.WriteLine($"Loan Transactions: {transactionsLoan[i]}");
                            writer.WriteLine("--------------------------------------");
                        }
                    }

                    Console.WriteLine($"Backup saved to file: {backupFileName}");
                }

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
                            Console.WriteLine("Choose you are:\n 1. Admin || 2. Customer ");
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
                            ShutdownBankSystem(); // Runs backup prompt and exits the app
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
                string pin = ReadPasswordMasked(); // read password without showing input
                string hashed = HashPassword(pin); // hash the password
                PinCodeCustomers.Add(hashed); // Add the hashed password to the list
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
                string pin = ReadPasswordMasked(); // Read password without showing input
                string hashed = HashPassword(pin); // Hash the password
                PinCodeAdmins.Add(hashed);
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
                Console.Write("Enter National ID: ");
                string nationalId = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = ReadPasswordMasked(); // read password without showing input
                string hashed = HashPassword(password); // Hash the password

                int index = NationalId.IndexOf(nationalId); // Find the index of the national ID in the list

                if (index != -1) // Check if the national ID exists
                {
                    if (isAccountLocked[index]) // Check if the account is locked
                    {
                        Console.WriteLine(" Your account is locked due to 3 failed login attempts. Contact admin to unlock.");
                        return;
                    }

                    if (Passwords[index] == hashed) // Check if the password matches
                    {
                        failedLoginAttempts[index] = 0;  // reset on success
                        DisplayCustomerMenu();           // login success
                    }
                    else
                    {
                        failedLoginAttempts[index]++; // Increment failed login attempts
                        Console.WriteLine("Incorrect password.");

                        if (failedLoginAttempts[index] >= 3) // Check if failed attempts reached 3
                        {
                            isAccountLocked[index] = true; // Lock the account
                            Console.WriteLine(" Your account has been locked after 3 failed attempts.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("National ID not found.");
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
                    Console.WriteLine("    ║  [11]   Monthly Statement            ║");
                    Console.WriteLine("    ║  [12]   Currency Converter           ║");
                    Console.WriteLine("    ║  [13]   Update Phone / Address       ║");
                    Console.WriteLine("    ║  [14]   Book Appointment             ║");
                    Console.WriteLine("    ║  [15]   View My Appointments         ║");
                    Console.WriteLine("    ║  [16]   Currency Deposit             ║");
                    Console.WriteLine("    ║  [17]   Change Pin                   ║");
                    Console.WriteLine("    ║  [18]   Logout                       ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.Write("\n    Select an option (0-18): ");
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
                            GenerateMonthlyStatementByUser(); // New method
                            break;
                        case "12":
                            CurrencyConverter(); // Currency converter 
                            break;
                        case "13":
                            UpdateAccountInfo(); // Update phone number or address
                            break;
                        case "14":
                            BookAppointment(); // Book an appointment
                            break;
                        case "15":
                            ViewMyAppointment(); // View my appointments
                            break;
                        case "16":
                            DepositWithCurrencyConversion(); // Deposit with currency conversion
                            break;
                        case "17":
                            ChangePinCustomers(); // Change Pin
                            break;
                        case "18":
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
                if (Admin == "admin")
                {
                    Console.WriteLine("The ID 'admin' is reserved. Please choose another.");
                    return;
                }

                Console.Write("Enter Admin Pin: ");
                string Adminpin = ReadPasswordMasked(); // read password without showing input
                string AdminhashedPin = HashPassword(Adminpin); // Hash the password

                // Check if the Addmin Name and Pin are valid
                if (Admins.Count == 0 || PinCodeAdmins.Count == 0)
                {
                    Console.WriteLine("No Admins found. Please create an admin account first.");
                    return;
                }
                int index = Admins.IndexOf(Admin); // Find the index of the admin in the list
                if (index != -1 && PinCodeAdmins[index] == AdminhashedPin) // Check if the admin exists and the pin matches
                {
                    Console.WriteLine("Admin login successful.");
                    Console.WriteLine("Welcome, " + Admin + "!");
                    DisplayAdminMenu(); // Access granted
                }
                else if (Admin == "admin" && Adminpin == "Admin@123") // Check for super admin
                {
                    Console.WriteLine("Super Admin Access Granted.");
                    DisplayAdminMenu();
                }
                else
                {
                    Console.WriteLine("Invalid Admin credentials.");
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
                    Console.WriteLine("    ║  [12]   View Average Rating          ║");
                    Console.WriteLine("    ║  [13]   View All Appointments        ║");
                    Console.WriteLine("    ║  [14]   View Appointment by Account  ║");
                    Console.WriteLine("    ║  [15]   Currency Converter           ║");
                    Console.WriteLine("    ║  [16]   Unlock Locked Account        ║");
                    Console.WriteLine("    ║  [17]   View Locked Accounts         ║");
                    Console.WriteLine("    ║  [18]   View Accounts with Loans     ║");
                    Console.WriteLine("    ║  [19]   View Low Balance Accounts    ║");
                    Console.WriteLine("    ║  [20]   Logout                       ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.Write("\n    Select an option (1-20): ");
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
                            ShowTopRichestCustomers(); // Show top three richest customers
                            break;
                        case "12":
                            ViewAverageRating(); // View average rating
                            break;
                        case "13":
                            ViewAppointments(); // Show all formatted appointments
                            break;
                        case "14":
                            ViewAppointmentByAccount(); // View appointment by account
                            break;
                        case "15":
                            CurrencyConverter(); // Currency converter
                            break;
                        case "16":
                            UnlockAccountByAdmin(); // Unlock locked account
                            break;
                        case "17":
                            ShowLockedAccounts(); // Show locked accounts
                            break;
                        case "18":
                            ShowAccountsWithActiveLoans(); // Show accounts with active loans
                            break;
                        case "19":
                            ShowLowBalanceAccounts(); // Show accounts with low balance
                            break;
                        case "20":
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

        // Unlock Account by Admin
        public static void UnlockAccountByAdmin()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Unlock Locked Account ===");

                Console.Write("Enter Account Number to unlock: ");
                if (!int.TryParse(Console.ReadLine(), out int accountNumber)) // Check if input is a valid integer
                {
                    Console.WriteLine("Invalid input.");
                    return;
                }

                int index = accountNumbers.IndexOf(accountNumber); // Find the index of the account number in the list
                if (index == -1) // Check if the account number exists
                {
                    Console.WriteLine("Account not found.");
                    return;
                }

                if (!isAccountLocked[index]) // Check if the account is already unlocked
                {
                    Console.WriteLine("This account is not currently locked.");
                    return;
                }

                isAccountLocked[index] = false; // Unlock the account
                failedLoginAttempts[index] = 0; // Reset failed login attempts

                Console.WriteLine(" Account unlocked successfully.");
                SaveAllData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
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
                    Console.WriteLine("    ║  [3]   Generate Account Statement    ║");
                    Console.WriteLine("    ║  [4]   Back to Admin Menu            ║");
                    Console.WriteLine("    ║                                      ║");
                    Console.WriteLine("    ╚══════════════════════════════════════╝");

                    Console.ResetColor();
                    Console.Write("\n    Select report type (1-4): ");
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
                            GenerateAccountStatement(); // Generate account statement
                            break;
                        case "4":
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
                        // Login protection
                        isAccountLocked.Add(false); // account is not locked
                        failedLoginAttempts.Add(0); // reset failed login attempts

                        Console.Write("Enter a password: ");
                        string password = ReadPasswordMasked();
                        string hashedPassword = HashPassword(password);
                        Passwords.Add(hashedPassword); // Set password
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
                    SaveAllData();
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
                // Add login protection 
                isAccountLocked.Add(false); // Account is not locked
                failedLoginAttempts.Add(0); // Reset failed login attempts

                Console.Write("Enter Phone Number: ");
                string phone = Console.ReadLine();
                phoneNumbers.Add(phone);

                Console.Write("Enter Address: ");
                string address = Console.ReadLine();
                addresses.Add(address);


                // Choose password
                Console.Write("Enter a password: ");
                string password = ReadPasswordMasked();
                string hashedPassword = HashPassword(password);
                Passwords.Add(hashedPassword);
                transactions.Add(""); // Empty transaction history
                transactionsLoan.Add("");

                Console.WriteLine($"Account created successfully! Your Account Number is: {newAccountNumber}");
                SaveAllData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
           
        }

        // Update Account Info
        public static void UpdateAccountInfo()
        {
            Console.Clear();
            Console.WriteLine("=== Update Account Information ===");
            Console.Write("Enter Account Number: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int accountNumber)) // Check if input is a valid integer
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            Console.Write("Enter Password: ");
            string password = ReadPasswordMasked(); 
            string hashed = HashPassword(password);

            int index = accountNumbers.IndexOf(accountNumber); // Find the index of the account number in the list
            if (index == -1 || Passwords[index] != hashed) // Check if the account number exists and the password matches
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }

            Console.WriteLine($"Current Phone: {phoneNumbers[index]}"); // Display current phone number
            Console.WriteLine($"Current Address: {addresses[index]}"); // Display current address

            Console.Write("Enter New Phone Number (or press Enter to keep current): ");
            string newPhone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newPhone)) // Check if input is not empty
                phoneNumbers[index] = newPhone; // Update phone number

            Console.Write("Enter New Address (or press Enter to keep current): ");
            string newAddress = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAddress)) // Check if input is not empty
                addresses[index] = newAddress; // Update address

            SaveAllData();
            Console.WriteLine("Account information updated successfully.");
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
                SaveAllData();
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
                Console.WriteLine($"Phone Number: {phoneNumbers[accountNumbers.IndexOf(accountNumber)]}");
                Console.WriteLine($"Address: {addresses[accountNumbers.IndexOf(accountNumber)]}");
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
                    Console.WriteLine($"Phone Number: {phoneNumbers[i]}");
                    Console.WriteLine($"Address: {addresses[i]}");
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
                if (!int.TryParse(input, out int accountNumber)) // Check if input is a valid integer
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                Console.Write("Enter Password: ");
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != hashed) // Check if the password is valid
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
                SaveAllData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
          
        }

        // Deposit with Currency Conversion
        public static void DepositWithCurrencyConversion()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Deposit with Currency Conversion ===");
                Console.Write("Enter Account Number: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber)) // Check if input is a valid integer
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                Console.Write("Enter Password: ");
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);
                int index = accountNumbers.IndexOf(accountNumber);

                if (index == -1 || Passwords[index] != hashed) // Check if the account number exists and the password matches
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }

                Console.WriteLine($"Welcome, {accountNames[index]}");
                Console.WriteLine($"Your current balance is: {balances[index]}");

                // Choose currency
                Console.WriteLine("\nChoose Currency to Deposit:");
                Console.WriteLine("1. OMR (Local)");
                Console.WriteLine("2. USD (1 USD = 3.8 OMR)");
                Console.WriteLine("3. EUR (1 EUR = 4.1 OMR)");
                Console.Write("Choice: ");
                string currencyOption = Console.ReadLine(); 

                Console.Write("Enter Deposit Amount: ");
                if (!double.TryParse(Console.ReadLine(), out double originalAmount) || originalAmount <= 0) // Check if Amount is a valid double and greater than 0
                {
                    Console.WriteLine("Invalid deposit amount.");
                    return;
                }

                double convertedAmount = originalAmount; // Initialize converted amount
                string currency = "OMR"; // Default currency

                switch (currencyOption)
                {
                    case "2":
                        convertedAmount = originalAmount * 3.8;
                        currency = "USD";
                        break;
                    case "3":
                        convertedAmount = originalAmount * 4.1;
                        currency = "EUR";
                        break;
                    case "1":
                    default:
                        currency = "OMR";
                        break;
                }

                balances[index] += convertedAmount; // Update the balance with the converted amount
                transactions[index] += $"Currency Deposit: {originalAmount} {currency} => {convertedAmount} OMR | {DateTime.Now} "; // Add the deposit transaction to the history

                Console.WriteLine($"Deposit successful! New balance: {balances[index]}");
                SaveAllData();
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
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != hashed)
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
                SaveAllData();
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
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);
                if (Passwords[accountNumbers.IndexOf(fromAccountNumber)] != hashed)
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
                SaveAllData();
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
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }
                // Check if the account number exists
                CheckAccountExists(accountNumber);

                Console.Write("Enter Password: ");
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != hashed)
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

                Console.Write("Enter Account Number: ");
                if (!int.TryParse(Console.ReadLine(), out int accountNumber))
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                CheckAccountExists(accountNumber);

                Console.Write("Enter Password: ");
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);
                int index = accountNumbers.IndexOf(accountNumber);

                if (index == -1 || Passwords[index] != hashed)
                {
                    Console.WriteLine("Incorrect credentials.");
                    return;
                }

                Console.WriteLine("\nChoose filter option:");
                Console.WriteLine("1. View All Transactions");
                Console.WriteLine("2. View Last N Transactions");
                Console.WriteLine("3. View Transactions After Specific Date");

                string option = Console.ReadLine();
                string[] entries = transactions[index].Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries); // Split transactions by " | " delimiter
                List<string> filtered = new List<string>(); // List to hold filtered transactions

                switch (option)
                {
                    case "1":
                        filtered = entries.ToList(); // View all transactions
                        break;

                    case "2":
                        Console.Write("Enter N: ");
                        if (int.TryParse(Console.ReadLine(), out int n))
                        {
                            filtered = entries.Reverse().Take(n).Reverse().ToList(); // Get last N transactions
                        }
                        else
                        {
                            Console.WriteLine("Invalid number.");
                            return;
                        }
                        break;

                    case "3":
                        Console.Write("Enter date (yyyy-MM-dd): ");
                        string inputDate = Console.ReadLine();
                        if (DateTime.TryParse(inputDate, out DateTime filterDate)) // Parse the date
                        {
                            // Filter transactions after the specified date
                            foreach (var entry in entries)
                            {
                                int i = entry.LastIndexOf(':'); // Find the last colon to get the date part
                                // Ensure the entry has a date part after the last colon
                                if (i != -1 && DateTime.TryParse(entry.Substring(i + 1).Trim(), out DateTime entryDate)) // Parse the date from the entry
                                {
                                    if (entryDate >= filterDate) // Check if the entry date is on or after the filter date
                                        filtered.Add(entry); // Add to filtered list
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid date.");
                            return;
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        return;
                }

                // Print formatted table
                Console.WriteLine("\nFormatted Transaction History:");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("| Date                | Type       | Amount     | Balance   |");
                Console.WriteLine("------------------------------------------------------------");

                double runningBalance = 0;
                foreach (var entry in filtered)
                {
                    string[] parts = entry.Split(':');
                    if (parts.Length < 2) continue;

                    string type = parts[0].Trim();
                    string rest = parts[1].Trim();
                    string[] amountAndDate = rest.Split(' ');

                    if (amountAndDate.Length < 2) continue;

                    if (double.TryParse(amountAndDate[0], out double amount))
                    {
                        string dateStr = string.Join(" ", amountAndDate.Skip(1));
                        if (!DateTime.TryParse(dateStr, out DateTime date)) continue;

                        if (type.ToLower().Contains("deposit") || type.ToLower().Contains("transfer from"))
                            runningBalance += amount;
                        else if (type.ToLower().Contains("withdrawal") || type.ToLower().Contains("transfer to"))
                            runningBalance -= amount;

                        Console.WriteLine($"| {date,-19} | {type,-10} | {amount,-10:F2} | {runningBalance,-9:F2} |");
                    }
                }

                Console.WriteLine("------------------------------------------------------------");

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
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);
                if (Passwords[accountNumbers.IndexOf(accountNumber)] != hashed)
                {
                    Console.WriteLine("Incorrect password.");
                    return;
                }
                int index = accountNumbers.IndexOf(accountNumber);

                if (index != -1)
                {
                    // Minimum balance 5000
                    if (balances[index] < 5000)
                    {
                        Console.WriteLine("You must have at least 5000 OMR to apply for a loan.");
                        return;
                    }

                    // No active loan
                    if (loans[index] > 0)
                    {
                        Console.WriteLine("You already have an active loan. Repay before applying again.");
                        return;
                    }
                    Console.WriteLine($"Welcome, {accountNames[accountNumbers.IndexOf(accountNumber)]}.");
                    Console.Write("Enter Loan Amount: ");
                    double loanAmount = double.Parse(Console.ReadLine());
                    int index1 = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                    if (index1 != -1) // Check if the account number exists
                    {
                        // Check if the loan amount is valid
                        if (loanAmount <= 0)
                        {
                            Console.WriteLine("Invalid loan amount.");
                            return;
                        }
                        // Update the loan amount and transaction history
                        loans[index1] = loanAmount; // Add the loan amount to the account
                        transactionsLoan[index1] += $"Loan Applied: {loanAmount} | {DateTime.Now} "; // Add the loan application to the transaction history
                        Console.WriteLine($"Loan application successful! Loan Amount: {loanAmount}"); // Display the loan amount
                    }
                    else
                    {
                        Console.WriteLine("Account not found.");
                    }
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
                Console.Clear(); // Clear the console for a clean display
                Console.WriteLine("=== Pending Loan Applications ===");

                bool loansPending = false; // Flag to check if there are any loans to approve

                // Loop through all accounts and find pending loans
                for (int i = 0; i < loans.Count; i++)
                {
                    if (loans[i] > 0) // Loan amount > 0 means loan is pending
                    {
                        Console.WriteLine($"[{i}] Account Number: {accountNumbers[i]} | Name: {accountNames[i]} | Loan Requested: {loans[i]}");
                        loansPending = true; // At least one loan is pending
                    }
                }

                // If no loans are pending, exit the method
                if (!loansPending)
                {
                    Console.WriteLine("No pending loan applications.");
                    Console.ReadLine(); // Wait for user input
                    return;
                }

                // Ask admin to select which account to approve/reject
                Console.Write("\nEnter the number of the account to process (or -1 to cancel): ");
                if (!int.TryParse(Console.ReadLine(), out int selectedIndex) || selectedIndex < -1 || selectedIndex >= loans.Count)
                {
                    Console.WriteLine("Invalid selection."); // Invalid input handling
                    Console.ReadLine();
                    return;
                }

                // If admin decides to cancel
                if (selectedIndex == -1)
                {
                    Console.WriteLine("Operation cancelled.");
                    return;
                }

                // Ask admin to approve or reject the selected loan
                Console.Write($"Approve loan for {accountNames[selectedIndex]} (y/n)? ");
                string approve = Console.ReadLine().ToLower();

                if (approve == "y") // If admin approves the loan
                {
                    transactionsLoan[selectedIndex] += $"Loan Approved: {loans[selectedIndex]} | {DateTime.Now}\n"; // Add approval record
                    Console.WriteLine("Loan approved successfully!");
                }
                else // If admin rejects the loan
                {
                    transactionsLoan[selectedIndex] += "Loan Rejected | " + DateTime.Now + "\n"; // Add rejection record
                    loans[selectedIndex] = 0; // Reset loan amount to 0
                    Console.WriteLine("Loan rejected.");
                }

                SaveAllData(); // Save updated data to file
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message); // Catch and display any unexpected errors
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
                SaveAllData();
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
                using (StreamWriter writer = new StreamWriter(ReportFilePath))
                {
                   foreach (var line in reportLines)
                   {
                      writer.WriteLine(line);
                   }
                }
                    Console.WriteLine("Report saved successfully.");
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

        public static void GenerateMonthlyStatementByUser()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Monthly Statement Generator ===");

                // Get account number
                Console.Write("Enter Account Number: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber))
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }

                // Check account exists and verify password
                CheckAccountExists(accountNumber);

                Console.Write("Enter Password: ");
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);

                int index = accountNumbers.IndexOf(accountNumber);
                if (index == -1 || Passwords[index] != hashed)
                {
                    Console.WriteLine("Invalid credentials.");
                    return;
                }

                // Ask user to enter desired month and year
                Console.Write("Enter month (MM): ");
                string monthInput = Console.ReadLine();
                Console.Write("Enter year (YYYY): ");
                string yearInput = Console.ReadLine();

                if (!int.TryParse(monthInput, out int month) || !int.TryParse(yearInput, out int year)) // Check if month and year are valid integers
                {
                    Console.WriteLine("Invalid month or year input.");
                    return;
                }

                if (month < 1 || month > 12 || year < 1) // Validate month and year range
                {
                    Console.WriteLine("Month must be between 1 and 12, and year must be positive.");
                    return;
                }

                Console.WriteLine($"Generating statement for {accountNames[index]} for {month:D2}/{year}...");

                // Filter transactions by the selected month/year
                string[] allTransactions = transactions[index].Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries); // Split transactions by " | " delimiter
                List<string> filtered = new List<string>();

                // Loop through all transactions and filter by month and year
                foreach (string entry in allTransactions)
                {
                    int colonIndex = entry.IndexOf(':'); // Find the first colon to separate type and date
                    if (colonIndex == -1) continue; // Skip if no colon found

                    string datePart = entry.Substring(colonIndex + 1).Trim(); // Get the date part after the colon
                    if (DateTime.TryParse(datePart, out DateTime parsedDate)) // Try to parse the date
                    {
                        if (parsedDate.Month == month && parsedDate.Year == year) // Check if the transaction date matches the selected month and year
                        {
                            // Add the entry to the filtered list
                            filtered.Add(entry);
                        }
                    }
                }

                // Save to file in text format 
                string dir = "Statements";
                
                // Create directory if it doesn't exist
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir); // Create the directory

                string fileName = Path.Combine(dir, $"Statement_Acc{accountNumber}_{year}-{month:D2}.txt"); // Create a file name with account number and date
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine($"Monthly Statement for Account #{accountNumber}");
                    writer.WriteLine($"Account Name: {accountNames[index]}");
                    writer.WriteLine($"Month: {month:D2}/{year}");
                    writer.WriteLine("----------------------------------------");

                    if (filtered.Count == 0)
                    {
                        writer.WriteLine("No transactions found for this month.");
                    }
                    else
                    {
                        foreach (string line in filtered)
                        {
                            writer.WriteLine(line);
                        }
                    }

                    writer.WriteLine("----------------------------------------");
                    writer.WriteLine($"Final Balance: {balances[index]}");
                }

                Console.WriteLine($"\nStatement saved to file: {fileName}");
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
                        int accountNumber = (i < accountNumbers.Count) ? accountNumbers[i] : 0;
                        string accountName = (i < accountNames.Count) ? accountNames[i] : "Unknown";
                        string nationalId = (i < NationalId.Count) ? NationalId[i] : "Unknown";
                        double balance = (i < balances.Count) ? balances[i] : 0.0;
                        double loan = (i < loans.Count) ? loans[i] : 0.0;
                        string password = (i < Passwords.Count) ? Passwords[i] : "password123";
                        string transaction = (i < transactions.Count) ? transactions[i] : "";
                        string transactionLoan = (i < transactionsLoan.Count) ? transactionsLoan[i] : "";
                        string phoneNumber = (i < phoneNumbers.Count) ? phoneNumbers[i] : "Unknown";
                        string address = (i < addresses.Count) ? addresses[i] : "Unknown";

                        // Write account details to the file
                        writer.WriteLine($"{accountNumbers[i]}|{accountName}|{nationalId}|{balance}|{loan}|{password}|{transaction}|{transactionLoan}|{phoneNumbers[i]}|{addresses[i]}");

                    }
                }
               
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
                        if (parts.Length >= 9)
                        {
                            accountNumbers.Add(int.Parse(parts[0]));
                            accountNames.Add(parts[1]);
                            NationalId.Add(parts[2]);
                            balances.Add(double.Parse(parts[3]));
                            loans.Add(double.Parse(parts[4]));
                            Passwords.Add(parts[5]);
                            transactions.Add(parts[6]);
                            transactionsLoan.Add(parts.Length > 7 ? parts[7] : "");
                            phoneNumbers.Add(parts[8]);
                            addresses.Add(parts[9]);
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
                    }
                }
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

                        string line = $"{accountNumbers[i]}," + "" +
                                     $"\"{accountNames[i]}\"," + "" +
                                     $"\"{NationalId[i]}\"," + "" +
                                     $"{balances[i]}," + "" +
                                     $"{loans[i]}," + "" +
                                     $"\"{Passwords[i]}\"," + "" +
                                     $"\"{transactions[i]}\"," + "" +
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
                Console.WriteLine("Please enter your review and rate the service.");
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

                Console.Write("Rate the service (1 to 5): ");
                if (int.TryParse(Console.ReadLine(), out int rating) && rating >= 1 && rating <= 5) // Check if parse to integer
                {
                    ratings.Add(rating);
                    Console.WriteLine("Rating submitted.");
                }
                else
                {
                    Console.WriteLine("Invalid rating. Skipping rating submission.");
                }
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

        // View Average Rate
        public static void ViewAverageRating()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Average Rating ===");

                if (ratings.Count == 0) // If rating count is 0 then no rate applied
                {
                    Console.WriteLine("No ratings submitted yet.");
                    return;
                }

                double average = ratings.Average(); 
                Console.WriteLine($"Average Rating: {average:F2} / 5\n");

                Console.WriteLine("Individual Ratings:");
                foreach (int r in ratings)
                {
                    Console.WriteLine($"- {r}/5");
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

                var matched = accountNumbers
                    .Select((acc, i) => new { Index = i, Name = accountNames[i], ID = NationalId[i] })
                    .Where(x => x.Name == searchInput || x.ID == searchInput)
                    .ToList();

                if (!matched.Any())
                {
                    Console.WriteLine("No account found with the provided name or national ID.");
                }
                else
                {
                    foreach (var m in matched)
                    {
                        Console.WriteLine("===================================");
                        Console.WriteLine($"Account Number: {accountNumbers[m.Index]}");
                        Console.WriteLine($"Account Name: {accountNames[m.Index]}");
                        Console.WriteLine($"National ID: {NationalId[m.Index]}");
                        Console.WriteLine($"Balance: {balances[m.Index]}");
                        Console.WriteLine($"Loan: {loans[m.Index]}");
                        Console.WriteLine($"Transaction History: {transactions[m.Index]}");
                        Console.WriteLine($"Loan Transaction History: {transactionsLoan[m.Index]}");
                        Console.WriteLine($"Phone Number: {phoneNumbers[m.Index]}");
                        Console.WriteLine($"Address: {addresses[m.Index]}");
                        Console.WriteLine("===================================");
                    }
                }

                bool found = matched.Any(); // Check if any account was found
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
                string oldPin = ReadPasswordMasked();
                string hashedOld = HashPassword(oldPin);
                int index = Admins.IndexOf(name);
                if (index == -1 || PinCodeAdmins[index] != hashedOld)
                {
                    Console.WriteLine("Incorrect pin.");
                    return;
                }

                Console.Write("Enter New Pin: ");
                string newPin = ReadPasswordMasked();
                string hashedNew = HashPassword(newPin);
                int index1 = Admins.IndexOf(name); // Get the index of the account number
                if (index1 != -1)
                {
                    PinCodeAdmins[index1] = hashedNew; // Update the password
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
                string oldPin = ReadPasswordMasked();
                string hashedOld = HashPassword(oldPin);
                int index = Customers.IndexOf(name);
                if (index == -1 || PinCodeCustomers[index] != hashedOld)
                {
                    Console.WriteLine("Incorrect pin.");
                    return;
                }

                Console.Write("Enter New Pin: ");
                string newPin = ReadPasswordMasked();
                string hashedNew = HashPassword(newPin);
                int index1 = Customers.IndexOf(name); // Get the index of the account number
                if (index1 != -1)
                {
                    PinCodeCustomers[index1] = hashedNew; // Update the password
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
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number format.");
                    return;
                }

                // Check if the account number exists
                CheckAccountExists(accountNumber);

                Console.Write("Enter Old Password: ");
                string oldPassword = ReadPasswordMasked();
                string hashedOld = HashPassword(oldPassword);
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                if (Passwords[index] != hashedOld)
                {
                    Console.WriteLine("Incorrect old password.");
                    return;
                }

                Console.Write("Enter New Password: ");
                string newPassword = ReadPasswordMasked();
                string hashedNew = HashPassword(newPassword);
                int index1 = accountNumbers.IndexOf(accountNumber); // Get the index of the account number
                if (index1 != -1)
                {
                    Passwords[index1] = hashedNew; // Update Password
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
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber))
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
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber)) // Check if the account number is valid
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

        // Book Appointment
        public static void BookAppointment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Book an Appointment ===");

                Console.Write("Enter Account Number: ");
                if (!int.TryParse(Console.ReadLine(), out int accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                CheckAccountExists(accountNumber); // Check if the account number exists
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number

                if (index == -1) // If the account number does not exist
                {
                    Console.WriteLine("Account not found.");
                    return;
                }

                Console.Write("Enter Password: ");
                string password = ReadPasswordMasked(); // Read the password masked
                string hashed = HashPassword(password); // Hash the password

                if (index == -1 || Passwords[index] != hashed) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect credentials.");
                    return;
                }

                // Check if already booked
                foreach (string appointment in appointments) // Iterate through each appointment
                { 
                    if (appointment.Contains($"#{accountNumber}")) // Check if the appointment contains the account number
                    {
                        Console.WriteLine("You already have an appointment booked.");
                        return;
                    }
                }

                Console.Write("Choose Service [1] Loan Discussion  [2] Account Consultation: ");
                string service = Console.ReadLine() == "1" ? "Loan Discussion" : "Account Consultation"; // Choose service based on user input

                Console.Write("Enter preferred date and time (yyyy-MM-dd HH:mm): ");
                string input = Console.ReadLine();

                if (!DateTime.TryParse(input, out DateTime dateTime)) // Try to parse the date and time
                {
                    Console.WriteLine("Invalid date/time format.");
                    return;
                }

                string entry = $"#{accountNumber} - {accountNames[index]} - {service} - {dateTime}";
                appointments.Enqueue(entry); // Add the appointment to the queue

                Console.WriteLine("Appointment booked successfully!");
                SaveAllData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        }

        // View My Appointment
        public static void ViewMyAppointment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== View My Appointment ===");

                Console.Write("Enter Account Number: ");
                if (!int.TryParse(Console.ReadLine(), out int accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                CheckAccountExists(accountNumber); // Check if the account number exists
                int index = accountNumbers.IndexOf(accountNumber); // Get the index of the account number

                Console.Write("Enter Password: ");
                string password = ReadPasswordMasked();
                string hashed = HashPassword(password);

                if (index == -1 || Passwords[index] != hashed) // Check if the password is valid
                {
                    Console.WriteLine("Incorrect credentials.");
                    return;
                }

                bool found = false; // Flag to check if appointment is found
                foreach (string appointment in appointments) // Iterate through each appointment
                {
                    if (appointment.Contains($"#{accountNumber}")) // Check if the appointment contain account number
                    {
                        Console.WriteLine("Your Appointment Details:");
                        Console.WriteLine("- " + appointment); // Print the appointment details
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("You have no active appointments.");
                }

                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        }

        // View Appointment by Account Number
        public static void ViewAppointmentByAccount()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Search Appointment by Account Number ===");

                Console.Write("Enter Account Number: ");
                if (!int.TryParse(Console.ReadLine(), out int accountNumber)) // Check if the account number is valid
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                bool found = false; // Flag to check if appointment is found
                foreach (string appt in appointments) // Iterate through each appointment
                {
                    if (appt.StartsWith($"#{accountNumber}")) // Check if the appointment starts with the account number
                    {
                        Console.WriteLine("\nAppointment Found:");
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine(appt); // Print the appointment details
                        Console.WriteLine("------------------------------------------");
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No appointment found for this account.");
                }

                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Due to: " + ex.Message);
            }
        }

        // View All Appointments
        public static void ViewAppointments()
        {
            Console.Clear();
            Console.WriteLine("=== All Booked Appointments ===");

            if (appointments.Count == 0) // Check if there are any appointments booked
            {
                Console.WriteLine("No appointments booked.");
                return;
            }

            Console.WriteLine("\nFormatted Appointments:");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("| Account # | Name               | Service           | Date/Time           |");
            Console.WriteLine("--------------------------------------------------------------");

            foreach (var appt in appointments) // Iterate through each appointment
            {
                // Example format: "#123456 - John Doe - Loan Discussion - 2025-07-01 14:30:00"
                string[] parts = appt.Split(new[] { " - " }, StringSplitOptions.None); // Split the appointment string into parts
                if (parts.Length == 4) // Check if the appointment has the correct number of parts
                {
                    string acc = parts[0].Replace("#", "").Trim(); // Remove the '#' and trim whitespace
                    string name = parts[1].Trim(); // Get the name and trim whitespace
                    string service = parts[2].Trim(); // Get the service and trim whitespace
                    string date = parts[3].Trim(); // Get the date and trim whitespace

                    Console.WriteLine($"| {acc,-9} | {name,-18} | {service,-16} | {date,-18} |");
                }
            }

            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"Total Appointments: {appointments.Count}");
            Console.WriteLine("Press any key to return...");
            Console.ReadLine();
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
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int accountNumber)) // Check if the account number is valid
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
                Console.WriteLine("=== Top 3 Richest Customers Using LINQ ===");

                var top3 = accountNumbers // LINQ to get top 3 richest customers
                    .Select((acc, i) => new { AccountNumber = acc, Name = accountNames[i], Balance = balances[i], Loan = loans[i] }) // Create an anonymous object with account details
                    .OrderByDescending(x => x.Balance).Take(3).ToList(); // Order by balance descending and take top 3 customers and convert it to list

                if (!top3.Any()) // Check if there are any accounts
                {
                    Console.WriteLine("No accounts available.");
                    return;
                }

                foreach (var customer in top3) // Iterate through each top 3 customer
                {
                    Console.WriteLine("===================================");
                    Console.WriteLine($"Account Number: {customer.AccountNumber}");
                    Console.WriteLine($"Name: {customer.Name}");
                    Console.WriteLine($"Balance: {customer.Balance}");
                    Console.WriteLine($"Loan: {customer.Loan}");
                    Console.WriteLine("===================================");
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

        // Show Locked Accounts
        public static void ShowLockedAccounts()
        {
            Console.Clear();
            Console.WriteLine("=== Locked Accounts ===");

            var locked = accountNumbers // LINQ to filter locked accounts
                .Select((acc, i) => new
                { // Create an anonymous object with account details
                    AccountNumber = acc,
                    Name = accountNames[i],
                    Locked = isAccountLocked[i]
                })
                .Where(x => x.Locked).ToList(); // Filter accounts that are locked and convert it to list

            if (locked.Count == 0) // Check if there are any locked accounts
            {
                Console.WriteLine("No locked accounts.");
                return;
            }

            foreach (var acc in locked) // Iterate through each locked account
            {
                Console.WriteLine($"Account: {acc.AccountNumber} | Name: {acc.Name}");
            }
        }

        // Show Accounts with Active Loans
        public static void ShowAccountsWithActiveLoans()
        {
            Console.Clear();
            Console.WriteLine("=== Accounts with Active Loans ===");

            var activeLoans = accountNumbers // LINQ to filter accounts with active loans
                .Select((acc, i) => new
                { // Create an anonymous object with account details
                    AccountNumber = acc,
                    Name = accountNames[i],
                    Loan = loans[i]
                })
                .Where(x => x.Loan > 0).ToList(); // Filter accounts with loan greater than 0 and convert it to list

            if (activeLoans.Count == 0) // Check if there are any active loans
            {
                Console.WriteLine("No active loans found.");
                return;
            }

            foreach (var acc in activeLoans) // Iterate through each account with active loan
            {
                Console.WriteLine($"Account #: {acc.AccountNumber} | Name: {acc.Name} | Loan: {acc.Loan}");
            }
        }

        // Show Accounts with Low Balance
        public static void ShowLowBalanceAccounts()
        {
            Console.Clear();
            Console.WriteLine("=== Accounts with Low Balance (< 100) ===");

            var low = accountNumbers // LINQ to filter accounts with low balance
                .Select((acc, i) => new
                { // Create an anonymous object with account details
                    AccountNumber = acc, // Account number
                    Name = accountNames[i], // Account name
                    Balance = balances[i] // Account balance
                })
                .Where(x => x.Balance < 100) // Filter accounts with balance less than 100
                .ToList(); // Convert to a list for easy iteration

            if (!low.Any()) // Check if there are any low balance accounts
            {
                Console.WriteLine("No low balance accounts.");
                return;
            }

            foreach (var acc in low) // Iterate through each low balance account
            {
                Console.WriteLine($"Account #: {acc.AccountNumber} | Name: {acc.Name} | Balance: {acc.Balance}");
            }
        }


        // Hash a password using SHA256
        public static string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create()) // Create a new instance of SHA256
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password); // Convert the password to bytes using UTF8 encoding
                var hash = sha.ComputeHash(bytes); // Compute the hash of the password bytes
                return Convert.ToBase64String(hash); // Convert the hash to a Base64 string for storage
            }
        }

        // Read password with masking 
        public static string ReadPasswordMasked()
        {
            // I use String Builder to build or manipulate strings because i do a lot of string modify like appending, removing, or inserting text.

            StringBuilder password = new StringBuilder(); // Initialize a StringBuilder to hold the password
            ConsoleKeyInfo key; // Declare a variable to hold the key pressed

            while (true)
            {
                key = Console.ReadKey(true); // Read the key without displaying it in the console
                if (key.Key == ConsoleKey.Enter) // If the Enter key is pressed
                {
                    Console.WriteLine(); 
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0) // If the Backspace key is pressed and the password is not empty
                {
                    password.Remove(password.Length - 1, 1); // Remove the last character from the password
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar)) // If the key pressed is not a control character
                {
                    password.Append(key.KeyChar); // Append the character to the password
                    Console.Write("*");
                }
            }

            return password.ToString();
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

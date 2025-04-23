using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace MiniBankSystem
{
    internal class Program
    {
        const string BankName = "Mini Bank";
        const double MinimumBalance = 50.0;
        const string AccountsFilePath = "accounts.txt";
        const string ReviewsFilePath = "reviews.txt";

        // Global lists
        static List<int> accountNumbers = new List<int>();
        static List<string> accountNames = new List<string>();
        static List<double> balances = new List<double>();
        static List<double> loans = new List<double>();
        static List<string> Passwords = new List<string>();
        static List<string> transactions = new List<string>();
        static Queue<string> createAccountRequests = new Queue<string>(); 
        static Stack<string> reviewsStack = new Stack<string>();

        // Account number generator
        static int lastAccountNumber;

        static void Main()
        {
            RunBankSystem();
        }
        public static void DisplaySystemUtilitiesMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== System Utilities ===");
                Console.WriteLine("1. Initialize System");
                Console.WriteLine("2. Shutdown System");
                Console.WriteLine("3. Display Main Menu");
                Console.WriteLine("4. Exit");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        InitializeBankSystem();
                        break;
                    case "2":
                        ShutdownBankSystem();
                        break;
                    case "3":
                        DisplayLoginScreen();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
        }
        // Login Screen
        public static void DisplayLoginScreen()
        {
            bool GO = true;

            while (GO)
            {
                Console.Clear();
                Console.WriteLine("||========================================||");
                Console.WriteLine("             ___________               ");
                Console.WriteLine("            //         \\\\        ");
                Console.WriteLine("           //   BANK    \\\\        ");
                Console.WriteLine("          //_____________\\\\     ");
                Console.WriteLine("          ||  _   _   _  ||         ");
                Console.WriteLine("          || |_| |_| |_| ||          ");
                Console.WriteLine("          || |_| |_| |_| ||         ");
                Console.WriteLine("          || |_| |_| |_| ||         ");
                Console.WriteLine("          ||============ ||         ");
                Console.WriteLine("          ||    ___      ||         ");
                Console.WriteLine("          ||   |___|     ||         ");
                Console.WriteLine("          ||_____=_______||          ");
                Console.WriteLine("       ========================== ");
                Console.WriteLine("      || 1. Customer Login      ||");
                Console.WriteLine("      || 2. Administrator Login ||");
                Console.WriteLine("      || 3. Customer Sign Up    ||");
                Console.WriteLine("      || 4. Exit System         ||");
                Console.WriteLine("      ||   Select option:       ||");
                Console.WriteLine("       ==========================");
                Console.WriteLine("||========================================||");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CustomerLogin();
                        break;
                    case "2":
                        AdminLogin();
                        break;
                    case "3":
                        CustomerSignUp();
                        break;
                    case "4":
                        SaveAccountsToFile();
                        SaveReviews();
                        GO = false;
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Sigin Up for Customer
        public static void CustomerSignUp()
        {
            Console.Clear();
            Console.WriteLine("=== Customer Sign-Up ===");

            Console.Write("Enter your full name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty. Sign-up failed.");
                return;
            }

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty. Sign-up failed.");
                return;
            }

            // Generate new account number
            int newAccountNumber = ++lastAccountNumber;

            // Initial balance setup
            double initialBalance = 0;
            while (true)
            {
                Console.Write($"Enter initial deposit (minimum {MinimumBalance}): ");
                if (double.TryParse(Console.ReadLine(), out initialBalance) && initialBalance >= MinimumBalance)
                {
                    break;
                }
                Console.WriteLine("Invalid amount. Please try again.");
            }

            // Store new account details
            accountNumbers.Add(newAccountNumber);
            accountNames.Add(name);
            balances.Add(initialBalance);
            loans.Add(0.0);  // Assuming loan starts at zero
            Passwords.Add(password); // Store password

            // Save to file
            SaveAccountsToFile();

            Console.WriteLine($"Account created successfully!");
            Console.WriteLine($"Your Account Number is: {newAccountNumber}");
            Console.ReadLine();
        }

        // Customer Authentication and Menu
        public static void CustomerLogin()
        {
            Console.Clear();
            Console.WriteLine("=== Customer Login ===");

            Console.Write("Enter Account Number: ");
            int enteredAccountNumber = int.Parse(Console.ReadLine());
            if (!int.TryParse(Console.ReadLine(), out enteredAccountNumber))
            {
                Console.WriteLine("Invalid account number format.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Password: ");
            string enteredPassword = Console.ReadLine();

            // Find index of the entered account number
            int index = accountNumbers.IndexOf(enteredAccountNumber);
            if (index == -1)
            {
                Console.WriteLine("Account number not found.");
                Console.ReadLine();
                return;
            }

            // Check if password matches

            if (Passwords[index] != enteredPassword)
            {
                Console.WriteLine("Incorrect password.");
                Console.ReadLine();
                return;
            }

            // Successful login
            Console.WriteLine($"Login successful! Welcome, {accountNames[index]}.");
            Console.ReadLine();

            DisplayCustomerMenu(accountNumbers[index].ToString());
        }

        public static void DisplayCustomerMenu(string account)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Customer Portal | Account: ===");
                Console.WriteLine("0. Check your Money");
                Console.WriteLine("1. Deposit Funds");
                Console.WriteLine("2. Withdraw Funds");
                Console.WriteLine("3. Transfer Funds");
                Console.WriteLine("4. View Transaction History");
                Console.WriteLine("5. View Account Details");
                Console.WriteLine("6. Apply for Loan");
                Console.WriteLine("7. Submit Review");
                Console.WriteLine("8. Request for create account");
                Console.WriteLine("9. Logout");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "0":
                        CheckMoney(int.Parse(account));
                        break;
                    case "1":
                        ProcessDeposit();
                        break;
                    case "2":
                        ProcessWithdrawal();
                        break;
                    case "3":
                        ProcessTransfer();
                        break;
                    case "4":
                        ViewTransactionHistory();
                        break;
                    case "5":
                        ViewAccountDetails();
                        break;
                    case "6":
                        ApplyForLoan();
                        break;
                    case "7":
                        SubmitReview();
                         break;
                    case "8":
                        RequestNewAccounts();
                        break;
                    case "9":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        // Admin Authentication and Menu
        public static void AdminLogin()
        {
            Console.Write("Enter Admin ID: ");
            string adminId = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            // Basic authentication (replace with secure method)
            if (adminId == "admin" && password == "admin123")
            {
                DisplayAdminMenu();
            }
            else
            {
                Console.WriteLine("Invalid credentials!");
            }
        }

        public static void DisplayAdminMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Admin Menu ===");
                Console.WriteLine("1. Account Management");
                Console.WriteLine("2. Customer Management");
                Console.WriteLine("3. Transaction Processing");
                Console.WriteLine("4. Loan Management");
                Console.WriteLine("5. Reviews");
                Console.WriteLine("6. Reporting");
                Console.WriteLine("7. Process Account Requests ");
                Console.WriteLine("8. Logout");
                Console.WriteLine("===            ===");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        DisplayAccountManagementMenu();
                        break;
                    case "2":
                        DisplayCustomerManagementMenu();
                        break;
                    case "3":
                        DisplayTransactionManagementMenu();
                        break;
                    case "4":
                        DisplayLoanManagementMenu();
                        break;
                    case "5":
                        ViewReviews();
                        break;
                    case "6":
                        DisplayReportingMenu();
                        break;
                    case "7":
                        ProcessAccountRequests();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        // Sub-menus for Admin
        public static void DisplayAccountManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Account Management ===");
                Console.WriteLine("1. Create New Account");
                Console.WriteLine("2. Close Account");
                Console.WriteLine("3. View Account Details");
                Console.WriteLine("4. List All Accounts");
                Console.WriteLine("5. Back to Admin Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CreateNewAccount();
                        break;
                    case "2":
                        DeleteAccount();
                        break;
                    case "3":
                        ViewAccountDetails();
                        break;
                    case "4":
                        ListAllAccounts();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void DisplayCustomerManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Customer Management ===");
                Console.WriteLine("1. Register New Customer");
                Console.WriteLine("2. Update Customer Information");
                Console.WriteLine("3. View Customer Details");
                Console.WriteLine("4. Back to Admin Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        RegisterCustomer();
                        break;
                    case "2":
                        UpdateCustomerInformation();
                        break;
                    case "3":
                        ViewCustomerDetails();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void DisplayTransactionManagementMenu(string account)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Transaction Management ===");
                Console.WriteLine("1. Process Deposit");
                Console.WriteLine("2. Process Withdrawal");
                Console.WriteLine("3. Process Transfer");
                Console.WriteLine("4. Check your Money");
                Console.WriteLine("5. View Transaction History");
                Console.WriteLine("6. Back to Admin Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ProcessDeposit();
                        break;
                    case "2":
                        ProcessWithdrawal();
                        break;
                    case "3":
                        ProcessTransfer();
                        break;
                    case "4":
                        CheckMoney(int.Parse(account));
                        break;
                    case "5":
                        ViewTransactionHistory();
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

        public static void DisplayLoanManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Loan Management ===");
                Console.WriteLine("1. Approve Loan Application");
                Console.WriteLine("2. Process Loan Payment");
                Console.WriteLine("3. Back to Admin Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ApproveLoanApplication();
                        break;
                    case "2":
                        ProcessLoanPayment();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void DisplayReportingMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Reporting ===");
                Console.WriteLine("1. Generate Daily Report");
                Console.WriteLine("2. Generate Monthly Statement");
                Console.WriteLine("3. Back to Admin Menu");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        GenerateDailyReport();
                        break;
                    case "2":
                        GenerateMonthlyStatement();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void InitializeBankSystem()
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
            balances.Clear();
            loans.Clear();
            transactions.Clear();
            createAccountRequests.Clear();
            reviewsStack.Clear();

            // Reset account number tracking
            lastAccountNumber = 0;

            // Load data from files
            LoadAccountsFromFile();
            LoadReviews();

            Console.WriteLine("Bank system initialized successfully.");
            Console.ReadLine();

        }

        public static void RunBankSystem()
        {
            DisplaySystemUtilitiesMenu();
            ShutdownBankSystem();
        }

        public static void ShutdownBankSystem()
        {
            Console.Clear();
            Console.WriteLine("Shutting down the Mini Bank System...");

            // Save data before exiting
            SaveAccountsToFile();
            SaveReviews();

            Console.WriteLine("All data saved successfully.");
            Console.WriteLine("System has been shut down safely. Press any key to exit.");
            Console.ReadLine();

            Environment.Exit(0); // Terminate the application
        }

        // Account Operations
        public static void RequestNewAccounts()
        {
            Console.Write("Enter your full name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your National ID: ");
            string nationalID = Console.ReadLine();

            string request = name + "||" + nationalID;
            Console.WriteLine(request);
            createAccountRequests.Enqueue(request);

            Console.WriteLine("Your account request has been submitted.");
        }

        public static void ProcessAccountRequests()
        {
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
                return;
            }

            //var (name, nationalID) = createAccountRequests.Dequeue();
            string request = createAccountRequests.Dequeue();
            string[] parts = request.Split('|');
            string name = parts[0];
            string nationalID = parts[1];

            int newAccountNumber = lastAccountNumber + 1;

            accountNumbers.Add(newAccountNumber);
            accountNames.Add($"{name} ");
            balances.Add(0.0);

            lastAccountNumber = newAccountNumber;

            Console.WriteLine($"Account created for {name} with Account Number: {newAccountNumber}");
        }

        public static void CreateNewAccount()
        {
        
        }

        public static void DeleteAccount()
        {
     
        }

        public static void ViewAccountDetails()
        {

        }

        public static void ListAllAccounts()
        {

        }

        // Customer Operations
        public static void RegisterCustomer()
        {

        }

        public static void UpdateCustomerInformation()
        {
    
        }

        public static void ViewCustomerDetails()
        {
       
        }

        // Transaction Processing
        public static void ProcessDeposit()
        {
    
        }

        public static void ProcessWithdrawal()
        {

        }

        public static void ProcessTransfer()
        {

        }

        public static void CheckMoney(int accountNumber)
        {
            int index = accountNumbers.IndexOf(accountNumber);
            if (index != -1)
            {
                Console.WriteLine("=== Account Balance ===");
                Console.WriteLine($"Account Holder: {accountNames[index]}");
                Console.WriteLine($"Current Balance: {balances[index]:C}");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        public static void ViewTransactionHistory()
        {
    
        }

        // Loan Operations
        public static void ApplyForLoan()
        {
        
        }

        public static void ApproveLoanApplication()
        {
  
        }

        public static void ProcessLoanPayment()
        {

        }

        // Reporting
        public static void GenerateDailyReport()
        {

        }

        public static void GenerateMonthlyStatement()
        {

        }

        // Save in Files
        static void SaveAccountsToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        string dataLine = $"{accountNumbers[i]},{accountNames[i]},{balances[i]},{Passwords[i]},{transactions[i]}";
                        writer.WriteLine(dataLine);
                    }
                }
                Console.WriteLine("Accounts saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }
        }

        static void LoadAccountsFromFile()
        {
            try
            {
                if (!File.Exists(AccountsFilePath))
                {
                    Console.WriteLine("No saved data found.");
                    return;
                }

                accountNumbers.Clear();
                accountNames.Clear();
                balances.Clear();
                Passwords.Clear();
                loans.Clear();
                transactions.Clear();

                using (StreamReader reader = new StreamReader(AccountsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        int accNum = Convert.ToInt32(parts[0]);
                        accountNumbers.Add(accNum);
                        accountNames.Add(parts[1]);
                        balances.Add(Convert.ToDouble(parts[2]));
                        Passwords.Add(parts[3]);
                        loans.Add(Convert.ToDouble(parts[4]));
                        transactions.Add(parts[5]);

                        if (accNum > lastAccountNumber)
                            lastAccountNumber = accNum;
                    }
                }

                Console.WriteLine("Accounts loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }

        }

        // Reviews

        static void SubmitReview()
        {
            Console.Write("Enter your review or complaint: ");
            string review = Console.ReadLine();
            reviewsStack.Push(review);
            Console.WriteLine("Thank you! Your feedback has been recorded.");
        }

        static void ViewReviews()
        {
            if (reviewsStack.Count == 0)
            {
                Console.WriteLine("No reviews or complaints submitted yet.");
                return;
            }

            Console.WriteLine("Recent Reviews/Complaints (most recent first):");
            foreach (string r in reviewsStack)
            {
                Console.WriteLine("- " + r);
            }
        }

        static void SaveReviews()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ReviewsFilePath))
                {
                    foreach (var review in reviewsStack)
                    {
                        writer.WriteLine(review);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error saving reviews.");
            }
        }

        static void LoadReviews()
        {
            try
            {
                if (!File.Exists(ReviewsFilePath)) return;

                using (StreamReader reader = new StreamReader(ReviewsFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        reviewsStack.Push(line);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error loading reviews.");
            }
        }

        // Utility Functions
        public static int GenerateAccountNumber()
        {
            // Empty - to be implemented
            return 0;
        }

        public static int GenerateCustomerId()
        {
            // Empty - to be implemented
            return 0;
        }

        public static string FindAccount(int accountNumber)
        {
            int index = accountNumbers.IndexOf(accountNumber);
            if (index != -1)
            {
                string name = accountNames[index];
                double balance = balances[index];
                return $"Account Found: {name} | Balance: {balance:C}";
            }
            else
            {
                return $"The Account Not Found by {accountNumber} ";
            }
        }

        public static void FindCustomer()
        {

        }
    }
}

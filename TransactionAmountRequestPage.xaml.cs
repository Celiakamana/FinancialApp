using Microsoft.Maui.Controls;
using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui;
using Microsoft.Data.SqlClient;

namespace FinancialApp
{
    public partial class TransactionAmountRequestPage : ContentPage
    {
        private bool isScanned = false; // Flag to track if QR code is already scanned

        public TransactionAmountRequestPage()
        {
            InitializeComponent();
            AmountEntry.TextChanged += OnAmountEntryTextChanged;
        }

        // Method to handle the "Back" button click event
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the previous page
            await Navigation.PopAsync();
        }

        // Method to handle the "Cancel" button click event
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to TransactionsPage
            await Navigation.PushAsync(new TransactionsPage());
        }

        // Method to handle the "By Phone" button click event
        private async void OnByPhoneButtonClicked(object sender, EventArgs e)
        {
            // Check if an amount has been entered and that it is greater than $1
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 1)
            {

                // Navigate to the RequestByPhonePage when "By Phone" is selected and amount is greater than a dollar
                await Navigation.PushAsync(new RequestByPhonePage(amount));
            }
            else
            {
                // Show an error message if the amount is not valid
                await DisplayAlert("Invalid Amount", "Please enter an amount greater than $1.00", "OK");
            }
        }

        // Method to handle the "By QR" button click event
        private async void OnByQRButtonClicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 1)
            {
                // Create a new ContentPage to host the QR code scanner
                var scannerPage = new ContentPage
                {
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new Button
                            {
                                Text = "✕",
                                HorizontalOptions = LayoutOptions.End,
                                VerticalOptions = LayoutOptions.Center,
                                BackgroundColor = Colors.Purple,
                                TextColor = Colors.White,
                                HeightRequest = 50,
                                WidthRequest = 50,
                                CornerRadius = 25,
                                FontSize = 20,
                                Command = new Command(async () => await Navigation.PopAsync())
                            },
                            new CameraBarcodeReaderView
                            {
                                HeightRequest = 500,
                                WidthRequest = 500,
                                AutomationId = "QRScanner",
                                Options = new BarcodeReaderOptions
                                {
                                    Formats = BarcodeFormat.QrCode
                                }
                            }
                        }
                    }
                };

                var barcodeReaderView = (CameraBarcodeReaderView)((StackLayout)scannerPage.Content).Children[1];

                // Subscribe to the BarcodesDetected event
                barcodeReaderView.BarcodesDetected += async (s, e) =>
                {
                    if (isScanned) return; // If already scanned, exit

                    // Ensure the results contain at least one item
                    var results = e?.Results?.ToList();
                    if (results == null || results.Count == 0) return;

                    // Set flag before processing to prevent multiple scans
                    isScanned = true;

                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        try
                        {
                            // Extract user info from QR code
                            string[] qrInfo = results[0].Value.Split(',');

                            if (qrInfo.Length == 2)
                            {
                                // Extract userID and recipientPhoneNumber
                                if (int.TryParse(qrInfo[0], out int userID) && !string.IsNullOrEmpty(qrInfo[1]))
                                {
                                    string recipientPhoneNumber = qrInfo[1];

                                    // Validate that the user is not sending money to themselves
                                    if (recipientPhoneNumber == MainPage.CurrentUserPhoneNumber)
                                    {
                                        await DisplayAlert("Error", "You cannot send money to your own account.", "OK");
                                        isScanned = false; // Allow rescan in case of error
                                        return;
                                    }

                                    // Query the database to fetch additional details of the recipient (firstName, lastName)
                                    string connectionString = DatabaseConfig.ConnectionString;
                                    string query = @"SELECT FirstName, LastName FROM UsersTable WHERE UserID = @UserID AND PhoneNumber = @PhoneNumber";

                                    using (SqlConnection conn = new SqlConnection(connectionString))
                                    {
                                        await conn.OpenAsync();
                                        using (SqlCommand cmd = new SqlCommand(query, conn))
                                        {
                                            // Add parameters to SQL command
                                            cmd.Parameters.AddWithValue("@UserID", userID);
                                            cmd.Parameters.AddWithValue("@PhoneNumber", recipientPhoneNumber);

                                            using (var reader = await cmd.ExecuteReaderAsync())
                                            {
                                                if (reader.HasRows && await reader.ReadAsync())
                                                {
                                                    string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                                    string lastName = reader.GetString(reader.GetOrdinal("LastName"));

                                                    // Unsubscribe from the event to prevent further scans
                                                    barcodeReaderView.BarcodesDetected -= null;

                                                    // Proceed with transaction
                                                    await Navigation.PushAsync(new RequestConfirmationPage(firstName, lastName, amount));
                                                }
                                                else
                                                {
                                                    await DisplayAlert("Error", "User not found", "OK");
                                                    isScanned = false; // Allow rescan in case of error
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Error", "Invalid QR code format.", "OK");
                                    isScanned = false; // Allow rescan in case of error
                                }
                            }
                            else
                            {
                                await DisplayAlert("Error", "Invalid QR code format.", "OK");
                                isScanned = false; // Allow rescan in case of error
                            }
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", $"Database error: {ex.Message}", "OK");
                            isScanned = false; // Allow rescan in case of error
                        }
                    });
                };

                await Navigation.PushAsync(scannerPage);
            }
            else
            {
                await DisplayAlert("Invalid Amount", "Please enter an amount greater than $1.00", "OK");
            }
        }

        //Method to handle "By USSD" button click event
        private async void OnByUSSDButtonClicked(object sender, EventArgs e)
        {
            // Check if an amount has been entered and that it is greater than $1
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 1)
            {
                string ussdCode = await DisplayPromptAsync("Enter USSD Code", "Format: *12*firstName*lastName*phoneNumber#");

                // If user cancels the prompt, `ussdCode` will be null
                if (string.IsNullOrEmpty(ussdCode))
                {
                    // User cancelled the prompt, so simply return
                    return;
                }

                if (ValidateUSSDCode(ussdCode))
                {
                    // Extract user info from USSD code
                    var parts = ussdCode.Split('*');
                    if (parts.Length == 5 && parts[0] == "" && parts[1] == "12" && parts[^1].EndsWith("#"))
                    {
                        string firstName = parts[2];
                        string lastName = parts[3];
                        string recipientphoneNumber = parts[4].TrimEnd('#');

                        // Validate that the user is not sending money to themselves
                        if (recipientphoneNumber == MainPage.CurrentUserPhoneNumber)
                        {
                            await DisplayAlert("Error", "You cannot send money to your own account.", "OK");
                            return;
                        }

                        // Verify if provided information matches a user in the database
                        try
                        {
                            // Connection string to the UserRegistrationDB database
                            string connectionString = DatabaseConfig.ConnectionString;

                            // SQL query to verify if the provided information matches a user in the database
                            string query = @"SELECT COUNT(1) FROM UsersTable WHERE FirstName = @FirstName AND LastName = @LastName AND PhoneNumber = @PhoneNumber";

                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                await conn.OpenAsync();

                                using (SqlCommand cmd = new SqlCommand(query, conn))
                                {
                                    // Prevent SQL injection
                                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                                    cmd.Parameters.AddWithValue("@LastName", lastName);
                                    cmd.Parameters.AddWithValue("@PhoneNumber", recipientphoneNumber);

                                    // Execute the query
                                    int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                                    // If a match is found, navigate to SendConfirmationPage
                                    if (count == 1)
                                    {
                                        await Navigation.PushAsync(new RequestConfirmationPage(firstName, lastName, amount));
                                    }
                                    else
                                    {
                                        // If no match is found, show an error message
                                        await DisplayAlert("Error", "User not found", "OK");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle any errors that occur during database interaction
                            await DisplayAlert("Error", $"Database error: {ex.Message}", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Invalid USSD code format", "OK");
                }
            }
            else
            {
                // Show an error message if the amount is not valid
                await DisplayAlert("Invalid Amount", "Please enter an amount greater than $1.00", "OK");
            }
        }

        // Validate USSD code
        private bool ValidateUSSDCode(string code)
        {
            return !string.IsNullOrEmpty(code) && code.StartsWith("*12*") && code.EndsWith("#");
        }

        // Event handler to format the AmountEntry with two decimal places
        private void OnAmountEntryTextChanged(object? sender, TextChangedEventArgs e)
        {
            //just digits
            if (decimal.TryParse(AmountEntry.Text, out decimal amount))
            {
                string currentText = e.NewTextValue;

                // Remove any non-digit characters
                currentText = string.Concat(currentText.Where(char.IsDigit));

                // If the field is not empty, treat it as a fixed decimal with two decimal places
                if (currentText.Length > 0)
                {
                    // Format as two decimal places, treating the input as a whole number in cents
                    decimal value = decimal.Parse(currentText) / 100;
                    AmountEntry.Text = value.ToString("0.00");
                    AmountEntry.CursorPosition = AmountEntry.Text.Length;
                }
                else
                {
                    // If field is empty, reset to zero with two decimal places
                    AmountEntry.Text = "0.00";
                    AmountEntry.CursorPosition = AmountEntry.Text.Length;
                }
            }

            // Re-add the event handler
            AmountEntry.TextChanged += OnAmountEntryTextChanged;



        }
    }
}

using Microsoft.Data.SqlClient;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using QRCoder;
using System.IO;



namespace FinancialApp
{
    public partial class TransactionsPage : ContentPage
    {
        public TransactionsPage()
        {
            InitializeComponent();
            LoadUserWelcomeMessage();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to LoginPage(MainPage
            await Navigation.PushAsync(new MainPage());
        }

        private async void OnQRCodeClicked(object sender, EventArgs e)
        {
            // Gather the necessary data to include in the QR Code
          
            string recipientPhoneNumber = MainPage.CurrentUserPhoneNumber;
            int userID = MainPage.CurrentUserID;

            // Create data string including UserID and PhoneNumber
            string qrData = $"{userID},{recipientPhoneNumber}";

            // Generate QR code using QRCoder with PngByteQRCode
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
                using (var pngQrCode = new PngByteQRCode(qrCodeData))
                {
                    // Get QR Code as PNG byte array
                    byte[] qrCodeAsPngByteArr = pngQrCode.GetGraphic(20);

                    // Convert the byte array to a MemoryStream
                    using (var stream = new MemoryStream(qrCodeAsPngByteArr))
                    {
                        var qrCodeImageSource = ImageSource.FromStream(() => stream);

                        // Save QR code data to the UsersTable
                        string connectionString = DatabaseConfig.ConnectionString;
                        string updateQuery = @"UPDATE UsersTable SET QRCodeData = @QRCodeData WHERE UserID = @UserID";

                        try
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                await conn.OpenAsync();
                                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                                {
                                    cmd.Parameters.AddWithValue("@QRCodeData", qrData);
                                    cmd.Parameters.AddWithValue("@UserID", userID);
                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", $"Failed to save QR code: {ex.Message}", "OK");
                        }

                        // Display the QR code using an Image control
                        var qrCodePage = new ContentPage
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
                                    new Image
                                    {
                                        Source = qrCodeImageSource,
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.Start,
                                        HeightRequest = 300,
                                        WidthRequest = 300
                                    }
                                }
                            }
                        };

                        await Navigation.PushAsync(qrCodePage);
                    }
                }
            }
        }

        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the TransactionAmountPage when the Send button is clicked
            await Navigation.PushAsync(new TransactionAmountSendPage());
        }

        private async void OnRequestButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the TransactionAmountPage when the Request button is clicked
            await Navigation.PushAsync(new TransactionAmountRequestPage());
        }

        private async void OnPayBillButtonClicked(object sender, EventArgs e)
        {
            // Logic for handling Pay Bill (placeholder for future implementation)
            await DisplayAlert("Pay Bill", "This button's logic will be implemented later.", "OK");
        }

        private async void OnHomeIconClicked(object sender, EventArgs e)
        {
            // Navigate to the home page
            await Navigation.PushAsync(new HomePage());
        }

        private async  void OnTransactionIconClicked(object sender, EventArgs e)
        {
            // Handle transaction icon click
            await DisplayAlert("Recent Transactions List", "This button's logic will be implemented later.", "OK");
        }

        private async void OnNotificationIconClicked(object sender, EventArgs e)
        {
            // Handle Notifications icon click
            await DisplayAlert("Recent Notifications List", "This button's logic will be implemented later.", "OK");
        }

        //To load the name wih the welcome  label
         private async void LoadUserWelcomeMessage()
        {
            string connectionString = DatabaseConfig.ConnectionString;
            string query = @"SELECT FirstName FROM UsersTable WHERE UserID = @UserID";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", MainPage.CurrentUserID);

                        var firstNameObj = await cmd.ExecuteScalarAsync();

                        if (firstNameObj != null)
                        {
                            string firstName = firstNameObj?.ToString() ?? string.Empty;

                            if (!string.IsNullOrEmpty(firstName))
                            {
                                // Capitalize the first letter of the first name
                                string firstNameStr = char.ToUpper(firstName[0]) + firstName.Substring(1).ToLower();
                                WelcomeLabel.Text = $"Welcome Back {firstNameStr},";
                            }
                            else
                            {
                                WelcomeLabel.Text = "Welcome Back,";
                            }
                        }
                        else
                        {
                            WelcomeLabel.Text = "Welcome Back,";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load user data: {ex.Message}", "OK");
            }
         }

    }
}
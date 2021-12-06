using DecryptV3.Dtos;
using DecryptV3.ServiceFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DecryptDemoWinApp
{

    
    /// <summary>
    /// Interaction logic for DecryptCardSwipe.xaml
    /// </summary>
    public partial class DecryptCardSwipe : Window
    {
        private List<AdditionalRequestData> _additionalRequestData = new List<AdditionalRequestData>();
        public DecryptCardSwipe()
        {
            InitializeComponent();
            dgAdditionalRequestData.ItemsSource = _additionalRequestData;
            dgAdditionalRequestData.ColumnWidth = new DataGridLength(100);
        }

        private async void btn_Process_Click(object sender, RoutedEventArgs e)
        {
            
            DecryptCardSwipeRequestDto decryptRequest = new DecryptCardSwipeRequestDto();
            try
            {
                decryptRequest.BillingLabel = "";
                decryptRequest.CustomerTransactionID = txt_customerTransactionId.Text;
                decryptRequest.CustomerCode = txt_Customercode.Text;
                decryptRequest.Password = txt_Password.Password;
                decryptRequest.Username = txt_Username.Text;
                decryptRequest.DeviceSN = txt_DeviceSn.Text;
                decryptRequest.KSN = txt_KSn.Text;
                decryptRequest.KeyType = txt_KeyType.Text;
                decryptRequest.MagnePrint = txt_MagnePrint.Text;
                decryptRequest.MagnePrintStatus = txt_MagnePrintStatus.Text;
                var track1 = txt_Track1.Text;
                decryptRequest.Track1 = track1;
                decryptRequest.Track2 = txt_Track2.Text;
                decryptRequest.Track3 = txt_Track3.Text;
                decryptRequest.AdditionalRequestData = new List<KeyValuePair<string, string>>();
                foreach (var item in _additionalRequestData)
                {
                    var temp = new KeyValuePair<string, string>(item.Key, item.Value);
                    decryptRequest.AdditionalRequestData.Add(temp);
                }

                var validator = new DecryptCardSwipeRequestDtoValidator();
                var results = validator.Validate(decryptRequest);
                if (!results.IsValid)
                {
                    var errorMsg = string.Empty;
                    var failures = results.Errors.ToList();
                    failures.ForEach(x => errorMsg = errorMsg + x + Environment.NewLine);
                    MessageBox.Show(errorMsg, "Validation Failure");
                }
                else
                {
                    IServiceCollection services = new ServiceCollection();
                    IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
                    services.AddSingleton<IConfiguration>(config);
                    services.AddSingleton<IDecryptV3Client, DecryptV3Client>();
                    if (config.GetValue<string>("CERTIFICATE_FILENAME").Trim() == "")
                    {
                        MessageBox.Show("Certificate FileName Not Found In appsettings.json");
                    }
                    else if (config.GetValue<string>("CERTIFICATE_PASSWORD").Trim() == "")
                    {
                        MessageBox.Show("Certificate Password Not Found In appsettings.json");
                    }
                    else
                    {
                        IServiceProvider serviceProvider = services.BuildServiceProvider();
                        var svc = serviceProvider.GetService<IDecryptV3Client>();
                        var result = await svc.DecryptCardSwipe(decryptRequest);
                        if ((result.Response != null) && (result.SoapDetails != null))
                        {
                            txt_Response.Text = result.Response.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txt_Response.Text = "Error while DecryptCardSwipe : " + ex.Message.ToString();
            }
        }



        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            var Mainwindow = new MainWindow();
            Mainwindow.Show();
            this.Close();
        }

        public static IEnumerable<T> GetChildrenOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int index = 0; index < VisualTreeHelper.GetChildrenCount(dependencyObject); index++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, index);
                    if (child != null && child is T) yield return (T)child;
                    foreach (T childOfChild in GetChildrenOfType<T>(child)) yield return childOfChild;
                }
            }
        }
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            _additionalRequestData = new List<AdditionalRequestData>();
            dgAdditionalRequestData.ItemsSource = _additionalRequestData;
            GetChildrenOfType<TextBox>(this.ContainerGrid).ToList().ForEach(textBox => textBox.Clear());
            txt_Password.Password = "";
        }
    }
}


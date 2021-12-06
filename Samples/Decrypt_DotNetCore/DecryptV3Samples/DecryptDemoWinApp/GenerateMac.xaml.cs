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
    /// Interaction logic for GenerateMac.xaml
    /// </summary>
    public partial class GenerateMac : Window
    {
        private List<AdditionalRequestData> _additionalRequestData = new List<AdditionalRequestData>();
        public GenerateMac()
        {
            InitializeComponent();
            dgAdditionalRequestData.ItemsSource = _additionalRequestData;
            dgAdditionalRequestData.ColumnWidth = new DataGridLength(100);

        }

        private async void btn_Process_Click(object sender, RoutedEventArgs e)
        {
            GenerateMacRequestDto generateMacRequest = new GenerateMacRequestDto();

            try
            {
                generateMacRequest.BillingLabel = txt_billinglabel.Text;

                generateMacRequest.CustomerTransactionID = txt_customerTransactionId.Text;
                generateMacRequest.CustomerCode = txt_Customercode.Text;
                generateMacRequest.Password = txt_Password.Password;
                generateMacRequest.Username = txt_Username.Text;
                var dataToMAC = txt_DataToMac.Text;
                generateMacRequest.DataToMAC = dataToMAC;
                generateMacRequest.KSN = txt_KSn.Text;
                generateMacRequest.KeyDerivationType = txt_KeyType.Text;
                generateMacRequest.AdditionalRequestData = new List<KeyValuePair<string, string>>();
                foreach (var item in _additionalRequestData)
                {
                    var temp = new KeyValuePair<string, string>(item.Key, item.Value);
                    generateMacRequest.AdditionalRequestData.Add(temp);
                }
                var validator = new GenerateMacRequestDtoValidator();
                var results = validator.Validate(generateMacRequest);
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
                        var result = await svc.GenerateMac(generateMacRequest);
                        if ((result.Response != null) && (result.SoapDetails != null))
                        {
                            txt_Response.Text = result.Response.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txt_Response.Text = "Error while GenerateMac : " + ex.Message.ToString();
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

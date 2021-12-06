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
    /// Interaction logic for DecryptData.xaml
    /// </summary>
    public partial class DecryptData : Window
    {
        private List<AdditionalRequestData> _additionalRequestData = new List<AdditionalRequestData>();
        public DecryptData()
        {
            InitializeComponent();
            dgAdditionalRequestData.ItemsSource = _additionalRequestData;
            dgAdditionalRequestData.ColumnWidth = new DataGridLength(100);
        }

        private async void btn_Process_Click(object sender, RoutedEventArgs e)
        {
            DecryptDataRequestDto decryptDataRequestDto = new DecryptDataRequestDto();

            try
            {

                decryptDataRequestDto.BillingLabel = txt_billinglabel.Text;
                decryptDataRequestDto.CustomerTransactionID = txt_customerTransactionId.Text;
                decryptDataRequestDto.CustomerCode = txt_Customercode.Text;
                decryptDataRequestDto.Password = txt_Password.Password;
                decryptDataRequestDto.Username = txt_Username.Text;
                var encrypteddata = txt_EncryptedData.Text;
                decryptDataRequestDto.EncryptedData = encrypteddata;
                decryptDataRequestDto.KSN = txt_KSn.Text;
                decryptDataRequestDto.KeyType = txt_KeyType.Text;
                decryptDataRequestDto.AdditionalRequestData = new List<KeyValuePair<string, string>>();
                foreach (var item in _additionalRequestData)
                {
                    var temp = new KeyValuePair<string, string>(item.Key, item.Value);
                    decryptDataRequestDto.AdditionalRequestData.Add(temp);
                }

                var validator = new DecryptDataRequestDtoValidator();
                var results = validator.Validate(decryptDataRequestDto);
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
                        var result = await svc.DecryptData(decryptDataRequestDto);
                        if ((result.Response != null) && (result.SoapDetails != null))
                        {
                            txt_Response.Text = result.Response.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                txt_Response.Text = "Error while DecryptData : " + ex.Message.ToString();
            }
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

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            var Mainwindow = new MainWindow();
            Mainwindow.Show();
            this.Close();
        }
    }
}

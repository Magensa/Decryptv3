using DecryptV3.Dtos;
using DecryptV3.ServiceFactory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DecryptV3.UIFactory
{
    /// <summary>
    ///  Factory of Decrypt operations
    /// </summary>
    public class DecryptUIFactory : IDecryptUIFactory
    {
        IServiceProvider _serviceProvider;
        public DecryptUIFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void ShowUI(DecryptUI decryptUI)
        {
            switch (decryptUI)
            {
                case DecryptUI.DECRYPTCARDSWIPE:
                    ShowDecryptCardSwipeUI();
                    break;
                case DecryptUI.DECRYPTDATA:
                    ShowDecryptDataUI();
                    break;
                case DecryptUI.GENERATEMAC:
                    ShowGenerateMacUI();
                    break;

            }
        }
        private void ShowDecryptCardSwipeUI()
        {
            DecryptCardSwipeRequestDto decryptRequest = new DecryptCardSwipeRequestDto();
            try
            {
                Console.WriteLine("=====================Request building start======================");
                decryptRequest.BillingLabel = Read_String_Input("Please enter the BillingLabel:", true);
                decryptRequest.CustomerTransactionID = Read_String_Input("Please enter the CustomerTransactionID:", true);
                decryptRequest.CustomerCode = Read_String_Input("Please enter the CustomerCode:", false);
                decryptRequest.Username = Read_String_Input("Please enter the Username:", false);
                decryptRequest.Password = Read_String_Input("Please enter the Password:", false);
                decryptRequest.DeviceSN = Read_String_Input("Please enter the DeviceSN:", false);
                decryptRequest.KSN = Read_String_Input("Please enter the KSN:", false);
                decryptRequest.KeyType = Read_KeyType_Input("Please enter the KeyType:");
                decryptRequest.MagnePrint = Read_String_Input("Please enter the MagnePrint:", false);
                decryptRequest.MagnePrintStatus = Read_String_Input("Please enter the MagnePrintStatus:", false);
                decryptRequest.Track1 = Read_LongString_Input("Please enter the Track1:", false);
                decryptRequest.Track2 = Read_LongString_Input("Please enter the Track2:", false);
                decryptRequest.Track3 = Read_LongString_Input("Please enter the Track3:", true);
                decryptRequest.AdditionalRequestData = Read_MultipleKeysInput("AdditionalRequestData");
                Console.WriteLine("=====================Request building End======================");
                var svc = _serviceProvider.GetService<IDecryptV3Client>();
                var result = svc.DecryptCardSwipe(decryptRequest).Result;
                if ((result.Response != null) && (result.SoapDetails != null))
                {
                    Console.WriteLine("=====================Response Start======================");
                    Console.WriteLine("Request:");
                    Console.Write(PrettyXml(result.SoapDetails.RequestXml) + "\n");
                    Console.WriteLine("Response:");
                    Console.Write(PrettyXml(result.SoapDetails.ResponseXml) + "\n");
                    Console.WriteLine("=====================Response End======================");
                    Console.WriteLine("=====================Parsed Response Start======================");
                    Console.WriteLine(result.Response.ToString());
                    Console.WriteLine("=====================Parsed Response End======================");
                }
                else
                {
                    Console.WriteLine("Response is null, Please check with input values given and try again");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while Decryptcardswipe : " + ex.Message.ToString());
            }
        }
        private void ShowDecryptDataUI()
        {
            DecryptDataRequestDto decryptDataRequestDto = new DecryptDataRequestDto();
            try
            {
                Console.WriteLine("=====================Request building start======================");
                decryptDataRequestDto.BillingLabel = Read_String_Input("Enter BillingLabel:", true);
                decryptDataRequestDto.CustomerTransactionID = Read_String_Input("Enter CustomerTransactionID:", true);
                decryptDataRequestDto.CustomerCode = Read_String_Input("Enter CustomerCode:", false);
                decryptDataRequestDto.Username = Read_String_Input("Enter UserName:", false);
                decryptDataRequestDto.Password = Read_String_Input("Enter Password:", false);
                decryptDataRequestDto.EncryptedData = Read_LongString_Input("Enter EncryptedData:", false);
                decryptDataRequestDto.KSN = Read_String_Input("Enter KSN:", false);
                decryptDataRequestDto.KeyType = Read_KeyType_Input("Enter KeyType:");
                decryptDataRequestDto.AdditionalRequestData = Read_MultipleKeysInput("AdditionalRequestData");
                var svc = _serviceProvider.GetService<IDecryptV3Client>();
                var result = svc.DecryptData(decryptDataRequestDto).Result;
                if ((result.Response != null) && (result.SoapDetails != null))
                {
                    Console.WriteLine("=====================Response Start======================");
                    Console.WriteLine("Request:");
                    Console.Write(PrettyXml(result.SoapDetails.RequestXml) + "\n");
                    Console.WriteLine("Response:");
                    Console.Write(PrettyXml(result.SoapDetails.ResponseXml) + "\n");
                    Console.WriteLine("=====================Response End======================");
                    Console.WriteLine("=====================Parsed Response Start======================");
                    Console.WriteLine(result.Response.ToString());
                    Console.WriteLine("=====================Parsed Response End======================");
                }
                else
                {
                    Console.WriteLine("Response is null, Please check with input values given and try again");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while DecryptData : " + ex.Message.ToString());
            }
        }
        private void ShowGenerateMacUI()
        {
            GenerateMacRequestDto req = new GenerateMacRequestDto();
            try
            {
                req.BillingLabel = Read_String_Input("Enter BillingLabel:", true);
                req.CustomerTransactionID = Read_String_Input("Enter CustomerTransactionID:", true);
                req.CustomerCode = Read_String_Input("Enter CustomerCode:", false);
                req.Username = Read_String_Input("Enter UserName:", false);
                req.Password = Read_String_Input("Enter Password:", false);
                req.DataToMAC = Read_String_Input("Enter DataToMAC:", false);
                req.KSN = Read_String_Input("Enter KSN:", false);
                req.KeyDerivationType = Read_KeyDerivationType_Input("Enter KeyDerivationType:");
                req.AdditionalRequestData = Read_MultipleKeysInput("AdditionalRequestData");
                var svc = _serviceProvider.GetService<IDecryptV3Client>();
                var result = svc.GenerateMac(req).Result;
                if ((result.Response != null) && (result.SoapDetails != null))
                {
                    Console.WriteLine("=====================Response Start======================");
                    Console.WriteLine("Request:");
                    Console.Write(PrettyXml(result.SoapDetails.RequestXml) + "\n");
                    Console.WriteLine("Response:");
                    Console.Write(PrettyXml(result.SoapDetails.ResponseXml) + "\n");
                    Console.WriteLine("=====================Response End======================");
                    Console.WriteLine("=====================Parsed Response Start======================");
                    Console.WriteLine(result.Response.ToString());
                    Console.WriteLine("=====================Parsed Response End======================");
                }
                else
                {
                    Console.WriteLine("Response is null, Please check with input values given and try again");
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Error while GenerateMac : " + ex.Message.ToString());
                
            }
        }

        #region Helper Functions

        private static List<KeyValuePair<string, string>> Read_MultipleKeysInput(string question)
        {
            var noOfKeys = Read_Intuser_Input($"Please Enter No of Keys for {question}");
            var result = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < noOfKeys; i++)
            {
                var key = Read_Optional_String_Input("Key");
                var val = Read_Optional_String_Input("Value");
                result.Add(new KeyValuePair<string, string>(key, val));
            }
            return result;
        }
        private static string Read_Optional_String_Input(string question)
        {
            return Read_String_Input($"{question}:", true);
        }
        private static int Read_Intuser_Input(string question)
        {
            Console.WriteLine(question);
            var ans = Console.ReadLine();
            try
            {
                var temp = int.Parse(ans);
                return temp;
            }
            catch
            {
                Console.WriteLine("Invalid Input.");
                return Read_Intuser_Input(question);
            }
        }
        private static string Read_KeyType_Input(string question)
        {
            List<string> keyTypes = new List<string> { "Pin", "Data" };
            Console.WriteLine(question);
            var ans = Console.ReadLine();
            if (keyTypes.Contains<string>(ans))
                return ans;
            else
            {
                Console.WriteLine("Invalid Input.");
                return Read_KeyType_Input(question);
            }
        }
        private static string Read_KeyDerivationType_Input(string question)
        {
            List<string> keyDerivationTypes = new List<string> { "DUKPT", "Fixed" };
            Console.WriteLine(question);
            var ans = Console.ReadLine();
            if (keyDerivationTypes.Contains<string>(ans))
                return ans;
            else
            {
                Console.WriteLine("Invalid Input.");
                return Read_KeyDerivationType_Input(question);
            }
        }
        private static string Read_String_Input(string question, bool isOptional)
        {
            Console.WriteLine(question);
            var ans = Console.ReadLine();
            if ((!isOptional) && string.IsNullOrWhiteSpace(ans))
            {
                return Read_String_Input(question, isOptional);
            }
            return ans;
        }
        /// <summary>
        /// Accepts large string input, as the default string implemenattion has limitations.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="isOptional"></param>
        /// <returns></returns>
        private static string Read_LongString_Input(string userMessage, bool isOptional)
        {
            Console.WriteLine(userMessage);
            byte[] inputBuffer = new byte[262144];
            Stream inputStream = Console.OpenStandardInput(262144);
            Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));
            string strInput = Console.ReadLine();
            if ((!isOptional) && string.IsNullOrWhiteSpace(strInput))
            {
                return Read_LongString_Input(userMessage, isOptional);
            }
            return strInput;
        }
        /// <summary>
        /// check string is a valid xml or not
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>A boolean output as the xml string is parsed</returns>
        public static bool IsValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Validates the xml string input and returns the formatted xml string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>A formatted string xml</returns>
        public static string PrettyXml(string xml)
        {
            if (IsValidXml(xml)) //print xml in beautiful format
            {
                var stringBuilder = new StringBuilder();
                var element = XElement.Parse(xml);
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }
                return stringBuilder.ToString();
            }
            else
            {
                return xml;
            }
        }
        #endregion

    }
}

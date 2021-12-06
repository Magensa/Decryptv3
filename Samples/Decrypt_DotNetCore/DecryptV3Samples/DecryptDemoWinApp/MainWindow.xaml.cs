using System.Windows;

namespace DecryptDemoWinApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close method shut down/ closes running the Application instance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Open method navigates the window to corresponding selected choice operation window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Open_Click(object sender, RoutedEventArgs e)
        {
            if (rb_DecryptCardSwipe.IsChecked == true)
            {
                var decryptCardSwipe = new DecryptCardSwipe();
                decryptCardSwipe.Show();
                this.Close();
            }
            else if (rb_DecryptData.IsChecked == true)
            {
                var decryptData = new DecryptData();
                decryptData.Show();
                this.Close();
            }
            else if (rb_GenerateMac.IsChecked == true)
            {
                var generateMac = new GenerateMac();
                generateMac.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Select atleast one operation choice to process", "Information");
            }

        }

    }



}

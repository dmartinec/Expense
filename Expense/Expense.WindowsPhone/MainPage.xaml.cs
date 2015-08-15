using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Expense
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StorageFolder expensesFolder;
        string expensesFileName;
        string expenses;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            expensesFolder = Windows.Storage.KnownFolders.PicturesLibrary;
            expensesFileName = "expenses.csv";
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            if (FileExists(await expensesFolder.GetFilesAsync(), expensesFileName))
            {
                StorageFile file = await expensesFolder.GetFileAsync(expensesFileName);
                expenses = await FileIO.ReadTextAsync(file);
                OutputTextBlock.Text = "Loaded from " + expensesFolder.Name + "/" + expensesFileName
                    + ":\n" + expenses;
            }
            else
                OutputTextBlock.Text = "Doesn't exist: " + expensesFolder.Name + "/" + expensesFileName;

            UpdateMileageUI();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await expensesFolder.CreateFileAsync(expensesFileName,
                                    CreationCollisionOption.ReplaceExisting);
            var dateTime = DateTime.Now;
            var category = (string)((ComboBoxItem)CategoriesCombo.SelectedItem).Content;
            var expense = dateTime + "," + ValueText.Text + ","
                + category + "," + CommentText.Text + "," + FuelText.Text + "," + OdometerText.Text + ",";
            // add the new expense to the beginning
            expenses = expense + "\n" + expenses;
            await FileIO.WriteTextAsync(file, expenses);
            OutputTextBlock.Text = "Saved to " + expensesFolder.Name + "/" + file.Name + ":" + "\n" + expenses;

            // clear the (potentially) filled-in text boxes
            ValueText.Text = "";
            CommentText.Text = "";
            ClearMileageTexts();
        }

        private void ClearMileageTexts()
        {
            FuelText.Text = "";
            OdometerText.Text = "";
        }

        private void CategoriesCombo_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (CategoriesCombo != null)
                UpdateMileageUI();
        }

        private void UpdateMileageUI()
        {
            var category = (string)((ComboBoxItem)CategoriesCombo.SelectedItem).Content;
            ClearMileageTextsAndSetVisibility(category == "travel");
        }

        public void ClearMileageTextsAndSetVisibility(bool visible)
        {
            if(!visible)
                ClearMileageTexts();
            SetMileageVisibility(visible);
        }

        public void SetMileageVisibility(bool visible)
        {
            double opacity = visible ? 1 : 0;

            FuelText.Opacity = opacity;
            FuelTextBlock.Opacity = opacity;
            OdometerText.Opacity = opacity;
            OdometerTextBlock.Opacity = opacity;
        }

        public bool FileExists(IReadOnlyList<StorageFile> storageFileList, string fileName)
        {
            StorageFile storageFile = (from StorageFile s in storageFileList
                                       where s.Name == fileName
                                       select s).FirstOrDefault();

            return storageFile != null;
        }
    }
}

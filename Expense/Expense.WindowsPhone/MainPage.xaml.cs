using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
//using System.Windows.Input;
//using System.Text.RegularExpressions;
//using Microsoft.Live;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Expense
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IFolderPickerContinuable, IFileOpenPickerContinuable
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

        /*private void eventhandler(object sender, TextChangedEventArgs e) //TextCompositionChangedEventArgs e) //TextChangedEventArgs e)
        {
            //e.
            //ValueText.Text = "a"; // new Regex("[^0-9]+").IsMatch(e.Text);
        }*/

        // handle for TextChanged event
        //public class NumberTextBox:TextBox 
        //{
            //private string PreviousText;

            /*NumberTextBox()
            {
                PreviousText = Text;
            }*/

            //protected override void OnTextChanged(
            //    TextChangedEventArgs e) 
            /*public void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                var a = new Regex("[^0-9]+").IsMatch(ValueText.Text);
                if (!a)
                    ValueText.Text = PreviousText;
                else
                    PreviousText = ValueText.Text;
                //e.Handled = !AreAllValidNumericChars(e.Text); 
                //base.OnTextChanged(e); 
            }*/

        //}

        /*private void ValueText_TextChanged_1(object sender, TextCompositionChangedEventArgs e)
        {
            ValueText.Text = new Regex("[^0-9]+").IsMatch(ValueText.Text);
            /*int num;
            try
            {
                num = int.Parse(ValueText.Text);
            }
            catch
            {
                if (ValueText.Text.Length > 0)
                {
                    ValueText.Text = ValueText.Text.Remove(ValueText.Text.Length - 1);
                }
            }*//*
        }*/

        /*void MyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        // handle for PreviewTextInput event
        /*public class NumberTextBox:TextBox 
        {

            protected override void OnPreviewTextInput(
                System.Windows.Input.TextCompositionEventArgs e) 
            { 
                e.Handled = !AreAllValidNumericChars(e.Text); 
                base.OnPreviewTextInput(e); 
            } 

        }*/

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
        }

        /*// Signs the user into their Microsoft account.
        private async void SignIn(object sender, RoutedEventArgs e)
        {
            try
            {
                LiveAuthClient auth = new LiveAuthClient();
                LiveLoginResult loginResult =
                    await auth.LoginAsync(new string[] { "wl.signin wl.basic"}); //"wl.basic" });
                if (loginResult.Status == LiveConnectSessionStatus.Connected)
                {
                    this.infoTextBlock.Text = "Signed in.";
                }
            }
            catch (LiveAuthException exception)
            {
                this.infoTextBlock.Text = "Error signing in: "
                   + exception.Message;
            }
        }*/

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            /*LiveConnectSession session = new LiveConnectSession();
            LiveConnectClient client = new LiveConnectClient(session); //LiveConnectSession());

            Task<LiveOperationResult> client. .GetAsync(
                "GET https://apis.live.net/v5.0/me/skydrive?access_token=ACCESS_TOKEN");*/

            /*FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg"); //".jpg"); //".csv");

            // Launch file open picker and caller app is suspended and may be terminated if required 
            openPicker.PickSingleFileAndContinue();*/


            /*var openPicker = new FolderPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;// .DocumentsLibrary;

            // Launch folder open picker and caller app is suspended and may be terminated if required 
            openPicker.PickFolderAndContinue();*/


            // DocumentsLibrary produces an access error (Microsoft strongly discourages its use in apps). Thus,
            // use PicturesLibrary instead

            StorageFile file = await expensesFolder.CreateFileAsync(expensesFileName,
                                    CreationCollisionOption.ReplaceExisting);
            var dateTime = DateTime.Now;
            var category = (string)((ComboBoxItem)CategoriesCombo.SelectedItem).Content;
            var expense = /*a.ToString("dd,MM,yyyy,")*/dateTime + "," + ValueText.Text + ","
                + category + "," + CommentText.Text + "," + FuelText.Text + "," + OdometerText.Text + ",";
            // add the new expense to the beginning
            expenses = expense + "\n" + expenses;
            await FileIO.WriteTextAsync(file, expenses);
            OutputTextBlock.Text = "Saved to " + expensesFolder.Name + "/" + file.Name + ":" + "\n" + expenses;

            // clear the (potentially) filled-in text boxes
            ValueText.Text = "";
            CommentText.Text = "";
            FuelText.Text = "";
            OdometerText.Text = "";

            //string folder = "Documents";

            // Application now has read/write access to all contents in the picked folder (including other sub-folder contents)
            //StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
        }

        public bool FileExists(IReadOnlyList<StorageFile> storageFileList, string fileName)
        {
            StorageFile storageFile = (from StorageFile s in storageFileList
                                       where s.Name == fileName
                                       select s).FirstOrDefault();

            return storageFile != null;
        }

        /// <summary> 
        /// Handle the returned files from file picker 
        /// This method is triggered by ContinuationManager based on ActivationKind 
        /// </summary> 
        /// <param name="args">File open picker continuation activation argment. It cantains the list of files user selected with file open picker </param> 
        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files.Count > 0)
            {
                var file = args.Files[0];
                OutputTextBlock.Text = "Picked expense file: " + file.Name;
                /*StorageFolder folder =
                    await StorageFolder.GetFolderFromPathAsync(file.Path);
                StorageFile fileNew =
                    await folder.CreateFileAsync(file.Name,
                        CreationCollisionOption.ReplaceExisting);*/
                StorageFile fileNew = await StorageFile.GetFileFromPathAsync(file.Path);
                // Do something with the new file.
                Stream x = await fileNew.OpenStreamForWriteAsync();
                string s = "hi";
                char[] a = s.ToCharArray();
                byte[] b = new byte[a.Length];
                for (int i = 0; i < a.Length; ++i)
                    b[i] = (byte)a[i];
                x.Write(b, 0, b.Length); // << "a"; //.Write("Hi {0}");
                await x.FlushAsync();

                /*Windows.Storage.Streams.IOutputStream outputStrm =
                     writeStrm.GetOutputStreamAt(0);

                Windows.Storage.Streams.DataWriter dataWriter =
                     new Windows.Storage.Streams.DataWriter(outputStrm);

                string strTmp = string.Format("Hello World: {0}\n", 12);
                dataWriter.WriteString(strTmp);

                await dataWriter.StoreAsync();
                outputStrm.FlushAsync().Start();*/

            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
        }

        /// <summary> 
        /// Handle the returned folder from folder picker 
        /// This method is triggered by ContinuationManager based on ActivationKind 
        /// </summary> 
        /// <param name="args">Folder open picker continuation activation argment. It contains the list of folder user selected with folder open picker </param> 
        public async void ContinueFolderPicker(FolderPickerContinuationEventArgs args)
        {
            var folder = KnownFolders.PicturesLibrary; // args.Folder;
            if (folder != null)
            {
                OutputTextBlock.Text = "Picked folder: " + folder.Name;
#if true
                StorageFile fileNew = await folder.CreateFileAsync("expensesRecent.csv", //.csv",
                        CreationCollisionOption.ReplaceExisting);
                string s = "Hi,here,";
                await FileIO.WriteTextAsync(fileNew, s);
#else
                StorageFile f = await folder.GetFileAsync("sample.jpg"); //"WP_20150413_002.jpg"); //"sample.jpg");
                /*string fileContent = await FileIO.ReadTextAsync(f);
                StorageFile fileNew = await folder.CreateFileAsync("sample.jpg", //.csv",
                        CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(fileNew, fileContent);
                OutputTextBlock.Text = "In file: " + fileContent;*/
                IBuffer buffer = await FileIO.ReadBufferAsync(f);
                StorageFile fileNew = await folder.CreateFileAsync("sample.jpg", //.csv",
                        CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBufferAsync(fileNew, buffer);
                OutputTextBlock.Text = "In file:";
#endif
            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
        }
    }
}

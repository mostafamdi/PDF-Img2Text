using FileToText_WPF.helpers;
using FileToText_WPF.services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace FileToText_WPF.windows
{
    /// <summary>
    /// Interaction logic for ConvertWin.xaml
    /// </summary>
    public partial class ConvertWin : MahApps.Metro.Controls.MetroWindow
    {
        int numberFiles = 0, counter = 0;
        bool isSelectedFile = false;
        OpenFileDialog ofg;
        List<string> listOfFiles, listLanName;
        ConvertFile convertFile;

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listOfFiles = new List<string>();
            listLanName = new List<string>();
            ofg = new OpenFileDialog();
            ofg.Multiselect = true;
            ofg.Filter = "Pdf Files (*.pdf)|*.pdf";
            fillComboBox();
            loafInfo();
        }
        void loafInfo()
        {
            tesseractDIRTxt.Text = Properties.Settings.Default.TesseractDataDir;
        }
        private void selectFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ofg.ShowDialog() == true)
            {
                listOfFiles.Clear();
                isSelectedFile = true;
                foreach (var path in ofg.FileNames)
                {
                    listOfFiles.Add(path);
                }
            }
        }

        private async void convertFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedFile)
            {
                var resultSelection = Assistant.IsAnyButtonsSelected(pdfFileRadio, imgFileRadio);
                if (resultSelection.success == true)
                {
                    string? language = listCountryNameCombo.SelectedValue.ToString();

                    switch (resultSelection.name)
                    {
                        case "pdfFileRadio":
                            loadingProgress.IsIndeterminate = true;
                            pleaseWaitLbl.Visibility = Visibility.Visible;
                            foreach (var file in listOfFiles)
                            {
                                convertFile = new ConvertFile(file, Properties.Settings.Default.TesseractDataDir, language, "300");
                                convertFileBtn.IsEnabled = false;
                                var resultRun = await Task.Run(() => convertFile.ConvertNormalPdfToText());
                                if (resultRun.Success)
                                {
                                    convertFileBtn.IsEnabled = true;
                                    loadingProgress.IsIndeterminate = false;
                                    pleaseWaitLbl.Visibility = Visibility.Hidden;
                                    MessageBox.Show("Done", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    convertFileBtn.IsEnabled = true;
                                    loadingProgress.IsIndeterminate = false;
                                    pleaseWaitLbl.Visibility = Visibility.Hidden;
                                    MessageBox.Show(resultRun.Message, "Result", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            break;
                        case "imgFileRadio":
                            foreach (var file in listOfFiles)
                            {
                                convertFile = new ConvertFile(file, Properties.Settings.Default.TesseractDataDir, language, "300");
                                var resultImg2Txt = await Task.Run(() => convertFile.ConvertPic2Text(file, language));
                                if (resultImg2Txt.success == true)
                                {
                                    MessageBox.Show("Done!", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show(resultImg2Txt.FileName.First(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Please select the file type", "Notice", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select at least one file", "Notice", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void pdfFileRadio_Click(object sender, RoutedEventArgs e)
        {
            ofg.Filter = "Pdf Files (*.pdf)|*.pdf";
        }

        private void imgFileRadio_Click(object sender, RoutedEventArgs e)
        {
            ofg.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
        }

        private void saveInfo_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TesseractDataDir = tesseractDIRTxt.Text;
            Properties.Settings.Default.Save();
            notifyLbl.Visibility = Visibility.Visible;
        }

        private void settingBtn_Click(object sender, RoutedEventArgs e)
        {
            FlyoutControlSettings.IsOpen = true;
        }

        void fillComboBox()
        {
            var langs = new List<LanguageItem> {
            new LanguageItem { Name = "English", Code = "eng" },
            new LanguageItem { Name = "Arabic",  Code = "ara" },
            new LanguageItem { Name = "German",  Code = "deu" },
            new LanguageItem { Name = "French",  Code = "fra" },
            new LanguageItem { Name = "Spanish", Code = "spa" },
            new LanguageItem { Name = "Russian", Code = "rus" },
            new LanguageItem { Name = "Chinese (Simplified)",  Code = "chi_sim" },
            new LanguageItem { Name = "Chinese (Traditional)", Code = "chi_tra" },
            new LanguageItem { Name = "Japanese", Code = "jpn" },
            new LanguageItem { Name = "Italian", Code = "ita" },
            new LanguageItem { Name = "Irish",  Code = "gle" },
            new LanguageItem { Name = "Croatian",  Code = "hrv" },
            new LanguageItem { Name = "Indonesian",  Code = "ind" },
            new LanguageItem { Name = "Swedish", Code = "swe" },
            new LanguageItem { Name = "Czech", Code = "ces" },
            new LanguageItem { Name = "Ukrainian", Code = "ukr" },
            new LanguageItem { Name = "Korean", Code = "kor" },
            new LanguageItem { Name = "Turkish", Code = "tur" },
            new LanguageItem { Name = "Persian(Farsi)", Code = "fas" }
        };
           // langs.Sort();
            listCountryNameCombo.ItemsSource = langs;
            listCountryNameCombo.DisplayMemberPath = "Name";
            listCountryNameCombo.SelectedValuePath = "Code";
            listCountryNameCombo.SelectedIndex = 0;
        }
        public ConvertWin()
        {
            InitializeComponent();
        }
    }
}

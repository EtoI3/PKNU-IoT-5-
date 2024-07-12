﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using raspiDisplay.Helpers;
using System.Security.Cryptography;

namespace raspiDisplay
{
    public sealed partial class numChoice : Page
    {
        private Button selectedButton = null;
        private string phoneNumber;
        private string selectedButtonNumber;
        private FirestoreHelper firestoreHelper;
        private static Random random = new Random();



        public numChoice()
        {
            this.InitializeComponent();
            firestoreHelper = new FirestoreHelper();
            LoadButtonStates();
        }

        public static string GenerateRandomNumber()
        {
            int randomNumber = random.Next(100000, 999999);
            return randomNumber.ToString();
        }

        private async void ShowMessage(string title, string content)
        {
            var dialog = new MessageDialog(content, title);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            phoneNumber = e.Parameter as string;
        }

        private void backBtn3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(sender));
        }

        private void homeBtn3_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        //async Task<bool> FindBoxNum(string selectedButtonNumber)
        //{
        //    Query qref = db.Collection("Box").WhereEqualTo("BoxNum", selectedButtonNumber);
        //    QuerySnapshot snap = await qref.GetSnapshotAsync();

        //    foreach (DocumentSnapshot docsnap in snap)
        //    {
        //        if (docsnap.Exists)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private async Task LoadButtonStates()
        {
            for (int i = 1; i <= 4; i++)
            {
                string buttonNumber = i.ToString();
                bool isFilled = await firestoreHelper.IsBoxFilledAsync(buttonNumber);

                Button button = FindName($"box{buttonNumber}") as Button;
                if (button != null)
                {
                    if (isFilled)
                    {
                        button.IsEnabled = false;
                        button.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
                    }
                    else
                    {
                        button.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                        button.Click += Button_Click;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            var textBlock = clickedButton.Content as TextBlock;
            if (textBlock != null)
            {
                selectedButtonNumber = textBlock.Text;
            }

            clickedButton.IsEnabled = false;

            if (selectedButton != null && selectedButton != clickedButton)
            {
                selectedButton.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
                selectedButton.IsEnabled = true;
            }

            selectedButton = clickedButton; ;
        }

        private async void SaveDataToFirestore()
        {
            string RandNum = GenerateRandomNumber();
            await firestoreHelper.SaveDataAsync(phoneNumber, selectedButtonNumber, RandNum);
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedButtonNumber))
            {
                SaveDataToFirestore();
                Frame.Navigate(typeof(numCheck2), selectedButtonNumber);
            }
            else
            {
                ShowMessage("오류", "박스 번호를 선택해주세요.");
            }
        }

        
    }
}

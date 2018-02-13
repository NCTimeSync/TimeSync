﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeSync.UI
{
    /// <summary>
    /// Interaction logic for UserAccount.xaml
    /// </summary>
    public partial class UserAccount : Page
    {
        private bool usernameTextBoxInitialFocus = false;
        private bool passwordTextBoxInitialFocus = false;

        public UserAccount()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClickUserAccount(object sender, RoutedEventArgs e)
        {
            UserAccount userAccount = new UserAccount();
            this.NavigationService.Navigate(userAccount);
        }

        private void ButtonBase_OnClickToolkits(object sender, RoutedEventArgs e)
        {
            Toolkits toolkits = new Toolkits();
            this.NavigationService.Navigate(toolkits);
        }

        private void ButtonBase_OnClickTimeregistrations(object sender, RoutedEventArgs e)
        {
            Timeregistrations timeregistrations = new Timeregistrations();
            this.NavigationService.Navigate(timeregistrations);
        }

        private void UsernameRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            if (!usernameTextBoxInitialFocus)
            {
                RichTextBox rbox = sender as RichTextBox;
                rbox.Document.Blocks.Clear();
                usernameTextBoxInitialFocus = true;
            }
        }

        private void PasswordRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!passwordTextBoxInitialFocus)
            {
                RichTextBox rbox = sender as RichTextBox;
                rbox.Document.Blocks.Clear();
                passwordTextBoxInitialFocus = true;
            }
        }

        private void UsernameRichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            RichTextBox rbox = sender as RichTextBox;
            string richText = new TextRange(rbox.Document.ContentStart, rbox.Document.ContentEnd).Text;
            if (string.IsNullOrEmpty(richText))
            {
                Paragraph richParagraph = new Paragraph();
                richParagraph.Inlines.Add(new Run("Initials"));
                richParagraph.FontSize = 12;
                richParagraph.FontStyle = FontStyles.Italic;
                richParagraph.Foreground = Brushes.Gray;
                richParagraph.FontWeight = FontWeights.Light;
                rbox.Document.Blocks.Add(richParagraph);
                usernameTextBoxInitialFocus = false;
            }
        }

        private void PasswordRichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            RichTextBox rbox = sender as RichTextBox;
            string richText = new TextRange(rbox.Document.ContentStart, rbox.Document.ContentEnd).Text;
            if (string.IsNullOrEmpty(richText))
            {
                Paragraph richParagraph = new Paragraph();
                richParagraph.Inlines.Add(new Run("Password"));
                richParagraph.FontSize = 12;
                richParagraph.FontStyle = FontStyles.Italic;
                richParagraph.Foreground = Brushes.Gray;
                richParagraph.FontWeight = FontWeights.Light;
                rbox.Document.Blocks.Add(richParagraph);
                usernameTextBoxInitialFocus = false;
            }
        }
    }
}
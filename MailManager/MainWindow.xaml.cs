using System;
using System.Net;
using System.Net.Mail;
using System.Windows;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace MailManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            SMTP();
            txtBody.Text = "Text";
            txtSubject.Text = "Subject";
            txtTo.Text = "To";

        }

        private void ViewInbox_Click(object sender, RoutedEventArgs e)
        {
            IMAP();
        }

        private void SMTP()
        {
            var client = new SmtpClient("smtp.mail.ru", 587);
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential(txtUsername.Text, txtPassword.Password);
            var message = new MailMessage()
            {
                Body = txtBody.Text,
                Subject = txtSubject.Text
            };

            try
            {
                message.From = new MailAddress(txtUsername.Text);
                message.To.Add(new MailAddress(txtTo.Text));
                client.Send(message);
                MessageBox.Show("Email sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email. Error: " + ex.Message);
            }
        }

        private void IMAP()
        {
            var imapClient = new ImapClient();
            imapClient.Connect("imap.mail.ru", 993, true);
            imapClient.Authenticate(txtUsername.Text, txtPassword.Password);
            var inbox = imapClient.GetFolder("Inbox");
            inbox.Open(MailKit.FolderAccess.ReadOnly);
            var ids = inbox.Search(SearchQuery.All);
            foreach (var id in ids)
            {
                var message = inbox.GetMessage(id);
                lstEmails.Items.Add(new
                {
                    Subject = message.Subject,
                    From = message.From.ToString(),
                    Date = message.Date.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text != "" && txtPassword.Password != "")
            {
                LoginPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Enter mail and password!!!");
            }
        }
    }
}

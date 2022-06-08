using Project.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.UI.Infrastructure
{
    internal class EmailSender
    {
        private UserDTO user;

        public EmailSender(UserDTO user)
        {
            this.user = user;
        }
        public void SendPassword()
        {
            var fromAddress = new MailAddress("alwayswannachocopie@gmail.com", "HospitalProgram");
            var fromPassword = "";
            var toAddress = new MailAddress(user.Email);

            string subject = "Your Password";
            string body = $"Your Password is '{user.Password}'";

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
            MessageBox.Show("Sent");
        }
        public void SendInfoAfterBuyingDrug()
        {
            var fromAddress = new MailAddress("alwayswannachocopie@gmail.com", "HospitalProgram");
            var fromPassword = "";
            var toAddress = new MailAddress(user.Email);

            string subject = "Information";
            string body = $"You can take your drugs in Kyiv, Khreshchatyk 17 st. at our Pharmacy";

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
            MessageBox.Show("Sent");
        }
    }
}

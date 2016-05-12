//The MIT License (MIT)

//Copyright (c) Microsoft Corporation

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SendGrid;
using System.Configuration;
using System.Net.Mail;

namespace WebJobSendEmail
{
    public class Functions
    {
        // Runs once every 30 seconds
        public static void TimerJob([TimerTrigger("00:00:30")] TimerInfo timer,
            [SendGrid(Subject = "Important News", Text = "The timer has fired and this is your notification")] SendGridMessage message)
        {
            try
            {
                List<MailAddress> recipients = new List<MailAddress>() { new MailAddress(ConfigurationManager.AppSettings.Get("recipient")) };

                message.To = recipients.ToArray();

                var webTransport = new SendGrid.Web(ConfigurationManager.AppSettings.Get("AzureWebJobsSendGridApiKey"));

                webTransport.DeliverAsync(message).Wait();

                Console.WriteLine("Success: mail sent to recipient!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}

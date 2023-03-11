using Acme.BookStore.EmailServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Domain.Services;

namespace Acme.BookStore.EmailService
{
    public class EmailService : DomainService
    {
        public EmailService()
        {
            _EmailOptions = new EmailOptions(); ;
        }
        EmailOptions _EmailOptions;

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public async Task<bool> SendEmailAsync(EmailParams @params)
        {
            var host = _EmailOptions.SmtpServer; //指定SMTP服务器
            var port = 25;// 服务器端口
            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient(host, port);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
                                                                   //smtpClient.Host = @params.SmtpServer;

            smtpClient.EnableSsl = true;// SSL 端口使用 465 TSl 使用 25  SmtpClient 只能使用 SSL 加 25
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_EmailOptions.MailFrom, _EmailOptions.UserPassword);//用户名和密码

            // 发送邮件设置       
            MailMessage mailMessage = new MailMessage(_EmailOptions.MailFrom, @params.MailTo); // 发送人和收件人
            mailMessage.Subject = @params.MailSubject;//主题
            mailMessage.Body = @params.MailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级
            try
            {
                await smtpClient.SendMailAsync(mailMessage); // 发送邮件
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }

    }
}

namespace Acme.BookStore.EmailServices
{

    public class EmailOptions
    {
        /// <summary>
        /// smtp 服务器地址
        /// </summary>
        public string SmtpServer { get; set; } = "hwhzimap.qiye.163.com";
        /// <summary>
        /// 发送账号
        /// </summary>
        public string MailFrom { get; set; } = "yangmingke@cxzdh.cn";
        /// <summary>
        /// 账号令牌
        /// </summary>
        public string UserPassword { get; set; } = "DAHHzn1wFGPNMzSw";
    }

    /// <summary>
    /// Email参数
    /// </summary>
    public class EmailParams
    {
        /// <summary>
        /// 目标邮箱
        /// </summary>
        public string MailTo { get; set; } = "yangmingke@cxzdh.cn";
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string MailSubject { get; set; } = "成功了";
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string MailContent { get; set; } = "真的真的真的成功";
    }
}

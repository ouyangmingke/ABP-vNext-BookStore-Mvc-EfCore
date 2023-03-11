using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Identity;

using Xunit;

namespace Acme.BookStore
{
    public class EmailServiceTests : BookStoreDomainTestBase
    {
        private readonly EmailService.EmailService _emailService;

        public EmailServiceTests()
        {
            _emailService = GetRequiredService<EmailService.EmailService>();
        }

        [Fact]
        public async Task SendEmailTest()
        {
            await _emailService.SendEmailAsync(new EmailServices.EmailParams());
        }
    }
}

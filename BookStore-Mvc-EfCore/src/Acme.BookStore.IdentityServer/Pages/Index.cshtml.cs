using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.IdentityServer.Clients;

namespace Acme.BookStore.Pages;

public class IndexModel : AbpPageModel
{
    public List<Client> Clients { get; protected set; }
    
    protected IClientRepository ClientRepository { get; }
    
    public IndexModel(IClientRepository clientRepository)
    {
        this.ClientRepository = clientRepository;
    }

    public async Task OnGetAsync()
    {
        Clients = await ClientRepository.GetListAsync(includeDetails: true);
    }
}
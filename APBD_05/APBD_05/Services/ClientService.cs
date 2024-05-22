using APBD_05.Context;
using Microsoft.AspNetCore.Mvc;

namespace APBD_05.Services;

public class ClientService : IClientService
{
    private readonly ApbdContext _apbdContext;

    public ClientService(ApbdContext apbdContext)
    {
        _apbdContext = apbdContext;
    }

    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var result = _apbdContext.ClientTrips.FirstOrDefault(x => x.IdClient == idClient);
        if (result != null)
        {
            return new BadRequestObjectResult("Trip for a client exists!");

        }

        var client = _apbdContext.Clients.SingleOrDefault(c => c.IdClient == idClient);

        if (client != null)
        {
            _apbdContext.Clients.Remove(client);
            await _apbdContext.SaveChangesAsync();
        }
        return new OkObjectResult("Client deleted!");
    }
    
}


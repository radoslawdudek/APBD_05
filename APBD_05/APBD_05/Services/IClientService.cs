using Microsoft.AspNetCore.Mvc;

namespace APBD_05.Services;

public interface IClientService
{
    Task<IActionResult> DeleteClient(int idClient);
}
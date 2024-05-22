using APBD_05.ModelsDTO;
using APBD_05.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_05.Controllers;

[Route("/api/trips")]
[ApiController]
public class TripController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        var result = await _tripService.GetTrips();
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] ClientTripDto clientTripDto)
    {
        var result = await _tripService.AddClientToTrip(idTrip,clientTripDto);
        return result;
    }
}
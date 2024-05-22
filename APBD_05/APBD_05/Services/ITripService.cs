using APBD_05.ModelsDTO;
using Microsoft.AspNetCore.Mvc;

namespace APBD_05.Services;

public interface ITripService
{
    Task <List<TripDto>> GetTrips();
    Task <IActionResult> AddClientToTrip(int idTrip, ClientTripDto clientTripDto);
}
using APBD_05.Context;
using APBD_05.Models;
using APBD_05.ModelsDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_05.Services;

public class TripService : ITripService
{
    private readonly ApbdContext _apbdContext;

    public TripService(ApbdContext apbdContext)
    {
        _apbdContext = apbdContext;
    }

    public async Task<List <TripDto>> GetTrips()
    {
        var result = new List<TripDto>();
        result = await _apbdContext.Trips
            .Include(t => t.IdCountries)
            .Include(t => t.ClientTrips)
            .OrderByDescending(t => t.DateFrom)
            .Select(c => new TripDto
            {
                name = c.Name,
                description = c.Description,
                dateFrom = c.DateFrom,
                dateTo = c.DateTo,
                maxPeople = c.MaxPeople,
                countries = c.IdCountries.Select(c => new CountryDto
                {
                    name = c.Name
                }),

                clients = c.ClientTrips.Select(c => new ClientDto
                {
                    firstName = c.IdClientNavigation.FirstName,
                    lastName = c.IdClientNavigation.LastName,
                })
            }).ToListAsync();


        return result;

    }

    public async Task <IActionResult> AddClientToTrip(int idTrip, ClientTripDto clientTripDto)
    {
        var peselExist = _apbdContext.Clients.FirstOrDefault(c => c.Pesel == clientTripDto.Pesel);
        if (peselExist == null)
        {

            var newClient = new Client
            {

                IdClient = _apbdContext.Clients.Select(c => c.IdClient).DefaultIfEmpty().Max() + 1,
                FirstName = clientTripDto.FirstName,
                LastName = clientTripDto.LastName,
                Email = clientTripDto.Email,
                Telephone = clientTripDto.Telephone,
                Pesel = clientTripDto.Pesel
                
            };
            await _apbdContext.Clients.AddAsync(newClient);
            await _apbdContext.SaveChangesAsync();
        }


        var clientTrip = _apbdContext.ClientTrips
            .Include(c => c.IdClientNavigation)
            .FirstOrDefault(c => c.IdTrip == clientTripDto.IdTrip && c.IdClientNavigation.Pesel == clientTripDto.Pesel);

        if (clientTrip != null)
        {
            return new BadRequestObjectResult("Client is already signed up for this trip!");
        }


        var trip = _apbdContext.Trips.FirstOrDefault(c => c.IdTrip == clientTripDto.IdTrip);

        if (trip == null)
        {
            return new BadRequestObjectResult("This trip does not exist!");
        }



        var clientId = _apbdContext.Clients.FirstOrDefault(c => c.Pesel == clientTripDto.Pesel);

        if (clientId != null)
        {
            var addClientToTrip = new ClientTrip
            {
                IdClient = clientId.IdClient,
                IdTrip = clientTripDto.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientTripDto.PaymentDate
            };

            _apbdContext.ClientTrips.Add(addClientToTrip);
            await _apbdContext.SaveChangesAsync();
        }
        else
        {
            return new BadRequestObjectResult("There is no client, but there should be!");

        }

        return new OkObjectResult("Client added to trip!");
    }
}
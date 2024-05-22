namespace APBD_05.ModelsDTO;

public class TripDto
{
    public string name { get; set; }
    public string description { get; set; }
    public DateTime dateFrom { get; set; }
    public DateTime dateTo { get; set; }
    public int maxPeople { get; set; }

    public IEnumerable<CountryDto> countries { get; set; }

    public IEnumerable<ClientDto> clients { get; set; }

}
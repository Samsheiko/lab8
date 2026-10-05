using System;

namespace BusStation.Models
{
    [Serializable]
    public class BusRoute
    {
        public int Id { get; set; }

        public string RouteNumber { get; set; }

        public string Destination { get; set; }

        public DateTime DepartureTime { get; set; }

        public int Platform { get; set; }

        public decimal TicketPrice { get; set; }

        public int AvailableSeats { get; set; }

        public BusRoute()
        {
            RouteNumber = string.Empty;
            Destination = string.Empty;
        }

        public BusRoute(
            int id,
            string routeNumber,
            string destination,
            DateTime departureTime,
            int platform,
            decimal ticketPrice,
            int availableSeats)
        {
            Id = id;
            RouteNumber = routeNumber;
            Destination = destination;
            DepartureTime = departureTime;
            Platform = platform;
            TicketPrice = ticketPrice;
            AvailableSeats = availableSeats;
        }

        public override string ToString()
        {
            return $"ID: {Id}\n" +
                   $"Номер рейса: {RouteNumber}\n" +
                   $"Пункт назначения: {Destination}\n" +
                   $"Время отправления: {DepartureTime:dd.MM.yyyy HH:mm}\n" +
                   $"Платформа: {Platform}\n" +
                   $"Стоимость билета: {TicketPrice:F2} руб.\n" +
                   $"Свободных мест: {AvailableSeats}\n";
        }
    }
}

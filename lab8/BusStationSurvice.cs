using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BusStation.Models;
using lab8;

namespace BusStation.Services
{
    public static class BusStationService
    {
        private const string FilePath = "bus_schedule.dat";

        
        public static List<BusRoute> LoadData()
        {
            List<BusRoute> routes = new List<BusRoute>();

            try
            {
                if (!File.Exists(FilePath))
                {
                    return routes;
                }

                using FileStream fileStream = new FileStream(
                    FilePath,
                    FileMode.Open,
                    FileAccess.Read);

                using BinaryReader reader = new BinaryReader(fileStream);

                while (fileStream.Position < fileStream.Length)
                {
                    BusRoute route = new BusRoute
                    {
                        Id = reader.ReadInt32(),
                        RouteNumber = reader.ReadString(),
                        Destination = reader.ReadString(),
                        DepartureTime = DateTime.FromBinary(reader.ReadInt64()),
                        Platform = reader.ReadInt32(),
                        TicketPrice = reader.ReadDecimal(),
                        AvailableSeats = reader.ReadInt32()
                    };

                    routes.Add(route);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
            }

            return routes;
        }

        public static void SaveData(List<BusRoute> routes)
        {
            try
            {
                using FileStream fileStream = new FileStream(
                    FilePath,
                    FileMode.Create,
                    FileAccess.Write);

                using BinaryWriter writer = new BinaryWriter(fileStream);

                foreach (BusRoute route in routes)
                {
                    writer.Write(route.Id);
                    writer.Write(route.RouteNumber);
                    writer.Write(route.Destination);
                    writer.Write(route.DepartureTime.ToBinary());
                    writer.Write(route.Platform);
                    writer.Write(route.TicketPrice);
                    writer.Write(route.AvailableSeats);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения файла: {ex.Message}");
            }
        }

        public static void ShowAll(List<BusRoute> routes)
        {
            if (!routes.Any())
            {
                Console.WriteLine("Расписание пусто.");
                return;
            }

            routes
                .OrderBy(route => route.DepartureTime)
                .ToList()
                .ForEach(route => Console.WriteLine(route));
        }

        public static void AddRoute(List<BusRoute> routes, BusRoute route)
        {
            bool exists = routes.Any(r => r.Id == route.Id);

            if (exists)
            {
                Console.WriteLine("Рейс с таким ID уже существует.");
                return;
            }

            routes.Add(route);
            SaveData(routes);

            Console.WriteLine("Рейс успешно добавлен.");
        }


        public static void RemoveRoute(List<BusRoute> routes, int id)
        {
            BusRoute? route = routes.FirstOrDefault(r => r.Id == id);

            if (route == null)
            {
                Console.WriteLine("Рейс не найден.");
                return;
            }

            routes.Remove(route);
            SaveData(routes);

            Console.WriteLine("Рейс успешно удалён.");
        }

        public static void GetRoutesByDestination(List<BusRoute> routes, string destination)
        {
            var result = routes
                .Where(r => r.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => r.DepartureTime)
                .ToList();

            if (!result.Any())
            {
                Console.WriteLine($"Рейсов до пункта '{destination}' не найдено.");
                return;
            }

            result.ForEach(Console.WriteLine);
        }

        public static void GetAvailableBudgetRoutes(List<BusRoute> routes, decimal maxPrice)
        {
            var result = routes
                .Where(r => r.TicketPrice <= maxPrice && r.AvailableSeats > 0)
                .OrderBy(r => r.TicketPrice)
                .ToList();

            if (!result.Any())
            {
                Console.WriteLine($"Рейсов дешевле {maxPrice:F2} руб. со свободными местами не найдено.");
                return;
            }

            result.ForEach(Console.WriteLine);
        }
        public static void ShowTotalAvailableSeats(List<BusRoute> routes)
        {
            int totalSeats = routes.Sum(r => r.AvailableSeats);
            Console.WriteLine($"Общее количество свободных мест на автостанции: {totalSeats}");
        }

        public static void ShowCheapestRoute(List<BusRoute> routes)
        {
            BusRoute? cheapest = routes
                .OrderBy(r => r.TicketPrice)
                .FirstOrDefault();

            if (cheapest == null)
            {
                Console.WriteLine("Расписание пусто.");
                return;
            }

            Console.WriteLine($"Самый дешевый рейс:\n{cheapest}");
        }
    }
}

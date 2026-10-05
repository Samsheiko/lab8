using System;
using System.Collections.Generic;
using BusStation.Models;
using BusStation.Services;

namespace BusStation
{
    internal class Program
    {
        private static void Main()
        {
            List<BusRoute> routes = BusStationService.LoadData();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("=== РАСПИСАНИЕ АВТОСТАНЦИИ ===");
                Console.WriteLine("1. Просмотр базы данных");
                Console.WriteLine("2. Добавить рейс");
                Console.WriteLine("3. Удалить рейс");
                Console.WriteLine("4. Запрос: Поиск рейсов по направлению (Список)");
                Console.WriteLine("5. Запрос: Бюджетные рейсы со свободными местами (Список)");
                Console.WriteLine("6. Запрос: Общее количество свободных мест (Значение)");
                Console.WriteLine("7. Запрос: Самый дешевый рейс (Значение)");
                Console.WriteLine("8. Выход");
                Console.Write("Выберите пункт меню: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowRoutes(routes);
                        break;

                    case "2":
                        AddRoute(routes);
                        break;

                    case "3":
                        RemoveRoute(routes);
                        break;

                    case "4":
                        SearchByDestination(routes);
                        break;

                    case "5":
                        SearchBudgetRoutes(routes);
                        break;

                    case "6":
                        ShowTotalSeats(routes);
                        break;

                    case "7":
                        ShowCheapest(routes);
                        break;

                    case "8":
                        return;

                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        Pause();
                        break;
                }
            }
        }

        private static void ShowRoutes(List<BusRoute> routes)
        {
            Console.Clear();
            BusStationService.ShowAll(routes);
            Pause();
        }

        private static void AddRoute(List<BusRoute> routes)
        {
            Console.Clear();

            int id = ReadInt("Введите ID: ");

            Console.Write("Введите номер рейса (например, 104-А): ");
            string routeNumber = ReadRequiredString();

            Console.Write("Введите пункт назначения: ");
            string destination = ReadRequiredString();

            DateTime departureTime = ReadDateTime("Введите дату и время отправления (ДД.ММ.ГГГГ ЧЧ:ММ): ");

            int platform = ReadInt("Введите номер платформы: ");

            decimal ticketPrice = ReadDecimal("Введите стоимость билета: ");

            int availableSeats = ReadInt("Введите количество свободных мест: ");

            BusRoute route = new BusRoute(
                id,
                routeNumber,
                destination,
                departureTime,
                platform,
                ticketPrice,
                availableSeats);

            BusStationService.AddRoute(routes, route);

            Pause();
        }

        private static void RemoveRoute(List<BusRoute> routes)
        {
            Console.Clear();

            int id = ReadInt("Введите ID удаляемого рейса: ");

            BusStationService.RemoveRoute(routes, id);

            Pause();
        }

        private static void SearchByDestination(List<BusRoute> routes)
        {
            Console.Clear();
            Console.Write("Введите пункт назначения для поиска: ");
            string destination = ReadRequiredString();

            Console.WriteLine($"\n--- Рейсы до пункта: {destination} ---");
            BusStationService.GetRoutesByDestination(routes, destination);
            Pause();
        }

        private static void SearchBudgetRoutes(List<BusRoute> routes)
        {
            Console.Clear();
            decimal maxPrice = ReadDecimal("Введите максимальную стоимость билета: ");

            Console.WriteLine($"\n--- Доступные рейсы до {maxPrice:F2} руб. ---");
            BusStationService.GetAvailableBudgetRoutes(routes, maxPrice);
            Pause();
        }

        private static void ShowTotalSeats(List<BusRoute> routes)
        {
            Console.Clear();
            BusStationService.ShowTotalAvailableSeats(routes);
            Pause();
        }

        private static void ShowCheapest(List<BusRoute> routes)
        {
            Console.Clear();
            BusStationService.ShowCheapestRoute(routes);
            Pause();
        }

        private static int ReadInt(string message)
        {
            int value;

            while (true)
            {
                Console.Write(message);

                bool success = int.TryParse(Console.ReadLine(), out value);

                if (success && value >= 0)
                {
                    return value;
                }

                Console.WriteLine("Ошибка ввода. Введите неотрицательное целое число.");
            }
        }

        private static decimal ReadDecimal(string message)
        {
            decimal value;

            while (true)
            {
                Console.Write(message);

                bool success = decimal.TryParse(Console.ReadLine(), out value);

                if (success && value >= 0)
                {
                    return value;
                }

                Console.WriteLine("Ошибка ввода. Введите корректную стоимость (положительное число).");
            }
        }

        private static DateTime ReadDateTime(string message)
        {
            DateTime value;

            while (true)
            {
                Console.Write(message);

                bool success = DateTime.TryParse(Console.ReadLine(), out value);

                if (success)
                {
                    return value;
                }

                Console.WriteLine("Ошибка ввода. Введите дату в правильном формате, например: 25.10.2024 14:30");
            }
        }

        private static string ReadRequiredString()
        {
            while (true)
            {
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Строка не может быть пустой.");
            }
        }

        private static void Pause()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}

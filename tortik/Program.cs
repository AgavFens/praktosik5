using System;
using System.Collections.Generic;
using System.IO;

class CakeOrder
{
    private string selectedParameter;
    private string selectedValue;
    private int price;

    public CakeOrder(string parameter, string value, int price)
    {
        selectedParameter = parameter;
        selectedValue = value;
        this.price = price;
    }

    public void SaveToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"Выбрано: {selectedParameter} - {selectedValue} за {price} рублей");
        }
    }

    public int GetPrice()
    {
        return price;
    }

    public string GetSelection()
    {
        return $"{selectedParameter}: {selectedValue}";
    }
}

class Menu
{
    public static void DisplayMenu(List<string> menu, int selectedIndex)
    {
        for (int i = 0; i < menu.Count; i++)
        {
            if (i == selectedIndex)
            {
                Console.Write("--> ");
            }
            Console.WriteLine(menu[i]);
            Console.ResetColor();
        }
    }
}

class Program
{
    static void Main()
    {
        string filePath = @"C:\Users\Agav\Desktop\text.txt";
        List<string> cakeMenu = new List<string>
        {
            "Форма торта",
            "Размер торта",
            "Вкус коржей",
            "Количество коржей",
            "Глазурь",
            "Декор",
            "Конец заказа"
        };

        int selectedCakeIndex = 0;
        int selectedSubMenuIndex = 0;

        Dictionary<string, Dictionary<string, int>> menuPrices = new Dictionary<string, Dictionary<string, int>>
        {
            { "Форма торта", new Dictionary<string, int> { { "Круглый", 150 }, { "Квадратный", 200 }, {"Треугольный", 300} } },
            { "Размер торта", new Dictionary<string, int> { { "Маленький", 300 }, { "Средний", 450 }, { "Большой", 600 } } },
            { "Вкус коржей", new Dictionary<string, int> { { "Ванильный", 50 }, { "Шоколадный", 75 }, { "Миндальный", 100 } } },
            { "Количество коржей", new Dictionary<string, int> { { "Три слоя", 200 }, { "Четыре слоя", 300 }, { "Пять слоев", 400 } } },
            { "Глазурь", new Dictionary<string, int> { { "Классическая сливочная глазурь", 30 }, { "Ганаш из темного шоколада глазурь", 40 }, { "Крем-сырная глазурь", 50 } } },
            { "Декор", new Dictionary<string, int> { { "Свежие ягоды", 20 }, { "Шоколадные стружки", 15 }, { "Цветы и листья", 25 } } }
        };

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int totalPrice = 0;
        string selectedChoices = "";
        List<CakeOrder> orders = new List<CakeOrder>();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в нашу кондитерскую!");

            Console.WriteLine($"Общая цена торта: {totalPrice} рублей");
            Console.WriteLine($"Общий выбор параметров: {selectedChoices}");

            Console.WriteLine("Выберите параметры торта");
            Console.WriteLine("-------------------------------------");

            Menu.DisplayMenu(cakeMenu, selectedCakeIndex);

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.DownArrow && selectedCakeIndex < cakeMenu.Count - 1)
            {
                selectedCakeIndex++;
            }
            else if (key.Key == ConsoleKey.UpArrow && selectedCakeIndex > 0)
            {
                selectedCakeIndex--;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (selectedCakeIndex == cakeMenu.Count - 1)
                {
                    // Заказ завершен
                    Console.WriteLine("\nЗаказ оформлен!");
                    Console.WriteLine($"Общая сумма заказа: {totalPrice} рублей");
                    Console.WriteLine("Заказ сохранен в файл.");
                    foreach (var order in orders)
                    {
                        order.SaveToFile(filePath);
                    }
                    Console.WriteLine("\nДля нового заказа нажмите любую клавишу.");
                    Console.ReadKey();
                    orders.Clear();
                    totalPrice = 0;
                    selectedChoices = "";
                    selectedCakeIndex = 0;
                }
                else
                {
                    string selectedParameter = cakeMenu[selectedCakeIndex];
                    List<string> subMenu = GetSubMenu(selectedParameter);
                    int subMenuSize = subMenu.Count;
                    selectedSubMenuIndex = 0;

                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Добро пожаловать в нашу кондитерскую!");

                        Console.WriteLine($"Общая цена торта: {totalPrice} рублей");
                        Console.WriteLine($"Общий выбор параметров: {selectedChoices}");

                        Console.WriteLine($"Выберите {selectedParameter}");
                        Console.WriteLine("-------------------------------------");

                        Menu.DisplayMenu(subMenu, selectedSubMenuIndex);

                        for (int i = 0; i < subMenuSize; i++)
                        {
                            var price = menuPrices[selectedParameter][subMenu[i]];
                            Console.WriteLine($" - {subMenu[i]} ({price} рублей)");
                        }

                        var key1 = Console.ReadKey();

                        if (key1.Key == ConsoleKey.DownArrow && selectedSubMenuIndex < subMenuSize - 1)
                        {
                            selectedSubMenuIndex++;
                        }
                        else if (key1.Key == ConsoleKey.UpArrow && selectedSubMenuIndex > 0)
                        {
                            selectedSubMenuIndex--;
                        }
                        else if (key1.Key == ConsoleKey.Enter)
                        {
                            string selectedValue = subMenu[selectedSubMenuIndex];
                            int price = menuPrices[selectedParameter][selectedValue];
                            Console.WriteLine($"\nВыбрано: {selectedValue} за {price} рублей");

                            totalPrice += price;

                            selectedChoices += $"{selectedParameter}: {selectedValue} ";
                            CakeOrder order = new CakeOrder(selectedParameter, selectedValue, price);
                            orders.Add(order);

                            Console.WriteLine("\nДля возврата к выбору параметров нажмите любую клавишу.");
                            Console.ReadKey();
                            break;
                        }
                    }
                }
            }
        }
    }

    static List<string> GetSubMenu(string parameter)
    {
        switch (parameter)
        {
            case "Форма торта":
                return new List<string> { "Круглый", "Квадратный", "Треугольный" };
            case "Размер торта":
                return new List<string> { "Маленький", "Средний", "Большой" };
            case "Вкус коржей":
                return new List<string> { "Ванильный", "Шоколадный", "Миндальный" };
            case "Количество коржей":
                return new List<string> { "Три слоя", "Четыре слоя", "Пять слоев" };
            case "Глазурь":
                return new List<string> { "Классическая сливочная глазурь", "Ганаш из темного шоколада глазурь", "Крем-сырная глазурь" };
            case "Декор":
                return new List<string> { "Свежие ягоды", "Шоколадные стружки", "Цветы и листья" };
            default:
                return new List<string>();
        }
    }
}

using System;
using ConsoleTables;
internal class FinancialCalculator
{
    private const int DaysInMonth = 30;
    private const int FillUpsInMonth = 4;
    private const int K = 1000;
    private decimal GymCost = 300;
    private decimal OilCost = 70;
    private decimal ParkingCost = 90;
    private decimal NetCost = 100;

    public void Run()
    {
        // Input data
        decimal salaryUsdt = ReadDecimal("Enter the salary in USDT for the month: ");
        decimal salaryRub = ReadDecimal("Enter the salary in RUB for the month in k: ");
        decimal salaryVnd = ReadDecimal("Enter the salary in VND in month in k: ");
        decimal exchangeRate = ReadDecimal("Enter the USDT to VND exchange rate: ");
        decimal exchangeRub = ReadDecimal("Enter the RUB to VND exchange rate: ");
        decimal foodDayCost = ReadDecimal("Enter the amount in dong you want to spend per day on food in k: ");
        decimal roomCost = ReadDecimal("Enter the cost of your room per month in k: ");
        decimal otherExpenses = ReadDecimal("Enter other expenses in k: ");

        // Calculate totals
        decimal salaryTotal = salaryUsdt * exchangeRate + salaryVnd * K + salaryRub * K * exchangeRub;
        decimal expensesTotal = (foodDayCost * DaysInMonth + GymCost + OilCost * FillUpsInMonth + ParkingCost + roomCost + NetCost + otherExpenses) * K;
        decimal savingsTotal = salaryTotal - expensesTotal;

        // Display results
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Your monthly salary is {salaryTotal:N0} VND");
        Console.WriteLine($"Your expenses per month are {expensesTotal:N0} VND");
        Console.WriteLine($"Your savings per month are {savingsTotal:N0} VND");

        decimal newFoodCost = CalculateFoodCost(salaryTotal, expensesTotal, foodDayCost);
        Console.WriteLine($"To avoid overspending, you should spend no more than {newFoodCost:N0} VND per day on food.");

        // Display data in a table
        var table = new ConsoleTable("Category", "Value (VND)");
        table.AddRow("Food", $"{foodDayCost * K * DaysInMonth:N0}");
        table.AddRow("Gym", $"{GymCost * K:N0}");
        table.AddRow("Net", $"{NetCost * K:N0}");
        table.AddRow("Gasoline", $"{OilCost * K * FillUpsInMonth:N0}");
        table.AddRow("Parking", $"{ParkingCost * K:N0}");
        table.AddRow("Room", $"{roomCost * K:N0}");
        table.AddRow("Other expenses", $"{otherExpenses * K:N0}");

        int freeMoney = savingsTotal > 0 ? (int)(savingsTotal / 2) : 0;
        table.AddRow("Money for other things", $"{freeMoney:N0}");

        table.Write();
        Console.WriteLine("Press any key to exit... \uD83D");
        Console.ReadKey();
    }

    private decimal ReadDecimal(string message)
    {
        decimal value;
        do
        {
            Console.Write(message);
        } while (!decimal.TryParse(Console.ReadLine(), out value));

        return value;
    }

    private decimal CalculateFoodCost(decimal income, decimal expenses, decimal foodCost)
    {
        decimal difference = income - expenses;
        if (difference >= 0) return foodCost;

        int days = (int)Math.Floor(income / foodCost);
        decimal savings = Math.Abs(difference);
        decimal newFoodCost = (income - savings) / days;

        return newFoodCost;
    }
}
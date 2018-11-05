using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

internal static class Program
{
    private static void Main(string[] args)
    {
        var input = GetInput();
        var validTable = ResetAllUnnecessaryNumbers(input);
        var allPaths = GetAllPaths(validTable);
        var result = GetPathWithSum(allPaths);
        Console.WriteLine($"Max sum:  {result.Keys.First()}");
        Console.WriteLine($"Path: {result.Values.First()}");
        Console.ReadKey();
    }

    // Input preparation
    private static int[,] GetInput()
    {
        const string input = @"
                                215
                                192 124
                                117 269 442
                                218 836 347 235
                                320 805 522 417 345
                                229 601 728 835 133 124
                                248 202 277 433 207 263 257
                                359 464 504 528 516 716 871 182
                                461 441 426 656 863 560 380 171 923
                                381 348 573 533 448 632 387 176 975 449
                                223 711 445 645 245 543 931 532 937 541 444
                                330 131 333 928 376 733 017 778 839 168 197 197
                                131 171 522 137 217 224 291 413 528 520 227 229 928
                                223 626 034 683 839 052 627 310 713 999 629 817 410 121
                                924 622 911 233 325 139 721 218 253 223 107 233 230 124 233";

        var inputArray = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        var inputTable = new int[inputArray.Length, inputArray.Length + 1];

        for (var row = 0; row < inputArray.Length; row++)
        {
            var eachCharactersInRow = inputArray[row].ExtractNumber();

            for (var column = 0; column < eachCharactersInRow.Length; column++)
                inputTable[row, column] = eachCharactersInRow[column];
        }

        return inputTable;
    }

    // Extract number from the row
    private static int[] ExtractNumber(this string rows)
    {
        return Regex.Matches(rows, "[0-9]+").Cast<Match>().Select(m => int.Parse(m.Value)).ToArray();
    }

    // Leave only the required numbers in the table (according to the rule)
    private static int[,] ResetAllUnnecessaryNumbers(this int[,] inputTable)
    {
        var length = inputTable.GetLength(0);
        var previous = 0;
        var currentValue = 0;
        var suitablePrevious = 0;

        for (var i = 0; i < length; i++)
        {
            previous = suitablePrevious;

            var previousIndex = i;
            for (var j = 0; j < length; j++)
            {
                currentValue = inputTable[i, j];

                if (currentValue == 0) continue;

                if ((previous.IsEven() && currentValue.IsEven()) || (!previous.IsEven() && !currentValue.IsEven()))
                {
                    inputTable[i, j] = 0;
                    currentValue = 0;
                }

                if (currentValue != 0)
                {
                    suitablePrevious = currentValue;
                }
            }
        }

        return inputTable;
    }

    // Should get all suitable paths (could not figure out how to do this)
    private static List<int>[] GetAllPaths(this int[,] inputTable)
    {
        var tempresult = inputTable;
        var length = inputTable.GetLength(0);
        var root = inputTable[0, 0];
        var result = new Dictionary<int, string>();
        List<int>[] allPaths = new List<int>[5];

        for (var pathIndex = 0; pathIndex < 5; pathIndex++)
        {
            List<int> list = new List<int>();

            for (var i = 0; i < length; i++)
            {
                var listElement = 0;

                for (var j = 0; j < length; j++)
                {
                    listElement = inputTable[i, j];

                    if (listElement == 0)
                        continue;
                    else
                    {
                        list.Add(listElement);
                        break;
                    }
                }
            }

            allPaths[pathIndex] = list;
        }

        return allPaths;
    }

    public static Dictionary<int, string> GetPathWithSum(List<int>[] allPaths)
    {
        var result = new Dictionary<int, string>();
        var previousSum = 0;

        foreach (var path in allPaths)
        {
            var sum = 0;
            var pathStringValue = string.Empty;

            for (var i = 0; i < path.Count; i++)
            {
                sum += path[i];
                pathStringValue += path[i].ToString() + ", ";
            }

            if (sum >= previousSum)
            {
                result.Clear();
                result.Add(sum, pathStringValue.Substring(0, pathStringValue.Length - 2));
                previousSum = sum;
            }
        }

        return result;
    }

    // Check if number is even
    public static bool IsEven(this int number)
    {
        return number % 2 == 0 ? true : false;
    }
}
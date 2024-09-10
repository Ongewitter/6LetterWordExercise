using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var targetLength = 6; // Word length we are looking for

        HashSet<string> lineSet = new(GetInput()); // Performant and gets rid of duplicates: https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-collections-generic-hashset%7Bt%7D
        List<string> validCombinations = GetCombinations(lineSet, targetLength);

        Console.WriteLine($"Found {validCombinations.Count} valid combinations"); // Fun metrics are fun
        WriteAll(validCombinations); // Handle output

    }

    private static string[] GetInput()
    {
        // We copy our file under Assets/input.txt to the build dir when building
        string filePath = Path.Combine(AppContext.BaseDirectory, "Assets", "input.txt");
        return File.ReadAllLines(filePath);
    }
    private static List<string> GetCombinations(HashSet<string> lineSet, int targetLength)
    {
        List<string> result = [];
        HashSet<string> targetWords = lineSet.Where(l => l.Length == targetLength).ToHashSet(); // Remember our target words of targetLength, for comparison
        GetCombinationsRecursive(lineSet, targetWords, "", "", targetLength, result);
        return result;
    }
    private static void GetCombinationsRecursive(HashSet<string> lineSet, HashSet<string> targetWords, string current, string separatedCurrent, int targetLength, List<string> result)
    {
        if (current.Length == targetLength)
        {
            if (lineSet.Contains(current))
            {
                result.Add(separatedCurrent[1..]);  // Omit our starting "+"
            }
            return;
        }

        foreach (var line in lineSet)
        {
            if (current.Length + line.Length <= targetLength)
            {
                var newCurrent = current + line;
                if (targetWords.Any(word => word.StartsWith(newCurrent))) // Only check words that start as expected
                {
                    var newSeparatedCurrent = separatedCurrent + "+" + line; // Has the side-effect of prepending "+" to every hit, handle it later
                    GetCombinationsRecursive(lineSet, targetWords, newCurrent, newSeparatedCurrent, targetLength, result);
                }
            }
        }
    }
    private static void WriteAll(List<string> strings)
    {
        foreach (var s in strings)
        {
            Console.WriteLine(s);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "grades.txt");

        try
        {
            Console.WriteLine("Processing grades...");
            List<double> grades = ReadGradesFromFile(filePath, out int totalGrades, out int invalidGrades);

            if (grades.Count > 0)
            {
                double average = CalculateAverage(grades);
                Console.WriteLine("\nSummary:");
                Console.WriteLine($"Total grades processed: {totalGrades}");
                Console.WriteLine($"Valid grades: {grades.Count}");
                Console.WriteLine($"Average grade: {average:F2}");
            }
            else
            {
                Console.WriteLine("No valid grades found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    static List<double> ReadGradesFromFile(string filePath, out int totalGrades, out int invalidGrades)
    {
        List<double> validGrades = new List<double>();
        totalGrades = 0;
        invalidGrades = 0;

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: The file '{filePath}' was not found.");
            return validGrades;
        }

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    totalGrades++;

                    if (TryParseGrade(line, out double grade))
                    {
                        validGrades.Add(grade);
                    }
                    else
                    {
                        invalidGrades++;
                        Console.WriteLine($"Error on line {lineNumber}: Invalid grade format '{line}'");
                    }
                }
            }
        }
        catch (IOException ioEx)
        {
            Console.WriteLine($"I/O Error: {ioEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while reading file: {ex.Message}");
        }

        return validGrades;
    }

    static bool TryParseGrade(string input, out double grade)
    {
        if (double.TryParse(input, out grade))
        {
            if (grade >= 0 && grade <= 100)
                return true;

            Console.WriteLine($"Error: Grade out of range '{input}' (Skipping)");
        }
        else
        {
            return false;
        }

        grade = 0;
        return false;
    }

    static double CalculateAverage(List<double> grades)
    {
        double sum = 0;
        foreach (double grade in grades)
        {
            sum += grade;
        }
        return sum / grades.Count;
    }
}

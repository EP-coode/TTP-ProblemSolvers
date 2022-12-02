using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProblemSolvers.ProblemLoader;

public record ProblemMetaData
{
    public string PROBLEM_NAME { get; init; }
    public string KNAPSACK_DATA_TYPE { get; init; }
    public int DIMENSION { get; init; }
    public int NUMBER_OF_ITEMS { get; init; }
    public int CAPACITY_OF_KNAPSACK { get; init; }
    public float MIN_SPEED { get; init; }
    public float MAX_SPEED { get; init; }
    public float RENTING_RATIO { get; init; }
    public string EDGE_WEIGHT_TYPE { get; init; }
};

public record Item
{
    public int Index { get; init; }
    public int Profit { get; init; }
    public int Weight { get; init; }
    public int AssignedNodeIndex { get; init; }
};

public record City
{
    public int Index { get; init; }
    public float X { get; init; }
    public float Y { get; init; }
    public Item[] Items { get; init; }
};

public class TravelingThiefProblem
{
    private static Regex metaDataValueFinder = new Regex(@"(.*):\s*(.+)");
    public ProblemMetaData ProblemMetaData { get; set; }
    public City[] Cities { get; set; }
    public Item[] Items { get; set; }

    public void LoadFromFile(string fileSrc, int metadataHeaderLines = 9)
    {
        string[] lines = File.ReadAllLines(fileSrc);

        ProblemMetaData = ParseProblemMetaData(lines, metadataHeaderLines);

        int firstCityLine = metadataHeaderLines + 1;
        int lastCityline = firstCityLine + ProblemMetaData.DIMENSION;

        int firstItemLine = lastCityline + 1;
        int lastItemline = firstItemLine + ProblemMetaData.NUMBER_OF_ITEMS;

        Items = ParseItems(lines[firstItemLine..lastItemline]);
        Cities = ParseNodes(lines[firstCityLine..lastCityline], Items);

    }

    public int[] GetGens()
    {
        return Enumerable.Range(0, Cities.Length).ToArray();
    }

    public double GetCitiesDistnce(int cityIndex1, int cityIndex2)
    {
        City city1 = Cities[cityIndex1];
        City city2 = Cities[cityIndex2];
        return GetCitiesDistnce(city1, city2);
    }

    public static double GetCitiesDistnce(City city1, City city2)
    {
        return Math.Sqrt(Math.Pow(city1.X - city2.X, 2) + Math.Pow(city1.Y - city2.Y, 2));
    }

    private City[] ParseNodes(string[] rawNodesData, Item[] items)
    {
        City[] nodes = new City[rawNodesData.Length];
        char[] whitespaces = new[] { ' ', '\t' };

        for (int i = 0; i < rawNodesData.Length; i++)
        {
            string[] data = rawNodesData[i]
                .Trim()
                .Replace('.', ',')
                .Split(whitespaces);

            var nodeItems = items.Where(item => item.AssignedNodeIndex - 1 == i).ToArray();

            // TODO: add int float parsing error handling
            nodes[i] = new City
            {
                Index = int.Parse(data[0]) - 1,
                X = float.Parse(data[1]),
                Y = float.Parse(data[2]),
                Items = nodeItems
            };

        }

        return nodes;
    }

    private Item[] ParseItems(string[] rawItemsData)
    {
        Item[] items = new Item[rawItemsData.Length];
        char[] whitespaces = new[] { ' ', '\t' };

        for (int i = 0; i < rawItemsData.Length; i++)
        {
            string[] data = rawItemsData[i]
                .Trim()
                .Replace('.', ',')
                .Split(whitespaces);

            // TODO: add int float parsing error handling
            items[i] = new Item
            {
                Index = int.Parse(data[0]) - 1,
                Profit = int.Parse(data[1]),
                Weight = int.Parse(data[2]),
                AssignedNodeIndex = int.Parse(data[3]) - 1,
            };

        }

        return items;
    }


    private ProblemMetaData ParseProblemMetaData(string[] data, int metaDataLinesCount)
    {
        if (data.Length < metaDataLinesCount)
        {
            throw new InvalidOperationException($"File has no proper metadata header. Expected {metaDataLinesCount} linex of metadata but have less than {data.Length}");
        }

        Dictionary<string, string> metaData = new Dictionary<string, string>();

        for (int i = 0; i < metaDataLinesCount; i++)
        {
            var match = metaDataValueFinder.Match(data[i]);
            if (match.Success)
            {
                string propName = match.Groups[1].Value;
                string propValue = match.Groups[2].Value;
                metaData[propName] = propValue;
            }
            else
            {
                throw new Exception($"Syntax error in line {i} of file");
            }
        }

        // TODO: add int float parsing error handling
        string problem_name = metaData["PROBLEM NAME"];
        string knapsack_data_type = metaData["KNAPSACK DATA TYPE"];
        int dimension = int.Parse(metaData["DIMENSION"]);
        int number_of_items = int.Parse(metaData["NUMBER OF ITEMS"]);
        int capacity_of_knapsack = int.Parse(metaData["CAPACITY OF KNAPSACK"]);
        float min_speed = float.Parse(metaData["MIN SPEED"].Replace('.', ','));
        float max_speed = float.Parse(metaData["MAX SPEED"].Replace('.', ','));
        float renting_ratio = float.Parse(metaData["RENTING RATIO"].Replace('.', ','));
        string edge_weight_type = metaData["EDGE_WEIGHT_TYPE"];


        ProblemMetaData problemMeta = new ProblemMetaData
        {
            PROBLEM_NAME = problem_name,
            KNAPSACK_DATA_TYPE = knapsack_data_type,
            DIMENSION = dimension,
            NUMBER_OF_ITEMS = number_of_items,
            CAPACITY_OF_KNAPSACK = capacity_of_knapsack,
            MIN_SPEED = min_speed,
            MAX_SPEED = max_speed,
            RENTING_RATIO = renting_ratio,
            EDGE_WEIGHT_TYPE = edge_weight_type
        };

        return problemMeta;
    }
}
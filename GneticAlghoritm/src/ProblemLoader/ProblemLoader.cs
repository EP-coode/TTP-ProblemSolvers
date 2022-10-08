﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GneticAlghoritm.ProblemLoader;

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

public record Node
{
    public int Index { get; init; }
    public float X { get; init; }
    public float Y { get; init; }
};

class TravellingThiefProblemLoader
{
    private static Regex metaDataValueFinder = new Regex(@"(.*):\s*(.+)");
    public ProblemMetaData ProblemMetaData { get; set; }
    private Node[] cities;
    private Item[] items;

    public void LoadFromFile(string fileSrc, int metadataHeaderLines = 9)
    {
        string[] lines = File.ReadAllLines(fileSrc);

        ProblemMetaData = ParseProblemMetaData(lines, metadataHeaderLines);

        int firstCityLine = metadataHeaderLines + 1;
        int lastCityline = firstCityLine + ProblemMetaData.DIMENSION;

        cities = ParseNodes(lines[firstCityLine..lastCityline]);

        int firstItemLine = lastCityline + 1;
        int lastItemline = firstItemLine + ProblemMetaData.NUMBER_OF_ITEMS;

        items = ParseItems(lines[firstItemLine..lastItemline]);

    }

    private Node[] ParseNodes(string[] rawNodesData)
    {
        Node[] nodes = new Node[rawNodesData.Length];
        char[] whitespaces = new[] { ' ', '\t' };

        for (int i = 0; i < rawNodesData.Length; i++)
        {
            string[] data = rawNodesData[i]
                .Trim()
                .Replace('.', ',')
                .Split(whitespaces);

            // TODO: add int float parsing error handling
            nodes[i] = new Node
            {
                Index = int.Parse(data[0]) - 1,
                X = float.Parse(data[1]),
                Y = float.Parse(data[2]),
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
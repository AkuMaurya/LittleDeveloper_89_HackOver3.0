using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public static class Shuffeling
//{
//    private static System.Random rng = new System.Random();

//    public static void Shuffle<T>(this IList<T> list)
//    {
//        int n = list.Count;
//        while (n > 1)
//        {
//            n--;
//            int k = rng.Next(n + 1);
//            T value = list[k];
//            list[k] = list[n];
//            list[n] = value;
//        }
//    }
//}

//public class MapLocation
//{
//    public int x;
//    public int z;
//    public MapLocation(int _x, int _z)
//    {
//        x = _x;
//        z = _z;
//    }
//}

public class PathDemo : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>()
    {
        new MapLocation(1,0),
        new MapLocation(0,1),
        new MapLocation(-1,0),
        new MapLocation(0,-1)
    };

    public int width = 20;
    public int depth = 20;
    private int scale = 8;
    // public GameObject player;
    public byte[,] map;

    void Start()
    {
        Initialize();
        
        DrawMap();
    }

    private void Initialize()
    {
        map = new byte[width, depth];
        for (int z = 0; z < width; z++)
        {
            for (int x = 0; x < depth; x++)
            {
                map[x, z] = 1;
            }
        }
    }

    public void Generate()
    {
        Generate(Random.Range(1, width - 1), Random.Range(1, depth - 1));
    }
    void Generate(int x, int z)
    {
        if (CountSquareNeighbours(x, z) >= 2) return;
        map[x, z] = 0;
        directions.Shuffle();
        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x + directions[2].x, z + directions[2].z);
        Generate(x + directions[3].x, z + directions[3].z);
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    void DrawMap()
    {
        for (int z = 0; z < width; z++)
        {
            for (int x = 0; x < depth; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject walls = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    walls.transform.localScale = new Vector3(8f, 4f, 8f);
                    walls.transform.position = pos;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Generate();
        }
    }
}

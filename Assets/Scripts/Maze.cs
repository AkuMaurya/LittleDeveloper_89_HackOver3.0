using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shuffeling
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class MapLocation
{
    public int x;
    public int z;
    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

public class Maze : MonoBehaviour
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

    [System.Serializable]
    public struct Module
    {
        public GameObject prefab;// storing prefab
        public Vector3 rotation; // storing rotation value
    }
    public Module verticalPath;
    public Module HorizontalPath;
    public Module TopLeftCorner;
    public Module BottomLeftCorner;
    public Module BottomRightCorner;
    public Module TopRightCorner;
    public Module LeftEnd;
    public Module RightEnd;
    public Module DownEnd;
    public Module UpEnd;
    public Module LeftT;
    public Module RightT;
    public Module UpT;
    public Module DownT;
    public Module Cross;
    //public Module Floor;
    //public Module WallPieceTop;
    //public Module WallPieceBottom;
    //public Module WallPieceLeft;
    //public Module WallPieceRight;
    //public Module Piller;
    //public Module MachineGun;


    public enum PieceType
    {
        vertical_Path,
        Horizontal_Path,
        Corners,
        TopLeft_Corner,
        BottomLeft_Corner,
        TopRight_Corner,
        BottomRight_Corner,
        Left_End,
        Right_End,
        Up_End,
        Down_End,
        Left_T,
        Right_T,
        Up_T,
        Down_T,
        Cross,
        Floors,
        Walls,
        Pillers,
        Rooms

    }
    // public GameObject ply;
    public struct Pieces
    {
        public PieceType piece;
        public GameObject model;
        public Pieces(PieceType pt, GameObject m)
        {
            piece = pt;
            model = m;
        }
    }

    public Pieces[,] piecePlaces;
    public List<MapLocation> locations = new List<MapLocation>();

    void Start(){
        Initialize();
        Generate();
        AddRooms(3,4,6);
        DrawMap();
    }

    private void Initialize()
    {
        map = new byte[width, depth];
        for(int z=0;z<width;z++)
        {
            for(int x = 0;x<depth;x++)
            {
                map[x,z]=1;
            }
        }
    }

    //private void Generate()
    //{
    //    for (int z = 1; z < width - 1; z++)
    //    {
    //        for (int x = 1; x < depth - 1; x++)
    //        {
    //            if (Random.Range(0, 100) > 50)
    //            {
    //                map[x, z] = 0;
    //            }
    //        }
    //    }
    //}

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

    public int CountDialogNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z >= depth - 1) return 5;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        return count;
    }

    public void AddRooms(int count, int minSize, int maxSize)
    {
        for(int c=0; c < count; c++)
        {
            int startX = Random.Range(2 + count, width - 3);
            int startZ = Random.Range(2 + count, depth - 3);
            int roomWidth = Random.Range(minSize, maxSize);
            int roomDepth = Random.Range(minSize, maxSize);

            for(int x = startX;x<width-3 && x<startZ + roomDepth; x++)
            {
                for(int z = startZ; z<depth-3 && z<startZ + roomDepth; z++)
                {
                    map[x, z] = 0;
                }
            }

        }
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
                    walls.transform.localScale = new Vector3(8f, 4f, 7f);
                    walls.transform.position = pos;
                }

                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 }))
                {
                    GameObject obj = Instantiate(LeftEnd.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(LeftEnd.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Left_End;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 }))
                {
                    GameObject obj = Instantiate(RightEnd.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(RightEnd.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Right_End;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 }))
                {
                    GameObject obj = Instantiate(UpEnd.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(UpEnd.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Up_End;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 }))
                {
                    GameObject obj = Instantiate(DownEnd.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(DownEnd.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Down_End;
                    piecePlaces[x, z].model = obj;
                }

                //Horizontal and Vertical
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(verticalPath.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(verticalPath.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.vertical_Path;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(HorizontalPath.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(HorizontalPath.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Horizontal_Path;
                    piecePlaces[x, z].model = obj;
                }

                //Corners
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(TopLeftCorner.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(TopLeftCorner.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Corners;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(BottomLeftCorner.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(BottomLeftCorner.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Corners;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(TopRightCorner.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(TopRightCorner.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Corners;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(BottomRightCorner.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(BottomRightCorner.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Corners;
                    piecePlaces[x, z].model = obj;
                }

                //Tshape
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(LeftT.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(LeftT.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Left_T;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(RightT.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(RightT.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Right_T;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(UpT.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(UpT.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Up_T;
                    piecePlaces[x, z].model = obj;
                }
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(DownT.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(DownT.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Down_T;
                    piecePlaces[x, z].model = obj;
                }

                //Cross
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 }))
                {
                    // Vector3 pos = Vector3()
                    GameObject obj = Instantiate(Cross.prefab, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
                    obj.transform.Rotate(Cross.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Cross;
                    piecePlaces[x, z].model = obj;
                }
            }
        }
    }

    bool Search2D(int c, int r, int[] pattern)
    {
        int count = 0;
        int pos = 0;
        for (int z = 1; z > -2; z--)
        {
            for (int x = -1; x < 2; x++)
            {
                if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)
                    count++;
                pos++;
            }
        }
        return (count == 9);
    }
}

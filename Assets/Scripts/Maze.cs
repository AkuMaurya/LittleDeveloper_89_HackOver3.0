using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// private System.Random rng = new System.Random();
public class MapLocation
{
    public int x;
    public int z;
    public MapLocation(int _x, int _z)
    {
        x=_x;
        z=_z;
    }
}

public class Maze : MonoBehaviour
{
    // public GameObject verticalPath;
    public List<MapLocation> directions = new List<MapLocation>(){
        new MapLocation(1,0),
        new MapLocation(0,1),
        new MapLocation(-1,0),
        new MapLocation(0,-1)
    };

    public int width = 20;
    public int depth = 20;
    public byte[,] map;
    private int scale = 8;
   
    public GameObject player;
    
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
    public Module Floor;
    public Module WallPieceTop;
    public Module WallPieceBottom;
    public Module WallPieceLeft;
    public Module WallPieceRight;
    public Module Piller;
    public Module MachineGun;



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

    void Start()
    {
        
        Initialize();
        Generate();
        AddRooms(3,4,6);
        DrawMap();
        //Instantiate(player,new Vector3(0,10,0),Quaternion.identity);

        if(player != null)
        {
            PlaceFPC();
        }
        placed();
    }
    void placed()
    {
        PlaceObject[] placeObjects = GetComponents<PlaceObject>();
        if (placeObjects.Length > 0)
            foreach (PlaceObject po in placeObjects)
                po.Go();
    }
    void Initialize()
    {
        map = new byte[width,depth];
        piecePlaces = new Pieces[width, depth];
        for(int z=0;z<width;z++)
        {
            for(int x=0;x<depth;x++)
            {
                map[x,z]=1;
            }
        }
    }

    public virtual void PlaceFPC()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 0)
                {
                    player.transform.position = new Vector3(x * scale, 10, z * scale);
                    return;
                }
            }
    }

    public virtual void Generate()
    {
        for(int z=1; z<width-1; z++)
        {
            for(int x = 1; x<depth-1; x++)
            {
                if(Random.Range(0,100)>50)
                {
                    map[x,z]=0;
                    // Debug.Log("done");
                }
            }
        }
    }

    public void AddRooms(int count, int minSize, int maxSize)
    {
        for(int c=0; c<count; c++)
        {
            int startX = Random.Range(2+count,width-3);
            int startZ = Random.Range(2+count,depth-3);
            int roomWidth = Random.Range(minSize, maxSize);
            int roomDepth = Random.Range(minSize, maxSize);

            for(int x=startX;x<width-3 && x<startX + roomWidth; x++)
            {
                for(int z=startZ; z<depth-3 && z<startZ +roomDepth;z++)
                {
                    map[x,z]=0;
                }
            }
        }
    }
    void DrawMap()
    {
        for(int z=0;z<width;z++)
        {
            for(int x=0;x<depth;x++)
            {
                if(map[x,z]==1)
                {
                    // Vector3 pos = new Vector3(x*scale,0,z*scale);
                    // GameObject walls = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    // walls.transform.localScale=new Vector3(8f,4f,7f);
                    // walls.transform.position = pos;
                }
                //DeadEnds
                else if(Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 }))
                {
                    GameObject obj = Instantiate(LeftEnd.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(LeftEnd.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Left_End;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 }))
                {
                    GameObject obj = Instantiate(RightEnd.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(RightEnd.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Right_End;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 }))
                {
                    GameObject obj = Instantiate(UpEnd.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(UpEnd.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Up_End;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 }))
                {
                    GameObject obj = Instantiate(DownEnd.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(DownEnd.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Down_End;
                    piecePlaces[x, z].model = obj;
                }

                //Horizontal and Vertical
                else if(Search2D(x,z, new int[] {5, 0, 5, 1, 0, 1, 5, 0, 5}))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(verticalPath.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(verticalPath.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.vertical_Path;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x,z, new int[] {5, 1, 5, 0, 0, 0, 5, 1, 5}))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(HorizontalPath.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(HorizontalPath.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Horizontal_Path;
                    piecePlaces[x, z].model = obj;
                }

                //Corners
                else if(Search2D(x,z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 }))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(TopLeftCorner.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(TopLeftCorner.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Corners;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x,z, new int[] {1, 0, 5, 5, 0, 1, 5, 1, 5}))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(BottomLeftCorner.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(BottomLeftCorner.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Corners;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x,z, new int[] {5, 1, 5, 1, 0, 0, 5, 0, 1}))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(TopRightCorner.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(TopRightCorner.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Corners;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x,z, new int[] {5, 0, 1, 1, 0, 0, 5, 1, 5}))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(BottomRightCorner.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(BottomRightCorner.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Corners;
                    piecePlaces[x, z].model = obj;
                }

                //Tshape
                else if(Search2D(x,z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 }))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(LeftT.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(LeftT.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Left_T;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x,z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 }))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(RightT.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(RightT.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Right_T;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x,z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 }))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(UpT.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(UpT.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Up_T;
                    piecePlaces[x, z].model = obj;
                }
                else if(Search2D(x,z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 }))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(DownT.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(DownT.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Down_T;
                    piecePlaces[x, z].model = obj;
                }

                //Cross
                else if(Search2D(x,z, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 }))
                {  
                    // Vector3 pos = Vector3()
                    GameObject obj =Instantiate(Cross.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                    obj.transform.Rotate(Cross.rotation);
                    // obj.transform.SetParent(this.gameObject.transform);
                    piecePlaces[x, z].piece = PieceType.Cross;
                    piecePlaces[x, z].model = obj;
                }
                else if (map[x, z] == 0 && (CountSquareNeighbours(x, z) > 1 && CountDiagonalNeighbours(x, z) >= 1 ||
                                            CountSquareNeighbours(x, z) >= 1 && CountDiagonalNeighbours(x, z) > 1))
                {
                        // GameObject Gun;
                        GameObject floor =Instantiate(Floor.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                        // if(map[x+1,z]==1 && map[x,z+1]==1)
                        //     {
                        //         Gun = Instantiate(MachineGun.prefab, new Vector3(x*scale,0,z*scale),Quaternion.identity);
                        //         // Gun.transform.Rotate(MachineGun.rotation);
                        //     }
                        GameObject PillerCorner;
                        
                        // floor.transform.SetParent(this.gameObject.transform);
                        piecePlaces[x, z].piece = PieceType.Rooms;
                        piecePlaces[x, z].model = floor;
                        locateWalls(x,z);
                        if(top)
                        {
                            GameObject wall1 = Instantiate(WallPieceTop.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                            wall1.transform.Rotate(WallPieceTop.rotation);
                            if(map[x+1, z] == 0 && map[x+1, z+1]==0)
                            {
                                PillerCorner = Instantiate(Piller.prefab,new Vector3((x+1)*scale,0,z*scale),Quaternion.identity);
                                // PillerCorner.transform.Rotate(0,90,0);
                            }
                            if(map[x-1, z] == 0 && map[x-1, z+1]==0)
                            {
                                PillerCorner = Instantiate(Piller.prefab,new Vector3((x-1)*scale,0,z*scale),Quaternion.identity);
                                PillerCorner.transform.Rotate(0,90,0);
                            }
                            

                        }
                        if(bottom)
                        {
                            GameObject wall2 = Instantiate(WallPieceBottom.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                            wall2.transform.Rotate(WallPieceBottom.rotation);
                            if(map[x+1, z] == 0 && map[x+1, z-1]==0)
                            {
                                PillerCorner = Instantiate(Piller.prefab,new Vector3((x+1)*scale,0,z*scale),Quaternion.identity);
                                PillerCorner.transform.Rotate(0,-90,0);
                            }
                            if(map[x-1, z] == 0 && map[x-1, z-1]==0)
                            {
                                PillerCorner = Instantiate(Piller.prefab,new Vector3((x-1)*scale,0,z*scale),Quaternion.identity);
                                PillerCorner.transform.Rotate(0,180,0);
                            }
                        }
                        if(left)
                        {
                            GameObject wall3 = Instantiate(WallPieceLeft.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                            wall3.transform.Rotate(WallPieceLeft.rotation);
                            if(map[x, z+1] == 0 && map[x-1, z+1]==0)
                            {
                                PillerCorner = Instantiate(Piller.prefab,new Vector3(x*scale,0,(z+1)*scale),Quaternion.identity);
                                PillerCorner.transform.Rotate(0,-90,0);
                            }
                            if(map[x, z-1] == 0 && map[x-1, z-1]==0)
                            {
                                PillerCorner = Instantiate(Piller.prefab,new Vector3(x*scale,0,(z-1)*scale),Quaternion.identity);
                                // PillerCorner.transform.Rotate(0,-90,0);
                            }
                        }
                        if(right)
                        {
                            GameObject wall4 = Instantiate(WallPieceRight.prefab,new Vector3(x*scale,0,z*scale),Quaternion.identity);
                            wall4.transform.Rotate(WallPieceRight.rotation);
                            if(map[x, z+1] == 0 && map[x+1, z+1]==0)
                            {
                                PillerCorner = Instantiate(Piller.prefab,new Vector3(x*scale,0,(z+1)*scale),Quaternion.identity);
                                PillerCorner.transform.Rotate(0,180,0);
                            }
                            if(map[x, z-1] == 0 && map[x-1, z-1]==0)
                            {
                                PillerCorner = Instantiate(Piller.prefab,new Vector3(x*scale,0,(z-1)*scale),Quaternion.identity);
                                PillerCorner.transform.Rotate(0,90,0);
                            }
                        }
                }

            }
        }
    }

    bool top;
    bool bottom;
    bool right;
    bool left;
    // bool corner;
    // public void locateCorner(int x, int z)
    // {
    //     corner = false;
    //     if(map[x+1,z]==1&&map[z+1,x]==1)
    //     corner = true;

    // }
    public void locateWalls(int x, int z)
    {
        top = false;
        bottom = false;
        right = false;
        left = false;

        if(x<=0 || x>= width-1 || z<=0 || z>= depth-1) return;
        if(map[x,z+1]==1) top = true;
        if(map[x,z-1]==1) bottom = true;
        if(map[x+1,z]==1) right = true;
        if(map[x-1,z]==1) left = true;

    }

    bool Search2D(int c, int r, int[] pattern)
    {
        int count = 0;
        int pos = 0;
        for (int z=1; z>-2;z--)
        {
            for(int x=-1;x<2;x++)
            {
                if(pattern[pos]==map[c+x, r+z] || pattern[pos] ==5)
                    count++;
                pos++;
            }
        }
        return (count == 9);
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if(x<=0 || x>=width-1 || z<=0 || z>=depth-1) return 5;
        if(map[x-1,z]==0) count++;
        if(map[x+1,z]==0) count++;
        if(map[x,z+1]==0) count++;
        if(map[x,z-1]==0) count++;
        return count;
    }

    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        return count;
    }
}

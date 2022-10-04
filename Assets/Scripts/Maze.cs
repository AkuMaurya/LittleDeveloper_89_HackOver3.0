using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{

    public int width = 20;
    public int depth = 20;
    private int scale = 8;
    // public GameObject player;
    public byte[,] map;

    void Start(){
        Initialize();
        Generate();
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

    private void Generate()
    {
        for(int z=1; z<width-1; z++)
        {
            for(int x = 1; x< depth-1;x++)
            {
                if(Random.Range(0,100)>50){
                    map[x,z]=0;
                }
            }
        }
    }

    void DrawMap(){
        for(int z = 0; z<width; z++)
        {
            for(int x = 0;x<depth;x++)
            {
                Vector3 pos = new Vector3(x*scale,0,z*scale);
                GameObject walls = GameObject.CreatePrimitive(PrimitiveType.Cube);
                walls.transform.localScale=new Vector3(8f,4f,7f);
                walls.transform.position = pos;
            }
        }
    }
}

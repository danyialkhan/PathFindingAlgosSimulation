using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class MapData : MonoBehaviour
{

    public int width = 10;
    public int height = 5;

    public TextAsset text;
    public Texture2D textureMap;
    public string resourcePath = "MapData";

    public Color32 openColor = Color.white;
    public Color32 blockedColor = Color.black;
    public Color32 lightTerrainColor = new Color32(124, 194, 78, 255);
    public Color32 mediumTerrainColor = new Color32(252, 255, 52, 255);
    public Color32 heavyTerrainColor = new Color32(255, 129, 12, 255);

    static Dictionary<Color32, NodeType> terrainLookupTable = new Dictionary<Color32, NodeType>();

    public List<string> getMapFromTexture(Texture2D texture)
    {
        List<string> lines = new List<string>();

        if (texture != null)
        {
            for (int y = 0; y < texture.height; y++)
            {
                string line = "";
                for (int x = 0; x < texture.width; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);
                    if (terrainLookupTable.ContainsKey(pixelColor))
                    {
                        NodeType nodeType = terrainLookupTable[pixelColor];
                        int nodeTypeNum = (int)nodeType;
                        line += nodeTypeNum;
                    }
                    else
                    {
                        line += "0";
                    }
                }
                lines.Add(line);
                //Debug.Log(line);
            }
        }
        else
        {
            Debug.LogWarning("MAPDATA GetTextureFromFile Error: Invalid Texture Asset");
        } 

        return lines;
    }

    public void setDimensions(List<string> textLines)
    {
        height = textLines.Count;

        foreach(string line in textLines)
        {
            if(line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    public List<string> GetMapFromTextFile(TextAsset textAsset)
    {
        List<string> lines = new List<string>();

        if(textAsset != null)
        {
            string textData = textAsset.text;
            string[] delimiters = { "\r\n", "\n" };

            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }

        return lines;
    }

    void Awake()
    {
        setupLookupTable();
    }

    void Start()
    {
        string levelName = SceneManager.GetActiveScene().name;
        if (textureMap == null)
        {
            textureMap = Resources.Load(resourcePath + "/" + levelName) as Texture2D;
        }
        if (text == null)
        {
            text = Resources.Load(resourcePath + "/" + levelName) as TextAsset;
        }
    }

    public List<string> GetMapFromTextFile()
    {
        return GetMapFromTextFile(text);
    }

    public int[,] makeMap()
    {
        List<string> lines = new List<string>();

        if(textureMap != null)
        {
            lines = getMapFromTexture(textureMap);
        }
        else
        {
            lines = GetMapFromTextFile();
        }
        setDimensions(lines);

        int[,] map = new int[width, height];
        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)char.GetNumericValue(lines[y][x]);
                }   
            }
        }

        return map;
    }

    void setupLookupTable()
    {
        terrainLookupTable.Add(openColor, NodeType.Open);
        terrainLookupTable.Add(blockedColor, NodeType.Blocked);
        terrainLookupTable.Add(lightTerrainColor, NodeType.LightTerrain);
        terrainLookupTable.Add(mediumTerrainColor, NodeType.MediumTerrain);
        terrainLookupTable.Add(heavyTerrainColor, NodeType.HeavyTerrain);
    }

    public static Color getColorFromNodeType(NodeType nodeType)
    {
        if (terrainLookupTable.ContainsValue(nodeType))
        {
            Color colorKey = terrainLookupTable.FirstOrDefault(x => x.Value == nodeType).Key;
            return colorKey;
        }
        else
        {
            return Color.white;
        }
    }
}

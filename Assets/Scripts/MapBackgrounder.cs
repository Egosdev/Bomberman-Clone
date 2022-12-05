using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBackgrounder : MonoBehaviour
{
    [SerializeField] private Tilemap backgroundMap;
    [SerializeField] private MapRandomizer mapRandomizerScript;
    [SerializeField] private Tile[] backgroundTiles;

    private void Start()
    {
        int row = mapRandomizerScript.row;
        int column = mapRandomizerScript.column;

        for (int i = 0; i < (row + 2); i++)
        {
            for (int j = 0; j < (column + 2); j++)
            {
                if(i % 2 == 0)
                {
                    if (j % 2 == 0)
                        backgroundMap.SetTile(new Vector3Int(j, -i, 0), backgroundTiles[0]);
                    else
                        backgroundMap.SetTile(new Vector3Int(j, -i, 0), backgroundTiles[1]);
                }
                else
                {
                    if (j % 2 == 0)
                        backgroundMap.SetTile(new Vector3Int(j, -i, 0), backgroundTiles[1]);
                    else
                        backgroundMap.SetTile(new Vector3Int(j, -i, 0), backgroundTiles[0]);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapRandomizer : MonoBehaviour
{
    [SerializeField] private Tilemap mainMap;
    public int row;
    public int column;
    [SerializeField] private Bomb BombScript;
    [SerializeField] private GameObject Door;
    [SerializeField] private List<GameObject> UpgradeTypes = new List<GameObject>();
    [SerializeField] private List<GameObject> EnemyTypes = new List<GameObject>();
    public List<GameObject> EnemyCounter = new List<GameObject>();

    private List<Vector3Int> allBlocks = new List<Vector3Int>();
    private List<Vector3Int> tempEnemyBlocks = new List<Vector3Int>();

    void Start()
    {
        for (int i = 0; i < (row + 2); i++)
        {
            for (int j = 0; j < (column + 2); j++) // (0, -1) (1, -1) (2, -1) j = x coord, I think that means column ASfsaEqweSFA
            {
                allBlocks.Add(new Vector3Int(j, -i, 0)); 
                if(i == 0)
                {
                    if (j == 0)
                    {
                        allBlocks.Remove(new Vector3Int(j, -i, 0));
                        mainMap.SetTile(new Vector3Int(j, -i, 0), BombScript.wallTiles[8]);
                    }
                    if (0 < j && j <= (column))
                    {
                        allBlocks.Remove(new Vector3Int(j, -i, 0));
                        mainMap.SetTile(new Vector3Int(j, -i, 0), BombScript.wallTiles[7]);
                    }
                    if (j == (column + 1))
                    {
                        allBlocks.Remove(new Vector3Int(j, -i, 0));
                        mainMap.SetTile(new Vector3Int(j, -i, 0), BombScript.wallTiles[6]);
                    }
                }
                for(int k = 3; k < (row + 1); k += 2)
                {
                    if(i + 1 == k)
                    { 
                        for(int m = 2; m < (column); m += 2)
                        {
                            allBlocks.Remove(new Vector3Int(m, -k+1, 0));
                            mainMap.SetTile(new Vector3Int(m, -k+1, 0), BombScript.wallTiles[4]);
                        }
                    }
                } // middle walls
                if(i == row + 1)
                {
                    if (j == 0)
                    {
                        allBlocks.Remove(new Vector3Int(j, -i, 0));
                        mainMap.SetTile(new Vector3Int(j, -i, 0), BombScript.wallTiles[2]);
                    }
                    if (0 < j && j <= (column))
                    {
                        allBlocks.Remove(new Vector3Int(j, -i, 0));
                        mainMap.SetTile(new Vector3Int(j, -i, 0), BombScript.wallTiles[1]);
                    }
                    if (j == (column + 1))
                    {
                        allBlocks.Remove(new Vector3Int(j, -i, 0));
                        mainMap.SetTile(new Vector3Int(j, -i, 0), BombScript.wallTiles[0]);
                    }
                }
            }
            if(i != 0 && i != (row+1))
            {
                allBlocks.Remove(new Vector3Int(0, -i, 0));
                mainMap.SetTile(new Vector3Int(0, -i, 0), BombScript.wallTiles[5]);

                allBlocks.Remove(new Vector3Int((column + 1), -i, 0));
                mainMap.SetTile(new Vector3Int((column + 1), -i, 0), BombScript.wallTiles[3]);
            } //left and right walls
        }
        allBlocks.Remove(new Vector3Int(1, -1, 0));
        allBlocks.Remove(new Vector3Int(2, -1, 0));
        allBlocks.Remove(new Vector3Int(1, -2, 0));

        //StartCoroutine(SetRemainTiles());
        SetRemainTiles();
    }
    void SetRemainTiles()
    {
        tempEnemyBlocks = new List<Vector3Int>(allBlocks);
        List<Vector3Int> tempBrickBlocks = new List<Vector3Int>();
        foreach (Vector3Int remainBlocks in allBlocks)
        {
            float randomDir = UnityEngine.Random.value;
            if (randomDir < 0.5f)
            {
                mainMap.SetTile(remainBlocks, BombScript.destroyableTile);
                tempEnemyBlocks.Remove(remainBlocks);
                tempBrickBlocks.Add(remainBlocks);
            }
            //yield return new WaitForSeconds(0.005f);
        }

        int randomUpgrade = Random.Range(0, tempBrickBlocks.Count);
        Vector3 centeredUpgrade = BombScript.mainMap.GetCellCenterLocal(tempBrickBlocks[randomUpgrade]) + new Vector3(0, 0, 1);
        Instantiate(UpgradeTypes[Random.Range(0, UpgradeTypes.Count)], centeredUpgrade, Quaternion.identity);
        tempBrickBlocks.Remove(tempBrickBlocks[randomUpgrade]);

        int randomDoor = Random.Range(0, tempBrickBlocks.Count);
        Vector3 centeredDoor = BombScript.mainMap.GetCellCenterLocal(tempBrickBlocks[randomDoor]) + new Vector3(0, 0, 1);
        Instantiate(Door, centeredDoor, Quaternion.identity);
    }
    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            for (int i = 0; i < 1; i++)
            {
                int randomEnemyLocation = Random.Range(0, tempEnemyBlocks.Count);
                Vector3 centeredEnemy = BombScript.mainMap.GetCellCenterLocal(tempEnemyBlocks[randomEnemyLocation]);
                GameObject spawnedEnemy = Instantiate(EnemyTypes[0], centeredEnemy, Quaternion.identity);
                EnemyCounter.Add(spawnedEnemy);
            }
        }
    }

    public void SpawnEnemiesAtDoor(Vector3 doorCoord)
    {
        Vector3Int doorCell = BombScript.mainMap.WorldToCell(doorCoord);
        Vector3 centeredEnemy = BombScript.mainMap.GetCellCenterLocal(doorCell);

        for (int i = 0; i < 4; i++)
        {
            GameObject spawnedEnemy = Instantiate(EnemyTypes[0], centeredEnemy, Quaternion.identity);
            EnemyCounter.Add(spawnedEnemy);
            StartCoroutine(spawnedEnemy.GetComponent<EnemyOne>().makeInvincible(1f));

            switch (i)
            {
                case 0:
                    spawnedEnemy.GetComponent<EnemyOne>().nextDirection = Vector3.up;
                    break;
                case 1:
                    spawnedEnemy.GetComponent<EnemyOne>().nextDirection = Vector3.right;
                    break;
                case 2:
                    spawnedEnemy.GetComponent<EnemyOne>().nextDirection = Vector3.down;
                    break;
                case 3:
                    spawnedEnemy.GetComponent<EnemyOne>().nextDirection = Vector3.left;
                    break;
            }
        }
    }
}
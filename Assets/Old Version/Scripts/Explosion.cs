using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Bomb BombScript;
    private List<GameObject> activeFires = new List<GameObject>();
    private SpriteRenderer bombSprite;
    private Animator bombAnimator;
    public bool isExploded = false;

    private Collider2D bombCollider;
    public float timeRemaining;
    private int fireLength;
    private void Start()
    {
        bombCollider = GetComponent<Collider2D>();
        bombSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        bombAnimator = GetComponent<Animator>();
        BombScript = GameObject.Find("Engine").GetComponent<Bomb>();
        timeRemaining = BombScript.timeRemaining;
        fireLength = BombScript.fireLength;
    }
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;        
        }
        else
        {
            if(isExploded)
            {
                return;
            }
            isExploded = true;
            Explode();
            bombAnimator.enabled = false;
            bombSprite.sprite = null;
        }
    }

    private void CheckRight(Vector3Int tileCoordparam)
    {
        for (int i = 1; i < fireLength; i++)
        {
            if (BombScript.mainMap.GetTile(tileCoordparam + new Vector3Int(i, 0, 0)) != null)
            {
                ClearTile(tileCoordparam + new Vector3Int(i, 0, 0));
                if (CheckIsWall(tileCoordparam + new Vector3Int(i, 0, 0)))
                    return;
                Vector3 centeredTileCoord = BombScript.mainMap.GetCellCenterLocal(tileCoordparam + new Vector3Int(i, 0, 0));
                Instantiate(BombScript.brickPref, centeredTileCoord, Quaternion.identity);
                return;
            }
            //BombScript.mainMap.SetTile(tileCoordparam + new Vector3Int(i, 0, 0), BombScript.fireTiles[2]); //right
            //this.activeFires.Add(tileCoordparam + new Vector3Int(i, 0, 0));

            Vector3 centeredFireCoord = BombScript.mainMap.GetCellCenterLocal(tileCoordparam + new Vector3Int(i, 0, 0));
            if (i == fireLength - 1)
            {
                GameObject cloneFireCenter = Instantiate(BombScript.FireHeadPref, centeredFireCoord, Quaternion.Euler(new Vector3(0, 0, -90)));
                cloneFireCenter.transform.parent = this.gameObject.transform;
                this.activeFires.Add(cloneFireCenter);
                Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);
                if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(i, 0, 0)))
                {
                    cloneFireCenter.GetComponent<Animator>().enabled = false;
                    cloneFireCenter.GetComponent<SpriteRenderer>().sprite = null;
                    return;
                }
            }
            else
            {
                GameObject cloneFireCenter = Instantiate(BombScript.FireTailPref, centeredFireCoord, Quaternion.Euler(new Vector3(0, 0, 90)));
                cloneFireCenter.transform.parent = this.gameObject.transform;
                this.activeFires.Add(cloneFireCenter);
                Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);
                if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(i, 0, 0)))
                {
                    cloneFireCenter.GetComponent<Animator>().enabled = false;
                    cloneFireCenter.GetComponent<SpriteRenderer>().sprite = null;
                    return;
                }
            }
            //if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(i, 0, 0))) return;
        }
    }
    private void CheckLeft(Vector3Int tileCoordparam)
    {
        for (int i = 1; i < fireLength; i++)
        {
            if (BombScript.mainMap.GetTile(tileCoordparam + new Vector3Int(-i, 0, 0)) != null)
            {
                ClearTile(tileCoordparam + new Vector3Int(-i, 0, 0));
                if (CheckIsWall(tileCoordparam + new Vector3Int(-i, 0, 0)))
                    return;
                Vector3 centeredTileCoord = BombScript.mainMap.GetCellCenterLocal(tileCoordparam + new Vector3Int(-i, 0, 0));
                Instantiate(BombScript.brickPref, centeredTileCoord, Quaternion.identity);
                return;
            }
            //BombScript.mainMap.SetTile(tileCoordparam + new Vector3Int(-i, 0, 0), BombScript.fireTiles[2]); //left
            //this.activeFires.Add(tileCoordparam + new Vector3Int(-i, 0, 0));
            
            Vector3 centeredFireCoord = BombScript.mainMap.GetCellCenterLocal(tileCoordparam + new Vector3Int(-i, 0, 0));
            if (i == fireLength-1)
            {
                GameObject cloneFireCenter = Instantiate(BombScript.FireHeadPref, centeredFireCoord, Quaternion.Euler(new Vector3(0, 0, 90)));
                cloneFireCenter.transform.parent = this.gameObject.transform;
                this.activeFires.Add(cloneFireCenter);
                Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);
                if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(-i, 0, 0)))
                {
                    cloneFireCenter.GetComponent<Animator>().enabled = false;
                    cloneFireCenter.GetComponent<SpriteRenderer>().sprite = null;
                    return;
                }
            }
            else
            {
                GameObject cloneFireCenter = Instantiate(BombScript.FireTailPref, centeredFireCoord, Quaternion.Euler(new Vector3(0, 0, 90)));
                cloneFireCenter.transform.parent = this.gameObject.transform;
                this.activeFires.Add(cloneFireCenter);
                Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);
                if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(-i, 0, 0)))
                {
                    cloneFireCenter.GetComponent<Animator>().enabled = false;
                    cloneFireCenter.GetComponent<SpriteRenderer>().sprite = null;
                    return;
                }
            }
            //if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(-i, 0, 0))) return;
        }
    }
    private void CheckUp(Vector3Int tileCoordparam)
    {
        for (int i = 1; i < fireLength; i++)
        {
            if (BombScript.mainMap.GetTile(tileCoordparam + new Vector3Int(0, i, 0)) != null)
            {
                ClearTile(tileCoordparam + new Vector3Int(0, i, 0));
                if (CheckIsWall(tileCoordparam + new Vector3Int(0, i, 0)))
                    return;
                Vector3 centeredTileCoord = BombScript.mainMap.GetCellCenterLocal(tileCoordparam + new Vector3Int(0, i, 0));
                Instantiate(BombScript.brickPref, centeredTileCoord, Quaternion.identity);
                return;
            }
            //BombScript.mainMap.SetTile(tileCoordparam + new Vector3Int(0, i, 0), BombScript.fireTiles[1]); //up
            //this.activeFires.Add(tileCoordparam + new Vector3Int(0, i, 0));

            Vector3 centeredFireCoord = BombScript.mainMap.GetCellCenterLocal(tileCoordparam + new Vector3Int(0, i, 0));
            if (i == fireLength - 1)
            {
                GameObject cloneFireCenter = Instantiate(BombScript.FireHeadPref, centeredFireCoord, Quaternion.identity);
                cloneFireCenter.transform.parent = this.gameObject.transform;
                this.activeFires.Add(cloneFireCenter);
                Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);
                if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(0, i, 0)))
                {
                    cloneFireCenter.GetComponent<Animator>().enabled = false;
                    cloneFireCenter.GetComponent<SpriteRenderer>().sprite = null;
                    return;
                }
            }
            else
            {
                GameObject cloneFireCenter = Instantiate(BombScript.FireTailPref, centeredFireCoord, Quaternion.identity);
                cloneFireCenter.transform.parent = this.gameObject.transform;
                this.activeFires.Add(cloneFireCenter);
                Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);
                if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(0, i, 0)))
                {
                    cloneFireCenter.GetComponent<Animator>().enabled = false;
                    cloneFireCenter.GetComponent<SpriteRenderer>().sprite = null;
                    return;
                }
            }
            //if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(0, i, 0))) return;
        }
    }
    private void CheckDown(Vector3Int tileCoordparam)
    {
        for (int i = 1; i < fireLength; i++)
        {
            if (BombScript.mainMap.GetTile(tileCoordparam + new Vector3Int(0, -i, 0)) != null)
            {
                ClearTile(tileCoordparam + new Vector3Int(0, -i, 0));
                if (CheckIsWall(tileCoordparam + new Vector3Int(0, -i, 0)))
                    return;
                Vector3 centeredTileCoord = BombScript.mainMap.GetCellCenterLocal(tileCoordparam + new Vector3Int(0, -i, 0));
                Instantiate(BombScript.brickPref, centeredTileCoord, Quaternion.identity);
                return;
            }
            //BombScript.mainMap.SetTile(tileCoordparam + new Vector3Int(0, -i, 0), BombScript.fireTiles[1]); //down
            Vector3 centeredFireCoord = BombScript.mainMap.GetCellCenterLocal(tileCoordparam + new Vector3Int(0, -i, 0));
            if (i == fireLength - 1)
            {
                GameObject cloneFireCenter = Instantiate(BombScript.FireHeadPref, centeredFireCoord, Quaternion.Euler(new Vector3(0, 0, 180)));
                cloneFireCenter.transform.parent = this.gameObject.transform;
                this.activeFires.Add(cloneFireCenter);
                Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);
                if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(0, -i, 0)))
                {
                    cloneFireCenter.GetComponent<Animator>().enabled = false;
                    cloneFireCenter.GetComponent<SpriteRenderer>().sprite = null;
                    return;
                }
            }
            else
            {
                GameObject cloneFireCenter = Instantiate(BombScript.FireTailPref, centeredFireCoord, Quaternion.identity);
                cloneFireCenter.transform.parent = this.gameObject.transform;
                this.activeFires.Add(cloneFireCenter);
                Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);
                if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(0, -i, 0)))
                {
                    cloneFireCenter.GetComponent<Animator>().enabled = false;
                    cloneFireCenter.GetComponent<SpriteRenderer>().sprite = null;
                    return;
                }
            }
            //if (BombScript.bombTileCoords.Contains(tileCoordparam + new Vector3Int(0, -i, 0))) return;
        }
    }

    private void Explode()
    {
        Vector3Int tileCoord = BombScript.mainMap.WorldToCell(gameObject.transform.localPosition);
        //BombScript.mainMap.SetTile(tileCoord, BombScript.fireTiles[0]); //0- self 1- vertical 2- horizontal

        Vector3 centeredTileCoord = BombScript.mainMap.GetCellCenterLocal(tileCoord);
        GameObject cloneFireCenter = Instantiate(BombScript.FireCenterPref, centeredTileCoord, Quaternion.identity);
        cloneFireCenter.transform.parent = this.gameObject.transform;
        this.activeFires.Add(cloneFireCenter);
        Physics2D.IgnoreCollision(cloneFireCenter.GetComponent<Collider2D>(), this.bombCollider);

        CheckRight(tileCoord);
        CheckLeft(tileCoord);
        CheckUp(tileCoord);
        CheckDown(tileCoord);
        StartCoroutine(waitFireAnim());
    }
    private void ClearTile(Vector3Int newCoordForClean)
    {
        if (CheckIsWall(newCoordForClean))
            return;
        BombScript.mainMap.SetTile(newCoordForClean, null);
    }

    private bool CheckIsWall(Vector3Int newCoordForClean)
    {
        foreach (Tile i in BombScript.wallTiles)
        {
            if (BombScript.mainMap.GetTile(newCoordForClean) == i)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator waitFireAnim()
    {
        yield return new WaitForSeconds(0.85f);
        Vector3Int currentTileCoord = this.BombScript.mainMap.WorldToCell(gameObject.transform.localPosition);
        BombScript.bombTileCoords.Remove(currentTileCoord);
        Destroy(gameObject);
    }
}

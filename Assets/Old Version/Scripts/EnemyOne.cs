using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyOne : MonoBehaviour
{
    private Rigidbody2D rb;
    private Bomb BombScript;
    [SerializeField] private Vector3 direction;
    public Vector3 nextDirection;

    public float walkSpeed = 4f;
    [SerializeField] private bool isStuck;
    public bool invincible = false;

    void Start()
    {
        BombScript = GameObject.Find("Engine").GetComponent<Bomb>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (nextDirection == null) DefineNextDirection(this.BombScript.mainMap.WorldToCell(gameObject.transform.localPosition));
        direction = nextDirection;
    }
    private void FixedUpdate()
    {
        Vector3Int currentTileCoord = this.BombScript.mainMap.WorldToCell(gameObject.transform.localPosition);

        if (!this.isStuck)
        {
            if (this.walkSpeed != 0)
            {
                rb.MovePosition(transform.position + direction * Time.deltaTime * walkSpeed);
            }
            this.CheckDirectionCanWalkable(currentTileCoord);
        }
        else
        {
            this.DefineNextDirection(currentTileCoord);
        }
        Flip();
    }

    void CheckDirectionCanWalkable(Vector3Int currentTileCoord)
    {
        foreach (Vector3Int eachBombCoord in BombScript.bombTileCoords)
        {
            if (Vector3Int.FloorToInt(currentTileCoord) + direction == eachBombCoord)
            {
                DefineNextDirection(currentTileCoord);
                CheckAtCenter(currentTileCoord);
                return;
            }
        }
        if (this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + Vector3Int.FloorToInt(direction)) != null)
        {
            DefineNextDirection(currentTileCoord);
        }
        CheckAtCenter(currentTileCoord);
    }
    void DefineNextDirection(Vector3Int currentTileCoord)
    {
        List<Vector3> canGo = new List<Vector3>();
        if (this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(1, 0, 0)) == null)
        {
            CheckBomb(currentTileCoord, Vector3Int.right, canGo, Vector3.right);
        }
        if (this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(-1, 0, 0)) == null)
        {
            CheckBomb(currentTileCoord, Vector3Int.left, canGo, Vector3.left);
        }
        if (this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(0, 1, 0)) == null)
        {
            CheckBomb(currentTileCoord, Vector3Int.up, canGo, Vector3.up);
        }
        if (this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(0, -1, 0)) == null)
        {
            CheckBomb(currentTileCoord, Vector3Int.down, canGo, Vector3.down);
        }
        if (canGo.Count == 0)
        {
            this.isStuck = true;
            return;
        }

        this.isStuck = false;
        int randomDir = UnityEngine.Random.Range(0, canGo.Count);
        this.nextDirection = canGo[randomDir];
    }

    void CheckBomb(Vector3Int currentTileCoord, Vector3Int direction, List<Vector3> canGo, Vector3 canGoDirection)
    {
        foreach (Vector3Int eachBombCoord in BombScript.bombTileCoords)
        {
            if (Vector3Int.FloorToInt(currentTileCoord) + direction == eachBombCoord)
            {
                Debug.Log(direction);
                return;
            }
        }
        canGo.Add(canGoDirection);
    }

    void CheckAtCenter(Vector3Int currentTileCoord)
    {
        Vector3 currentPlayerTile = new Vector3((float)decimal.Round((decimal)this.gameObject.transform.localPosition.x, 1, MidpointRounding.AwayFromZero), (float)decimal.Round((decimal)gameObject.transform.localPosition.y, 1, MidpointRounding.AwayFromZero), 0);
        Vector3 currentTileCoordVector3 = this.BombScript.mainMap.GetCellCenterLocal(currentTileCoord);

        if (currentPlayerTile == currentTileCoordVector3)
        {
            this.direction = this.nextDirection;

            if ((this.direction == new Vector3(1, 0, 0)) || (this.direction == new Vector3(-1, 0, 0)))
                this.transform.position = new Vector3(transform.position.x, currentTileCoordVector3.y, 0); //right left
            if ((this.direction == new Vector3(0, 1, 0)) || (this.direction == new Vector3(0, -1, 0)))
                this.transform.position = new Vector3(currentTileCoordVector3.x, transform.position.y, 0); //up down
        }
    }

    public IEnumerator makeInvincible(float howManySec)
    {
        this.invincible = true;
        yield return new WaitForSeconds(howManySec);
        this.invincible = false;
    }

    private void Flip()
    {
        if (direction == Vector3.right)
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (direction == Vector3.left)
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}








//public class EnemyOne : MonoBehaviour
//{
//    private Rigidbody2D rb;
//    private Bomb BombScript;
//    [SerializeField] private Vector3 direction;
//    [SerializeField] private Vector3 nextDirection;

//    public float walkSpeed = 4f;
//    public float checkDirRate = 5f;
//    public bool canPassWall = false;

//    void Start()
//    {
//        BombScript = GameObject.Find("Engine").GetComponent<Bomb>();
//        rb = this.gameObject.GetComponent<Rigidbody2D>();
//        InvokeRepeating("checkDirection", 0f, checkDirRate);
//    }
//    void SetCorrectDirection(Vector3Int currentTileCoord, Vector3 currentTileCoordVector3)
//    {
//        List<Vector3> canGo = new List<Vector3>();
//        if(this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(1, 0, 0)) == null)
//        {
//            canGo.Add(new Vector3(1, 0, 0));
//            //this.nextDirection = new Vector3(1, 0, 0);
//            //this.transform.position = new Vector3(transform.position.x, currentTileCoordVector3.y, 0);
//        }
//        if (this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(-1, 0, 0)) == null)
//        {
//            canGo.Add(new Vector3(-1, 0, 0));
//            //this.nextDirection = new Vector3(-1, 0, 0);
//            //this.transform.position = new Vector3(transform.position.x, currentTileCoordVector3.y, 0);
//        }
//        if (this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(0, 1, 0)) == null)
//        {
//            canGo.Add(new Vector3(0, 1, 0));
//            //this.nextDirection = new Vector3(0, 1, 0);
//            //this.transform.position = new Vector3(currentTileCoordVector3.x, transform.position.y, 0);
//        }
//        if (this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(0, -1, 0)) == null)
//        {
//            canGo.Add(new Vector3(0, -1, 0));
//            //this.nextDirection = new Vector3(0, -1, 0);
//            //this.transform.position = new Vector3(currentTileCoordVector3.x, transform.position.y, 0);
//        }
//        if (canGo.Count == 0) return;
//        int randomDir = UnityEngine.Random.Range(0, canGo.Count);
//        this.nextDirection = canGo[randomDir];
//    }
//    void checkDirection()
//    {
//        float randomDir = UnityEngine.Random.value;
//        if (randomDir < 0.25f)
//        {
//            //Debug.Log("Up");
//            if (this.direction != new Vector3(0, -1, 0))
//                this.nextDirection = new Vector3(0, 1, 0);
//        }
//        else if (randomDir >= 0.25f && randomDir < 0.5f)
//        {
//            //Debug.Log("Right");
//            if (this.direction != new Vector3(-1, 0, 0))
//            {
//                this.nextDirection = new Vector3(1, 0, 0);
//                this.transform.rotation = Quaternion.Euler(0, 0, 0);
//            }
//        }
//        else if (randomDir >= 0.5f && randomDir < 0.75f)
//        {
//            //Debug.Log("Left");
//            if(this.direction != new Vector3(1, 0, 0))
//            {
//                this.nextDirection = new Vector3(-1, 0, 0);
//                this.transform.rotation = Quaternion.Euler(0, 180, 0);
//            }
//        }
//        else
//        {
//            //Debug.Log("Down");
//            if (this.direction != new Vector3(0, 1, 0))
//                this.nextDirection = new Vector3(0, -1, 0);
//        }
//    }

//    private void Update()
//    {
//        if (direction != null && nextDirection != null)
//        {
//            if (walkSpeed != 0)
//            {
//                rb.MovePosition(transform.position + direction * Time.deltaTime * walkSpeed);
//            }
//        }
//        this.checkEnemyAtCenter();
//    }
//    void checkEnemyAtCenter()
//    {
//        Vector3 currentPlayerTile = new Vector3((float)decimal.Round((decimal)this.gameObject.transform.localPosition.x, 1, MidpointRounding.AwayFromZero), (float)decimal.Round((decimal)gameObject.transform.localPosition.y, 1, MidpointRounding.AwayFromZero), 0);
//        Vector3Int currentTileCoord = this.BombScript.mainMap.WorldToCell(gameObject.transform.localPosition);

//        Vector3 currentTileCoordVector3 = this.BombScript.mainMap.GetCellCenterLocal(currentTileCoord);

//        if (this.nextDirection == new Vector3(0, 1, 0) && this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(0, 1, 0)) == null)
//        {
//            if (currentPlayerTile == currentTileCoordVector3)
//            {
//                this.transform.position = new Vector3(currentTileCoordVector3.x, transform.position.y, 0);
//                this.direction = new Vector3(0, 1, 0);
//            }
//        }
//        else if (this.nextDirection == new Vector3(1, 0, 0) && this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(1, 0, 0)) == null)
//        {
//            if (currentPlayerTile == currentTileCoordVector3)
//            {
//                this.transform.position = new Vector3(transform.position.x, currentTileCoordVector3.y, 0);
//                this.direction = new Vector3(1, 0, 0);
//                this.transform.rotation = Quaternion.Euler(0, 0, 0);
//            }
//        }
//        else if (this.nextDirection == new Vector3(-1, 0, 0) && this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(-1, 0, 0)) == null)
//        {
//            if (currentPlayerTile == currentTileCoordVector3)
//            {
//                this.transform.position = new Vector3(transform.position.x, currentTileCoordVector3.y, 0);
//                this.direction = new Vector3(-1, 0, 0);
//                this.transform.rotation = Quaternion.Euler(0, 180, 0);
//            }
//        }
//        else if (this.nextDirection == new Vector3(0, -1, 0) && this.BombScript.mainMap.GetTile(Vector3Int.FloorToInt(currentTileCoord) + new Vector3Int(0, -1, 0)) == null)
//        {
//            if (currentPlayerTile == currentTileCoordVector3)
//            {
//                this.transform.position = new Vector3(currentTileCoordVector3.x, transform.position.y, 0);
//                this.direction = new Vector3(0, -1, 0);
//            }
//        }
//        else
//        {
//            SetCorrectDirection(currentTileCoord, currentTileCoordVector3);
//        }
//    }

//    void OnCollisionEnter2D(Collision2D col)
//    {
//        if (col.gameObject.name == "Objects")
//        {
//            this.Flip();
//        }
//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.gameObject.tag == "Bomb" && collision.gameObject.GetComponent<Explosion>().timeRemaining < 2.9f)
//        {
//            this.direction = this.direction * -1;
//            this.nextDirection = this.direction;

//            if (this.direction == new Vector3(1, 0, 0))
//            {
//                this.transform.rotation = Quaternion.Euler(0, 180, 0);
//            }
//            else if (this.direction == new Vector3(-1, 0, 0))
//            {
//                this.transform.rotation = Quaternion.Euler(0, 0, 0);
//            }
//        }    
//    }

//    public void Flip()
//    {

//        if (this.direction == new Vector3(1, 0, 0)) //if facing right
//        {
//            this.transform.rotation = Quaternion.Euler(0, 180, 0);
//            this.direction = new Vector3(-1, 0, 0);
//        }
//        else if(this.direction == new Vector3(-1, 0, 0))
//        {
//            this.transform.rotation = Quaternion.Euler(0, 0, 0);
//            this.direction = new Vector3(1, 0, 0);
//        }
//        if (this.direction == new Vector3(0, 1, 0)) //up
//        {
//            this.direction = new Vector3(0, -1, 0);
//        }
//        else if (this.direction == new Vector3(0, -1, 0))
//        {
//            this.direction = new Vector3(0, 1, 0);
//        }
//    }
//}
//bool onCollide = false;
//public Vector3 colliderOffset;
//public float gizmosLength = 0.05f;

//private void OnDrawGizmos()
//{
//    Gizmos.color = Color.blue;
//    //Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + direction * gizmosLength);
//    Gizmos.DrawLine(transform.position + colliderOffset, transform.position + direction);

//}

//void Update()
//{
//    rb.MovePosition(transform.position + direction * Time.deltaTime * 3);
//    onCollide = Physics2D.Raycast(this.transform.position + colliderOffset, transform.position + direction, 1f);

//    if (onCollide)
//    {
//        Flip();
//        //direction = new Vector3(-1, 0, 0);
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Vector3 movement;
    private Rigidbody2D rb;
    public float runSpeed;
    public bool invincible = false;
    [SerializeField] private Bomb BombScript;

    [SerializeField] private bool firstRaycast;
    [SerializeField] private bool secondRaycast;
    [SerializeField] private bool failsafeRaycast;

    [SerializeField] private LayerMask mapLayer;

    private void Start()
    {
        //Application.targetFrameRate = 20;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        #region Smooth movement with three raycasts

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            firstRaycast = Physics2D.Raycast(transform.position + new Vector3(0, 0.48f, 0), Vector2.right, 0.6f, mapLayer);
            secondRaycast = Physics2D.Raycast(transform.position + new Vector3(0, -0.48f, 0), Vector2.right, 0.6f, mapLayer);
            failsafeRaycast = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, mapLayer);

            if (firstRaycast && !secondRaycast && !failsafeRaycast)
            {
                movement.y = -1f;
            }
            if (!firstRaycast && secondRaycast && !failsafeRaycast)
            {
                movement.y = 1f;
            }
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            firstRaycast = Physics2D.Raycast(transform.position + new Vector3(0, 0.48f, 0), Vector2.left, 0.6f, mapLayer);
            secondRaycast = Physics2D.Raycast(transform.position + new Vector3(0, -0.48f, 0), Vector2.left, 0.6f, mapLayer);
            failsafeRaycast = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, mapLayer);

            if (firstRaycast && !secondRaycast && !failsafeRaycast)
            {
                movement.y = -1f;
            }
            if (!firstRaycast && secondRaycast && !failsafeRaycast)
            {
                movement.y = 1f;
            }
        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            firstRaycast = Physics2D.Raycast(transform.position + new Vector3(0.48f, 0, 0), Vector2.up, 0.6f, mapLayer);
            secondRaycast = Physics2D.Raycast(transform.position + new Vector3(-0.48f, 0, 0), Vector2.up, 0.6f, mapLayer);
            failsafeRaycast = Physics2D.Raycast(transform.position, Vector2.up, 0.6f, mapLayer);

            if (firstRaycast && !secondRaycast && !failsafeRaycast)
            {
                movement.x = -1f;
            }
            if (!firstRaycast && secondRaycast && !failsafeRaycast)
            {
                movement.x = 1f;
            }
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            firstRaycast = Physics2D.Raycast(transform.position + new Vector3(0.48f, 0, 0), Vector2.down, 0.6f, mapLayer);
            secondRaycast = Physics2D.Raycast(transform.position + new Vector3(-0.48f, 0, 0), Vector2.down, 0.6f, mapLayer);
            failsafeRaycast = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, mapLayer);

            if (firstRaycast && !secondRaycast && !failsafeRaycast)
            {
                movement.x = -1f;
            }
            if (!firstRaycast && secondRaycast && !failsafeRaycast)
            {
                movement.x = 1f;
            }
        }
        #endregion

        if (Input.GetKeyDown("space"))
        {
            PlaceBomb();
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * runSpeed, movement.y * runSpeed);
    }
    void PlaceBomb()
    {
        Vector3Int playerTileCoord = BombScript.mainMap.WorldToCell(gameObject.transform.localPosition);
        Vector3 centeredTileCoord = BombScript.mainMap.GetCellCenterLocal(playerTileCoord);
        foreach (Tile anyWallTile in BombScript.wallTiles)
        {
            if (BombScript.mainMap.GetTile(playerTileCoord) == anyWallTile) return;
        }
        if (BombScript.mainMap.GetTile(playerTileCoord) == BombScript.destroyableTile) return;
        if (BombScript.bombTileCoords.Contains(playerTileCoord)) return;

        BombScript.bombTileCoords.Add(playerTileCoord);
        Instantiate(BombScript.bombPref, centeredTileCoord, Quaternion.identity);
    }
}
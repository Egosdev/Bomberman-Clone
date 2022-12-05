using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
public class Bomb : MonoBehaviour
{
    public Tilemap mainMap;
    public Tile destroyableTile;
    public Tile[] wallTiles;
    public GameObject bombPref;
    public GameObject brickPref;

    public GameObject FireCenterPref;
    public GameObject FireHeadPref;
    public GameObject FireTailPref;

    public float timeRemaining = 3;
    public int fireLength = 2;

    public List<Vector3Int> bombTileCoords = new List<Vector3Int>();

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] [Range(0, 5)] float lerpTime;
    [SerializeField] private float currentScore;
    [SerializeField] private float newScore;

    public void AddScore(float amount)
    {
        newScore += amount;
    }
    private void Update()
    {
        if (currentScore != newScore)
        {
            currentScore = Mathf.Lerp(currentScore, newScore, lerpTime * Time.deltaTime);
            scoreText.text = currentScore.ToString("00000000");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tileCoord = mainMap.WorldToCell(worldPosition);
            Vector3 centeredTileCoord = mainMap.GetCellCenterLocal(tileCoord);
            foreach (Tile anyWallTile in wallTiles)
            {
                if (mainMap.GetTile(tileCoord) == anyWallTile) return;
            }
            if (mainMap.GetTile(tileCoord) == destroyableTile) return;

            CheckClickedObject(centeredTileCoord, tileCoord);
        }
    }
    private void CheckClickedObject(Vector3 centeredTileCoord, Vector3Int tileCoord)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null && hit.transform.tag == "Bomb")
        {
            return;
        }
        bombTileCoords.Add(tileCoord);
        Instantiate(bombPref, centeredTileCoord, Quaternion.identity);
    }
}
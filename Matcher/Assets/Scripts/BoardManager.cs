using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public List<Sprite> colours = new List<Sprite>();  
    public GameObject tile;    
    public int width, height;
    public GameObject restTxt;

    private GameObject[,] tiles;    

    public bool isRefreshing { get; set; }  

    void Start()
    {
        instance = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
        restTxt.SetActive(true);
    }

    private void CreateBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[width, height];     

        float startX = transform.position.x;     
        float startY = transform.position.y;

        Sprite[] prevLeft = new Sprite[height];
        Sprite prevDown = null;

        for (int x = 0; x < width; x++)
        {      
            for (int y = 0; y < height; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
                tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<Sprite> okColour = new List<Sprite>();
                okColour.AddRange(colours);

                okColour.Remove(prevLeft[y]);
                okColour.Remove(prevDown);

                Sprite newSprite = okColour[Random.Range(0, okColour.Count)];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

                prevLeft[y] = newSprite;
                prevDown = newSprite;

            }
        }
    }
    public IEnumerator FindNullTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
                }
            }
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y].GetComponent<Tile>().ClearAllMatches();
            }
        }

    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .03f)
    {
        isRefreshing = true;
        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        int nullCnt = 0;

        for (int y = yStart; y < height; y++)
        {  
            SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null)
            { 
                nullCnt++;
            }
            renderers.Add(render);
        }

        for (int i = 0; i < nullCnt; i++)
        { 
            yield return new WaitForSeconds(shiftDelay);
            for (int k = 0; k < renderers.Count - 1; k++)
            { 
                renderers[k].sprite = renderers[k + 1].sprite;
                renderers[k + 1].sprite = GetNewSprite(x, height - 1);
            }
        }
        isRefreshing = false;
    }

    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleColours = new List<Sprite>();
        possibleColours.AddRange(colours);

        if (x > 0)
        {
            possibleColours.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < width - 1)
        {
            possibleColours.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleColours.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possibleColours[Random.Range(0, possibleColours.Count)];
    }

}

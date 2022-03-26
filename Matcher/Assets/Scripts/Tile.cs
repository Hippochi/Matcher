using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
	private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
	public static Tile prevSelected = null;
	public int matches;

	private SpriteRenderer renderer;
	private bool isSelected = false;
	private bool matchFound = false;


	private Vector2[] adjDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

	void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
	}

	private void Select()
	{
		isSelected = true;
		renderer.color = selectedColor;
		prevSelected = gameObject.GetComponent<Tile>();
		
	}

	private void Deselect()
	{
		isSelected = false;
		renderer.color = Color.white;
		prevSelected = null;
	}

	void OnMouseDown()
    {
		if (renderer.sprite == null || UIManager.instance.isGameOver)
		{
			return;
		}

		if (isSelected)
		{ 
			Deselect();
		}
		else
		{
			if (prevSelected == null)
			{ 
				Select();
			}
			else
			{
				if (GetAllAdjacentTiles().Contains(prevSelected.gameObject))
				{ 
					SwapSprite(prevSelected.renderer);
					prevSelected.ClearAllMatches();

					prevSelected.Deselect();
					ClearAllMatches();

				}
				else
				{ 
					prevSelected.GetComponent<Tile>().Deselect();
					Select();
					
				}
			}

		}
	}

	public void SwapSprite(SpriteRenderer renderer2)
	{ 
		if (renderer.sprite == renderer2.sprite)
		{ 
			return;
		}
		UIManager.instance.moves -= 1;
		Sprite tSprite = renderer2.sprite; 
		renderer2.sprite = renderer.sprite;
		renderer.sprite = tSprite;
		
	}

	private GameObject GetAdjacent(Vector2 castDir)
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);	
		if (hit.collider != null)
		{
			return hit.collider.gameObject;
			
		}
		return null;
	}

	private List<GameObject> GetAllAdjacentTiles()
	{
		List<GameObject> adjTiles = new List<GameObject>();
		for (int i = 0; i < adjDirections.Length; i++)
		{
			adjTiles.Add(GetAdjacent(adjDirections[i]));
		}
		return adjTiles;
	}

	private List<GameObject> FindMatch(Vector2 castDir)
	{ 
		List<GameObject> matchTiles = new List<GameObject>(); 
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir); 
		while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == renderer.sprite)
		{ 
			matchTiles.Add(hit.collider.gameObject);
			hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
		}
		return matchTiles; 
	}

	private void ClearMatch(Vector2[] paths) 
	{
		List<GameObject> matchingTiles = new List<GameObject>(); 
		for (int i = 0; i < paths.Length; i++) 
		{
			matchingTiles.AddRange(FindMatch(paths[i]));
		}
		if (matchingTiles.Count >= matches) 
		{
			for (int i = 0; i < matchingTiles.Count; i++) 
			{
				matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
			}
			matchFound = true; 
		}
	}

	public void ClearAllMatches()
	{
		if (renderer.sprite == null)
			return;

		ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
		ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
		if (matchFound)
		{
			renderer.sprite = null;
			matchFound = false;
			StopCoroutine(BoardManager.instance.FindNullTiles());
			StartCoroutine(BoardManager.instance.FindNullTiles());

			SoundManager.instance.Play();
			UIManager.instance.matches -= 1;
		}
	}

}

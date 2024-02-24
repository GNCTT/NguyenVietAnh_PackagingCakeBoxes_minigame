using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite candySprite;
    [SerializeField] private Sprite frameSprite;
    [SerializeField] private Sprite gifBoxSprite;
    [SerializeField] private Sprite cakeSprite;

    [SerializeField] private ParticleCake particalCakePrefabs;
    private SpriteRenderer spriteRenderer;
    private TileType type;
    private Vector2Int coord;

    public Vector2Int Coord
    {
        get
        {
            return coord;
        }
        set
        {
            coord = value;
        }
    }

    public TileType Type
    {
        get {
            return type;
        }
        set
        {

        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(int type, Vector2Int pos)
    {
        this.type = (TileType)type;
        this.coord = pos;
        switch (this.type)
        {
            case TileType.Candy:
                spriteRenderer.sprite = candySprite;
                break;
            case TileType.Frame:
                spriteRenderer.sprite = frameSprite;
                break;
            case TileType.Giftbox:
                spriteRenderer.sprite = gifBoxSprite;
                break;
            case TileType.Cake:
                spriteRenderer.sprite = cakeSprite;
                break;
        }
    }

    public void MoveTo(Vector2 newPos, Action action = null)
    {

        transform.DOMove(newPos, .2f).OnComplete(()=>
        {
            action?.Invoke();
        });
    }

    public void DestroySelf()
    {
        var partical = Instantiate(particalCakePrefabs, this.transform.position, Quaternion.identity);
        partical.transform.parent = this.transform;
        Destroy(this.gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float magtitude = 0.2f;
    [SerializeField] private Board board;

    private bool isDragging = false;
    private Vector2 posDown;

    [SerializeField] private bool enable = true;
    public bool Enable
    {
        get
        {
            return enable;
        }
        set
        {
            enable = value;
        }
    }
    private void OnMouseDown()
    {
        posDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging && enable)
        {
            var pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var check = pos - posDown;
            var dir = checkDir(check, magtitude);
            if (dir != Vector2.zero)
            {
                isDragging = false;
                if (dir == Vector2.left)
                {
                    board.SlideLeft();
                }
                if (dir == Vector2.right)
                {
                    board.SlideRight();
                }
                if (dir == Vector2.up)
                {
                    board.SlideUp();
                }
                if (dir == Vector2.down)
                {
                    board.SlideDown();
                }
            } else {
                return;
            }
            board.CheckMatch(dir);
            board.DisplayChange();
        }
    }

    private Vector2 checkDir(Vector2 vector, float magtitude)
    {
        var horizontal = Mathf.Abs(vector.x);
        var vertical = Mathf.Abs(vector.y);

        if (horizontal > vertical)
        {
            if (horizontal < magtitude) return Vector2.zero;
            if (vector.x > 0)
            {
                return Vector2.right;
            }
            return Vector2.left;
        } else
        {
            if (vertical < magtitude) return Vector2.zero;
            if (vector.y > 0)
            {
                return Vector2.up;
            }
            return Vector2.down;
        }
    }
}

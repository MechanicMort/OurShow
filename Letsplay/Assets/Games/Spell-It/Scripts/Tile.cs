using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WPM.SpellIt
{
    public class Tile : MonoBehaviour
    {
        public enum Direction { Up, Right, Down, Left, NeighboursCount };

        [SerializeField] public bool m_isWall = false;

        [SerializeField] public Tile[] m_neighbours;

        [SerializeField] Sprite wallPicture;
        [SerializeField] Sprite walkPicture;

        SpriteRenderer m_mySpriteRenderer;

        private void Awake()
        {
            m_mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            m_neighbours = new Tile[(int)Direction.NeighboursCount];
            SetNeighbours();
        }

        private void Start()
        {
            SetCorrectPicture();
        }

        private void SetCorrectPicture()
        {
            if (m_isWall)
            {
                m_mySpriteRenderer.sprite = wallPicture;
            } else
            {
                m_mySpriteRenderer.sprite = walkPicture;
            }
        }


        public bool IsNeighbourInDirection(Direction _direction)
        {
            if (m_neighbours[(int)_direction] != null)
            {
                return true;
            } else
            {
                return false;
            }
        }
        
        public Tile GetNeighbourInDirection(Direction _direction)
        {
            if (m_neighbours[(int)_direction] != null)
            {
                return m_neighbours[(int)_direction];
            } else
            {
                throw new NullReferenceException();
            }
        }

        public void SetNeighbours()
        {

            int layerMask = LayerMask.GetMask("Tiles");

            for (int i=0; i< (int)Direction.NeighboursCount; i++)
            {
                Vector2 t_neighbourPosition;
                switch (i)
                {
                    case 0:
                        t_neighbourPosition = new Vector2(transform.position.x, transform.position.y + 1);
                        break;
                    case 1:
                        t_neighbourPosition = new Vector2(transform.position.x + 1, transform.position.y);
                        break;
                    case 2:
                        t_neighbourPosition = new Vector2(transform.position.x, transform.position.y - 1);
                        break;
                    case 3:
                        t_neighbourPosition = new Vector2(transform.position.x - 1, transform.position.y);
                        break;
                    default:
                        t_neighbourPosition = new Vector2(transform.position.x, transform.position.y);
                        break;
                }

                try
                {
                    RaycastHit2D hit = Physics2D.Raycast(t_neighbourPosition, transform.TransformDirection(Vector3.forward), Mathf.Infinity, layerMask);
                    m_neighbours[i] = hit.collider.gameObject.GetComponent<Tile>();
                } catch
                {
                    m_neighbours[i] = null;
                }
            }
        }
    }
}

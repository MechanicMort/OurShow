using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WPM.SpellIt
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject m_dashPrefab;

        private Tile m_currentTile;
        float m_speed = 4.0f;

        Tile.Direction m_currentDirection;

        private int layerMask;

        [SerializeField] AudioSource m_myAudioSource;
        [SerializeField] AudioClip[] m_walkSFX;

        void Start()
        {
            m_myAudioSource = GetComponent<AudioSource>();
            layerMask = LayerMask.GetMask("Tiles");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.forward), Mathf.Infinity, layerMask);
            m_currentTile = hit.collider.gameObject.GetComponent<Tile>();
            Debug.Log(m_currentTile.transform.position);
        }

        void Update()
        {
            UpdateMovement();
        }

        private void CreateDash(float _angle)
        {
            GameObject t_dash = Instantiate(m_dashPrefab, transform.position, Quaternion.Euler(0f, 0f, _angle));
            Destroy(t_dash, t_dash.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }

        private void PlayWalk()
        {
            m_myAudioSource.clip = m_walkSFX[Random.Range(0, m_walkSFX.Length)];
            m_myAudioSource.Play();
        }

        private void UpdateMovement()
        {
            if (transform.position.x == m_currentTile.transform.position.x && transform.position.y == m_currentTile.transform.position.y)
            {
                if (Input.GetKeyDown("up"))
                {
                    PlayWalk();
                    m_currentDirection = Tile.Direction.Up;
                    CreateDash(270);
                    SetEdgeTile();
                }

                if (Input.GetKeyDown("right"))
                {
                    PlayWalk();
                    m_currentDirection = Tile.Direction.Right;
                    CreateDash(180);
                    SetEdgeTile();
                }

                if (Input.GetKeyDown("down"))
                {
                    PlayWalk();
                    m_currentDirection = Tile.Direction.Down;
                    CreateDash(90);
                    SetEdgeTile();
                }

                if (Input.GetKeyDown("left"))
                {
                    PlayWalk();
                    m_currentDirection = Tile.Direction.Left;
                    CreateDash(0);
                    SetEdgeTile();
                }


            }

            transform.position = Vector3.MoveTowards(transform.position, m_currentTile.transform.position, m_speed * Time.deltaTime);
        }

        private void SetEdgeTile()
        {
            while (m_currentTile.IsNeighbourInDirection(m_currentDirection) && !m_currentTile.GetNeighbourInDirection(m_currentDirection).m_isWall)
            {
                m_currentTile = m_currentTile.GetNeighbourInDirection(m_currentDirection);
            }
        }
    }
}

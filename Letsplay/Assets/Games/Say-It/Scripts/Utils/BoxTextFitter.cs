using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxTextFitter : MonoBehaviour
{
    BoxCollider2D m_myCollider;
    TextMeshPro m_wordTextComponent;
    SpriteRenderer m_backgroundSpriteRenderer;

    [SerializeField] float m_characterWidth = 0.3f;
    [SerializeField] float m_minWordElementSize = 0.5f;
    
    public Vector2 m_textBoxSize;

    private float m_wordMarginX = 0.1f;
    private float m_wordMarginY = 0.1f;

    private void Awake()
    {
        m_myCollider = GetComponent<BoxCollider2D>();
        m_wordTextComponent = GetComponentInChildren<TextMeshPro>();
        m_backgroundSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_textBoxSize = m_backgroundSpriteRenderer.size;

        UpdateBoxSize();
    }

    private void Update()
    {
        UpdateBoxSize();
        UpdateColliderSize();
    }

    public void UpdateBoxSize()
    {
        if (m_wordTextComponent == null) { return; }

        m_textBoxSize.x = 0;
        int l_textLength = 0;

        l_textLength = m_wordTextComponent.text.Length;

        m_textBoxSize.x = m_characterWidth * l_textLength;

        if (m_textBoxSize.x < m_minWordElementSize)
        {
            m_textBoxSize.x = m_minWordElementSize;
        }

        m_backgroundSpriteRenderer.size = m_textBoxSize;
    }

    /// <summary>
    /// Update the size of collider with every frame
    /// </summary>
    public void UpdateColliderSize()
    {
        m_myCollider.size = new Vector2(m_textBoxSize.x + m_wordMarginX, m_textBoxSize.y + m_wordMarginY);
    }
}

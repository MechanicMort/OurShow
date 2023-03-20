using System.Collections;
using UnityEngine;

namespace WPM.UI.Effects
{
    /// <summary>
    /// Class responsible for interface animation
    /// </summary>
    public class PopUpEffect : MonoBehaviour
    {
        /// <summary>
        /// Setting running coroutine to this attribute, when it's null no coroutine is currently running
        /// </summary>
        public IEnumerator m_popUpCoroutine;

        /// <summary>
        /// Base scale of the object
        /// </summary>
        [SerializeField] private Vector3 m_baseScale;
        /// <summary>
        /// Initial scale of the object when is created
        /// </summary>
        private Vector3 m_startingScale = new Vector3(0.0f, 0.0f, 0.0f);
        /// <summary>
        /// Determine how fast window will pop up
        /// </summary>
        [SerializeField] private float m_maximiseSpeed = 2.5f;
        private Vector3 m_maximiseScaleModifier;

        /// <summary>
        /// Set number of pops
        /// </summary>
        [SerializeField]private int m_numberOfPops = 2;
        /// <summary>
        /// Determines how many pops has been done by coroutine
        /// </summary>
        int m_currentPop = 0;

        /// <summary>
        /// Determine how fast scale of the window will be decreasing
        /// </summary>
        private bool m_isAtTopLimit = false;
        /// <summary>
        /// Determine the limit of how big will be the object at the first pop, when will start initially increasing it's scale
        /// </summary>
        [SerializeField] private float m_maximiseTopLimit = 1.3f;
        /// <summary>
        /// Determine the limit of how small will be the object after first pop, when will start decrasing it's scale
        /// </summary>
        [SerializeField] private float m_maximiseBotLimit = 0.8f;
        /// <summary>
        /// Determine how much limits will be decreased with every pop
        /// </summary>
        [SerializeField] private float m_maximiseLimitModifier = 0.25f;

        /// <summary>
        /// Determine how fast scale of the window will be decreasing
        /// </summary>
        [SerializeField] private float m_minimiseSpeed = 2.5f;
        private Vector3 m_minimiseScaleModifier;

        /// <summary>
        /// Set if window have effect when is created
        /// </summary>
        [SerializeField] private bool m_maximiseAtStart = true;


        /// <summary>
        /// Called while object is created to define initial attributes, before any other method is called
        /// </summary>
        void Start()
        {
            m_baseScale = transform.localScale;
            transform.localScale = m_startingScale;
            m_maximiseScaleModifier = new Vector3(m_maximiseSpeed, m_maximiseSpeed, m_maximiseSpeed);
            m_minimiseScaleModifier = new Vector3(m_minimiseSpeed, m_minimiseSpeed, m_minimiseSpeed);

            if (m_maximiseAtStart) { MaximiseWindow(); }
        }

        private void OnDisable()
        {

 
        }
    
        /// <summary>
        /// Giving a command to start maximise effect and setting up object as active
        /// </summary>
        public void MaximiseWindow()
        {
            gameObject.SetActive(true);
            if (gameObject.GetComponent<SpriteRenderer>() != null)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            m_popUpCoroutine = MaximiseEffect();
            StartCoroutine(m_popUpCoroutine);
        }

        /// <summary>
        /// Coroutine responsible for increasing and decreasing the scale of the object over time, while the window is maximised
        /// </summary>
        private IEnumerator MaximiseEffect()
        {
            yield return new WaitForSeconds(0.1f);

            while (m_currentPop != m_numberOfPops)
            {
                if (!m_isAtTopLimit)
                {
                    if (transform.localScale.y < m_maximiseTopLimit)
                    {
                        transform.localScale += m_maximiseScaleModifier * Time.deltaTime;
                        yield return null;
                    }
                    else
                    {
                        m_maximiseTopLimit -= m_maximiseLimitModifier;
                        m_isAtTopLimit = true;
                        m_currentPop++;
                    }
                }
                else
                {
                    if (transform.localScale.y > m_maximiseBotLimit)
                    {
                        transform.localScale -= m_maximiseScaleModifier * Time.deltaTime;
                        yield return null;
                    }
                    else
                    {
                        m_maximiseBotLimit += m_maximiseLimitModifier;
                        m_isAtTopLimit = false;
                        m_currentPop++;
                    }
                }
            }
            //else
        
            while (m_currentPop == m_numberOfPops) {


                if (transform.localScale.y > 0.9999f)
                {
                    transform.localScale = m_baseScale;
                    ResetMaximiseAttributes();
                }
                else
                {
                    if (transform.localScale.y < 0.9999f)
                    {
                        transform.localScale += m_maximiseScaleModifier * Time.deltaTime;
                        yield return null;
                    }
                    else
                    {
                        transform.localScale -= m_maximiseScaleModifier * Time.deltaTime;
                        yield return null;
                    }
                }
            }

            m_popUpCoroutine = null;
        }

        /// <summary>
        /// Coroutine responsible for decreasing the scale of the object over time
        /// </summary>
        private IEnumerator MinimiseEffect()
        {
            while (transform.localScale.x > 0.0f)
            {
                transform.localScale -= m_minimiseScaleModifier * Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
            m_popUpCoroutine = null;
        }

        /// <summary>
        /// Coroutine responsible for decreasing the scale of the object over time
        /// </summary>
        private IEnumerator MinimiseEffectSpriteDisabled()
        {
            while (transform.localScale.x > 0.0f)
            {
                transform.localScale -= m_minimiseScaleModifier * Time.deltaTime;
                yield return null;
            }

            if (gameObject.GetComponent<SpriteRenderer>() != null)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }else if (gameObject.GetComponent<ImageLoader>() != null)
            {
                gameObject.GetComponent<ImageLoader>().enabled = false;
            }

            m_popUpCoroutine = null;
        }

        /// <summary>
        /// Giving a command to start minimise effect and setting up object as inactive
        /// </summary>
        public void MinimiseWindow()
        {
            m_popUpCoroutine = MinimiseEffect();
            StartCoroutine(m_popUpCoroutine);
        }

        /// <summary>
        /// Giving a command to start minimise effect and setting up object as inactive
        /// </summary>
        public void MinimiseWindowSpriteDisabled()
        {
            m_popUpCoroutine = MinimiseEffectSpriteDisabled();
            StartCoroutine(m_popUpCoroutine);
        }

        /// <summary>
        /// Reset modified pop limit attributes after window is maximized
        /// </summary>
        private void ResetMaximiseAttributes()
        {
            m_maximiseTopLimit = 1.3f;
            m_maximiseBotLimit = 0.8f;
            m_currentPop = 0;
        }
    }
}
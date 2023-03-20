using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace WPM.SayIt.Core
{
    public class ObjectDragger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private TextMeshPro m_myTextMeshPro;
        private WordObject m_myWordObject;
        [SerializeField]
        private WordContainer m_myWordContainer;
        private WordController m_myWordController;
        [SerializeField]
        private SpeechBubbleController m_speechBubbleController;
        private Vector3 m_slotPosition;

        private int currentSlot = 0;

        [SerializeField]
        private float m_characterPullSpeed = 0.06f;

        private float m_inactiveRemainingTime = 30.0f;
        [SerializeField]
        private float m_initialInactivityLength = 15.0f;
        [SerializeField]
        private float m_inactivityLength = 10.0f;
        [SerializeField] 
        private float m_activePulseTime = 3.0f;

        private bool m_isDraggingObject = false;

        [SerializeField] 
        UnityEvent m_wordDroppedCorrectly;
        [SerializeField] 
        UnityEvent m_wordDroppedOnWrongSlot;
        [SerializeField]
        UnityEvent m_phraseFinished;
        [SerializeField]
        UnityEvent m_userInactive;

        RaycastHit2D m_draggedObjectHit;

        private void Awake()
        {
            m_myWordController = FindObjectOfType<WordController>(); // TODO change this later
        }

        private void Start()
        {
            m_inactiveRemainingTime = m_initialInactivityLength;
        }

        private void Update()
        {
            OnClick();
            if (m_inactiveRemainingTime > 0.0f)
            {
                m_inactiveRemainingTime -= Time.deltaTime;

            } else
            {
                m_userInactive.Invoke();
                PulseEffect();
            }
        }


        private void PulseEffect()
        {
            m_inactiveRemainingTime = m_activePulseTime;
        }

        private void ResetInactivityTimer()
        {
            m_inactiveRemainingTime = m_inactivityLength;
        }

        private void ResetInitialInactivityTimer()
        {
            m_inactiveRemainingTime = m_initialInactivityLength;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!m_myWordController.IsGameReady()) return;

            // Does the ray intersect any objects excluding the player layer
            if (m_draggedObjectHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.TransformDirection(Vector3.forward), Mathf.Infinity))
            {
                if (m_draggedObjectHit.collider.gameObject.tag == "WordSlot" &&
                    m_draggedObjectHit.collider.gameObject.GetComponent<WordObject>().GetSlotType() == SlotType.Container)
                {
                    m_isDraggingObject = true;
                    m_slotPosition = m_draggedObjectHit.collider.transform.position;
                    m_myTextMeshPro = m_draggedObjectHit.collider.GetComponentInChildren<TextMeshPro>();
                    m_myWordObject = m_draggedObjectHit.collider.GetComponent<WordObject>();
                    m_draggedObjectHit.collider.GetComponent<Rigidbody2D>().isKinematic = true; // TODO fix this later
                }
            }
        }

        
        public void OnDrag(PointerEventData eventData)
        {
            if (!m_isDraggingObject) return;

            Vector2 tempposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_myTextMeshPro.transform.position = new Vector3(tempposition.x, tempposition.y, m_draggedObjectHit.transform.position.z);
            m_myTextMeshPro.sortingOrder = 15;
        }

        public void OnClick()
        {
                    
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {

                RaycastHit2D l_hit;
                //check against slots as if clicked using m-wordslots need to maybe store last word done then invoke word dropped correctly
                if (l_hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.TransformDirection(Vector3.forward), Mathf.Infinity))
                {
                    m_myWordObject = l_hit.transform.GetComponent<WordObject>();
                    print(m_myWordObject.name);
                    if (m_myWordObject.IsCorrectID(m_myWordContainer.m_wordSlots[currentSlot].GetComponent<WordObject>()))    
                    {
                        m_myWordController.DisableHint();
                        m_myWordObject.SwapSlots(m_myWordController.GetActiveSpeechBubble().m_wordSlots[currentSlot].GetComponent<WordObject>());
                        SpeechBubble t_myActiveSpeechBubble = m_myWordController.GetActiveSpeechBubble();   
                        
                      
                        if (t_myActiveSpeechBubble.UpdateSlots())
                        {
                            m_wordDroppedCorrectly.Invoke();
                            ResetInactivityTimer();
                            currentSlot += 1;

                        }
                        else
                        {
                            currentSlot = 0;
                            m_phraseFinished.Invoke();
                            ResetInitialInactivityTimer();
                        }

                    }
                }
            }

       
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!m_isDraggingObject) return;

            RaycastHit2D l_hit;
            // Does the ray intersect any objects excluding the player layer
            if (l_hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.TransformDirection(Vector3.forward), Mathf.Infinity))
            {
                if (l_hit.collider.gameObject.tag == "WordSlot")
                {
                    WordObject t_otherWordObject = l_hit.collider.gameObject.GetComponent<WordObject>();
                    if (t_otherWordObject.GetSlotType() == SlotType.SpeechBubble)
                    {
                        if (m_myWordObject.IsCorrectID(t_otherWordObject))
                        {
                            m_myWordController.DisableHint();
                            m_myWordObject.SwapSlots(t_otherWordObject);

                            

                            SpeechBubble t_myActiveSpeechBubble = m_myWordController.GetActiveSpeechBubble();

                            if (t_myActiveSpeechBubble.UpdateSlots())
                            {
                                m_wordDroppedCorrectly.Invoke();
                                ResetInactivityTimer();
                                currentSlot += 1;
                            }
                            else
                            {
                                currentSlot = 0;
                                m_phraseFinished.Invoke();
                                ResetInitialInactivityTimer();
                            }

                            m_myTextMeshPro.transform.position = m_slotPosition;
                        }
                        else
                        {
                            //Wrong slot
                            m_wordDroppedOnWrongSlot.Invoke();
                            StartCoroutine("PathToPreviousPosition");
                        }
                    }
                    else
                    {
                        m_myWordObject.SwapSlots(t_otherWordObject);
                        StartCoroutine("PathToPreviousPosition");
                    }
                }
                else
                {

                    // It's not a slot object
                    StartCoroutine("PathToPreviousPosition");
                }
            }
            else
            {
                // An object wasn't found
                StartCoroutine("PathToPreviousPosition");
            }

            m_myTextMeshPro.sortingOrder = 10;
            m_isDraggingObject = false;
        }

        IEnumerator PathToPreviousPosition()
        {
            while (0.1f < Vector3.Distance(m_myTextMeshPro.transform.position, m_slotPosition))
            {
                m_myTextMeshPro.transform.position = Vector3.Lerp(m_myTextMeshPro.transform.position, m_slotPosition, m_characterPullSpeed);
                yield return null;
            }

            m_myTextMeshPro.transform.position = m_slotPosition;
            //this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
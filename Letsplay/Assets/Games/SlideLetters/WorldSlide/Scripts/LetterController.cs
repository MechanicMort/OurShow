using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class LetterController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI _letterText;

    [HideInInspector] public int CurrentColumn;

    private Action<float, LetterController> _onLetterMove;
    private Action<int, Transform> _onLetterMoveEnd;

    [SerializeField] protected RectTransform _myRectTransform;
    [SerializeField] protected CanvasGroup _myCanvasGroup;
    private Transform _myTransform;
    [SerializeField] private float _maxSpeed;
    private bool _holdLetter;

    [HideInInspector] public bool CancelLetterMovementTask;

    public void Initialize(string word, Action<float, LetterController> onLetterMove, Action<int, Transform> onLetterMoveEnd)
    {
        _maxSpeed = UnityEngine.Random.Range(_maxSpeed - 25, _maxSpeed);
        _myTransform = transform;
        _myRectTransform = GetComponent<RectTransform>();
        _letterText.text = word;
        _onLetterMove = onLetterMove;
        _onLetterMoveEnd = onLetterMoveEnd;
    }

    public async Task MoveLetterToTargetPosition(float targetY)
    {
        Vector3 _targetPosition = new Vector3(_myTransform.position.x, targetY, _myTransform.position.z);

        while (_holdLetter)
        {
            await Task.Yield();
        }

        while (_myTransform.position.y > _targetPosition.y)
        {
            if (CancelLetterMovementTask)
            {
                return;
            }

            _myTransform.position -= new Vector3(0, _maxSpeed * Time.deltaTime, 0);
            await Task.Yield();
        }
        _myTransform.position = new Vector3(_myTransform.position.x, _targetPosition.y, _myTransform.position.z);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _holdLetter = true;
        _myTransform.SetParent(_myTransform.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _myTransform.position = new Vector3(Input.mousePosition.x, _myTransform.position.y, _myTransform.position.z);
        _onLetterMove?.Invoke(_myRectTransform.anchoredPosition.x, this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _holdLetter = false;
        _onLetterMoveEnd?.Invoke(CurrentColumn, _myTransform);
    }

    public string GetWord()
    {
        return _letterText.text;
    }

    public void SpellCorrect()
    {
        Destroy(this);
    }    
}
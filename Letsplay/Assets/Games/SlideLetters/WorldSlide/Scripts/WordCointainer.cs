using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class WordCointainer : MonoBehaviour
{
    private const float _blockWidth = 165.0f;
    private float _targetY;
    private bool _allLetterReachToItsTargetPosition;
    private int _rowHalfWidth;

    [SerializeField] private LetterController _letterPrefab;
    private Transform _myTransform;
    [SerializeField] private GameObject _checkMarkPrefab;
    [SerializeField] private GameObject _ghostPrefab;
    private Action<WordCointainer> _onSpellCorrect;

    private List<LetterController> _letters = new List<LetterController>();
    [SerializeField] private List<RectTransform> _columns = new List<RectTransform>();
    private Task[] _letterMovementTask = new Task[0];
    private string _word;

    private void Start()
    {
        _myTransform = transform;
        _rowHalfWidth = (int)(GetComponent<RectTransform>().sizeDelta.x / 2);       
    }

    public void GenerateLetters(string word, Action<WordCointainer> onSpellCorrect)
    {
        _word = word;
        _onSpellCorrect = onSpellCorrect;
        List<Transform> refColumn = new List<Transform>(_columns);
        for (int i = 0; i < word.Length; i++)
        {
            Transform parent = refColumn[UnityEngine.Random.Range(0, refColumn.Count)];
            LetterController letter = Instantiate(_letterPrefab, parent);
            _letters.Add(letter);
            letter.Initialize(word[i].ToString().ToUpper(), OnLetterMove, OnLetterMoveEnd);
            letter.gameObject.transform.localPosition = Vector3.zero;
            letter.CurrentColumn = parent.GetSiblingIndex();
            refColumn.Remove(parent);
        }
    }

    public async void SetTargetPosition(float targetY)
    {        
        _targetY = targetY;
        _allLetterReachToItsTargetPosition = false;

        for (int i = 0; i < _letters.Count; i++)
        {
            _letters[i].CancelLetterMovementTask = true;
        }

        await Task.Yield();

        if (_letterMovementTask.Length == 0)
        {
            _letterMovementTask = new Task[_letters.Count];
        }

        for (int i = 0; i < _letters.Count; i++)
        {
            _letters[i].CancelLetterMovementTask = false;
            _letterMovementTask[i] = _letters[i].MoveLetterToTargetPosition(targetY);
        }

        await Task.WhenAll(_letterMovementTask);
        
        _allLetterReachToItsTargetPosition = true;
        CheckSpell();
    }

    private void OnLetterMove(float xpos, LetterController letterController)
    {
        int index = GetCurrentindex(xpos + _rowHalfWidth);

        if (index < 0 || index >= _columns.Count)
        {
            return;
        }

        if (letterController.CurrentColumn != index)
        {
            if (_columns[index].childCount != 0)
            {
                Transform letter = _columns[index].GetChild(0);
                letter.SetParent(_columns[letterController.CurrentColumn]);
                letter.GetComponent<RectTransform>().DOAnchorPosX(0, 0.35f).SetEase(Ease.OutQuint);
                letter.GetComponent<LetterController>().CurrentColumn = letterController.CurrentColumn;
            }

            letterController.CurrentColumn = index;
        }
    }

    private void OnLetterMoveEnd(int currentColumn, Transform letterTransform)
    {
        letterTransform.SetParent(_columns[currentColumn]);
        letterTransform.GetComponent<RectTransform>().DOAnchorPosX(0, 0.35f).SetEase(Ease.OutQuint);

        if (_allLetterReachToItsTargetPosition)
        {
            CheckSpell();
        }
    }

    private async void CheckSpell()
    {
        
        string Testword = GetWord();
        if (_word.Equals(Testword, StringComparison.CurrentCultureIgnoreCase))
        {
            await SpellCorrect();
            if (GameManager.Instance.GameMode == GameManager.GameType.Advance)
            {
                _ = ClearWord();
                _onSpellCorrect?.Invoke(this);
            }
            GameManager.Instance.SpellCorrect(_word);
        }
    }

    public string GetWord()
    {
        string word = "";
        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].childCount > 0)
            {
                word += _columns[i].GetChild(0).GetComponent<LetterController>().GetWord();
            }
            else
            {
                word += " ";
            }
        }
        return word.Trim();
    }

    private int GetCurrentindex(float xpos)
    {
        return (int)(xpos / _blockWidth);
    }

    public async Task SpellCorrect()
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].childCount > 0)
            {
                _columns[i].GetChild(0).GetComponent<LetterController>().SpellCorrect();
            }
        }

        for (int i = 0; i < _myTransform.childCount; i++)
        {
            if (_myTransform.GetChild(i).childCount > 0)
            {
                Transform letterTransform = _myTransform.GetChild(i).GetChild(0);
                letterTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).SetLoops(2, LoopType.Yoyo);
                await Task.Delay(50);
            }
        }

        Transform rightCheckParent = _myTransform.GetChild(0).childCount == 0 ?
                                     _myTransform.GetChild(0) : _myTransform.GetChild(_myTransform.childCount - 1);

        GameObject rightCheck = Instantiate(_checkMarkPrefab, rightCheckParent);
        rightCheck.transform.localPosition = Vector3.zero;
        rightCheck.transform.position = new Vector3(rightCheck.transform.position.x, _targetY, rightCheck.transform.position.z);
        rightCheck.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).OnComplete(() =>
        {
            rightCheck.transform.DOScale(Vector3.one, 0.2f);
        });

        for (int i = 0; i < _myTransform.childCount; i++)
        {
            if (_myTransform.GetChild(i).childCount == 0)
            {
                GameObject ghost = Instantiate(_ghostPrefab, _myTransform.GetChild(i));
                ghost.transform.localPosition = Vector3.zero;
                ghost.transform.position = new Vector3(ghost.transform.position.x, _targetY, ghost.transform.position.z);
            }
        }
    }

    public async Task ClearWord()
    {
        Task[] tasks = new Task[_columns.Count];
        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].childCount > 0)
            {
                RectTransform rectTransform = _columns[i].GetChild(0).GetComponent<RectTransform>();
                CanvasGroup canvasGroup = _columns[i].GetChild(0).GetComponent<CanvasGroup>();
                tasks[i] = ClearLetter(rectTransform, canvasGroup);
                await Task.Delay(100);
            }
        }
        await Task.WhenAll(tasks);
        Destroy(gameObject);
    }

    public async Task ClearLetter(RectTransform rectTransform, CanvasGroup canvasGroup)
    {
        rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f);
        await canvasGroup.DOFade(0, 0.3f).OnComplete(() =>
        {
            Destroy(rectTransform.gameObject);
        }).AsyncWaitForCompletion();
    }
}
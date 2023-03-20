using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _rows = new List<RectTransform>();
    [SerializeField] private WordCointainer _wordPrefab;

    private List<WordCointainer> _wordCointainers = new List<WordCointainer>();

    public void GenerateLetters(/*WordData*/string  word)
    {
        if (_wordCointainers.Count == _rows.Count)
        {
            // GAME OVER
            GameManager.Instance.GameOver();
            return;
        }

        WordCointainer wordCointainer = Instantiate(_wordPrefab, transform);
        wordCointainer.GenerateLetters(word, OnSpellCorerctInAdvanceMode);
        _wordCointainers.Add(wordCointainer);
        UpdateTargetPosition(wordCointainer);
    }


    private void UpdateTargetPosition(WordCointainer wordCointainer)
    {
        int currentRow = _rows.Count - 1 - _wordCointainers.IndexOf(wordCointainer);
        float targetY = _rows[currentRow].transform.position.y;
        wordCointainer.SetTargetPosition(targetY);
    }

    public async Task ClearGrid()
    {
        Task[] tasks = new Task[_wordCointainers.Count];
        for (int i = 0; i < _wordCointainers.Count; i++)
        {
            tasks[i] = _wordCointainers[i].ClearWord();
        }
        await Task.WhenAll(tasks);
        _wordCointainers.Clear();
    }

    private void OnSpellCorerctInAdvanceMode(WordCointainer wordCointainer)
    {
        int index = _wordCointainers.IndexOf(wordCointainer);
        _wordCointainers.Remove(wordCointainer);

        for (int i = index; i < _wordCointainers.Count; i++)
        {
            UpdateTargetPosition(_wordCointainers[i]);
        }        
    }

    public bool IsGridEmpty()
    {
        return _wordCointainers.Count == 0;
    }
}
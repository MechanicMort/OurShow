using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;

public class Parrot : MonoBehaviour
{
    [SerializeField] private RectTransform _speechBubble;
    [SerializeField] private TextMeshProUGUI _wordText;
    [SerializeField] private AudioSource _speechAudioSource;

    public async Task ShowSpeechBubble(/*AudioClip wordAudio*/ string word = "")
    {
        if (string.IsNullOrEmpty(word))
        {
         //   _speechAudioSource.clip = wordAudio;
        //    _speechAudioSource.Play();
         //   await Task.Delay((int)(wordAudio.length * 1000));
            return;
        }

        await _speechBubble.DOScale(Vector3.one, 1).SetEase(Ease.OutQuint).AsyncWaitForCompletion();

       // _speechAudioSource.clip = wordAudio;
   //     _speechAudioSource.Play();

        await ShowWordOnSpeechBubble(word);
    }

    private async Task ShowWordOnSpeechBubble(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            _wordText.text = word;
            await Task.Delay(100);
        }
    }

    public async Task HideSpeechBubble()
    {
        await _speechBubble.DOScale(Vector3.zero, 1).SetEase(Ease.OutQuint).AsyncWaitForCompletion();
        _wordText.text = "";
    }
}

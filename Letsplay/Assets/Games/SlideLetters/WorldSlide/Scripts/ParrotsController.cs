using System.Threading.Tasks;
using UnityEngine;

public class ParrotsController : MonoBehaviour
{
    [SerializeField] private Parrot _englishParrot;
    [SerializeField] private Parrot _nativeParrot;

    public async Task InitializeParrot(string wordData /*,AudioClip EnglishAudio ,AudioClip NativeWordAudio, string NativeWord*/)
    {
        await _englishParrot.ShowSpeechBubble(wordData);
        await Task.Delay(500);
        await _nativeParrot.ShowSpeechBubble(wordData);
    }

    public async Task SpellCorrect(string wordData /*,AudioClip audioClip*/)
    {
        _ = _nativeParrot.HideSpeechBubble();
        await _englishParrot.ShowSpeechBubble(/*audioClip,*/ wordData);
        await Task.Delay(1000);
        await _englishParrot.HideSpeechBubble();
    }
}
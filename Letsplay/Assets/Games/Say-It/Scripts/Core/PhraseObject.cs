namespace WPM.SayIt.Core
{
    public struct PhraseObject
    {
        public Characters name;
        public string sentence;
        public string[] singleWords;

        public PhraseObject(Characters _name, string _sentence)
        {
            name = _name;
            sentence = _sentence;
            singleWords = sentence.Split(' ');
        }        
    }
}
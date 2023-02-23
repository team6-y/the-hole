using UnityEngine.UI;

namespace Dialog.Scripts
{
    public interface ISpeakable
    {
        public void StartDialog(Text display, string convoTextFile);
        public void ShowNextSentence();
        public void EndDialog();
    }
}

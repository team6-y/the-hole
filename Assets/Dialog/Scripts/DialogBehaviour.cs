using UnityEngine;
using UnityEngine.UI;

namespace Dialog.Scripts
{
    /// <summary>
    /// Allows a UI panel and a text file with a conversation to be displayed.
    /// Passes the file to the DialogManger to parse.
    /// Handles interaction with player
    /// </summary>
    public class DialogBehaviour : MonoBehaviour
    {
        [SerializeField] private Text displayPanel;
        // [SerializeField] private Animator nextPageIcon; // stretch goal: bouncing continue animator
        [SerializeField] private TextAsset convoTextFile;
        [SerializeField] private DialogManager dialogManager;

        /// <summary>
        /// Passes the dialog to display and the text panel to the
        /// DialogManager to parse and display.
        /// Triggered by interacting with NPCs (Unity Event).
        /// </summary>
        void StartDialog()
        {
            dialogManager.StartDialog(displayPanel, convoTextFile.text);
        }
    }
}

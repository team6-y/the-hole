using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Dialog.Scripts
{
    /// <summary>
    /// Parses the text file and sends the output back to the DialogBehaviour
    /// Does not handle interaction.
    /// </summary>
    public class DialogManager : MonoBehaviour, ISpeakable // we should have an IManager interface
                                                           // with the singleton implementation
    {
        [SerializeField] private float textSpeed = 0.022f;

        private Queue<string> pages;
        private string parsedPage;
        private bool doneTalking, isTalking; // doneTalking == done showing all the text on one page. isTalking == still adding chars to the text box
        private static DialogManager _instance;
        public static DialogManager GetInstance()
        {
            return _instance;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            pages = new Queue<string>();
            doneTalking = true;
            isTalking = false;
        }


        void Update()
        {
            /*
            if (isTalking)
            {
                if (player presses cancel)
                {
                    EndDialog();
                    return;
                }
                
                if (player presses submit/interact/accept/A/yes)
                {
                    ShowNextSentence();
                    return;
                }
            }
            */
        }
        public void StartDialog(Text display, string convoTextFile)
        {
            // open dialog panel
            // start parsing the text file
            ParseTextFile(convoTextFile);
        }

        /// <summary>
        /// Should be moved under interaction
        /// </summary>
        public void ShowNextSentence()
        {
            if (pages.Count == 0)
            {
                EndDialog();
                return;
            }

            if (!doneTalking)
            {
                // show all remaining text in page, stop typing, prep next page
                // display.text = parsedPage;
                // StopCoroutine(typing);
                pages.Dequeue();
                
                // make the continue arrow bounce
                /*
                 * doneTalking = true;
                 * nextPageIcon.SetBool("doneTalking", true);
                 */
            }
            else
            {
                // StopCoroutine(TypePage);
                // StartCoroutine(TypePage(convoTextFile));
            }
        }
        public void EndDialog()
        {
            
        }

        private void ParseTextFile(string convoTextFile)
        {
            // Split text into pages using @
            string[] chunks = convoTextFile.Split("@"[0]);
            foreach (var c in chunks) pages.Enqueue(c);
            
            // get tagreader functoinality here if iplemenmting
        }

        /// <summary>
        /// Adds the page letter by letter to the display box.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private IEnumerator TypePage(string page)
        {
            yield return null;
        }
    }
}

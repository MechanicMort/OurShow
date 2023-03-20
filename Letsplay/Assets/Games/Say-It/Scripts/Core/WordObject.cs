using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WPM.SayIt.Core
{
    public enum SlotType
    {
        SpeechBubble,
        Container
    };

    public enum SlotStatus
    {
        Empty,
        Full
    };

    public class WordObject : MonoBehaviour
    {
        TextMeshPro m_myTextDisplay;

        public int m_wordID;
        string m_wordText;

        SlotType m_slotType;
        SlotStatus m_status;

        private void Awake()
        {
            m_myTextDisplay = GetComponentInChildren<TextMeshPro>();
            m_status = SlotStatus.Empty;
        }

        /// <summary>
        /// Compare ID of dragged word and slot to determine if, the word is placed to the correct slot
        /// </summary>
        public bool IsCorrectID(WordObject _otherWordSlot)
        {
            if (this.GetSlotID() == _otherWordSlot.GetSlotID())
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Make the first letter uppercase
        /// </summary>
        public string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new System.ArgumentNullException(nameof(input));
                case "": throw new System.ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        /// <summary>
        /// Set ID fpr the word, so it's can be compared with empty slot to check if it's correct
        /// </summary>
        public void SetWordID(int _wordID)
        {
            this.m_wordID = _wordID;
        }

        /// <summary>
        /// Return slot ID
        /// </summary>
        public int GetSlotID()
        {
            return m_wordID;
        }

        /// <summary>
        /// Setting up text
        /// </summary>
        public void SetText(string _wordText)
        {
            m_status = SlotStatus.Full;
            m_wordText = _wordText;
            m_myTextDisplay.text = m_wordText;
        }

        /// <summary>
        /// Return text
        /// </summary>
        public string GetText()
        {
            return m_wordText;
        }

        /// <summary>
        /// Setting up type of the slot, to determine wheter it's container slot or speech bubble slot
        /// </summary>
        public void SetSlotType(SlotType _slotType)
        {
            this.m_slotType = _slotType;
        }

        /// <summary>
        /// Trim all of the punctuations from the end of the words
        /// </summary>
        public SlotType GetSlotType()
        {
            return m_slotType;
        }

        /// <summary>
        /// Determine wheter slot is empty or contains word
        /// </summary>
        public void SetSlotStatus(SlotStatus _status)
        {
            this.m_status = _status;
        }

        /// <summary>
        /// Trim all of the punctuations from the end of the words
        /// </summary>
        public SlotStatus GetSlotStatus()
        {
            return m_status;
        }

        /// <summary>
        /// Set all word details, and also status of the slot
        /// </summary>
        public void SetAllWordDetails(int _wordID, SlotStatus _status, string _text)
        {
            this.m_wordID = _wordID;
            this.m_status = _status;
            this.SetText(_text);
        }

        /// <summary>
        /// Setting up speech bubble slot
        /// </summary>
        public void SetupSpeechBubbleSlot(int _wordID)
        {
            m_slotType = SlotType.SpeechBubble;
            m_status = SlotStatus.Empty;
            m_wordID = _wordID;
            m_wordText = "";
            m_myTextDisplay.text = m_wordText;
        }

        /// <summary>
        /// Setting up word container slot
        /// </summary>
        public void SetupContainerSlot(int _wordID, string _wordText)
        {
            m_slotType = SlotType.Container;
            m_status = SlotStatus.Full;
            m_wordID = _wordID;
            m_wordText = _wordText;
            m_myTextDisplay.text = m_wordText;
        }

        /// <summary>
        /// Swap slots for two words
        /// </summary>
        public void SwapSlots(WordObject _otherWordSlot)
        {
            int t_tempID = _otherWordSlot.GetSlotID();
            SlotStatus t_tempSlotStatus = _otherWordSlot.GetSlotStatus();
            string t_tempText = _otherWordSlot.GetText();
            if (t_tempID == 0 && _otherWordSlot.GetSlotType() == SlotType.SpeechBubble)
            {
                _otherWordSlot.SetAllWordDetails(this.GetSlotID(), this.GetSlotStatus(), FirstCharToUpper(this.GetText()));
            }
            else
            {
                _otherWordSlot.SetAllWordDetails(this.GetSlotID(), this.GetSlotStatus(), this.GetText());
            }
            this.SetAllWordDetails(t_tempID, t_tempSlotStatus, t_tempText);
        }

        /// <summary>
        /// Bring word to the front display while it's dragged
        /// </summary>
        private void DraggingActive()
        {
            m_myTextDisplay.sortingOrder = 15;
        }

        /// <summary>
        /// Set word to the standard sorting layer
        /// </summary>
        private void DisableDragging()
        {
            m_myTextDisplay.sortingOrder = 15;
        }

        /*
        public void SetEmpty()
        {
            m_status = SlotStatus.Empty;
            m_wordText = "";
            m_myTextDisplay.text = m_wordText;
        }
        */
    }
}
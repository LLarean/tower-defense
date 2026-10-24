using System;
using System.Collections;
using Globals;
using TMPro;
using UnityEngine;

namespace UI.Game
{
    public class NotificationsPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _message;
        
        private Coroutine _coroutine;

        public void ShowMessage(string message)
        {
            _message.text = message;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            _coroutine = StartCoroutine(DelayHidingMessage());
        }
        
        private IEnumerator DelayHidingMessage()
        {
            yield return new WaitForSeconds(GlobalParams.MessageDisplayTime);
            HideMessage();
        }

        private void HideMessage()
        {
            _message.text = String.Empty;
        }
    }
}
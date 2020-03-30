using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TrickSaber
{
    public abstract class Trick : MonoBehaviour
    {
        public SaberTrickManager SaberTrickManager;
        protected bool _trickStarted;
        protected bool _endRequested;
        protected bool _allowTrickStart = true;

        void Start()
        {
            OnConstruct();
        }

        public bool StartTrick()
        {
            if (!_trickStarted && _allowTrickStart)
            {
                SaberTrickManager.IsDoingTrick = true;
                _allowTrickStart = false;
                OnTrickStart();
                _trickStarted = true;
                return true;
            }

            return false;
        }

        public void EndTrick()
        {
            if (_trickStarted)
            {
                _endRequested = true;
                Plugin.Log.Debug("End Requested");
            }
        }

        protected void Reset()
        {
            _endRequested = false;
            _allowTrickStart = true;
            SaberTrickManager.IsDoingTrick = false;
        }

        void Update()
        {
            if (_trickStarted)
            {
                if(!_endRequested)OnTrickUpdate();
                else
                {
                    _trickStarted = false;
                    OnTrickEndRequested();
                }
            }
        }

        public abstract void OnTrickStart();

        public abstract void OnTrickUpdate();

        public abstract void OnTrickEndRequested();

        public abstract void OnConstruct();
    }
}

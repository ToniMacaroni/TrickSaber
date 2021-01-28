using System;
using System.Collections.Generic;

namespace TrickSaber.InputHandling
{
    internal class TrickInputHandler
    {
        public Dictionary<TrickAction, HashSet<InputHandler>> TrickHandlerSets =
            new Dictionary<TrickAction, HashSet<InputHandler>>();

        public TrickInputHandler()
        {
            //Initialize TrickHandlerSets for every trick with an empty InputHandler HashSet
            foreach (object value in Enum.GetValues(typeof(TrickAction)))
            {
                var action = (TrickAction) value;
                if (action == TrickAction.None) continue;
                TrickHandlerSets.Add(action, new HashSet<InputHandler>());
            }
        }

        public void Add(TrickAction action, InputHandler handler)
        {
            if (action == TrickAction.None) return;
            TrickHandlerSets[action].Add(handler);
        }

        public HashSet<InputHandler> GetHandlers(TrickAction action)
        {
            return TrickHandlerSets[action];
        }
    }
}
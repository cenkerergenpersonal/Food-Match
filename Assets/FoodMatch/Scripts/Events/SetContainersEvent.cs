using System.Collections.Generic;
using FoodMatch.Scripts.Common;
using UnityEngine;

namespace FoodMatch.Scripts.Events
{
    public class SetContainersEvent : IEvent
    {
        public List<Transform> Containers;

        public SetContainersEvent(List<Transform> containers)
        {
            Containers = containers;
        }
    }
}
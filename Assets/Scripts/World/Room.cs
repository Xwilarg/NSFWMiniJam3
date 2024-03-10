﻿using System.Linq;
using UnityEngine;

namespace NSFWMiniJam3.World
{
    public class Room : MonoBehaviour
    {
        [SerializeField]
        private bool _startingRoom;

        private Prop[] _props;

        public bool CanHideInRoom => _props.Any() && _props.All(x => x.HiddenNpc == null);

        public Prop RandomProp => _props[Random.Range(0, _props.Length)];

        public bool IsStartingRoom => _startingRoom;

        private void Awake()
        {
            foreach (var door in GetComponentsInChildren<IRoomOwned>())
            {
                door.ParentRoom = this;
            }

            _props = GetComponentsInChildren<Prop>();
        }

        /// <summary>
        /// Code executed when the player look inside an empty object
        /// </summary>
        public void FailHideCheck()
        {
            // TODO
        }
    }
}

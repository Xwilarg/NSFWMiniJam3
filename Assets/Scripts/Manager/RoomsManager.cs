﻿using NSFWMiniJam3.World;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace NSFWMiniJam3.Manager
{
    public class RoomsManager : MonoBehaviour
    {
        public static RoomsManager Instance { private set; get; }

        [SerializeField]
        private Npc[] _npcs;

        private Room[] _rooms;

        public int EnemyRunningAway { set; get; }

        private void Awake()
        {
            Instance = this;

            _rooms = FindObjectsOfType<Room>();

            Assert.AreEqual(1, _rooms.Count(x => x.IsStartingRoom), "There must be one and only one starting room");
        }

        private void Start()
        {
            ShowRoom(_rooms.First(x => x.IsStartingRoom));
        }

        public void RunAndHideAll(Room r, PlayerController pc)
        {
            foreach (var npc in _npcs)
            {
                r.RunAway(pc, npc.Info, _ => npc);
            }
        }

        private bool CheckDoorAccess(Room current, Room destination, List<Room> path)
        {
            if (current == destination)
            {
                return true;
            }

            if (path.Contains(destination))
            {
                Debug.Log($"[PATH] Rejecting path of {string.Join(", ", path.Select(x => x.name))} already containing {destination.name}");
                return false;
            }

            foreach (var room in current.Doors.Select(x => x.Destination).Where(x => !path.Any(p => p.gameObject.GetInstanceID() == x.gameObject.GetInstanceID()))) // Assume 2 doors don't go to the same room
            {
                List<Room> newPath = new();
                newPath.AddRange(path);
                newPath.Add(room);

                if (CheckDoorAccess(room, destination, newPath))
                {
                    return true;
                }
            }

            return false;
        }

        public Door[] GetDoorTo(Room current, Room destination)
        {
            return current.Doors
                .Where(x => CheckDoorAccess(x.Destination, destination, new() { x.Destination }))
                .ToArray();
        }

        /// <summary>
        /// Get a room where we can hide
        /// It means that there are props to hide and no one is hiding inside already
        /// </summary>
        /// <param name="ignore">Room that can't be returned</param>
        public Room GetRandomAvailableRoom(Room ignore)
        {
            var availableRooms = _rooms.Where(x => x.CanHideInRoom && x != ignore).ToArray();
            if (availableRooms.Any())
            {
                return availableRooms[Random.Range(0, availableRooms.Length)];
            }
            return null;
        }

        public void ShowRoom(Room r)
        {
            foreach (var room in _rooms)
            {
                room.gameObject.SetActive(false);
            }
            r.gameObject.SetActive(true);
        }
    }
}

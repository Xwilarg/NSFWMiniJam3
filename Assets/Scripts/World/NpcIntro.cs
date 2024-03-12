﻿using NSFWMiniJam3.Manager;
using UnityEngine;

namespace NSFWMiniJam3.World
{
    public class NpcIntro : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private TextAsset _introText;

        public string InteractionKey => "speak";

        public void Interact(PlayerController _)
        {
            DialogueManager.Instance.ShowStory(transform.position, _introText);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractableShop : Interactable
    {

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip open_sound;

        [SerializeField]
        private GameObject ShopUI;
        public override void Interact()
        {
            if (!ShopUI.activeInHierarchy)
            {
                audioSource.PlayOneShot(open_sound);
                ShopUI.SetActive(true);
                TopDownPlayerController.Instance.FreezeMovement();
            }
        }
    }
}
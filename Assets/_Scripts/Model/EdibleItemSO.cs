using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField] List<ModifierData> modifiersData = new List<ModifierData>();
        public string ActionName => "Consume";
        public AudioClip ActionSFX { get; private set; }

        public bool PerformAction(GameObject character)
        {
            foreach (var data in modifiersData)
            {
                data.startModifier.AffectCharacter(character, data.value);
            }
            return true;
        }
    }
    public interface IDestroyableItem
    {

    }
    public interface IItemAction
    {
        public string ActionName { get;}
        public AudioClip ActionSFX { get;}
        bool PerformAction(GameObject character);
    }
    [System.Serializable]
    public class ModifierData
    {
        public CharacterStartModifierSO startModifier;
        public float value;
    }
}
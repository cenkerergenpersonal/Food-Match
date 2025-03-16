using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoodMatch.Scripts.Data
{
    [CreateAssetMenu(fileName = "ItemSpriteSettings", menuName = "Game/ItemSpriteSettings")]
    public class ItemSpriteSettings : ScriptableObject
    {
        public List<ItemSprite> ItemSprites;
    }

    [Serializable]
    public class ItemSprite
    {
        public ItemSet ItemType;
        public List<Sprite> ItemSprites;
    }
}
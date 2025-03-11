using System;
using FoodMatch.Scripts.Game.Factory;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace FoodMatch.Scripts.Game
{
    public class GameController : MonoBehaviour
    {
        private IItemFactory _itemFactory;

        [Inject]
        public void Construct(IItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < 50; i++)
                {
                    ItemBase newItem = await _itemFactory.CreateItem();
                    if (newItem != null)
                    {
                        newItem.Initialize(Random.Range(0, 20));
                    }
                }
               
            }
        }
    }
}
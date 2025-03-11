using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Game;
using FoodMatch.Scripts.Game.Factory;
using UnityEngine;
using Zenject;

namespace FoodMatch.Scripts.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private int initialPoolSize = 10;

        public override void InstallBindings()
        {
            Container.Bind<ItemBase>().FromComponentInNewPrefab(itemPrefab).AsTransient();

            Container.Bind<ObjectPool<ItemBase>>()
                .AsSingle()
                .WithArguments(itemPrefab, initialPoolSize, transform);
            
            Container.Bind<IItemFactory>().To<ItemFactory>().AsSingle();
        }
    }
}

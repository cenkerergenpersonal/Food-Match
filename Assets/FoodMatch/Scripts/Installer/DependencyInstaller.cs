using System.Collections.Generic;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;
using FoodMatch.Scripts.Events;
using FoodMatch.Scripts.Game;
using FoodMatch.Scripts.Game.Factory;
using FoodMatch.Scripts.Views;
using UnityEngine;
using Zenject;

namespace FoodMatch.Scripts.Installer
{
    public class DependencyInstaller : MonoInstaller
    {
        [SerializeField] private PopupData popupLibrary;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private int initialPoolSize = 10;
        [SerializeField] private GameObject goalCheckerPrefab;
        [SerializeField] private Transform goalParent;
        [SerializeField] private List<Transform> _containers;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ItemManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MatchSystem>().AsSingle().NonLazy();

            Container.Bind<Item>().FromComponentInNewPrefab(itemPrefab).AsTransient();

            Container.Bind<ObjectPool<Item>>()
                .AsSingle()
                .WithArguments(itemPrefab, initialPoolSize, transform);
            
            Container.Bind<IItemFactory>().To<ItemFactory>().AsSingle();
            Container.Bind<LevelLoader>().AsSingle();

            Container.BindInstance(goalCheckerPrefab).WithId("GoalCheckerPrefab");
            Container.BindInstance(goalParent).WithId("GoalParent");


            if (!Container.HasBinding<GoalManager>())
            {
                Container.Bind<GoalManager>().AsSingle();
            }
            Container.Bind<GameUiController>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.BindInstance(popupLibrary).AsSingle();
            Container.Bind<PopupController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PopupManager>().FromComponentInHierarchy().AsSingle();
        }
        
        public override void Start()
        {
            base.Start();
            EventBus.Publish(new ShowPopupEvent(PopupType.Start));
            EventBus.Publish(new SetContainersEvent(_containers));
        }
    }
}

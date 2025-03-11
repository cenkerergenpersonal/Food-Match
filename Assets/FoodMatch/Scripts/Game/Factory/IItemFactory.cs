using Cysharp.Threading.Tasks;

namespace FoodMatch.Scripts.Game.Factory
{
    public interface IItemFactory
    {
        UniTask<ItemBase> CreateItem();
    }
}
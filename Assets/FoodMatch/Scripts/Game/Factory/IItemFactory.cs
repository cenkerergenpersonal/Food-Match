using Cysharp.Threading.Tasks;

namespace FoodMatch.Scripts.Game.Factory
{
    public interface IItemFactory
    {
        UniTask<Item> CreateItem(int setId,int itemId);
        void Remove(Item item);
    }
}
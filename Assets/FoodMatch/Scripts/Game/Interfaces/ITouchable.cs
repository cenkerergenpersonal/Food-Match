namespace FoodMatch.Scripts.Game.Interfaces
{
    public interface ITouchable
    {
        void OnTouch();

        void DisabledTouch();
        
        void EnabledTouch();
    }
}
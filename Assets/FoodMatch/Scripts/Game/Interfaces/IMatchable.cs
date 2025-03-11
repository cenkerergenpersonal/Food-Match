namespace FoodMatch.Scripts.Game.Interfaces
{
    public interface IMatchable
    {
        void Match();
        
        void Unmatch();
        
        void DisableMatch();
        
        void EnableMatch();
    }
}
using FoodMatch.Scripts.Common;

namespace FoodMatch.Scripts.Events
{
    public class StartGameEvent: IEvent
    {
        public string LevelFileName;

        public StartGameEvent(string levelFileName)
        {
            LevelFileName = levelFileName;
        }
    }
}
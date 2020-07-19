using System.Collections.Generic;
using System.Linq;
using Game.DI;

namespace Game.Gameplay.Doors.Keys
{
    [Injectable(Singleton = true)]
    public class KeyStorage
    {
        private readonly List<CollectedKey> collectedKeys = new List<CollectedKey>();

        private CollectedKey GetKeyForDoorType(DoorType doorType)
        {
            return collectedKeys.FirstOrDefault(x => x.DoorType == doorType);
        }

        public void CollectKey(Key key)
        {
            CollectedKey collectedKey = new CollectedKey(key.DoorType);
            collectedKeys.Add(collectedKey);
        }

        public bool UseKeyForDoor(DoorType doorType)
        {
            CollectedKey key = GetKeyForDoorType(doorType);

            if (key == null)
            {
                return false;
            }

            collectedKeys.Remove(key);

            return true;
        }
    }
}

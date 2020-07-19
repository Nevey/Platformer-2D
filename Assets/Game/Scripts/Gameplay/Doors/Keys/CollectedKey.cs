namespace Game.Gameplay.Doors.Keys
{
    public class CollectedKey
    {
        public DoorType DoorType { get; }

        public CollectedKey(DoorType doorType)
        {
            DoorType = doorType;
        }
    }
}

namespace Game.Utils
{
    public static class UnityObjectExtensions
    {
        public static bool IsNull(this UnityEngine.Object @object)
        {
            return @object == null || @object.ToString().Equals("null");
        }
    }
}

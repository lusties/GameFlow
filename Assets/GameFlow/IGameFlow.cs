namespace Lustie.GameFlow
{
    public interface IGameFlow
    {
        void AddSystem(IGameSystem system, int order = -1);
        void AddSubSystem(IGameSubsystem subSystem, int order = -1);
    }
}

using Cysharp.Threading.Tasks;

namespace Lustie.GameFlow
{
    public interface IGameSubsystem
    {
        UniTask InitSubsystemAsync();
    }
}

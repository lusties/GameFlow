using Cysharp.Threading.Tasks;

namespace Lustie.GameFlow
{
    public interface IGameSystem
    {
        UniTask InitSystemAsync();
    }
}

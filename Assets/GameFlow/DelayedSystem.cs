using Cysharp.Threading.Tasks;

namespace Lustie.GameFlow
{
    public class DelayedSystem : IGameSystem
    {
        public async UniTask InitSystemAsync()
        {
            await UniTask.Delay(1000);
        }
    }
}

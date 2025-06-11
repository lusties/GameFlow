using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Lustie.GameFlow
{
    public class GameFlowSystem : IGameFlow, IDisposable
    {
        public static GameFlowSystem Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => currentInstance;
        }

        private static GameFlowSystem currentInstance;

        private readonly List<IGameSystem> systems = new();
        private readonly List<IGameSubsystem> subsystems = new();
        private readonly List<IGameStartable> gameReadys = new();
        private readonly List<IGameUpdatable> unityUpdates = new();

        public GameFlowSystem(bool setCurrent)
        {
            if (setCurrent)
                currentInstance = this;
        }

        public async UniTask RunGame()
        {
            await InitSystems();
            await InitSubSystems();
            OnGameReady();
        }

        public void AddSystem(IGameSystem system, int order = -1)
        {
            if (order < 0)
                systems.Add(system);
            else
                systems.Insert(order, system);
        }

        public void AddSubSystem(IGameSubsystem system, int order = -1)
        {
            if (order < 0)
                subsystems.Add(system);
            else
                subsystems.Insert(order, system);
        }


        public async UniTask InitSystems()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                IGameSystem system = systems[i];
                await system.InitSystemAsync();
            }
        }

        public async UniTask InitSubSystems()
        {
            for (int i = 0; i < subsystems.Count; i++)
            {
                IGameSubsystem subsystem = subsystems[i];
                await subsystem.InitSubsystemAsync();
            }
        }

        public void OnGameReady()
        {
            for (int i = 0; i < gameReadys.Count; i++)
            {
                IGameStartable gameReady = gameReadys[i];
                gameReady.OnGameReady();
            }
        }

        public void Update()
        {
            for (int i = 0; i < unityUpdates.Count; i++)
            {
                IGameUpdatable updater = unityUpdates[i];
                updater.OnGameUpdate();
            }
        }

        public void RegisterObject(object obj)
        {
            bool any = false;
            if (obj is IGameSystem system)
            {
                systems.Add(system);
                any = true;
            }
            if (obj is IGameStartable gameReady)
            {
                gameReadys.Add(gameReady);
                any = true;
            }
            if (obj is IGameUpdatable unityUpdate)
            {
                unityUpdates.Add(unityUpdate);
                any = true;
            }

            if (!any)
                Debug.LogWarning($"Object of type {obj.GetType()} is not registered in GameFlowSystem.");
        }

        public void UnregisterObject(object obj)
        {
            if (obj is IGameSystem system)
            {
                systems.RemoveSwapBack(system);
            }
            if(obj is IGameSubsystem subsystem)
            {
                subsystems.RemoveSwapBack(subsystem);
            }
            if (obj is IGameUpdatable unityUpdate)
            {
                unityUpdates.RemoveSwapBack(unityUpdate);
            }
        }

        public void Register(IGameUpdatable unityUpdater)
        {
            unityUpdates.Add(unityUpdater);
        }

        public void Unregister(IGameUpdatable unityUpdater)
        {
            unityUpdates.RemoveSwapBack(unityUpdater);
        }

        public void RegisterPhase<TInterface>()
            where TInterface : class
        {

        }

        public void Dispose()
        {

        }

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            currentInstance = null;
        }
#endif
    }
}

using UnityEngine;

namespace Lustie.GameFlow
{
    [DefaultExecutionOrder(-999)]
    public abstract class GameEntryPoint : MonoBehaviour
    {
        public static int ON_GAME_LOAD = 0;
        public static int ON_SYSTEM_LOAD = 10;
        public static int ON_SUBSYSTEM_LOAD = 20;

        [SerializeField] private GameEntry[] gameEntries;

        [System.Serializable]
        struct GameEntry
        {
            public GameObject gameObject;
            public bool setParent;
        }


        private async void Awake()
        {
            GameFlowSystem singletonPlayerLoop = new GameFlowSystem(true);
            using GameFlowSystem emptyLoop = new GameFlowSystem(false);

            currentPlayerLoop = emptyLoop;

            int systemCount = gameEntries.Length;
            for (int i = 0; i < systemCount; i++)
            {
                var system = gameEntries[i];
                var systemInstance = Instantiate(system.gameObject, system.setParent ? transform : null);

                if (systemInstance.TryGetComponent(out IGameEntry gameSystem))
                {
                    await gameSystem.InitEntry();
                }
            }

            Configure(singletonPlayerLoop);
            await singletonPlayerLoop.RunGame();
            currentPlayerLoop = singletonPlayerLoop;
        }

        GameFlowSystem currentPlayerLoop;


        private void Update()
        {
            currentPlayerLoop.Update();
        }

        protected abstract void Configure(IGameFlow gameFlow);
    }
}
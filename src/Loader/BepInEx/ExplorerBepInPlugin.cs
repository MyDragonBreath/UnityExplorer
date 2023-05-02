#if BIE
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityExplorer.Config;
using UnityExplorer.Loader.BIE;
#if CPP6
using BepInEx.Unity.IL2CPP;
#endif
#if CPP
using BepInEx.IL2CPP;
#endif

namespace UnityExplorer
{
    [BepInPlugin(ExplorerCore.GUID, "UnityExplorer", ExplorerCore.VERSION)]

    #region MDB-Comment
    /// So you may be wondering
    /// why I took so long to publicly release this
    /// well 1 i forgot
    /// and 2 i hated my original methodology - and never intended to release it publicly.
    /// 
    /// so after stealing some code
    /// and using some of my own
    /// i'm confident that chris won't freak out.
    /// 
    /// The bepindependency below is so the chainloader makes sure unityexplorer enables last.
    /// Why? because it was the quickest and easiest way to fix a problem to an acceptable public standard.
    /// Rather than do hacky fixes
    /// 
    /// - MyDragonBreath
    /// 
    /// note i built reactor myself and commented out the below line and it worked.
    /// I found out why. Debug builds dont cause a memory error, but release builds do.
    /// 
    /// - MyDragonBreath
    #endregion
    [BepInDependency("gg.reactor.api", BepInDependency.DependencyFlags.SoftDependency)]

    public class ExplorerBepInPlugin :
#if MONO
        BaseUnityPlugin
#else
        BasePlugin
#endif
        , IExplorerLoader
    {
        public static ExplorerBepInPlugin Instance;

        public ManualLogSource LogSource
#if MONO
            => Logger;
#else
            => Log;
#endif
        const string IL2CPP_LIBS_FOLDER =
#if UNHOLLOWER
            "unhollowed"
#else
            "interop"
#endif
            ;
        public string UnhollowedModulesFolder => Path.Combine(Paths.BepInExRootPath, IL2CPP_LIBS_FOLDER);

        public ConfigHandler ConfigHandler => _configHandler;
        private BepInExConfigHandler _configHandler;

        public Harmony HarmonyInstance => s_harmony;
        private static readonly Harmony s_harmony = new(ExplorerCore.GUID);

        public string ExplorerFolderName => ExplorerCore.DEFAULT_EXPLORER_FOLDER_NAME;
        public string ExplorerFolderDestination => Paths.PluginPath;

        public Action<object> OnLogMessage => LogSource.LogMessage;
        public Action<object> OnLogWarning => LogSource.LogWarning;
        public Action<object> OnLogError => LogSource.LogError;

        private void Init()
        {
            Instance = this;
            _configHandler = new BepInExConfigHandler();
            ExplorerCore.Init(this);
        }

#if MONO // Mono
        internal void Awake()
        {
            Init();
        }

#else   // Il2Cpp
        public override void Load()
        {
            Init();
        }
#endif
    }
}
#endif
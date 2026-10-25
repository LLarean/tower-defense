using System;
using System.Reflection;
using Game.Core.Battles;
using Game.Stage;
using Game.Stage.Net;
using Game.Stage.Orders;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Autopilot : MonoBehaviour
{
    private const BindingFlags Hidden = BindingFlags.NonPublic | BindingFlags.Instance;

    private string _role = string.Empty;
    private bool _online;
    private string _codeFile = string.Empty;
    private string _mission = "Meadow";
    private int _difficulty = 1;
    private bool _briefed;
    private Game.Stage.Menu.Cloud _cloud = new Game.Stage.Menu.Cloud("default");
    private float _since;
    private int _logged = -1;
    private int _step;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Boot()
    {
        var args = Environment.GetCommandLineArgs();
        var role = Array.IndexOf(args, "-td-host") >= 0 ? "host" : Array.IndexOf(args, "-td-join") >= 0 ? "guest" : string.Empty;
        if (role.Length == 0)
        {
            return;
        }
        var pilot = new GameObject("Autopilot").AddComponent<Autopilot>();
        pilot._role = role;
        pilot._online = Array.IndexOf(args, "-td-online") >= 0;
        pilot._cloud = new Game.Stage.Menu.Cloud(role);
        pilot._codeFile = Value(args, "-td-codefile", string.Empty);
        pilot._mission = Value(args, "-td-mission", "Meadow");
        pilot._difficulty = int.Parse(Value(args, "-td-difficulty", "1"));
        DontDestroyOnLoad(pilot.gameObject);
        Application.targetFrameRate = 60;
        Debug.Log("[Pilot] role " + role);
    }

    private bool _hooked;

    private void Update()
    {
        _since += Time.deltaTime;
        var manager = NetworkManager.Singleton;
        if (!_hooked && manager != null)
        {
            _hooked = true;
            manager.LogLevel = Unity.Netcode.LogLevel.Developer;
            manager.OnClientDisconnectCallback += id => Debug.Log($"[Net] disconnect {id} reason '{manager.DisconnectReason}' scene {SceneManager.GetActiveScene().name}");
            manager.OnTransportFailure += () => Debug.Log("[Net] transport failure");
            manager.OnClientStopped += host => Debug.Log($"[Net] client stopped host={host} reason '{manager.DisconnectReason}'");
            manager.OnServerStopped += host => Debug.Log($"[Net] server stopped host={host}");
            SceneManager.sceneLoaded += (scene, mode) => Debug.Log($"[Net] scene loaded {scene.name} listening={manager.IsListening}");
        }
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Lobby(manager);
            return;
        }
        Battle();
    }

    private static string Value(string[] args, string key, string fallback)
    {
        var index = Array.IndexOf(args, key);
        return index >= 0 && index + 1 < args.Length ? args[index + 1] : fallback;
    }

    private async void Connect(Match match)
    {
        try
        {
            if (_role == "host")
            {
                var code = await _cloud.Host(4);
                System.IO.File.WriteAllText(_codeFile, code);
                Debug.Log("[Pilot] hosting online with code " + code);
            }
            else
            {
                while (!System.IO.File.Exists(_codeFile))
                {
                    await System.Threading.Tasks.Task.Delay(500);
                }
                var code = System.IO.File.ReadAllText(_codeFile).Trim();
                await _cloud.Join(code);
                Debug.Log("[Pilot] joined online code " + code);
            }
            match.Open();
            _step = 1;
        }
        catch (Exception error)
        {
            Debug.LogException(error);
        }
    }

    private void Lobby(NetworkManager manager)
    {
        var match = manager.GetComponent<Match>();
        if (_online && !manager.IsListening && _since > 2f && _step == 0)
        {
            _step = -1;
            if (_role == "host")
            {
                match.Choose(_difficulty);
            }
            Connect(match);
        }
        if (!_online && !manager.IsListening && _since > 2f && _step == 0)
        {
            if (_role == "host")
            {
                match.Choose(_difficulty);
            }
            _step = 1;
            var transport = manager.GetComponent<UnityTransport>();
            transport.SetConnectionData("127.0.0.1", 7790, "127.0.0.1");
            var started = _role == "host" ? manager.StartHost() : manager.StartClient();
            match.Open();
            Debug.Log($"[Pilot] network started {started}");
        }
        if (_role == "host" && manager.IsListening && match.Players >= 2 && _step == 1)
        {
            _step = 2;
            Debug.Log("[Pilot] starting match with " + match.Players);
            match.Begin(_mission);
        }
    }

    private void Battle()
    {
        var scene = FindFirstObjectByType<BattleScene>();
        if (scene == null)
        {
            return;
        }
        var pace = (IPace)typeof(BattleScene).GetField("_pace", Hidden).GetValue(scene);
        var hotkeys = typeof(BattleScene).GetField("_hotkeys", Hidden).GetValue(scene);
        var orders = (IOrders)hotkeys.GetType().GetField("_orders", Hidden).GetValue(hotkeys);
        var armory = (Game.Stage.Towers.Armory)hotkeys.GetType().GetField("_armory", Hidden).GetValue(hotkeys);
        var timeline = Timeline(pace);
        var match = NetworkManager.Singleton.GetComponent<Match>();
        if (_step < 10 && timeline.Tick > 30)
        {
            _step = 10;
            var me = match.Me;
            var spots = me == 0
                ? new[] { new Vector3(92f, 3f, 140f), new Vector3(100f, 3f, 120f) }
                : new[] { new Vector3(110f, 3f, 104f), new Vector3(78f, 3f, 140f) };
            orders.Build(armory.Specs[me == 0 ? 0 : 1], new System.Numerics.Vector3(spots[0].x, spots[0].y, spots[0].z));
            orders.Build(armory.Specs[me == 0 ? 3 : 2], new System.Numerics.Vector3(spots[1].x, spots[1].y, spots[1].z));
            if (_role == "host")
            {
                orders.Summon();
            }
            Debug.Log("[Pilot] built and summoned");
        }
        if (_step == 10 && timeline.Tick > 900)
        {
            _step = 11;
            foreach (var tower in timeline.Battle.Garrison.Towers)
            {
                orders.Upgrade(tower);
            }
            orders.Summon();
            Debug.Log("[Pilot] upgraded and summoned");
        }
        if (!_briefed)
        {
            _briefed = true;
            Debug.Log($"[Brief] scene={SceneManager.GetActiveScene().name} players={match.Players} me={match.Me} difficulty={match.Difficulty} lives={timeline.Battle.Lives.Left}");
        }
        var tick = timeline.Tick;
        if (tick % 300 == 0 && tick != _logged)
        {
            _logged = tick;
            Debug.Log($"[Sum] tick={tick} sum={timeline.Checksum()} towers={timeline.Battle.Garrison.Towers.Count} monsters={timeline.Battle.Arena.Horde.Monsters.Count} desync={pace.Desynced}");
        }
    }

    private static Timeline Timeline(IPace pace)
    {
        var field = pace.GetType().GetField(pace is HostPace ? "_host" : "_guest", Hidden);
        var peer = field.GetValue(pace);
        return (Timeline)peer.GetType().GetProperty("Timeline").GetValue(peer);
    }
}

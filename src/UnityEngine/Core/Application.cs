using System;
using System.Collections.Generic;

namespace UnityEngine
{
	public class Application
	{
		//static Application _instance;

		public static float targetFrameRate;

		static TimeManager timerManager = new TimeManager();
		static RenderManager renderManager = new RenderManager();
		static AnimationManager animationManager = new AnimationManager();
		static CollisionWorld collisionWorld = new CollisionWorld();
		static InputManager inputManager = new InputManager();

		static double startTime;

		static public int width;
		static public int height;

		public static bool isEditor = false;
		public static string persistentDataPath {get {return "";}}

		public static double GetTimeSinceStartup()
		{
			double now = (double)Environment.TickCount / (1000);
			return now - startTime;
		}

		public static bool isPlaying {get {return true;}}

		public Application ()
		{
			//_instance = this;
			startTime = (double)Environment.TickCount / (1000);
			TypeUtil.RegisterAssembly(typeof(Application).Assembly);
		}

		static internal RenderManager GetRenderManager()
		{
			return renderManager;
		}

		static internal AnimationManager GetAnimationManager()
		{
			return animationManager;
		}

		static internal CollisionWorld GetCollisionWorld()
		{
			return collisionWorld;
		}

		static internal InputManager GetInputManager()
		{
			return inputManager;
		}

		internal Texture GetTexture(string guid)
		{
			return Resources.GetTexture (guid);
		}

		internal MeshImport GetMeshImport(string guid)
		{
			return Resources.GetMeshImport (guid);
		}

		internal Material GetMaterial(string guid)
		{
			return Resources.GetMaterial (guid);
		}

		internal UnityEngine.Config.ResourceMap GetMap(string guid)
		{
			return Resources.rmManager.GetFromGuid(guid);
		}

		public void OnScreenSizeChanged(int width, int height)
		{
			Application.width = width;
			Application.height = height;
		}

		public void Start(string initialScene)
		{
			DoStart (this);
			LoadSettings();
			if (!String.IsNullOrEmpty (initialScene))
			{
				LoadLevel (initialScene);
			}
		}

		public void Start()
		{
			Start (null);
		}

		public void RunOnce()
		{
			timerManager.Update();
			inputManager.Update();
			DelayedCallManager.Update();
			BehaviourManager.Run ();
			Transform.UpdateAll();
			animationManager.Update();
			renderManager.Update();
		}

		// load a scene.
		// Need resources. placeholder here.
		public void LoadScene()
		{
		}

		public static void LoadLevel(string name)
		{
			// TODO: Destroy current everything.
			Resources.LoadScene(name);
		}

		static void LoadSettings()
		{
			var setting = Resources.LoadTagLayerSetting();
			var l = setting["layers"] as List<System.Object>;
			LayerMask.SetLayerSettings(l);
		}

		public static RuntimePlatform platform {
			get {
				return isMobileDevices() ? RuntimePlatform.IPhonePlayer : RuntimePlatform.OSXWebPlayer;
			}
		}

		static extern bool isMobileDevices();
		static extern void DoStart(Application app);
		static extern void DoUpdateRendering(Application app);
	}
}


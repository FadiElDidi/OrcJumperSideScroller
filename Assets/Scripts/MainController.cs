using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {
	private static MainController mainController;

	private string currentSceneName;
	private string nextSceneName;
	private AsyncOperation resourceUnloadTask;
	private AsyncOperation sceneLoadTask;
	private enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count };
	private SceneState sceneState;
	private delegate void UpdateDelegate();
	private UpdateDelegate[] updateDelegates;

	//======================================================================================
	// public static methods
	//======================================================================================
	public static void SwitchScene(string nextSceneName)
	{
		if (mainController != null)
		{
			if (mainController.currentSceneName != nextSceneName )
			{
				mainController.nextSceneName = nextSceneName;
			}
		}
	}

	//======================================================================================
	// protected mono methods
	//======================================================================================

	protected void Awake()
	{
		Object.DontDestroyOnLoad (gameObject);
		mainController = this;

		updateDelegates = new UpdateDelegate[(int)SceneState.Count];

		updateDelegates [(int)SceneState.Reset] = UpdateSceneReset;
		updateDelegates [(int)SceneState.Preload] = UpdateScenePreload;
		updateDelegates [(int)SceneState.Load] = UpdateSceneLoad;
		updateDelegates [(int)SceneState.Unload] = UpdateSceneUnload;
		updateDelegates [(int)SceneState.Postload] = UpdateScenePostload;
		updateDelegates [(int)SceneState.Ready] = UpdateSceneReady;
		updateDelegates [(int)SceneState.Run] = UpdateSceneRun;

		nextSceneName = "Menu Scene";
		sceneState = SceneState.Reset;
		GetComponent<Camera>().orthographicSize = Screen.height/2;

	}

	protected void OnDestroy()
	{
		if(updateDelegates != null)
		{
			for(int i = 0; i < (int)SceneState.Count; i++)
			{
				updateDelegates[i] = null;
			}
			updateDelegates = null;
		}
		updateDelegates = null;

		if(mainController != null)
		{
			mainController = null;
		}
	}

	protected void OnDisable()
	{
	}

	protected void OnEnable()
	{	
	}
	
	protected void Start () 
	{
	}

	protected void Update () 
	{
		if(updateDelegates[(int) sceneState] != null)
		{
			updateDelegates[(int) sceneState]();
		}
	}


	//======================================================================================
	// private methods
	//======================================================================================

	void UpdateSceneReset ()
	{
		System.GC.Collect ();
		sceneState = SceneState.Preload;
	}

	void UpdateScenePreload ()
	{
		sceneLoadTask = Application.LoadLevelAsync (nextSceneName);
		sceneState = SceneState.Load;
	}	

	void UpdateSceneLoad ()
	{
		if(sceneLoadTask.isDone == true)
		{
			sceneState = SceneState.Unload;
		}
		else
		{

		}
	}

	void UpdateSceneUnload ()
	{
		if(resourceUnloadTask == null)
		{
			resourceUnloadTask = Resources.UnloadUnusedAssets();
		}
		else
		{
			if( resourceUnloadTask.isDone == true)
			{
				resourceUnloadTask = null;
				sceneState = SceneState.Postload;
			}
		}
	}

	void UpdateScenePostload ()
	{
		currentSceneName = nextSceneName;
		sceneState = SceneState.Ready;
	}

	void UpdateSceneReady ()
	{
		System.GC.Collect ();
		sceneState = SceneState.Run;
	}

	void UpdateSceneRun ()
	{
		if(currentSceneName != nextSceneName)
		{
			sceneState = SceneState.Reset;
		}
	}

}

public class Room : Loadable
{
	public bool loadedOnStart = true;
	public Loadable[] roomObjects;

	void Start()
	{
		if (!loadedOnStart)
			Unload();
	}

	public override void Load()
	{
		gameObject.SetActive(true);
		foreach (Loadable loadable in roomObjects)
		{
			loadable.Load();
		}
	}

	public override void Unload()
	{
		gameObject.SetActive(false);
		foreach (Loadable loadable in roomObjects)
		{
			loadable.Unload();
		}
	}
}

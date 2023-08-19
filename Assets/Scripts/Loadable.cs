using UdonSharp;

public abstract class Loadable : UdonSharpBehaviour
{
    public abstract void Load();
    public abstract void Unload();
}

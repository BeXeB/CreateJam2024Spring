public delegate void OnCleared(IClearable clearable);
public interface IClearable
{
    public event OnCleared OnCleared;
}

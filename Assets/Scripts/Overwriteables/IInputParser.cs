public interface IInputParser<out T>
{
    public T Parse(string from);
}

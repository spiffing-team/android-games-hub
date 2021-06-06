public interface IView 
{
    void Show();
    void Hide();
    event EventHandler OnEnd;
}

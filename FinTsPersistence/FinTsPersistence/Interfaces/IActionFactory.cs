namespace FinTsPersistence.Interfaces
{
    public interface IActionFactory
    {
        IAction GetAction(string actionName);
    }
}
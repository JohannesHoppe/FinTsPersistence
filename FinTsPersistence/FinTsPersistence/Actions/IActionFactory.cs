namespace FinTsPersistence.Actions
{
    public interface IActionFactory
    {
        IAction GetAction(string actionName);
    }
}
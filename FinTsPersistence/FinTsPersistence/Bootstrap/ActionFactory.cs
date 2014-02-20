using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Metadata;
using FinTsPersistence.Interfaces;

namespace FinTsPersistence.Bootstrap
{
    public class ActionFactory : IActionFactory
    {
        public const string ActionName = "actionName";

        private readonly IEnumerable<Meta<IAction>> actions;

        public ActionFactory(IEnumerable<Meta<IAction>> actions)
        {
            this.actions = actions;
        }

        public IAction GetAction(string actionName)
        {
            return actions.First(a => a.Metadata[ActionName].Equals(actionName)).Value;
        }
    }
}

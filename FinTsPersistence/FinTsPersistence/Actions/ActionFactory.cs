using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Metadata;

namespace FinTsPersistence.Actions
{
    /// <summary>
    /// Resolves the right action (actuall FinTS/HBCI job to do) via the given name
    /// </summary>
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
            var firstOrDefault = actions.FirstOrDefault(a => a.Metadata[ActionName].Equals(actionName));

            if (firstOrDefault == null)
            {
                throw new ArgumentException(String.Format("Unkown action: {0}!", actionName), "actionName");
            }
            
            return firstOrDefault.Value;
        }
    }
}

﻿using System.Collections.Specialized;
using Subsembly.FinTS;

namespace FinTsPersistence.Actions
{
    /// <summary>
    /// Empty pseuso-action.
    /// </summary>
    public class ActionSync : ActionBase
    {
        public const string ActionName = "sync";

        protected override bool OnParse(string sAction, StringDictionary vsArgsDict)
        {
            return true;
        }

        protected override FinOrder OnCreateOrder(FinService aService)
        {
            return null;
        }
    }
}
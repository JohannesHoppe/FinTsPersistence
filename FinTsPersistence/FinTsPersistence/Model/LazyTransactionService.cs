using System;
using System.Collections.Specialized;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using System.Linq;
using FinTsPersistence.Code;

namespace FinTsPersistence.Model
{
    /// <summary>
    /// This is a very simple solution to synchronize to database
    /// The LAZY transaction service tries to avoid complex logic and duplicate-checking by only recognizing transactions until the last day
    /// 
    /// Strategy:
    /// The service will first determine that very last stored transaction and then adds all newer transactions but not the todays transactions.
    /// The service can be run more than once a day, but in that case nothing will happen.
    /// 
    /// Pros:
    /// 1. It is very easy to identify the transactions to store, because only full days are considered.
    /// 2. Since it is not required to identify last stored transaction and the first new transaction
    ///    it is also not necessary to check for duplicates, because only full days are considered.
    /// 
    /// Cons:
    /// 1. The current day is ignored and not visible.
    /// 
    /// Requirements & Assumption:
    /// This strategy assumes that the historic data send by the bank never changes again!
    /// This strategy heavily relies on the EntryDate. 
    /// </summary>
    //TODO: verification: order of transactions (by EntryDate)
    public class LazyTransactionService : ITransactionService
    {
        private readonly IFinTsService finTsService;
        private readonly IDate date;
        private readonly ITransactionRepository transactionRepository;

        public LazyTransactionService(ITransactionRepository transactionRepository, IFinTsService finTsService, IDate date)
        {
            this.finTsService = finTsService;
            this.transactionRepository = transactionRepository;
            this.date = date;
        }

        public IActionResult DoPersistence(StringDictionary arguments)
        {
            var lastStoredTransaction = transactionRepository.GetLastTransaction();
            
            var nextDayToPersist = lastStoredTransaction.IsNoTransaction() ?
                new DateTime(date.Now.Year, 1, 1) : 
                lastStoredTransaction.EntryDate.AddDays(1);

            DateTime yesterday = date.Now.AddDays(-1);

            arguments = arguments.NewCopy();
            arguments.Add(Arguments.FromDate, nextDayToPersist.ToIsoDate());

            IActionResult result = finTsService.DoAction(ActionPersist.ActionName, arguments);

            if (result.Status != Status.Success)
            {
                return result;
            }

            var filteredTransactions = result.Response.Transactions
                .Where(t => t.EntryDate <= yesterday)
                .Select(x => x.ToTransaction())
                .ToList();

            if (filteredTransactions.Any())
            {
                transactionRepository.SaveTransactions(filteredTransactions);
            }

            return result;
        }
    }
}

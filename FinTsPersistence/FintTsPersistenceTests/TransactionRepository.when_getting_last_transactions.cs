using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;
using It = Machine.Specifications.It;

namespace FintTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_getting_last_transactions : from_TransactionRepository_with_six_transactions
    {
        static List<Transaction> result;
        Because of = () => result = repository.GetLastTransactions(2).ToList();

        It should_only_return_the_requested_last_days = () => result.Should().OnlyContain(t => new[] {4, 5, 6}.Contains(t.TransactionId));
    }
}

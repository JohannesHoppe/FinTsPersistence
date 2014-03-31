using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace FintTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_trying_to_get_last_transaction : from_TransactionRepository_with_no_transactions
    {
        static Transaction result;
        Because of = () => result = repository.GetLastTransaction();

        It should_return_the_no_transaction_object = () => result.IsNoTransaction().Should().BeTrue();
    }
}

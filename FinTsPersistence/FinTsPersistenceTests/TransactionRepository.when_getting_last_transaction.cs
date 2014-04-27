using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace FinTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_getting_last_transaction : from_TransactionRepository_with_six_transactions
    {
        static Transaction result;
        Because of = () => result = repository.GetLastTransaction();

        It should_only_return_the_requested_last_days = () => result.TransactionId.Should().Be(6);
    }
}

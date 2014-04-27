using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace FinTsPersistenceTests
{
    [Subject("TransactionRepository")]
    internal class when_comparing_totally_equal_transactions
    {
        private static bool result;

        private Because of = () => result = new Transaction
                                                {
                                                    Value = 10,
                                                    Name = "equal",
                                                    PaymentPurpose = "equal"
                                                }.IsVeryEqualTo(new Transaction
                                                                    {
                                                                        Value = 10,
                                                                        Name = "equal",
                                                                        PaymentPurpose = "equal"
                                                                    });

        private It should_be_considered_as_very_equal = () => result.Should().BeTrue();
    }
}

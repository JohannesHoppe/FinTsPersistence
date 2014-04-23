using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace FintTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_comparing_totally_equal_transactions
    {
        static bool result;
        Because of = () => result = new Transaction
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

        It should_be_considered_as_very_equal = () => result.Should().BeTrue();
    }
}

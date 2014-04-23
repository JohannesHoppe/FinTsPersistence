using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;

namespace FintTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_comparing_very_similar_transactions_with_different_value
    {
        static bool result;
        Because of = () => result = new Transaction
                                        {
                                            Value = 10,
                                            Name = "equal",
                                            PaymentPurpose = "equal"
                                        }.IsVeryEqualTo(new Transaction
                                                            {
                                                                Value = 11,
                                                                Name = "equal",
                                                                PaymentPurpose = "equal"
                                                            });

        It should_not_be_considered_as_very_equal = () => result.Should().BeFalse();
    }
}
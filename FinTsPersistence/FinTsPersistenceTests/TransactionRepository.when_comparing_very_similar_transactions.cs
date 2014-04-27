using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;

namespace FintTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_comparing_very_similar_transactions
    {
        static bool result;
        Because of = () => result = new Transaction
                                        {
                                            Value = 10,
                                            Name = "EQUAL MÜller",
                                            PaymentPurpose = "equal"
                                        }.IsVeryEqualTo(new Transaction
                                                            {
                                                                Value = 10,
                                                                Name = "equal Muller",
                                                                PaymentPurpose = "equal"
                                                            });

        It should_be_considered_as_very_equal = () => result.Should().BeTrue();
    }
}
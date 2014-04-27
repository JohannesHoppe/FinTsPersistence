using System;
using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;

namespace FinTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_comparing_very_similar_transactions_with_different_valuedate
    {
        static bool result;
        Because of = () => result = new Transaction
                                        {
                                            ValueDate = new DateTime(2014, 04, 23), 
                                            Value = 10,
                                            Name = "equal",
                                            PaymentPurpose = "equal"
                                        }.IsVeryEqualTo(new Transaction
                                                            {
                                                                ValueDate = new DateTime(2014, 04, 24), 
                                                                Value = 10,
                                                                Name = "equal",
                                                                PaymentPurpose = "equal"
                                                            });

        It should_not_be_considered_as_very_equal = () => result.Should().BeFalse();
    }
}
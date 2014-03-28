using FakeDbSet;
using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using It = Machine.Specifications.It;

namespace FintTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_reveiving_old_transactions
    {
        static TransactionRepository repository;
        static List<Transaction> result;

        Establish context = () =>
        {
            InMemoryDbSet<Transaction> inMemoryDbSet = new InMemoryDbSet<Transaction>
            {
                new Transaction { TransactionId = 1, EntryDate = new DateTime(2014, 03, 1) },
                new Transaction { TransactionId = 2, EntryDate = new DateTime(2014, 03, 2)},
                new Transaction { TransactionId = 3, EntryDate = new DateTime(2014, 03, 2)},
                new Transaction { TransactionId = 4, EntryDate = new DateTime(2014, 03, 3)},
                new Transaction { TransactionId = 5, EntryDate = new DateTime(2014, 03, 3)},
                new Transaction { TransactionId = 6, EntryDate = new DateTime(2014, 03, 4)}
            };

            var mockedContext = new Mock<ITransactionContext>();
            mockedContext.Setup(m => m.Transactions).Returns(inMemoryDbSet);
            repository = new TransactionRepository(mockedContext.Object);
        };

        Because of = () => result = repository.GetLastTransactions(2).ToList();

        private It should_only_return_the_requested_last_days = () => result.Should().OnlyContain(t => new[] {4, 5, 6}.Contains(t.TransactionId));

    }
}

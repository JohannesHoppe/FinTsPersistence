using System;
using FakeDbSet;
using FinTsPersistence.Model;
using Machine.Specifications;
using Moq;

namespace FintTsPersistenceTests
{
    class from_TransactionRepository_with_six_transactions
    {
        internal static TransactionRepository repository;

        internal Establish context = () =>
        {
            InMemoryDbSet<Transaction> inMemoryDbSet = new InMemoryDbSet<Transaction>(true)
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

    }
}

using FakeDbSet;
using FinTsPersistence.Model;
using Machine.Specifications;
using Moq;

namespace FintTsPersistenceTests
{
    class from_TransactionRepository_with_no_transactions
    {
        internal static TransactionRepository repository;

        internal Establish context = () =>
        {
            InMemoryDbSet<Transaction> inMemoryDbSet = new InMemoryDbSet<Transaction>(true);


            var mockedContext = new Mock<ITransactionContext>();
            mockedContext.Setup(m => m.Transactions).Returns(inMemoryDbSet);
            repository = new TransactionRepository(mockedContext.Object);
        };

    }
}

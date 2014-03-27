using FakeDbSet;
using FinTsPersistence.Model;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace FintTsPersistenceTests
{
    [Subject("TransactionRepository")]
    class when_reveiving_old_transaction
    {
        static TransactionRepository repository;

        Establish context = () =>
        {
            InMemoryDbSet<Transaction> inMemoryDbSet = new FakeDbSet.InMemoryDbSet<Transaction>
            {
                new Transaction { TransactionId = 1, EntryDate = new DateTime(2014, 03, 1) },
                new Transaction { TransactionId = 2, EntryDate = new DateTime(2014, 03, 2)},
                new Transaction { TransactionId = 3, EntryDate = new DateTime(2014, 03, 3)},
                new Transaction { TransactionId = 4, EntryDate = new DateTime(2014, 03, 4)}
            };         


            var context = new Mock<T>

            repository = new TransactionRepository();
        };


        Because of => ()
        {
        


        }
    }
}

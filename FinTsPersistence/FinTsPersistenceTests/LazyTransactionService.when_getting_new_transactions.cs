using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using FinTsPersistence;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;
using Status = FinTsPersistence.Actions.Result.Status;

namespace FinTsPersistenceTests
{
    [Subject("LazyTransactionService")]
    internal class when_getting_new_transactions
    {
        static LazyTransactionService service;
        static Mock<ITransactionRepository> repository;
        static ActionResult result;

        Establish context = () =>
        {
            var today = new DateTime(2014, 03, 5);
            var yesterday = new DateTime(2014, 03, 4);
            var lastStoredTransaction = new Transaction { TransactionId = 6, EntryDate = yesterday };

            ActionResult mockedResult = new ActionResult(Status.Success)
                {
                    Response = new ResponseData
                        {
                            Transactions = new List<FinTsTransaction>()
                        }
                };

            var finTsService = new Mock<IFinTsService>();
            finTsService.Setup(m => m.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()))
                .Returns(mockedResult);

            repository = new Mock<ITransactionRepository>();
            repository.Setup(m => m.GetLastTransaction()).Returns(lastStoredTransaction);

            var date = new Mock<IDate>();
            date.Setup(m => m.Now).Returns(today);

            service = new LazyTransactionService(finTsService.Object, repository.Object, date.Object);
        };

        private Because of = () => result = service.DoPersistence(new StringDictionary()); 

        private It should_call_GetLastTransaction_from_repository = () => repository.Verify(m => m.GetLastTransaction(), Times.Once);

    }
}

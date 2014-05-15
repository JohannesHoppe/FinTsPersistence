using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using FinTsPersistence;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using FinTsPersistence.Code;
using FinTsPersistence.Model;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;
using Status = FinTsPersistence.Actions.Result.Status;

namespace FinTsPersistenceTests
{
    [Subject("LazyTransactionService")]
    internal class when_receiving_an_error
    {
        static LazyTransactionService service;
        static Mock<ITransactionRepository> repository;
        static Mock<IFinTsService> finTsService;
        static IActionResult result;
        static DateTime today;
        static DateTime lastStoredDay;      

        Establish context = () =>
        {
            today              = new DateTime(2014, 03, 5);
            lastStoredDay      = new DateTime(2014, 03, 3);
            var lastStoredTransaction = new Transaction { TransactionId = 1, EntryDate = lastStoredDay };

            repository = new Mock<ITransactionRepository>();
            repository.Setup(m => m.GetLastTransaction()).Returns(lastStoredTransaction);

            var mockedfinTsServiceResult = new Mock<IActionResult>();
            mockedfinTsServiceResult.Setup(m => m.Status).Returns(Status.ErrorResult);

            finTsService = new Mock<IFinTsService>();
            finTsService.Setup(m => m.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()))
                .Returns(mockedfinTsServiceResult.Object);

            var date = new Mock<IDate>();
            date.Setup(m => m.Now).Returns(today);

            var io = new Mock<IInputOutput>();
            io.Setup(m => m.Error).Returns(new Mock<IOutputError>().Object);
            io.Setup(m => m.Info).Returns(new Mock<IOutputInfo>().Object);

            service = new LazyTransactionService(repository.Object, finTsService.Object, date.Object, io.Object);
        };

        Because of = () => result = service.DoPersistence(new StringDictionary());                                                       

        It should_call_GetLastTransaction_from_repository = () => repository.Verify(v => v.GetLastTransaction(), Times.Once);

        It should_call_FinTsService = () => finTsService.Verify(v => v.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()));

        It should_not_call_SaveTransactions_from_repository = () => repository.Verify(v => v.SaveTransactions(Moq.It.IsAny<IEnumerable<Transaction>>()), Times.Never);

        It should_return_an_error_result = () => result.Status.Should().Be(Status.ErrorResult);
    }
}

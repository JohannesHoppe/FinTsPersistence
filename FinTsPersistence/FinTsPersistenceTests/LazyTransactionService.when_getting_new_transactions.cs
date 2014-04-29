using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
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
        static Mock<IFinTsService> finTsService;
        static StringDictionary finTsServiceArguments;
        static ActionResult result;

        Establish context = () =>
        {
            var today = new DateTime(2014, 03, 5);
            var yesterday = new DateTime(2014, 03, 4);
            var twoDaysBefore = new DateTime(2014, 03, 3);
            var lastStoredTransaction = new Transaction { TransactionId = 1, EntryDate = twoDaysBefore };

            repository = new Mock<ITransactionRepository>();
            repository.Setup(m => m.GetLastTransaction()).Returns(lastStoredTransaction);

            ActionResult mockedResult = new ActionResult(Status.Success)
                {
                    Response = new ResponseData
                        {
                            Transactions = new List<FinTsTransaction>
                            {
                                new Transaction { EntryDate = new DateTime(2014, 03, 4) },
                                new Transaction { EntryDate = new DateTime(2014, 03, 4) },
                                new Transaction { EntryDate = new DateTime(2014, 03, 4) },
                                new Transaction { EntryDate = new DateTime(2014, 03, 5) }
                            }
                        }
                };

            finTsService = new Mock<IFinTsService>();
            finTsService.Setup(m => m.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()))
                .Returns(mockedResult)
                .Callback<string, StringDictionary>((action, arguments) => finTsServiceArguments = arguments);


            var date = new Mock<IDate>();
            date.Setup(m => m.Now).Returns(today);

            service = new LazyTransactionService(repository.Object, finTsService.Object, date.Object);
        };

        private Because of = () => result = service.DoPersistence(new StringDictionary()); 

        private It should_call_GetLastTransaction_from_repository = () => repository.Verify(v => v.GetLastTransaction(), Times.Once);

        private It should_call_FinTsService =
            () => finTsService.Verify(
                v => v.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()));

        private It should_call_FinTsService_with_the_next_day_after_lastStoredTransaction = () => finTsServiceArguments[Arguments.FromDate].Should().Be("2014-03-4");

    }
}

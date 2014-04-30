using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    internal class when_getting_no_new_transactions
    {
        static LazyTransactionService service;
        static Mock<ITransactionRepository> repository;
        static Mock<IFinTsService> finTsService;
        static StringDictionary finTsService_doAction_Arguments;
        static IActionResult result;
        static DateTime today;
        static DateTime oneDayAfterLastDay;
        static DateTime lastStoredDay;      

        Establish context = () =>
        {
            today               = new DateTime(2014, 03, 5);
            oneDayAfterLastDay  = new DateTime(2014, 03, 5);
            lastStoredDay       = new DateTime(2014, 03, 4);
            var lastStoredTransaction = new Transaction { TransactionId = 1, EntryDate = lastStoredDay };

            repository = new Mock<ITransactionRepository>();
            repository.Setup(m => m.GetLastTransaction()).Returns(lastStoredTransaction);

            ActionResult mockedfinTsServiceResult = new ActionResult(Status.Success)
                {
                    Response = new ResponseData
                        {
                            Transactions = new List<FinTsTransaction>
                            {
                                new Transaction { EntryDate = new DateTime(2014, 03, 5) },
                                new Transaction { EntryDate = new DateTime(2014, 03, 5) },
                                new Transaction { EntryDate = new DateTime(2014, 03, 6) },
                            }
                        }
                };

            finTsService = new Mock<IFinTsService>();
            finTsService.Setup(m => m.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()))
                .Returns(mockedfinTsServiceResult)
                .Callback<string, StringDictionary>((action, arguments) => finTsService_doAction_Arguments = arguments);

            var date = new Mock<IDate>();
            date.Setup(m => m.Now).Returns(today);

            service = new LazyTransactionService(repository.Object, finTsService.Object, date.Object);
        };

        Because of = () => result = service.DoPersistence(new StringDictionary());                                                       

        It should_call_GetLastTransaction_from_repository = () => repository.Verify(v => v.GetLastTransaction(), Times.Once);

        It should_call_FinTsService = () => finTsService.Verify(v => v.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()));

        It should_call_FinTsService_with_one_day_after_last_day = () => finTsService_doAction_Arguments[Arguments.FromDate].Should().Be(oneDayAfterLastDay.ToIsoDate());

        It should_not_call_SaveTransactions_from_repository = () => repository.Verify(v => v.SaveTransactions(Moq.It.IsAny<IEnumerable<Transaction>>()), Times.Never);

        It should_return_a_successfull_result = () => result.Status.Should().Be(Status.Success);
    }
}

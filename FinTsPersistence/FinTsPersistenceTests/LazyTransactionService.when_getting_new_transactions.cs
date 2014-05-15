using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    internal class when_getting_new_transactions
    {
        static LazyTransactionService service;
        static Mock<ITransactionRepository> repository;
        static List<Transaction> repository_SaveTransactions_Argument;
        static Mock<IFinTsService> finTsService;
        static StringDictionary finTsService_doAction_Arguments;
        static IActionResult result;
        static DateTime today;
        static DateTime oneDayAfterLastDay;
        static DateTime lastStoredDay;      

        Establish context = () =>
        {
            today               = new DateTime(2014, 03, 5);
            oneDayAfterLastDay  = new DateTime(2014, 03, 4);
            lastStoredDay       = new DateTime(2014, 03, 3);
            var lastStoredTransaction = new Transaction { TransactionId = 1, EntryDate = lastStoredDay };

            repository = new Mock<ITransactionRepository>();
            repository.Setup(m => m.GetLastTransaction()).Returns(lastStoredTransaction);

            repository.Setup(m => m.SaveTransactions(Moq.It.IsAny<IEnumerable<Transaction>>()))
                .Callback<IEnumerable<Transaction>>(c => repository_SaveTransactions_Argument = c.ToList());

            var mockedfinTsServiceResult = new Mock<IActionResult>();
            mockedfinTsServiceResult.Setup(m => m.Status).Returns(Status.Success);
            mockedfinTsServiceResult.Setup(m => m.Response.Transactions).Returns(new List<FinTsTransaction>
                {
                    new Transaction { EntryDate = new DateTime(2014, 03, 4) },
                    new Transaction { EntryDate = new DateTime(2014, 03, 4) },
                    new Transaction { EntryDate = new DateTime(2014, 03, 4) },
                    new Transaction { EntryDate = new DateTime(2014, 03, 5) },
                    new Transaction { EntryDate = new DateTime(2014, 03, 5) },
                    new Transaction { EntryDate = new DateTime(2014, 03, 6) },
                });

            finTsService = new Mock<IFinTsService>();
            finTsService.Setup(m => m.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()))
                .Returns(mockedfinTsServiceResult.Object)
                .Callback<string, StringDictionary>((action, arguments) => finTsService_doAction_Arguments = arguments);


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

        It should_call_FinTsService_with_one_day_after_last_day = () => finTsService_doAction_Arguments[Arguments.FromDate].Should().Be(oneDayAfterLastDay.ToIsoDate());

        It should_insert_the_correct_number_of_transactions = () => repository_SaveTransactions_Argument.Should().HaveCount(3);

        It should_only_insert_dates_OnOrBefore_one_day_after_last_day = () => repository_SaveTransactions_Argument.ForEach(x => x.EntryDate.Should().BeOnOrAfter(oneDayAfterLastDay));

        It should_never_insert_dates_of_today = () => repository_SaveTransactions_Argument.ForEach(x => x.EntryDate.Should().NotBe(today));

        It should_never_insert_dates_of_the_future = () => repository_SaveTransactions_Argument.ForEach(x => x.EntryDate.Should().BeBefore(today));

        It should_return_a_successfull_result = () => result.Status.Should().Be(Status.Success);
    }
}

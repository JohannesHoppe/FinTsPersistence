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
    internal class when_starting_with_an_empty_database
    {
        static LazyTransactionService service;
        static Mock<ITransactionRepository> repository;
        static List<Transaction> repository_SaveTransactions_Argument;
        static Mock<IFinTsService> finTsService;
        static StringDictionary finTsService_doAction_Arguments;
        static IActionResult result;
        static DateTime today;
        static DateTime firstDayOfYear;    

        Establish context = () =>
        {
            today          = new DateTime(2014, 03, 5);
            firstDayOfYear = new DateTime(2014, 01, 1);

            repository = new Mock<ITransactionRepository>();
            repository.Setup(m => m.GetLastTransaction()).Returns(new NoTransaction());

            repository.Setup(m => m.SaveTransactions(Moq.It.IsAny<IEnumerable<Transaction>>()))
                .Callback<IEnumerable<Transaction>>(c => repository_SaveTransactions_Argument = c.ToList());
                
            finTsService = new Mock<IFinTsService>();
            finTsService.Setup(m => m.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()))
                .Returns(new Mock<IActionResult>().Object)
                .Callback<string, StringDictionary>((action, arguments) => finTsService_doAction_Arguments = arguments);

            var date = new Mock<IDate>();
            date.Setup(m => m.Now).Returns(today);

            service = new LazyTransactionService(repository.Object, finTsService.Object, date.Object);
        };

        Because of = () => result = service.DoPersistence(new StringDictionary()); 

        It should_call_GetLastTransaction_from_repository = () => repository.Verify(v => v.GetLastTransaction(), Times.Once);

        It should_call_FinTsService = () => finTsService.Verify(v => v.DoAction(ActionPersist.ActionName, Moq.It.IsAny<StringDictionary>()));

        It should_call_FinTsService_with_first_day_of_year = () => finTsService_doAction_Arguments[Arguments.FromDate].Should().Be(firstDayOfYear.ToIsoDate());
    }
}

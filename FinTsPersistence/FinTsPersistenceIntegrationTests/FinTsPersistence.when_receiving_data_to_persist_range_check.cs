using System;
using System.Linq;
using FinTsPersistence;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using FinTsPersistenceIntegrationTests.Helper;
using FluentAssertions;
using Machine.Specifications;
using Status = FinTsPersistence.Actions.Result.Status;

namespace FinTsPersistenceIntegrationTests
{
    /// <summary>
    /// Receiving data to persist from FinTS/HBCI
    /// This test ensures that the following assumptions are correct:
    /// 1. FromDate is related to 'EntryDate' (and NOT 'BookingDate')
    /// 2. FromDate defines the given day to be included (BeOnOrAfter)
    /// </summary>
    [Subject("FinTsPersistence")]
    public class when_receiving_when_receiving_data_to_persist_range_check
    {
        static string contactfileLocation;
        static CmdArguments cmdArguments;
        static DateTime fromDate;
        static IActionResult result1;
        static FinTsTransaction exampleTransaction;
        static IActionResult result2;

        Establish context = () =>
        {
            contactfileLocation = IntegrationTestData.GetContacfileLocation();
            cmdArguments = IntegrationTestData.GetCmdArguments();
            fromDate = DateTime.Now.AddDays(-7).Date;
        };

        Because of = () => 
        {
            // first: finding a date with transactions
            result1 = ContainerConfig.Resolve<IFinTsService>().DoAction(
            ActionPersist.ActionName,
            (new CommandLineHelper(null)).ExtractArguments(new[]
                {
                    "XXX",
                    Arguments.ContactFile, contactfileLocation,
                    Arguments.Pin, cmdArguments.Pin,
                    Arguments.AcctNo, cmdArguments.Acctno,
                    Arguments.AcctBankCode, cmdArguments.Acctbankcode,
                    Arguments.FromDate, fromDate.ToIsoDate()
                }
            ).Arguments);

            exampleTransaction = result1.Response.Transactions.First();

            // second: verifying assumption
            result2 = ContainerConfig.Resolve<IFinTsService>().DoAction(
            ActionPersist.ActionName,
            (new CommandLineHelper(null)).ExtractArguments(new[]
                {
                    "XXX",
                    Arguments.ContactFile, contactfileLocation,
                    Arguments.Pin, cmdArguments.Pin,
                    Arguments.AcctNo, cmdArguments.Acctno,
                    Arguments.AcctBankCode, cmdArguments.Acctbankcode,
                    Arguments.FromDate, exampleTransaction.EntryDate.ToIsoDate()
                }
            ).Arguments);
        };

        It should_execute_successfully = () => result2.Status.Should().Be(Status.Success);
        It should_return_a_list_of_transactions = () => result2.Response.Transactions.Should().NotBeEmpty();
        It should_return_transactions_with_entrydates_of_the_right_time = () => result2.Response.Transactions.First().EntryDate.Should().Be(exampleTransaction.EntryDate);
    }
}
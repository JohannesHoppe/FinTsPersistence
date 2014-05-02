using System;
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
        static IActionResult result2;

        Establish context = () =>
        {
            contactfileLocation = IntegrationTestData.GetContacfileLocation();
            cmdArguments = IntegrationTestData.GetCmdArguments();
            fromDate = DateTime.Now.AddDays(-10).Date;
        };

        Because of = () => 
        {
            result1 = ContainerConfig.Resolve<IFinTsService>().DoAction(
            ActionPersist.ActionName,
            (new CommandLineHelper(null)).ExtractArguments(new[]
            {
                "XXX",
                Arguments.ContactFile, contactfileLocation,
                Arguments.Pin, cmdArguments.Pin,
                Arguments.AcctNo, cmdArguments.Acctno,
                Arguments.AcctBankCode, cmdArguments.Acctbankcode,
                Arguments.Format, "csv",
                Arguments.FromDate, fromDate.ToIsoDate()
            }).Arguments);
        };

        It should_execute_successfully = () => result.Status.Should().Be(Status.Success);
        It should_return_a_list_of_transactions = () => result.Response.Transactions.Should().NotBeEmpty();
        It should_not_return_formatted_output = () => result.Response.Formatted.Should().BeNull();

        It should_return_transactions_with_entrydates_of_the_right_time = () => result.Response.Transactions.ForEach(x => x.EntryDate.Should().BeOnOrAfter(fromDate));
        It should_return_transactions_valuedates_of_the_right_time = () => result.Response.Transactions.ForEach(x => x.ValueDate.Should().BeOnOrAfter(fromDate));
    }
}
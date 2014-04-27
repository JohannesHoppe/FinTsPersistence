using System;
using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using FinTsPersistenceIntegrationTests.Helper;
using FluentAssertions;
using Machine.Specifications;
using Status = FinTsPersistence.Actions.Result.Status;

namespace FinTsPersistenceIntegrationTests
{
    /// <summary>
    /// Just getting a CSV statement (not required for persistence)
    /// </summary>
    [Subject("FinTsPersistence")]
    public class when_receiving_a_csv_statement
    {
        static string contactfileLocation;
        static CmdArguments cmdArguments;
        static DateTime fromDate;
        static ActionResult result;

        Establish context = () =>
        {
            contactfileLocation = IntegrationTestData.GetContacfileLocation();
            cmdArguments = IntegrationTestData.GetCmdArguments();
            fromDate = DateTime.Now.AddDays(-4).Date;
        };

        Because of = () => result = FinTsPersistence.Start.DoAction(new[]
            {
                ActionStatement.ActionName, 
                Arguments.ContactFile, contactfileLocation,
                Arguments.Pin, cmdArguments.Pin,
                Arguments.AcctNo, cmdArguments.Acctno,
                Arguments.AcctBankCode, cmdArguments.Acctbankcode,
                Arguments.Format, "csv",
                Arguments.FromDate, fromDate.ToIsoDate()
            });

        It should_execute_successfully = () => result.Success.Should().BeTrue();
        It should_have_status_ok = () => result.Status.Should().Be(Status.Success);
        It should_return_some_formatted_output = () => result.Response.Formatted.Should().NotBeEmpty();
    }
}
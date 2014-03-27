using FinTsPersistence.Actions.Result;
using FintTsPersistenceIntegrationTests.Helper;
using FluentAssertions;
using Machine.Specifications;
using Status = FinTsPersistence.Actions.Result.Status;

namespace FintTsPersistenceIntegrationTests
{
    /// <summary>
    /// Jst getting a CSV statement (not required for persistance)
    /// </summary>
    [Subject("FinTsPersistence")]
    public class when_receiving_a_csv_statement
    {
        static string contactfileLocation;
        static CmdArguments cmdArguments;
        static ActionResult result;

        Establish context = () =>
        {
            contactfileLocation = IntegrationTestData.GetContacfileLocation();
            cmdArguments = IntegrationTestData.GetCmdArguments();
        };

        Because of = () => result = FinTsPersistence.Start.DoAction(new[]
            {
                "statement", 
                "-contactfile", contactfileLocation,
                "-pin", cmdArguments.Pin,
                "-acctno", cmdArguments.Acctno,
                "-acctbankcode", cmdArguments.Acctbankcode,
                "-format", "csv",
                "-fromdate", "2014-03-10"
            });

        It should_execute_succesfully = () => result.Success.Should().BeTrue();
        It should_have_status_ok = () => result.Status.Should().Be(Status.Success);
        It should_return_a_list_of_transactions = () => result.Response.Transactions.Should().NotBeEmpty();
    }
}
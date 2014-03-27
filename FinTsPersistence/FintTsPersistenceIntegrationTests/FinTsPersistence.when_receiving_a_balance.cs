using FinTsPersistence.Actions.Result;
using FintTsPersistenceIntegrationTests.Helper;
using FluentAssertions;
using Machine.Specifications;
using Status = FinTsPersistence.Actions.Result.Status;

namespace FintTsPersistenceIntegrationTests
{
    /// <summary>
    /// Just receving a balance (not required for persistance) 
    /// </summary>
    [Subject("FinTsPersistence")]
    public class when_receiving_a_balance
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
                "balance", 
                "-contactfile", contactfileLocation,
                "-pin", cmdArguments.Pin,
                "-acctno", cmdArguments.Acctno,
                "-acctbankcode", cmdArguments.Acctbankcode
            });

        It should_execute_succesfully = () => result.Success.Should().BeTrue();
        It should_have_status_ok = () => result.Status.Should().Be(Status.Success);
    }
}
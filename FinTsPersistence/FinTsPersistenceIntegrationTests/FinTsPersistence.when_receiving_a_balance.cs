using FinTsPersistence.Actions;
using FinTsPersistence.Actions.Result;
using FinTsPersistenceIntegrationTests.Helper;
using FluentAssertions;
using Machine.Specifications;
using Status = FinTsPersistence.Actions.Result.Status;

namespace FinTsPersistenceIntegrationTests
{
    /// <summary>
    /// Just receiving a balance (not required for persistence) 
    /// </summary>
    [Subject("FinTsPersistence")]
    public class when_receiving_a_balance
    {
        static string contactfileLocation;
        static CmdArguments cmdArguments;
        static IActionResult result;

        Establish context = () =>
            {
                contactfileLocation = IntegrationTestData.GetContacfileLocation();
                cmdArguments = IntegrationTestData.GetCmdArguments();
            };

        Because of = () => result = FinTsPersistence.Start.DoAction(new[]
            {
                ActionBalance.ActionName, 
                Arguments.ContactFile, contactfileLocation,
                Arguments.Pin, cmdArguments.Pin,
                Arguments.AcctNo, cmdArguments.Acctno,
                Arguments.AcctBankCode, cmdArguments.Acctbankcode
            });

        It should_execute_successfully = () => result.Status.Should().Be(Status.Success);
    }
}
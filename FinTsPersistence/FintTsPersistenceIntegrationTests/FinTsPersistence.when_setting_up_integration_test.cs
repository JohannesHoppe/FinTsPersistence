using FintTsPersistenceIntegrationTests.Helper;
using FluentAssertions;
using Machine.Specifications;

namespace FintTsPersistenceIntegrationTests
{
    /// <summary>
    /// Makes sure that all other integration tests are executable
    /// </summary>
    [Subject("FinTsPersistence")]
    public class when_setting_up_integration_test
    {
        static string contactfileLocation;
        static string cmdArgumentsLocation;
        private static CmdArguments cmdArguments;

        Because of = () =>
            {
                contactfileLocation = IntegrationTestData.GetContacfileLocation();
                cmdArgumentsLocation = IntegrationTestData.GetCmdArgumentsLocation();
                cmdArguments = IntegrationTestData.GetCmdArguments();
            };

        It a_contactfile_must_exist = () => contactfileLocation.Should().Contain("\\Contactfile.xml");
        It a_cmdArguments_file_must_exist = () => cmdArgumentsLocation.Should().Contain("\\CmdArguments.xml");
        
        It a_pin_must_be_set = () => cmdArguments.Pin.Should().NotBeEmpty();
        It an_account_number_must_be_set = () => cmdArguments.Acctno.Should().NotBeEmpty();
        It an_account_bankcode_must_be_set = () => cmdArguments.Acctbankcode.Should().NotBeEmpty();
    }
}
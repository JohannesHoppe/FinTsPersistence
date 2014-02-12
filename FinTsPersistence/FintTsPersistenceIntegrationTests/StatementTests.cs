using FintTsPersistenceIntegrationTests.Helper;
using Machine.Specifications;

namespace FintTsPersistenceIntegrationTests
{
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

        It a_contactfile_must_exist = () => contactfileLocation.ShouldContain("\\Contactfile.xml");
        It a_cmdArguments_file_must_exist = () => cmdArgumentsLocation.ShouldContain("\\CmdArguments.xml");
        
        It a_pin_must_be_set = () => cmdArguments.Pin.ShouldNotBeEmpty();
        It an_account_number_must_be_set = () => cmdArguments.Acctno.ShouldNotBeEmpty();
        It an_account_bankcode_must_be_set = () => cmdArguments.Acctbankcode.ShouldNotBeEmpty();
    }

    /// <summary>
    /// Kontoumsatzdaten herunterladen
    /// </summary>
    [Subject("FinTsPersistence")]
    public class when_receiving_a_balance
    {
        static string contactfileLocation;
        static CmdArguments cmdArguments;
        static int exitCode;

        Establish context = () =>
        {
            contactfileLocation = IntegrationTestData.GetContacfileLocation();
            cmdArguments = IntegrationTestData.GetCmdArguments();
        };

        Because of = () => exitCode = FinTsPersistence.FinTsPersistence.Main(new[]
            {
                "balance", 
                "-contactfile", contactfileLocation,
                "-pin", cmdArguments.Pin,
                "-acctno", cmdArguments.Acctno,
                "-acctbankcode", cmdArguments.Acctbankcode
            });

        It the_exit_code_should_equal_20 = () => exitCode.ShouldEqual(20);
    }


}

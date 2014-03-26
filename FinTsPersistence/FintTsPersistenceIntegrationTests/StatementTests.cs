﻿using FinTsPersistence.Actions.Result;
using FintTsPersistenceIntegrationTests.Helper;
using FluentAssertions;
using Machine.Specifications;
using Status = FinTsPersistence.Actions.Result.Status;

#pragma warning disable 169
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

        It a_contactfile_must_exist = () => contactfileLocation.Should().Contain("\\Contactfile.xml");
        It a_cmdArguments_file_must_exist = () => cmdArgumentsLocation.Should().Contain("\\CmdArguments.xml");
        
        It a_pin_must_be_set = () => cmdArguments.Pin.Should().NotBeEmpty();
        It an_account_number_must_be_set = () => cmdArguments.Acctno.Should().NotBeEmpty();
        It an_account_bankcode_must_be_set = () => cmdArguments.Acctbankcode.Should().NotBeEmpty();
    }

    /// <summary>
    /// Kontoumsatzdaten herunterladen
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

    /// <summary>
    /// Getting a CSV statement
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
#pragma warning restore 169

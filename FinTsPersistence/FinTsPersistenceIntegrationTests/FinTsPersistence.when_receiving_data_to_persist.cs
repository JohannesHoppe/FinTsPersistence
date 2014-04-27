using System;
using FinTsPersistence.Actions.Result;
using FintTsPersistenceIntegrationTests.Helper;
using FluentAssertions;
using Machine.Specifications;
using Status = FinTsPersistence.Actions.Result.Status;

namespace FintTsPersistenceIntegrationTests
{
    /// <summary>
    /// Receving data to persist from FinTS/HBCI
    /// </summary>
    [Subject("FinTsPersistence")]
    public class when_receiving_when_receiving_data_to_persist
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
                "persist", 
                "-contactfile", contactfileLocation,
                "-pin", cmdArguments.Pin,
                "-acctno", cmdArguments.Acctno,
                "-acctbankcode", cmdArguments.Acctbankcode,
                "-format", "csv",
                "-fromdate", fromDate.ToString("yyyy-MM-dd")
            });

        It should_execute_successfully = () => result.Success.Should().BeTrue();
        It should_have_status_ok = () => result.Status.Should().Be(Status.Success);
        It should_return_a_list_of_transactions = () => result.Response.Transactions.Should().NotBeEmpty();
        It should_not_return_formatted_output = () => result.Response.Formatted.Should().BeNull();

        It should_return_transactions_with_entrydates_of_the_right_time = () => result.Response.Transactions.ForEach(x => x.EntryDate.Should().BeOnOrAfter(fromDate));
        It should_return_transactions_valuedates_of_the_right_time = () => result.Response.Transactions.ForEach(x => x.ValueDate.Should().BeOnOrAfter(fromDate));
        It should_return_transactions_with_a_name = () => result.Response.Transactions.ForEach(x => x.Name.Should().NotBeNullOrWhiteSpace());
        It should_return_transactions_with_a_payment_purpose = () => result.Response.Transactions.ForEach(x => x.PaymentPurpose.Should().NotBeNullOrWhiteSpace());
        It should_return_transactions_with_an_amount = () => result.Response.Transactions.ForEach(x => x.Value.Should().NotBe(0));
    }
}
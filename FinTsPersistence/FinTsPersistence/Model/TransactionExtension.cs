using FinTsPersistence.Actions.Result;

namespace FinTsPersistence.Model
{
    public static class TransactionExtension
    {
        public static Transaction ToTransaction(this FinTsTransaction fitTsTransaction)
        {
            return new Transaction
                {
                    EntryDate = fitTsTransaction.EntryDate,
                    ValueDate = fitTsTransaction.ValueDate,
                    Value = fitTsTransaction.Value,
                    AccountNumber = fitTsTransaction.AccountNumber,
                    BankCode = fitTsTransaction.BankCode,
                    Name = fitTsTransaction.Name,
                    PaymentPurpose = fitTsTransaction.PaymentPurpose,
                    EntryText = fitTsTransaction.EntryText,
                    PrimaNotaNumber = fitTsTransaction.PrimaNotaNumber,
                    TransactionTypeIdentificationCode = fitTsTransaction.TransactionTypeIdentificationCode,
                    TransactionZkaCode = fitTsTransaction.TransactionZkaCode,
                    TextKeyExtension = fitTsTransaction.TextKeyExtension,
                    BankReference = fitTsTransaction.BankReference,
                    OwnerReference = fitTsTransaction.OwnerReference,
                    SupplementaryDetails = fitTsTransaction.SupplementaryDetails
                };
        }

    }
}

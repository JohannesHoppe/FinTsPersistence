using System;

namespace FinTsPersistence.Actions.Result
{
    public class Transaction
    {
        public Transaction(
            DateTime entryDate,
            DateTime valueDate,
            decimal value,
            string accountNumber,
            string bankCode,
            string name,
            string paymentPurpose,
            string entryText,
            string primaNotaNumber,
            string transactionTypeIdentificationCode,
            string transactionZkaCode,
            string textKeyExtension,
            string bankReference,
            string ownerReference,
            string supplementaryDetails)
        {
            EntryDate = entryDate;
            ValueDate = valueDate;
            Value = value;
            AccountNumber = accountNumber;
            BankCode = bankCode;
            Name = name;
            PaymentPurpose = paymentPurpose;
            EntryText = entryText;
            PrimaNotaNumber = primaNotaNumber;
            TransactionTypeIdentificationCode = transactionTypeIdentificationCode;
            TransactionZkaCode = transactionZkaCode;
            TextKeyExtension = textKeyExtension;
            BankReference = bankReference;
            OwnerReference = ownerReference;
            SupplementaryDetails = supplementaryDetails;
        }

        /// <summary>
        /// The Entry Date (Buchungsdatum) of this transaction. 
        /// </summary>
        /// <remarks>Virtual Dates are not considered</remarks>
        public DateTime EntryDate
        {
            get;
            private set;
        }

        /// <summary>
        /// The Value Date (Wertstellungstag) of this transaction. 
        /// </summary>
        /// <remarks>Virtual Dates are not considered</remarks>
        public DateTime ValueDate
        {
            get;
            private set;
        }

        /// <summary>
        /// The actual decimal value of this transaction with sign.
        /// </summary>
        public decimal Value
        {
            get;
            private set;
        }

        /// <summary>
        /// The account number of the other payee or payer (not the account owner).
        /// </summary>
        public string AccountNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// The bank code number of the other payee or payer account (not the owner account).
        /// </summary>
        public string BankCode
        {
            get;
            private set;
        }

        /// <summary>
        /// The combined, complete payee/payer name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// The raw payment purpose extracted from field 86.
        /// </summary>
        public string PaymentPurpose
        {
            get;
            set;
        }

        /// <summary>
        /// The optional entry text (Buchungstext) of this transaction.
        /// </summary>
        public string EntryText
        {
            get;
            private set;
        }

        /// <summary>
        /// The optional prima nota number (Primanota-Nummer) of this transaction.
        /// </summary>
        public string PrimaNotaNumber
        {
            get;
            private set;
        }

        /// <summary>
        /// The Transaction Type Identification Code (Buchungsschlüssel) of this transaction.
        /// </summary>
        public string TransactionTypeIdentificationCode
        {
            get;
            private set;
        }

        /// <summary>
        /// The ZKA defined numeric transaction code (Geschäftsvorfall-Code) of this transaction.
        /// </summary>
        public string TransactionZkaCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Three digit text key extension of this transaction.
        /// </summary>
        public string TextKeyExtension
        {
            get;
            private set;
        }

        /// <summary>
        /// The optional Account Servicing Institution’s Reference (Bankreferenz) of this transaction.
        /// </summary>
        public string BankReference
        {
            get;
            private set;
        }

        /// <summary>
        /// The Reference for the Account Owner (Kundenreferenz) of this transaction.
        /// </summary>
        public string OwnerReference
        {
            get;
            private set;
        }

        /// <summary>
        /// The optional Supplementary Details (Weitere Informationen) of this transaction.
        /// </summary>
        public string SupplementaryDetails
        {
            get;
            private set;
        }
    }
}

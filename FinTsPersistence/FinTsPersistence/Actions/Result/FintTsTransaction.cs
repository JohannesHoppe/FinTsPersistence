using System;

namespace FinTsPersistence.Actions.Result
{
    /// <summary>
    /// Holds all relevant information about one transaction which was revieved via FinTS/HBCI
    /// </summary>
    public class FintTsTransaction
    {
        public FintTsTransaction()
        {
        }

        public FintTsTransaction(
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
            set;
        }

        /// <summary>
        /// The Value Date (Wertstellungstag) of this transaction. 
        /// </summary>
        /// <remarks>Virtual Dates are not considered</remarks>
        public DateTime ValueDate
        {
            get;
            set;
        }

        /// <summary>
        /// The actual decimal value of this transaction with sign.
        /// </summary>
        public decimal Value
        {
            get;
            set;
        }

        /// <summary>
        /// The account number of the other payee or payer (not the account owner).
        /// </summary>
        public string AccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The bank code number of the other payee or payer account (not the owner account).
        /// </summary>
        public string BankCode
        {
            get;
            set;
        }

        /// <summary>
        /// The combined, complete payee/payer name.
        /// </summary>
        public string Name
        {
            get;
            set;
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
            set;
        }

        /// <summary>
        /// The optional prima nota number (Primanota-Nummer) of this transaction.
        /// </summary>
        public string PrimaNotaNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The FintTsTransaction Type Identification Code (Buchungsschlüssel) of this transaction.
        /// </summary>
        public string TransactionTypeIdentificationCode
        {
            get;
            set;
        }

        /// <summary>
        /// The ZKA defined numeric transaction code (Geschäftsvorfall-Code) of this transaction.
        /// </summary>
        public string TransactionZkaCode
        {
            get;
            set;
        }

        /// <summary>
        /// Three digit text key extension of this transaction.
        /// </summary>
        public string TextKeyExtension
        {
            get;
            set;
        }

        /// <summary>
        /// The optional Account Servicing Institution’s Reference (Bankreferenz) of this transaction.
        /// </summary>
        public string BankReference
        {
            get;
            set;
        }

        /// <summary>
        /// The Reference for the Account Owner (Kundenreferenz) of this transaction.
        /// </summary>
        public string OwnerReference
        {
            get;
            set;
        }

        /// <summary>
        /// The optional Supplementary Details (Weitere Informationen) of this transaction.
        /// </summary>
        public string SupplementaryDetails
        {
            get;
            set;
        }
    }
}

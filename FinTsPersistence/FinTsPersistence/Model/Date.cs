using System;

namespace FinTsPersistence.Model
{
    public class Date : IDate
    {
        private readonly DateTime date;

        /// <summary>
        /// Stores date so that we can't struggle with date-changes during runtime
        /// </summary>
        public Date()
        {
            date = DateTime.Now.Date;
        }

        public DateTime Now
        {
            get { return date; }
        }
    }
}

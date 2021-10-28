using EmployeeApi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Domain.ValueObjects
{
    public class DateOnly : ValueObject
    {
        public DateTime Date { get; set; }

        private DateOnly(DateTime date)
        {
            Date = new DateTime(date.Year, date.Month, date.Day);
        }

        public int Year
        {
            get
            {
                return Date.Year;
            }
        }

        public int Month
        {
            get
            {
                return Date.Month;
            }
        }

        public int Day
        {
            get
            {
                return Date.Day;
            }
        }


        public static DateOnly From(DateTime date)
        {
            DateOnly newDate = new DateOnly(date);
            return newDate;
        }

        public static DateTime ToDate(DateOnly date)
        {
            DateTime newDate = new DateTime(date.Year, date.Month, date.Day);
            return newDate;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Date;
        }
    }
}

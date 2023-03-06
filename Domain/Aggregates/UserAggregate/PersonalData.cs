using Contracts;
using Domain.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Domain.Aggregates.UserAggregate
{
    public class PersonalData : ValueObject
    {
        private const string EMAIL_PATTERN = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        private const int MIN_LENGHT = 2;
        private const int MAX_LENGTH = 25;

        private readonly static Regex _emailPattern;

        static PersonalData()
        {
            _emailPattern = new Regex(EMAIL_PATTERN);
        }

        public string Email { get; private set; }

        public string Name { get; private set; }

        public string LastName { get; private set; }


        public PersonalData(string email, string name, string lastName)
        {
            Email = EnsuredUtils.EnsureStringIsNotEmptyAndMathPattern(
                email,
                _emailPattern);

            Name = EnsuredUtils.EnsureStringLengthIsCorrect(
                name,
                MIN_LENGHT,
                MAX_LENGTH);

            LastName = EnsuredUtils.EnsureStringLengthIsCorrect(
                lastName,
                MIN_LENGHT,
                MAX_LENGTH);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Email;
            yield return Name;
            yield return LastName;
        }


    }
}

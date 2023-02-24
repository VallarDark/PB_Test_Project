using Contracts;
using Domain.Exceptions;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Domain.Utils
{
    public static class EnsuredUtils
    {
        const string DEFAULT_STRING_NOT_EMPTY_ERROR = "Parameter can't be null or empty";
        const string DEFAULT_STRING_LENGTH_ERROR = "Parameter length is not valid";
        const string DEFAULT_STRING_NOT_MATCH_PATTERN_ERROR = "Parameter should match the pattern";
        const string DEFAULT_NOT_NULL_ERROR = "Parameter can't be null";
        const string DEFAULT_NOT_EMPTY_COLLECTION_ERROR = "Collection can't be empty or null";
        const string DEFAULT_WRONG_COLLECTION_ITEMS_COUNT_ERROR = "Wrong collection items count";
        const string DEFAULT_NOMBER_IS_NON_NEGATIVE_ERROR = "Parameter can't be less than 0";
        const string DEFAULT_NOMBER_IS_LESS_THEN_VALUE_ERROR = "Parameter can't be less then value";
        const string DEFAULT_NOMBER_IS_MORE_THEN_VALUE_ERROR = "Parameter can't be more then value";
        const string DEFAULT_ITEM_SHOULD_EXISTS_ERROR = "Current item does not exist";
        const string DEFAULT_ITEM_SHOULD_NOT_EXISTS_ERROR = "Item with same data already exists";
        const string DEFAULT_SAME_DATA_PARAM_ERROR = "Current element data is the same with new value";

        public static string EnsureStringIsNotEmptyAndMathPattern(
            string data,
            Regex regex = null,
            string errorMsg = DEFAULT_STRING_NOT_MATCH_PATTERN_ERROR)
        {
            var result = EnsureStringIsNotEmpty(data, errorMsg);

            if (regex != null && !regex.IsMatch(data))
            {
                throw new ArgumentException(errorMsg);
            }

            return result;
        }

        public static string EnsureStringIsNotEmpty(
            string data,
            string errorMsg = DEFAULT_STRING_NOT_EMPTY_ERROR)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException(errorMsg);
            }

            return data;
        }

        public static string EnsureStringLengthIsCorrect(
            string data,
            int minLength,
            int maxLength,
            string errorMsg = DEFAULT_STRING_LENGTH_ERROR)
        {
            EnsureStringIsNotEmpty(data);

            if (data.Length < minLength || data.Length > maxLength)
            {
                throw new ArgumentException(errorMsg);
            }

            return data;
        }

        public static T EnsureNotNull<T>(
            T data,
            string errorMsg = DEFAULT_NOT_NULL_ERROR)
        {
            if (data == null)
            {
                throw new NullValueException(errorMsg);
            }

            return data;
        }

        public static IEnumerable<T> EnsureNotEmptyCollection<T>(
           IEnumerable<T> collection,
           string errorMsg = DEFAULT_NOT_EMPTY_COLLECTION_ERROR)
        {
            EnsureNotNull(collection, errorMsg);

            if (!collection.Any())
            {
                throw new EmptyCollectionException(errorMsg);
            }

            return collection;
        }

        public static IEnumerable<T> EnsureCollectionItemsCountIsMoreOrEqualValue<T>(
           IEnumerable<T> collection,
           int count,
           string errorMsg = DEFAULT_WRONG_COLLECTION_ITEMS_COUNT_ERROR)
        {
            EnsureNotNull(collection, errorMsg);

            if (collection.Count() < count)
            {
                throw new WrongCollectionItemsCountException(errorMsg);
            }

            return collection;
        }

        public static Unit EnsureItemNotExists<T>(
           IReadeableRepository<T> repository,
           Expression<Func<T, bool>> predicate,
           string errorMsg = DEFAULT_ITEM_SHOULD_NOT_EXISTS_ERROR) where T : EntityBase
        {
            var existedData = repository.Get(predicate);

            if (existedData != null)
            {
                throw new ItemAlreadyExistsException(errorMsg);
            }

            return default(Unit);
        }

        public static T EnsureItemExists<T>(
          IReadeableRepository<T> repository,
          Expression<Func<T, bool>> predicate,
          string errorMsg = DEFAULT_ITEM_SHOULD_EXISTS_ERROR) where T : EntityBase
        {
            var existedData = repository.Get(predicate);

            if (existedData == null)
            {
                throw new ItemNotExistsException(errorMsg);
            }

            return existedData;
        }

        public static Unit EnsureNewValueIsNotSame<T>(
            T oldValue,
            T newValue,
            string errorMsg = DEFAULT_SAME_DATA_PARAM_ERROR)
        {
            if (oldValue.Equals(newValue))
            {
                throw new ArgumentException(errorMsg);
            }

            return default(Unit);
        }

        public static int EnsureNomberIsNonNegative(
            int nomber,
            string errorMsg = DEFAULT_NOMBER_IS_NON_NEGATIVE_ERROR)
        {
            if (nomber < 0)
            {
                throw new ArgumentException(errorMsg);
            }

            return nomber;
        }

        public static void EnsurePageParamsAreCorrect(
            int pageNumber,
            int countPerPage,
            int minItemsCount,
            int maxItemsCount)
        {
            EnsureNumberIsMoreOrEqualValue(pageNumber,
                1,
                "Invalid page number. Pagination starts from 1 st index");

            EnsureNumberIsMoreOrEqualValue(
                countPerPage,
                minItemsCount,
                $"Count can't be less then {minItemsCount}");

            EnsureNumberIsLessOrEqualValue(
                countPerPage,
                maxItemsCount,
                $"Count can't be more then {maxItemsCount}");
        }

        public static T EnsureNumberIsMoreOrEqualValue<T>(
            T nomber,
            T value,
            string errorMsg = DEFAULT_NOMBER_IS_LESS_THEN_VALUE_ERROR) where T : IComparable
        {
            if (nomber.CompareTo(value) < 0)
            {
                throw new ArgumentException(errorMsg);
            }

            return nomber;
        }

        public static T EnsureNumberIsLessOrEqualValue<T>(
            T nomber,
            T value,
            string errorMsg = DEFAULT_NOMBER_IS_MORE_THEN_VALUE_ERROR) where T : IComparable
        {
            if (nomber.CompareTo(value) > 0)
            {
                throw new ArgumentException(errorMsg);
            }

            return nomber;
        }
    }
}

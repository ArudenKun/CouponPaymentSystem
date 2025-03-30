#region Copyright

/* The MIT License (MIT)

Copyright (c) 2014 Anderson Luiz Mendes Matos (Brazil)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

#endregion Copyright

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using DataTables.AspNet.Core;
using DataTables.AspNet.Core.NameConvention;

namespace DataTables.AspNet.Mvc5
{
    /// <summary>
    /// Represents a model binder for DataTables request element.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class ModelBinder : IModelBinder
    {
        /// <summary>
        /// Binds request data/parameters/values into a 'IDataTablesRequest' element.
        /// </summary>
        /// <param name="controllerContext">Controller context for execution.</param>
        /// <param name="bindingContext">Binding context for data/parameters/values.</param>
        /// <returns>An IDataTablesRequest object or null if binding was not possible.</returns>
        public object BindModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext
        )
        {
            return BindModel(
                controllerContext,
                bindingContext,
                DataTableConfiguration.Options,
                ParseAdditionalParameters
            );
        }

        /// <summary>
        /// For internal and testing use only.
        /// Binds request data/parameters/values into a 'IDataTablesRequest' element.
        /// </summary>
        /// <param name="controllerContext">Controller context for execution.</param>
        /// <param name="bindingContext">Binding context for data/parameters/values.</param>
        /// <param name="options">DataTables.AspNet global options.</param>
        /// <param name="parseAdditionalParameters"></param>
        /// <returns>An IDataTablesRequest object or null if binding was not possible.</returns>
        public object BindModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            IOptions options,
            Func<
                ControllerContext,
                ModelBindingContext,
                IDictionary<string, object>
            > parseAdditionalParameters
        )
        {
            if (options == null || options.RequestNameConvention == null)
                return null;

            var values = bindingContext.ValueProvider;

            // Accordingly to DataTables docs, it is recommended to receive/return draw casted as int for security reasons.
            // This is meant to help prevent XSS attacks.
            var draw = values.GetValue(options.RequestNameConvention.Draw);
            int _draw = 0;
            if (options.IsDrawValidationEnabled && !Parse(draw, out _draw))
                return null;

            var start = values.GetValue(options.RequestNameConvention.Start);
            Parse(start, out int _start);

            var length = values.GetValue(options.RequestNameConvention.Length);
            // ReSharper disable once RedundantAssignment
            // ReSharper disable once InlineOutVariableDeclaration
            int _length = options.DefaultPageLength;
            Parse(length, out _length);

            var searchValue = values.GetValue(options.RequestNameConvention.SearchValue);
            Parse(searchValue, out string _searchValue);

            var searchRegex = values.GetValue(options.RequestNameConvention.IsSearchRegex);
            Parse(searchRegex, out bool _searchRegex);

            var search = new Search(_searchValue, _searchRegex);

            // Parse columns & column sorting.
            var columns = ParseColumns(values, options.RequestNameConvention);
            // ReSharper disable once PossibleMultipleEnumeration
            _ = ParseSorting(columns, values, options.RequestNameConvention);

            var sortField = values.GetValue(
                string.Format(options.RequestNameConvention.SortField, 0)
            );
            Parse(sortField, out int _sortField);

            var sortDirection = values.GetValue(
                string.Format(options.RequestNameConvention.SortDirection, 0)
            );
            Parse(sortDirection, out string _sortDirection);

            if (options.IsRequestAdditionalParametersEnabled && parseAdditionalParameters != null)
            {
                var additionalParameters = parseAdditionalParameters(
                    controllerContext,
                    bindingContext
                );
                return new DataTablesRequest(
                    _draw,
                    _start,
                    _length,
                    search,
                    // ReSharper disable once PossibleMultipleEnumeration
                    columns,
                    _sortField,
                    _sortDirection,
                    additionalParameters
                );
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return new DataTablesRequest(
                _draw,
                _start,
                _length,
                search,
                columns,
                _sortField,
                _sortDirection
            );
        }

        /// <summary>
        /// Provides custom aditional parameters processing for your request.
        /// You have to implement this to populate 'IDataTablesRequest' object with aditional (user-defined) request values.
        /// </summary>
        public Func<
            ControllerContext,
            ModelBindingContext,
            IDictionary<string, object>
        > ParseAdditionalParameters;

        /// <summary>
        /// For internal use only.
        /// Parse column collection.
        /// </summary>
        /// <param name="values">Request parameters.</param>
        /// <param name="names">Name convention for request parameters.</param>
        /// <returns></returns>
        private static IEnumerable<IColumn> ParseColumns(
            IValueProvider values,
            IRequestNameConvention names
        )
        {
            var columns = new List<IColumn>();

            int counter = 0;
            while (true)
            {
                // Parses Field value.
                var columnField = values.GetValue(string.Format(names.ColumnField, counter));
                if (!Parse(columnField, out string _columnField))
                    break;

                // Parses Name value.
                var columnName = values.GetValue(string.Format(names.ColumnName, counter));
                if (!Parse(columnName, out string _columnName))
                    break;

                // Parses Orderable value.
                var columnSortable = values.GetValue(
                    string.Format(names.IsColumnSortable, counter)
                );
                Parse(columnSortable, out bool _columnSortable);

                // Parses Searchable value.
                var columnSearchable = values.GetValue(
                    string.Format(names.IsColumnSearchable, counter)
                );
                Parse(columnSearchable, out bool _columnSearchable);

                // Parsed Search value.
                var columnSearchValue = values.GetValue(
                    string.Format(names.ColumnSearchValue, counter)
                );
                Parse(columnSearchValue, out string _columnSearchValue);

                // Parses IsRegex value.
                var columnSearchRegex = values.GetValue(
                    string.Format(names.IsColumnSearchRegex, counter)
                );
                Parse(columnSearchRegex, out bool _columnSearchRegex);

                var search = new Search(_columnSearchValue, _columnSearchRegex, _columnField);

                // Instantiates a new column with parsed elements.
                var column = new Column(
                    _columnName,
                    _columnField,
                    _columnSearchable,
                    _columnSortable,
                    search
                );

                // Adds the column to the return collection.
                columns.Add(column);

                // Increments counter to keep processing columns.
                counter++;
            }

            return columns;
        }

        /// <summary>
        /// For internal use only.
        /// Parse sort collection.
        /// </summary>
        /// <param name="columns">Column collection to use when parsing sort.</param>
        /// <param name="values">Request parameters.</param>
        /// <param name="names">Name convention for request parameters.</param>
        /// <returns></returns>
        private static IEnumerable<ISort> ParseSorting(
            IEnumerable<IColumn> columns,
            IValueProvider values,
            IRequestNameConvention names
        )
        {
            var sorting = new List<ISort>();

            for (int i = 0; i < columns.Count(); i++)
            {
                var sortField = values.GetValue(string.Format(names.SortField, i));
                if (!Parse(sortField, out int _sortField))
                    break;

                var column = columns.ElementAt(_sortField);

                var sortDirection = values.GetValue(string.Format(names.SortDirection, i));
                Parse(sortDirection, out string _sortDirection);

                if (column.SetSort(i, _sortDirection))
                    sorting.Add(column.Sort);
            }

            return sorting;
        }

        /// <summary>
        /// Parses a possible raw value and transforms into a strongly-typed result.
        /// </summary>
        /// <typeparam name="TElementType">The expected type for result.</typeparam>
        /// <param name="value">The possible request value.</param>
        /// <param name="result">Returns the parsing result or default value for type is parsing failed.</param>
        /// <returns>True if parsing succeeded, False otherwise.</returns>
        private static bool Parse<TElementType>(ValueProviderResult value, out TElementType result)
        {
            result = default;

            if (value == null)
                return false;
            if (value.RawValue == null)
                return false;

            try
            {
                result = (TElementType)
                    Convert.ChangeType(value.AttemptedValue, typeof(TElementType));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

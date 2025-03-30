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

using System.Collections.Generic;
using DataTables.AspNet.Core;

namespace DataTables.AspNet.Mvc5
{
    /// <summary>
    /// For internal use only.
    /// Represents a DataTables request.
    /// </summary>
    internal class DataTablesRequest : IDataTablesRequest
    {
        public IDictionary<string, object> AdditionalParameters { get; }
        public IEnumerable<IColumn> Columns { get; }
        public int Draw { get; }
        public int Length { get; }
        public ISearch Search { get; }
        public int Start { get; }
        public int Order { get; }
        public SortDirection OrderDirection { get; }

        public DataTablesRequest(
            int draw,
            int start,
            int length,
            ISearch search,
            IEnumerable<IColumn> columns,
            int order,
            string orderDirection
        )
            : this(draw, start, length, search, columns, order, orderDirection, null) { }

        public DataTablesRequest(
            int draw,
            int start,
            int length,
            ISearch search,
            IEnumerable<IColumn> columns,
            int order,
            string orderDirection,
            IDictionary<string, object> additionalParameters
        )
        {
            Draw = draw;
            Start = start;
            Length = length;
            Search = search;
            Columns = columns;
            Order = order;
            OrderDirection = orderDirection
                .ToLowerInvariant()
                .Equals(DataTableConfiguration.Options.RequestNameConvention.SortDescending)
                ? SortDirection.Descending // Descending sort should be explicitly set.
                : SortDirection.Ascending; // Default (when set or not) is ascending sort.
            AdditionalParameters = additionalParameters;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace EzyPlanData
{
    /// <summary>
    /// Minimal CSV-reader contract replacing LumenWorks
    /// <c>CachedCsvReader</c>. Any CSV library (or the built-in
    /// <see cref="SimpleCsvReader"/>) can implement this; Core stays
    /// dependency-free and cross-platform.
    /// </summary>
    public interface ICsvReader : IEnumerable<string[]>, IDisposable
    {
        void MoveToStart();
        bool ReadNextRecord();
    }

    /// <summary>
    /// Small built-in CSV reader (comma split; trims whitespace).
    /// Adequate for survey imports, which are plain numeric CSVs.
    /// </summary>
    public sealed class SimpleCsvReader : ICsvReader
    {
        private readonly List<string[]> _rows = new List<string[]>();
        private bool _disposed;

        public SimpleCsvReader(StreamReader stream, bool hasHeaders)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            string line;
            bool first = true;
            while ((line = stream.ReadLine()) != null)
            {
                if (first && hasHeaders)
                {
                    first = false;
                    continue;
                }
                first = false;
                _rows.Add(line.Split(','));
            }
        }

        public void MoveToStart()
        {
        }

        public bool ReadNextRecord()
        {
            return _rows.Count > 0;
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}

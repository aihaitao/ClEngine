using System;
using System.Collections;
using System.Collections.Generic;
using ClEngine.Properties;

namespace ClEngine.CoreLibrary.IO.Csv
{
	public partial class CsvReader
	{
	    public struct RecordEnumerator : IEnumerator<string[]>
        {
	        private CsvReader _reader;
	        private string[] _current;
	        private long _currentRecordIndex;

	        public RecordEnumerator(CsvReader reader)
	        {
	            _reader = reader ?? throw new ArgumentException("reader");
	            _current = null;

	            _currentRecordIndex = reader._currentRecordIndex;
	        }

            public void Dispose()
            {
                _reader = null;
                _current = null;
            }

            public bool MoveNext()
            {
                if (_reader._currentRecordIndex != _currentRecordIndex)
                    throw new InvalidOperationException(
                        Resources.EnumVersionCheckFail);

                if (_reader.ReadNextRecord())
                {
                    if (_current == null)
                        _current = new string[_reader._fieldCount];

                    _reader.CopyCurrentRecordTo(_current);
                    _currentRecordIndex = _reader._currentRecordIndex;

                    return true;
                }

                _current = null;
                _currentRecordIndex = _reader._currentRecordIndex;

                return false;
            }

            public void Reset()
            {
                if (_reader._currentRecordIndex != _currentRecordIndex)
					throw new InvalidOperationException(Resources.EnumVersionCheckFail);

				_reader.MoveTo(-1);

				_current = null;
				_currentRecordIndex = _reader._currentRecordIndex;
            }

            public string[] Current => _current;

            object IEnumerator.Current => Current;
        }
	}
}
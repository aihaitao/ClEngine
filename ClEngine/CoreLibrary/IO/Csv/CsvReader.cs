using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ClEngine.Properties;

namespace ClEngine.CoreLibrary.IO.Csv
{
    public partial class CsvReader : IEnumerable<string[]>, IDisposable
    {
        public const char DefaultComment = '#';
        public const char DefaultEscape = '"';
        public const char DefaultQuote = '"';
        public const int DefaultBufferSize = 0x1000;

        /// <summary>
        /// 包含CSV文件中的当前记录索引
        /// <see cref="M:Int32.MinValue"/>的值意味着读者还没有被初始化。
        /// 否则，负值意味着没有记录被读取
        /// </summary>
        private long _currentRecordIndex;

        /// <summary>
        /// 包含处置状态标志
        /// </summary>
        private bool _isDisposed;
        public event EventHandler Disposed;
        private TextReader _reader;

        /// <summary>
        /// 包含用于多线程目的的锁定对象
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// 包含读取缓冲区
        /// </summary>
        private char[] _buffer;

        /// <summary>
        /// 指示是否已到达读取器的末尾
        /// </summary>
        private bool _eof;

        /// <summary>
        /// 指示是否初始化了类.
        /// </summary>
        private bool _initialized;

        /// <summary>
        /// 指示第一个记录是否处于缓存中。
        /// 当初始化没有页眉的阅读器时可能发生这种情况
        /// 因为必须读取一条记录以自动获取字段计数
        /// </summary>
        private bool _firstRecordInCache;

        /// <summary>
        /// 指示当前记录是否丢失字段。
        /// 在每个成功记录读取之后重置。
        /// </summary>
        private bool _missingFieldsFlag;

        /// <summary>
        /// 包含缓冲区大小
        /// </summary>
        private int _bufferSize;

        /// <summary>
        /// 包含字段标题
        /// </summary>
        private string[] _fieldHeaders;

        /// <summary>
        /// 包含当前读取缓冲区的长度
        /// </summary>
        private int _bufferLength;
        private int _fieldCount;
        private bool _skipEmptyLines;
        private int _nextFieldStart;
        private char _comment;
        private string[] _fields;
        private bool _hasHeaders;
        private Dictionary<string, int> _fieldHeaderIndexes;
        private bool _supportsMultiline;
        private char _quote;
        private char _escape;

        private int _nextFieldIndex;
        private bool _trimSpaces;
        private ParseErrorAction _defaultParseErrorAction;

        private char _delimiter;
        private MissingFieldAction _missingFieldAction = MissingFieldAction.ReplaceByNull;

        /// <summary>
        /// 包含字段标题比较器
        /// </summary>
        private static readonly StringComparer FieldHeaderComparer = StringComparer.CurrentCultureIgnoreCase;

        public RecordEnumerator GetEnumerator()
        {
            return new RecordEnumerator(this);
        }

        IEnumerator<string[]> IEnumerable<string[]>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ReadNextRecord()
        {
            return ReadNextRecord(false, false);
        }

        protected virtual bool ReadNextRecord(bool onlyReadHeaders, bool skipToNextLine)
        {
            if (_eof)
            {
                if (!_firstRecordInCache) return false;
                _firstRecordInCache = false;
                _currentRecordIndex++;

                return true;

            }

            CheckDisposed();

            if (!_initialized)
            {
                _buffer = new char[_bufferSize];
                
                _fieldHeaders = new string[0];

                if (!ReadBuffer())
                    return false;

                if (!SkipEmptyAndCommentedLines(ref _nextFieldStart))
                    return false;
                

                _fieldCount = 0;
                _fields = new string[16];

                while (ReadField(_fieldCount, true, false) != null)
                {
                    _fieldCount++;

                    if (_fieldCount == _fields.Length)
                        Array.Resize(ref _fields, (_fieldCount + 1) * 2);
                }
                
                _fieldCount++;

                if (_fields.Length != _fieldCount)
                    Array.Resize(ref _fields, _fieldCount);

                _initialized = true;
                
                if (_hasHeaders)
                {
                    _currentRecordIndex = -1;

                    _firstRecordInCache = false;

                    _fieldHeaders = new string[_fieldCount];
                    _fieldHeaderIndexes = new Dictionary<string, int>(_fieldCount, FieldHeaderComparer);

                    for (var i = 0; i < _fields.Length; i++)
                    {
                        _fieldHeaders[i] = _fields[i];
                        if (!_fieldHeaderIndexes.ContainsKey(_fields[i]))
                        {
                            _fieldHeaderIndexes.Add(_fields[i], i);
                        }
                    }

                    if (onlyReadHeaders) return true;
                    if (!SkipEmptyAndCommentedLines(ref _nextFieldStart))
                        return false;

                    Array.Clear(_fields, 0, _fields.Length);
                    _nextFieldIndex = 0;

                    _currentRecordIndex++;
                    return true;
                }

                if (onlyReadHeaders)
                {
                    _firstRecordInCache = true;
                    _currentRecordIndex = -1;
                }
                else
                {
                    _firstRecordInCache = false;
                    _currentRecordIndex = 0;
                }
            }
            else
            {
                if (skipToNextLine)
                    SkipToNextLine(ref _nextFieldStart);
                else if (_currentRecordIndex > -1 && !_missingFieldsFlag)
                {
                    if (_supportsMultiline)
                        ReadField(_fieldCount - 1, false, true);
                    else
                        if (_nextFieldIndex > 0)
                        SkipToNextLine(ref _nextFieldStart);
                }

                if (!_firstRecordInCache && !SkipEmptyAndCommentedLines(ref _nextFieldStart))
                    return false;
                
                if (_firstRecordInCache)
                    _firstRecordInCache = false;
                else
                {
                    Array.Clear(_fields, 0, _fields.Length);
                    _nextFieldIndex = 0;
                }

                _missingFieldsFlag = false;
                _currentRecordIndex++;
            }

            return true;
        }

        public string[] GetFieldHeaders()
        {
            EnsureInitialize();
            Debug.Assert(_fieldHeaders != null, Resources.FiledHeaderMustNull);

            var fieldHeaders = new string[_fieldHeaders.Length];

            for (int i = 0; i < fieldHeaders.Length; i++)
                fieldHeaders[i] = _fieldHeaders[i];

            return fieldHeaders;
        }

        private void EnsureInitialize()
        {
            if (!_initialized)
                ReadNextRecord(true, false);

            Debug.Assert(_fieldHeaders != null);
            Debug.Assert(_fieldHeaders.Length > 0 || (_fieldHeaders.Length == 0 && _fieldHeaderIndexes == null));
        }

#if DEBUG && !XBOX360 && !WINDOWS_8
        /// <summary>
        /// 包含对象分配时的堆栈
        /// </summary>
        private StackTrace _allocStack;
#endif

        public CsvReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment,
            bool trimSpaces, int bufferSize)
        {
#if DEBUG && !WINDOWS_8 && !UWP
            var stackTrace = new StackTrace();
            _allocStack = stackTrace;
#endif

            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (bufferSize <= 0)
                throw new Exception(Resources.BufferSizeSmall);

            _bufferSize = bufferSize;

            if (reader is StreamReader streamReader)
            {
                var stream = streamReader.BaseStream;

                if (stream.CanSeek)
                {
                    // 处理返回小于或等于0的错误实现
                    if (stream.Length > 0)
                        _bufferSize = (int) Math.Min(bufferSize, stream.Length);
                }
            }

            _reader = reader;
            _delimiter = delimiter;
            _quote = quote;
            _escape = escape;
            _comment = comment;

            _hasHeaders = hasHeaders;
            _trimSpaces = trimSpaces;
            _supportsMultiline = true;
            _skipEmptyLines = true;

            _currentRecordIndex = -1;
            _defaultParseErrorAction = ParseErrorAction.RaiseEvent;
        }

        public virtual void MoveTo(long record)
        {
            if (record < 0)
                throw new ArgumentOutOfRangeException($"{Resources.RecordIsBad}: {record}");

            if (record < _currentRecordIndex)
                throw new InvalidOperationException(Resources.CannotMovePreviousRecord);

            // 获取要读取的记录数

            var offset = record - _currentRecordIndex;

            if (offset <= 0) return;
            do
            {
                if (!ReadNextRecord())
                    throw new EndOfStreamException(Resources.CantReadRecord);
            }
            while (--offset > 0);
        }

        private void SkipToNextLine(ref int pos)
        {
            while ((pos < _bufferLength || (ReadBuffer() && ((pos = 0) == 0))) && !ParseNewLine(ref pos))
                pos++;
        }

        private bool ParseNewLine(ref int pos)
        {
            Debug.Assert(pos <= _bufferLength);
            
            if (pos == _bufferLength)
            {
                pos = 0;

                if (!ReadBuffer())
                    return false;
            }

            var c = _buffer[pos];

            if (c == '\r' && _delimiter != '\r')
            {
                pos++;

                if (pos < _bufferLength)
                {
                    if (_buffer[pos] == '\n')
                        pos++;
                }
                else
                {
                    if (ReadBuffer())
                    {
                        pos = _buffer[0] == '\n' ? 1 : 0;
                    }
                }

                if (pos < _bufferLength) return true;
                ReadBuffer();
                pos = 0;

                return true;
            }

            if (c != '\n') return false;
            pos++;

            if (pos < _bufferLength) return true;
            ReadBuffer();
            pos = 0;

            return true;

        }

        private string ReadField(int field, bool initializing, bool discardValue)
        {
            if (!initializing)
            {
                if (field < 0 || field >= _fieldCount)
                    throw new ArgumentOutOfRangeException("field is bad: " + field);

                if (_currentRecordIndex < 0)
                    throw new InvalidOperationException("No current record");//ExceptionMessage.NoCurrentRecord);
            }

            if (_fields[field] != null)
                return _fields[field];
            if (_missingFieldsFlag)
                return HandleMissingField(null, field, ref _nextFieldStart);

            CheckDisposed();

            var index = _nextFieldIndex;

            while (index < field + 1)
            {
                if (_nextFieldStart == _bufferLength)
                {
                    _nextFieldStart = 0;
                    
                    ReadBuffer();
                }

                string value = null;
                var eol = false;

                if (_missingFieldsFlag)
                {
                    value = HandleMissingField(value, index, ref _nextFieldStart);
                }
                else if (_nextFieldStart == _bufferLength)
                {
                    if (index == field)
                    {
                        if (!discardValue)
                        {
                            value = string.Empty;
                            _fields[index] = value;
                        }
                    }
                    else
                    {
                        value = HandleMissingField(value, index, ref _nextFieldStart);
                    }
                }
                else
                {
                    if (_trimSpaces)
                        SkipWhiteSpaces(ref _nextFieldStart);

                    if (_eof)
                        value = string.Empty;
                    else if (_buffer[_nextFieldStart] != _quote)
                    {
                        var start = _nextFieldStart;
                        var pos = _nextFieldStart;

                        for (; ; )
                        {
                            while (pos < _bufferLength)
                            {
                                var c = _buffer[pos];

                                if (c == _delimiter)
                                {
                                    _nextFieldStart = pos + 1;

                                    break;
                                }

                                if (c == '\r' || c == '\n')
                                {
                                    _nextFieldStart = pos;
                                    eol = true;

                                    break;
                                }
                                pos++;
                            }

                            if (pos < _bufferLength)
                                break;
                            if (!discardValue)
                                value += new string(_buffer, start, pos - start);

                            start = 0;
                            pos = 0;
                            _nextFieldStart = 0;

                            if (!ReadBuffer())
                                break;
                        }

                        if (!discardValue)
                        {
                            if (!_trimSpaces)
                            {
                                if (!_eof && pos > start)
                                    value += new string(_buffer, start, pos - start);
                            }
                            else
                            {
                                if (!_eof && pos > start)
                                {
                                    pos--;
                                    while (pos > -1 && IsWhiteSpace(_buffer[pos]))
                                        pos--;
                                    pos++;

                                    if (pos > 0)
                                        value += new string(_buffer, start, pos - start);
                                }
                                else
                                    pos = -1;
                                
                                if (pos <= 0)
                                {
                                    pos = value?.Length - 1 ?? -1;
                                    
                                    while (pos > -1 && IsWhiteSpace(value[pos]))
                                        pos--;

                                    pos++;

                                    if (pos > 0 && pos != value.Length)
                                        value = value.Substring(0, pos);
                                }
                            }

                            if (value == null)
                                value = string.Empty;
                        }

                        if (eol || _eof)
                        {
                            eol = ParseNewLine(ref _nextFieldStart);
                            
                            if (!initializing && index != _fieldCount - 1)
                            {
                                if (value != null && value.Length == 0)
                                    value = null;

                                value = HandleMissingField(value, index, ref _nextFieldStart);
                            }
                        }

                        if (!discardValue)
                            _fields[index] = value;
                    }
                    else
                    {
                        var start = _nextFieldStart + 1;
                        var pos = start;

                        var quoted = true;
                        var escaped = false;

                        for (; ; )
                        {
                            while (pos < _bufferLength)
                            {
                                var c = _buffer[pos];

                                if (escaped)
                                {
                                    escaped = false;
                                    start = pos;
                                }
                                else if (c == _escape &&
                                    (_escape != _quote ||
                                        (pos + 1 < _bufferLength && _buffer[pos + 1] == _quote) || (pos + 1 == _bufferLength && _reader.Peek() == _quote)))
                                {
                                    if (!discardValue)
                                        value += new string(_buffer, start, pos - start);

                                    escaped = true;
                                }
                                else if (c == _quote)
                                {
                                    quoted = false;
                                    break;
                                }

                                pos++;
                            }

                            if (!quoted)
                                break;
                            if (!discardValue && !escaped)
                                value += new string(_buffer, start, pos - start);

                            start = 0;
                            pos = 0;
                            _nextFieldStart = 0;

                            if (ReadBuffer()) continue;

#if !SILVERLIGHT && !XBOX360 && !WINDOWS_PHONE && !MONOGAME
                            HandleParseError(new Exception(), ref _nextFieldStart);
#endif
                            return null;
                        }

                        if (!_eof)
                        {
                            if (!discardValue && pos > start)
                                value += new string(_buffer, start, pos - start);
                            
                            _nextFieldStart = pos + 1;
                            
                            SkipWhiteSpaces(ref _nextFieldStart);
                            
                            bool delimiterSkipped;
                            if (_nextFieldStart < _bufferLength && _buffer[_nextFieldStart] == _delimiter)
                            {
                                _nextFieldStart++;
                                delimiterSkipped = true;
                            }
                            else
                            {
                                delimiterSkipped = false;
                            }
                            
                            if (!_eof && !delimiterSkipped && (initializing || index == _fieldCount - 1))
                                eol = ParseNewLine(ref _nextFieldStart);

#if !SILVERLIGHT && !XBOX360 && !WINDOWS_PHONE && !MONOGAME
                            if (!delimiterSkipped && !_eof && !(eol || IsNewLine(_nextFieldStart)))
                                HandleParseError(new Exception(), ref _nextFieldStart);
#endif
                        }

                        if (!discardValue)
                        {
                            if (value == null)
                                value = string.Empty;

                            _fields[index] = value;
                        }
                    }
                }

                _nextFieldIndex = eol ? 0 : Math.Max(index + 1, _nextFieldIndex);

                if (index == field)
                {

                    if (initializing && (eol || _eof))
                        return null;
                    return value;
                }

                index++;
            }
            
            HandleParseError(new Exception(), ref _nextFieldStart);
            return null;
        }

        public void CopyCurrentRecordTo(string[] array)
        {
            CopyCurrentRecordTo(array, 0);
        }

        public void CopyCurrentRecordTo(string[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (index < 0 || index >= array.Length)
                throw new Exception($"{Resources.IndexIsInCorrect}: {index}");

            if (_currentRecordIndex < 0 || !_initialized)
                throw new InvalidOperationException(Resources.NoCurrentRecord);

            if (array.Length - index < _fieldCount)
                throw new ArgumentException(Resources.NotEnoughSpaceInArray, nameof(array));

            for (var i = 0; i < _fieldCount; i++)
                array[index + i] = this[i];
        }

        public virtual string this[int field] => ReadField(field, false, false);

        private bool IsNewLine(int pos)
        {
            Debug.Assert(pos < _bufferLength);

            var c = _buffer[pos];

            if (c == '\n')
                return true;

            return c == '\r' && _delimiter != '\r';
        }

        private bool IsWhiteSpace(char c)
        {
            if (c == _delimiter)
                return false;
            if (c <= '\x00ff')
                return (c == ' ' || c == '\t');
            return System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.SpaceSeparator;
        }

        private void HandleParseError(Exception error, ref int pos)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            switch (_defaultParseErrorAction)
            {
                case ParseErrorAction.ThrowException:
                    throw error;

                case ParseErrorAction.RaiseEvent:
                    var e = new ParseErrorEventArgs(error, ParseErrorAction.ThrowException);
                    OnParseError(e);

                    switch (e.Action)
                    {
                        case ParseErrorAction.ThrowException:
                            throw e.Error;

                        case ParseErrorAction.RaiseEvent:
                            throw new InvalidOperationException(Resources.ParseErrorActionInvalid);

                        case ParseErrorAction.AdvanceToNextLine:
                            if (pos >= 0)
                                ReadNextRecord(false, true);
                            break;

                        default:
                            throw new NotSupportedException(Resources.ParseErrorActionNotSupport);
                    }
                    break;

                case ParseErrorAction.AdvanceToNextLine:
                    if (pos >= 0)
                        ReadNextRecord(false, true);
                    break;

                default:
                    throw new NotSupportedException(Resources.ParseErrorActionNotSupport);
            }
        }

        public event EventHandler<ParseErrorEventArgs> ParseError;

        protected virtual void OnParseError(ParseErrorEventArgs e)
        {
            var handler = ParseError;

            handler?.Invoke(this, e);
        }

        private void SkipWhiteSpaces(ref int pos)
        {
            for (; ; )
            {
                while (pos < _bufferLength && IsWhiteSpace(_buffer[pos]))
                    pos++;

                if (pos < _bufferLength)
                    break;
                pos = 0;

                if (!ReadBuffer()) return;
            }
        }

        private string HandleMissingField(string value, int fieldIndex, ref int currentPosition)
        {
            if (fieldIndex < 0 || fieldIndex >= _fieldCount)
                throw new ArgumentOutOfRangeException("fieldIndex is wrong: " + fieldIndex);

            _missingFieldsFlag = true;

            for (var i = fieldIndex + 1; i < _fieldCount; i++)
                _fields[i] = null;

            if (value != null)
                return value;
            switch (_missingFieldAction)
            {
                case MissingFieldAction.ParseError:
                    HandleParseError(new Exception(), ref currentPosition);
                    return null;

                case MissingFieldAction.ReplaceByEmpty:
                    return string.Empty;

                case MissingFieldAction.ReplaceByNull:
                    return null;

                default:
                    throw new NotSupportedException(
                        "Missing field action not supported");
            }
        }

        private bool SkipEmptyAndCommentedLines(ref int pos)
        {
            if (pos < _bufferLength)
                DoSkipEmptyAndCommentedLines(ref pos);

            while (pos >= _bufferLength && !_eof)
            {
                if (ReadBuffer())
                {
                    pos = 0;
                    DoSkipEmptyAndCommentedLines(ref pos);
                }
                else
                    return false;
            }

            return !_eof;
        }

        private void DoSkipEmptyAndCommentedLines(ref int pos)
        {
            while (pos < _bufferLength)
            {
                if (_buffer[pos] == _comment)
                {
                    pos++;
                    SkipToNextLine(ref pos);
                }
                else if (_skipEmptyLines && ParseNewLine(ref pos))
                    // ReSharper disable once RedundantJumpStatement
                    continue;
                else
                    break;
            }
        }

        private bool ReadBuffer()
        {
            if (_eof)
                return false;

            CheckDisposed();

            _bufferLength = _reader.Read(_buffer, 0, _bufferSize);

            if (_bufferLength > 0)
                return true;
            _eof = true;
            _buffer = null;

            return false;
        }

        protected void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            try
            {
                if (!disposing) return;
                if (_reader == null) return;
                lock (_lock)
                {
                    if (_reader == null) return;
                    _reader.Dispose();

                    _reader = null;
                    _buffer = null;
                    _eof = true;
                }
            }
            finally
            {
                _isDisposed = true;
                    
                try
                {
                    OnDisposed(EventArgs.Empty);
                }
                catch
                {
                    // ignored
                }
            }
        }

        protected virtual void OnDisposed(EventArgs e)
        {
            var handler = Disposed;

            handler?.Invoke(this, e);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
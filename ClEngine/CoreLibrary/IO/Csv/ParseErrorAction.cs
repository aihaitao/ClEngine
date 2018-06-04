namespace ClEngine.CoreLibrary.IO.Csv
{
    public enum ParseErrorAction
    {
        /// <summary>
        /// 引发<see cref="M:CsvReader.ParseError"/>事件
        /// </summary>
        RaiseEvent = 0,

        /// <summary>
        /// 尝试前进到下一行
        /// </summary>
        AdvanceToNextLine = 1,

        /// <summary>
        /// 引发异常
        /// </summary>
        ThrowException = 2,
    }
}
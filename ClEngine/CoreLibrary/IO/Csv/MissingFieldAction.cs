namespace ClEngine.CoreLibrary.IO.Csv
{
    public enum MissingFieldAction
    {
        /// <summary>
        /// 视为解析错误
        /// </summary>
        ParseError = 0,

        /// <summary>
        /// 以空值取代。
        /// </summary>
        ReplaceByEmpty = 1,

        /// <summary>
        /// 由空值替换 (<see langword="null"/>).
        /// </summary>
        ReplaceByNull = 2,
    }
}
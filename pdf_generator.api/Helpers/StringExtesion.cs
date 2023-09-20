namespace PDF.Generator.API.Helpers
{
    public static class StringExtesion
    {
        private const string _DOT = ".";
        public static string ReplaceFileExtensionWith(this string value, string fileExt)
        {
            if(string.IsNullOrWhiteSpace(value) || !value.Contains(_DOT)){
                return null;
            }
            var index = value.LastIndexOf(_DOT);
            var pureFilename = value.Substring(0, index);
            return pureFilename + _DOT + fileExt;
        }

        public static string GetFileExtesion(this string value){
            if(string.IsNullOrWhiteSpace(value) || !value.Contains(_DOT)){
                return null;
            }
            var index = value.LastIndexOf(_DOT);
            var pureFilename = value.Substring(0, index);
            return value.Substring(index, value.Length);
        }
    }
}
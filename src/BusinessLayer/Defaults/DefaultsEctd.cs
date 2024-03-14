namespace BusinessLayer.Defaults
{
    public static class DefaultsEctd
    {
        public static string Slash { get; private set; } = "/";

        public static string DtdSegment { get; private set; } = "dtd";

        public static string UtilSegment { get; private set; } = "util";

        public static string SchemaXPath { get; private set; } = "//@*[local-name()='schemaLocation']";

        public static string UtilDtdPath { get; private set; } = string.Concat(UtilSegment, Slash, DtdSegment, Slash);
    }
}
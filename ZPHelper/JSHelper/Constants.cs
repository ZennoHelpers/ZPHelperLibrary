namespace ZPHelper
{
    // ReSharper disable once InconsistentNaming
    public static class JSBool
    {
        public const string True = "true";
        public const string False = "false";
    }

    // ReSharper disable once InconsistentNaming
    public static class JSType
    {
        // @formatter:off
        // 5 primitive types:
        public const string Undefined = "undefined";
        public const string Null      = "null";
        public const string Boolean   = "boolean";
        public const string String    = "string";
        public const string Number    = "number";
        //
        public const string Object    = "object";
        public const string Symbol    = "symbol";
        public const string Function  = "function";
        // @formatter:on
    }

    // ReSharper disable once InconsistentNaming
    public static class JSValue
    {
        public const string Null      = JSType.Null;
        public const string Undefined = JSType.Undefined;
        public const string EmptyObject = "{}";
        public const string EmptyObjAndProt = "Object.create(null)";
    }

    // ReSharper disable once InconsistentNaming
    public static class JSObject
    {
        public const string Window = "window";
        public const string DocumentElement = "document.documentElement";
    }

    public static class XPathResultType
    {
        // @formatter:off
        // ReSharper disable UnusedMember.Global InconsistentNaming
        public const string ANY_TYPE                     = "0";
        public const string NUMBER_TYPE                  = "1";
        public const string STRING_TYPE                  = "2";
        public const string BOOLEAN_TYPE                 = "3";
        public const string UNORDERED_NODE_ITERATOR_TYPE = "4";
        public const string ORDERED_NODE_ITERATOR_TYPE   = "5";
        public const string UNORDERED_NODE_SNAPSHOT_TYPE = "6";
        public const string ORDERED_NODE_SNAPSHOT_TYPE   = "7";
        public const string ANY_UNORDERED_NODE_TYPE      = "8";
        public const string FIRST_ORDERED_NODE_TYPE      = "9";
        // ReSharper restore UnusedMember.Global InconsistentNaming
        // @formatter:on
    }

//    public static class NodeType
//    {
//        // @formatter:off
//        // ReSharper disable UnusedMember.Global InconsistentNaming
//        public const string ELEMENT_NODE 	            = "1";
//        public const string ATTRIBUTE_NODE 	            = "2";
//        public const string TEXT_NODE 	                = "3";
//        public const string CDATA_SECTION_NODE 	        = "4";
//        public const string ENTITY_REFERENCE_NODE       = "5";
//        public const string ENTITY_NODE 	            = "6";
//        public const string PROCESSING_INSTRUCTION_NODE = "7";
//        public const string COMMENT_NODE 	            = "8";
//        public const string DOCUMENT_NODE 	            = "9";
//        public const string DOCUMENT_TYPE_NODE 	        = "10";
//        public const string DOCUMENT_FRAGMENT_NODE 	    = "11";
//        public const string NOTATION_NODE 	            = "12";
//        // ReSharper restore UnusedMember.Global InconsistentNaming
//        // @formatter:on
//    }
}

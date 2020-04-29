using CommandLine;

namespace Avp
{
    public class Options
    {
        [Option("count", HelpText = "If the count should be returned. If the result set is very big, this becomes an estimate. No additional value required.", Required = false)]
        public bool Count { get; set; }

        [Option("facets", HelpText = "Facets for Azure search. Refer to search documentation.", Required = false)]
        public string Facets { get; set; }

        [Option('i', "filter", HelpText = "Filters for Azure search.", Required = false)]
        public string Filter { get; set; }

        [Option('b', "orderby", HelpText = "Order by for Azure search. If omitted, the results are organized by match score.", Required = false)]
        public string OrderBy { get; set; }

        [Option('s', "search", HelpText = "Search parameters for Azure search", Required = false)]
        public string Search { get; set; }

        [Option("searchFields", HelpText = "Which fields to search", Required = false)]
        public string SearchFields { get; set; }

        [Option('m', "searchMode", HelpText = "any | all. Only one search parameter needs to be found or all need to be found.", Required = false, Default = "any")]
        public string SearchMode { get; set; }

        [Option('$', "select", HelpText = "Which fields to return for search results.", Required = false)]
        public string Select { get; set; }

        [Option('k', "skip", HelpText = "Skip this many search results.", Required = false)]
        public int? Skip { get; set; }

        [Option('t', "top", HelpText = "Take this many results per page.", Required = false)]
        public int? Top { get; set; }

        [Option("fetch", HelpText = "Fetch full notice bodies from api.", Required = false)]
        public bool FetchNoticeBodies { get; set; }

        [Option("configuration", HelpText = "Configuration file to use. Can maintain multiple environments with different configs.", Required = false, Default = "configuration.json")]
        public string ConfigurationFile { get; set; }
    }
}
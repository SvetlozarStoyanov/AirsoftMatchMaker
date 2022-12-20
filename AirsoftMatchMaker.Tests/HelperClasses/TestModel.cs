using AirsoftMatchMaker.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Tests.HelperClasses
{
    /// <summary>
    /// Model used for testing <see cref="HtmlSanitizingService"/>
    /// </summary>
    public class TestModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}

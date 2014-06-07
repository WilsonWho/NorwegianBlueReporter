using System;

namespace NorwegianBlue.Reporting
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ReportingMetaDataAttribute : System.Attribute
    {
        public readonly ReportingTypes ReportingType;
        
        public ReportingMetaDataAttribute(ReportingTypes reportingType)
        {
            ReportingType = reportingType;
        }
    }
}

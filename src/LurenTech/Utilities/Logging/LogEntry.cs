using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LurenTech.Utilities.Logging
{
    public enum LogType : int
    {
        Exception = 1,
        Error,
        Warning,
        Info,
        Debug
    }

    /// <summary>
    /// Represent a record of log message
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Unique key for easy tracking
        /// </summary>
        public Guid Key { get; private set; }

        /// <summary>
        /// Reference to another log if writing a chain log 
        /// </summary>
        public Guid? ReferenceTo { get; private set; }

        /// <summary>
        /// Type of the Log
        /// </summary>
        public LogType LogType { get; set; }

        /// <summary>
        /// Log message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Phisical path to write the log
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Log title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Date when the log created
        /// </summary>
        public DateTime CreatedOn { get; private set; }

        /// <summary>
        /// The namespace where the log is writting - Recommended for more focus the tracking
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// The class where the log is writting - Recommended for more focus the tracking
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// The method where the log is writting - Recommended for more focus the tracking
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// Which user caused for writing the log 
        /// </summary>
        public string CreatedBy { get; private set; }

        public LogEntry(
            LogType logType,
            string message,
            string title = default(string),
            string @namespace = default(string),
            string className = default(string),
            string method = default(string),
            string path = default(string),
            string createdBy = default(string),
            Guid? referenceTo = null)
        {
            this.LogType = logType;
            this.Key = Guid.NewGuid();
            this.CreatedOn = DateTime.Now;
            this.Title = title;
            this.Namespace = @namespace;
            this.ClassName = className;
            this.Method = method;
            this.Path = path;
            this.CreatedBy = createdBy;
            this.ReferenceTo = referenceTo;
        }
    }
}


using System;
using System.Collections.Generic;

namespace LurenTech.Utilities.Logging
{
    public abstract class BaseLogger
    {
        public abstract void WriteLogEntry(LogEntry entry);

        public void WriteLogEntries(IEnumerable<LogEntry> entries)
        {
            foreach (var entry in entries)
            {
                WriteLogEntry(entry);
            }
        }

        public Guid WriteLog(string message,
            string title = default(string),
            string @namespace = default(string),
            string className = default(string),
            string method = default(string),
            string path = default(string),
            string createdBy = default(string))
        {
            LogEntry entry = new LogEntry(message, title, @namespace, className, method, path, createdBy, null);

            WriteLogEntry(entry);

            return entry.Key;
        }
    }
}

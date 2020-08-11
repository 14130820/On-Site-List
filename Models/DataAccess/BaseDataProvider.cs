using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace DustSwier.OnSiteList.Models.DataAccess
{
    public abstract class BaseDataProvider
    {
        /// <summary>
        /// Don't forget to call the base.
        /// </summary>
        public BaseDataProvider()
        {
            // Load Logging
            if (!(File.Exists(LogSavePath) && TryOpen(LogSavePath, out loggingSystem)))
            {
                loggingSystem = new LoggingSystem();
            }

            loggingSystemWrapper = new LoggingSystemWrapper(loggingSystem);
            
        }

        ///// <summary>
        ///// Used for Dialog.Filter
        ///// Ex. "JSON file (*.json)|*.json"
        ///// </summary>
        //public abstract string FilterTypes { get; }
        /// <summary>
        /// Ex. ".json"
        /// </summary>
        protected abstract string Extention { get; }

        private string LogSavePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logdata" + Extention);

        protected readonly LoggingSystem loggingSystem;
        private LoggingSystemWrapper loggingSystemWrapper;
        public LoggingSystemWrapper LoggingSystem => loggingSystemWrapper;
        
        /// <summary>
        /// Save using last save path.
        /// </summary>
        /// <returns>False - No path set.</returns>
        public bool SaveLogs()
        {
            try
            {
                return Save(LogSavePath, loggingSystem);
            }
            catch
            {
                return false;
            }
        }
        
        protected abstract bool Save<T>(string filePath, T data);
        protected abstract bool TryOpen<T>(string filePath, out T data);
    }
}

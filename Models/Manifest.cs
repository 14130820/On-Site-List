using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustSwier.OnSiteList.Models
{
    public class Manifest
    {
        private readonly ObservableCollection<ManifestItem> items;
        private string _title;
        
        [JsonConstructor]
        public Manifest(ObservableCollection<ManifestItem> items, string title)
        {
            _title = title;
            this.items = items;
        }
        
        public Manifest()
        {
            items = new ObservableCollection<ManifestItem>();
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
            }
        }

        [JsonProperty(IsReference = true)]
        public ObservableCollection<ManifestItem> Items => items;

        /// <summary>
        /// Returns a hard copy of itself.
        /// </summary>
        public Manifest Clone()
        {
            var newItems = new ObservableCollection<ManifestItem>();

            var itemCount = items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                newItems.Add(items[i]);
            }
            
            return new Manifest(newItems, Title);
        }
    }
}

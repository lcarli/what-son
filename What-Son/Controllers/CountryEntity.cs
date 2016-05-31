using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace What_Son.Controllers
{
    public class CountryEntity : TableEntity
    {

        public CountryEntity(string country, string countryName)
        {
            this.PartitionKey = country;
            this.RowKey = countryName;

        }

        public CountryEntity() { }

        public string Contintent { get; set; }

        public string Capital { get; set; }

        public string Currency { get; set; }

        public string Population { get; set; }

        public string Area { get; set; }

        public string density { get; set; }

        public string GDP { get; set; }
        public string lifeexpectations { get; set; }

        public string BirthRate { get; set; }

        public string DeathRate { get; set; }




    }
}
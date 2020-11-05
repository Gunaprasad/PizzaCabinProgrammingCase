using System;
using System.ComponentModel.DataAnnotations;

namespace WFMConsumer.Console.Boilerplate.Models
{
    public class Schedule 
    {
        /// <summary>
        /// minutes
        /// </summary>
        public int ContractTimeMinutes { get; set; }
        /// <summary>
        /// Originally the Date is received in the Epoch Time Stamp, I am
        /// converting the date to Universal/ UTC time in order to make it in 
        /// a readable format
        /// </summary>
        public DateTime Date { get; set; }
        public bool IsFullDayAbsence { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Unique Identifier for the Person
        /// </summary>
        [Key]
        public string PersonId { get; set; }
    }



}

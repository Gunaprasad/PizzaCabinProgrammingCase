using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFMConsumer.Console.Boilerplate.Models
{
    public class Projection
    {

        /// <summary>
        /// This Guid is used because DBContext identifies each row 
        /// uniquely if we dont specify it here it gives error. By default
        /// it requires a row identifier. Since its a Code First Approach
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Foreign key to Schedule table referencing PersonId
        /// </summary>
        [ForeignKey("Schedule")]
        public string PersonId { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Originally we are receiving the date in the Epoch Unix Time Stamp from
        /// the api, I am converting it to UTC time to put it in a readable format.
        /// </summary>
        public DateTime Start { get; set; }
        public int minutes { get; set; }
       
    }
}
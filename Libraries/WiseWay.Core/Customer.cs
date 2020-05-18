using System.ComponentModel.DataAnnotations;

namespace WiseWay.Core
{
    public class Customer
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string CustomerType { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Msg { get; set; }
        public int UserId { get; set; }

    }
}

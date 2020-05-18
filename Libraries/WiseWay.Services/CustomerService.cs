using WiseWay.Core;
using WiseWay.Facade;

namespace WiseWay.Services
{
    public interface ICustomerService
    {
        public Customer AddUpdateCustomer(Customer customer);
        public string GetCustomerList();
        public Customer GetCustomerDetailById(int CustomerId);
        public string DeleteCustomer(int Id);
        public string GetCustomerTypeWiseCount();
    }
    public class CustomerService : ICustomerService
    {
        public  Customer AddUpdateCustomer(Customer customer)
        {
            return CustomerFacade.AddUpdateCustomer(customer);
        }
        public string GetCustomerList()
        {
            return CustomerFacade.GetCustomerList();
        }
        public Customer GetCustomerDetailById(int CustomerId)
        {
            return CustomerFacade.GetCustomerDetailById(CustomerId);
        }
        public string DeleteCustomer(int Id)
        {
            return CustomerFacade.DeleteCustomer(Id);
        }
        public string GetCustomerTypeWiseCount()
        {
            return CustomerFacade.GetCustomerTypeWiseCount();
        }
    }
}

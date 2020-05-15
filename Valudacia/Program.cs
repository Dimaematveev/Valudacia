using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valudacia
{
     class Program
    {
        static void Main(string[] args)
        {
            var _customerManager = new CustomerManager();

            var cust = new Customer
            {
                FirstName = "Дим1а",
                LastName = null,
                Age = -1,
            };

            _customerManager.Add(cust);

            Console.ReadKey();
        }
    }

   
    class CustomerManager
    {
        CustomerRepository _repo;
        CustomerValidator _validator;
        public CustomerManager()
        {
            _repo = new CustomerRepository();
            _validator = new CustomerValidator();
        }

        public void Add(Customer customer)
        {
            var result = _validator.Validate(customer);

            if (!result.IsValid)
            {
                Console.WriteLine($"{result.ToString()}");
                return;
            }

            _repo.Add(customer);
            Console.WriteLine($"Покупатель {customer.FirstName} добавлен");
        }
    }


    class CustomerValidator:AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(cust => cust.Age).InclusiveBetween(1, 150);
            RuleFor(cust => cust.FirstName).NotNull().MinimumLength(1).Must(s => s.All(c => Char.IsLetter(c) || c == '-'));
            RuleFor(cust => cust.LastName).NotNull().MinimumLength(1).Must(s => s.All(c => Char.IsLetter(c) || c == '-'));
            
        }
    }
    class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    class CustomerRepository
    {
        Customer Customer;
        public void Add(Customer customer)
        {
            Customer = customer;
        }
    }
}

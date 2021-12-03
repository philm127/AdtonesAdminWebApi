using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class InvoicePDFEmailModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime SettledDate { get; set; }
        public int CountryId { get; set; }
        public decimal Amount { get; set; }
    }


    public class Item
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Organisation { get; set; }

        public decimal ItemTotal()
        {
            decimal total = Price * Quantity;
            return total;
        }
    }


    public class Invoice
    {
        public Customer Customer { get; set; }
        public int Items { get; set; }
        public Item Item1 { get; set; }
        public List<Item> ItemList { get; set; }

        //private set
        public DateTime Date { get; private set; }

        public DateTime? SettledDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCountry { get; set; }

        public string InvoiceTax { get; set; }
        public string MethodOfPayment { get; set; }
        public int? typeOfPayment { get; set; }
        public string PONumber { get; set; }
        public string Imagepath { get; set; }
        public int MyProperty { get; set; }
        public decimal vat { get; set; }

        public int? CountryId { get; set; }
        public string CurrencySymbol { get; set; }

        //Constructors
        public Invoice()
        {
            //In reality would get from database for unique number
            Date = DateTime.Today;

        }

        public Invoice(Item Item1)
            : this()
        {
            ItemList = new List<Item>();
            ItemList.Add(Item1);
        }


        public decimal GetSubtotal()
        {



            decimal total = 0;

            foreach (var item in ItemList)
            {
                total += item.ItemTotal();
            }
            return total;

        }

        public decimal GetVATAmount()
        {
            decimal total = GetSubtotal();
            total *= vat;

            return total;
        }

        public decimal GetTotal()
        {
            decimal total = GetSubtotal() + GetVATAmount();
            return total;
        }

    }


    public class Customer
    {
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public int CustomerID { get; private set; }

        //Constructors
        public Customer()
        {
            CustomerID = 321;
        }
    }
}

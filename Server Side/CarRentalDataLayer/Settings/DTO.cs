using Azure;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CarRentalDataLayer.Settings
{
    public class DTO
    {
        public class PersonDTO
        {
            public int PersonId { get; set; }
            public personInfo PersonInfo { get; set; }
            public class personInfo
            {
                public string? FirstName { get; set; }
                public string? LastName { get; set; }
                public int? Age { get; set; }
                public DateTime? Birthdate { get; set; }

                public string? Adress { get; set; }

                public personInfo()
                {
                    FirstName = null;
                    LastName = null;
                    Age = null;
                    Birthdate = null;
                    Adress = null;
                }
                public personInfo(string fistname,string lastname,int age,DateTime birthdate,string adress)
                {
                    FirstName = fistname;
                    LastName = lastname;
                    Age = age;
                    Birthdate = birthdate;
                    Adress = adress;
                }

            }

            public PersonDTO(int personid, personInfo personinfo)
            {
                PersonId = personid;
                PersonInfo = personinfo;
            }

        }
        public class LicenseDTO
        {
            public int LicenseId { get; set; }
            public string LicenseNumber { get; set; }
            public DateTime IssuDate { get; set; }
            public DateTime ExpirationDate { get; set; }
            public bool IsActive { get; set; }
            public int CustomerId { get; set; }

            public LicenseDTO(int licenseid, string licensenumber, DateTime issuedate, DateTime expirationdate, bool isactive, int customerid)
            {
                LicenseId = licenseid;
                LicenseNumber = licensenumber;
                IssuDate = issuedate;
                ExpirationDate = expirationdate;
                IsActive = isactive;
                CustomerId = customerid;
            }
            public LicenseDTO()
            {
                LicenseId = 0;
                LicenseNumber = "";
                IssuDate = DateTime.Now;
                ExpirationDate = DateTime.Now;
                IsActive = false;
                CustomerId = 0;
            }


        }
        public class CustomerDTO 
        {
            public int CustomerId { get; set; }
            public int personId { get; set; }

            public List<LicenseDTO> LicensesInfo { get; set; }
            public CustomerInfo customerInfo { get; set; }

            public class CustomerInfo
            {
                public string Email { get; set; }
                public string PassWord { get; set; }

                public PersonDTO.personInfo PersonInfo { get; set; }
                public CustomerInfo()
                {
                    PersonInfo = new PersonDTO.personInfo();
                    Email = "";
                    PassWord = "";
                }
            }

            public CustomerDTO()
            {
                LicensesInfo = new List<LicenseDTO>();
                customerInfo = new CustomerInfo();
            }

        }
        public class UserDTO : PersonDTO.personInfo
        {
            public int? UserId { get; set; }
            public string? UserName { get; set; }
            public string? Password { get; set; }
            public bool? IsActive { get; set; }
            public bool? CanManageUsers { get; set; }

            public UserDTO()
            {
                UserName = null;
                Password = null;
                UserId = null;
                IsActive = null;
                CanManageUsers = null;
            }
            public UserDTO(int userid,string username,string password,bool isactive, bool canmanageusers, string fistname, string lastname, 
                                                                                          int age, DateTime birthdate, string adress)
                       : base(fistname,lastname,age,birthdate,adress)
            {
                UserId = userid;
                UserName = username;
                Password = password;
                IsActive = isactive;
                CanManageUsers = canmanageusers;

            }

        }
        
        public class VehicleStatusDTO
        {
            public int VehicleStatusId { get; set; }
            public StatusInfo VehicleStatus { get; set; }
            public class StatusInfo
            {
                public string Status { get; set; }
                public StatusInfo(string status)
                {
                    Status = status;
                }
                public StatusInfo()
                {
                    Status = "";
                }
            }
            public VehicleStatusDTO()
            {
                VehicleStatus = new StatusInfo();
                
            }
            public VehicleStatusDTO(int vehicleStatusId, string status)
            {
                VehicleStatus = new StatusInfo(status);
                VehicleStatusId = vehicleStatusId;
            }
        }
        public class LocationDTO
        {
            public int LocationId { get; set; }
            public LocationInfo LocationInfor { get; set; }
            public class LocationInfo
            {
                public string LocationName { get; set; }
                public string Adress { get; set; }
                public string Phone { get; set; }

                public LocationInfo(string locationName, string adress, string phone)
                {
                    LocationName = locationName;
                    Adress = adress;
                    Phone = phone;
                }

                public LocationInfo()
                {
                    LocationName = "";
                    Adress = "";
                    Phone = "";
                }
            }
            public LocationDTO(int locationId, LocationInfo locationInfor)
            {
                LocationId = locationId;
                LocationInfor = locationInfor;
            }
            public LocationDTO()
            {
                LocationId = 0;
                LocationInfor = new LocationInfo();
            }
        }
        public class FuelTypeDTO
        {
            public int FuelTypeId { get; set; }
            public FuelTypeInfo FuelTypeInfor { get; set; }
            public class FuelTypeInfo
            {
                public string FuelType { get; set; }
                public FuelTypeInfo()
                {
                    FuelType = "";
                }
                public FuelTypeInfo(string fuelType)
                {
                    FuelType = fuelType;
                }
            }
            public FuelTypeDTO()
            {
                FuelTypeInfor = new FuelTypeInfo();
            }
            public FuelTypeDTO(int fuelTypeId,string fueltypeName)
            {
                FuelTypeId = fuelTypeId;
                FuelTypeInfor = new FuelTypeInfo(fueltypeName);
            }
             
        }
        public class VehicleCategoryDTO
        {
            public int VehicleCategoryId { get; set; }
            public VehiclCeategoryInfo CategoryInfo { get; set; }
            public class VehiclCeategoryInfo
            {
             public string Category { get; set; }
                public VehiclCeategoryInfo()
                {
                    Category = "";
                }
                public VehiclCeategoryInfo(string category)
                {
                    Category = category;
                }
            }
            public VehicleCategoryDTO()
            {
                CategoryInfo = new VehiclCeategoryInfo();
            }
            public VehicleCategoryDTO(int vehicleCategoryId, string CategoryName)
            {
                VehicleCategoryId = vehicleCategoryId;
                CategoryInfo = new VehiclCeategoryInfo(CategoryName);
            }
        }

        public class  VehicleIdiesDTO
        {
            public int? VehicleId { get; set; }
            public int? VehicleStatusId { get; set; }
            public int? LocationId { get; set; }
            public int? FuelTypeId { get; set; }
            public int? VehicleCategoryId { get; set; }


        }
        public class VehicleNamesDTO
        {
            public string? VehicleName { get; set; }
            public decimal? PricePerday { get; set; }
            public string? Make { get; set; }
            public string? Model { get; set; }
            public int? Year { get; set; }
            public string? LicensePlate { get; set; }

            public VehicleNamesDTO()
            {
                this.VehicleName = null;
                this.PricePerday = null;
                this.Make = null;
                this.Year = null;
                this.LicensePlate = null;
                this.Model = null;
            }
        }
        public class VehicleInfo
        {

            public VehicleStatusDTO.StatusInfo Status { get; set; }
            public VehicleCategoryDTO.VehiclCeategoryInfo Category { get; set; }
            public FuelTypeDTO.FuelTypeInfo FuelType { get; set; }
            public LocationDTO.LocationInfo Location { get; set; }

            public VehicleInfo()
            {

                this.Status = new VehicleStatusDTO.StatusInfo();
                this.Category = new VehicleCategoryDTO.VehiclCeategoryInfo();
                this.FuelType = new FuelTypeDTO.FuelTypeInfo();
                this.Location = new LocationDTO.LocationInfo();
            }


        }

        public class UserViewVehicleDTO
        {
            public VehicleNamesDTO VehicleInfo { get; set; }
            public VehicleIdiesDTO VehiclePropertyIdies { get; set; }
            public UserViewVehicleDTO()
            {
                VehicleInfo = new VehicleNamesDTO();
                VehiclePropertyIdies = new VehicleIdiesDTO();
            }
            public UserViewVehicleDTO(VehicleNamesDTO vehicleInfo,VehicleIdiesDTO vehicleidies)
            {
                this.VehicleInfo = vehicleInfo;
                this.VehiclePropertyIdies = vehicleidies;
            }
        }
        public class CustomerViewVehicleDTO
        {
            public VehicleNamesDTO VehicleInfo { get; set; }
            public VehicleInfo VehicleDetailInfo { get; set; }
            public CustomerViewVehicleDTO()
            {
                VehicleInfo = new VehicleNamesDTO();
                VehicleDetailInfo = new VehicleInfo();
            }
            public CustomerViewVehicleDTO(VehicleNamesDTO vehicleinfo,VehicleInfo vehicledetail)
            {
                VehicleInfo = vehicleinfo;
                VehicleDetailInfo = vehicledetail;
            }
        }


        public class BookingStatusDTO
        {
            public int? BookingStatusId { get; set; }
            public BookingStatusInfo BookingStatusInfor { get; set; }
            public class BookingStatusInfo
            {
                public string? Status { get; set; }
                public BookingStatusInfo(string status)
                {
                    this.Status = status;
                }
                public BookingStatusInfo()
                {
                    this.Status = "";
                }
            }
            public BookingStatusDTO()
            {
                this.BookingStatusInfor = new BookingStatusInfo();
            }
            public BookingStatusDTO(int statusid,BookingStatusInfo bookingStatus)
            {
                this.BookingStatusInfor = bookingStatus;
                this.BookingStatusId = statusid;
            }
        }
        public class BookingVehicleDTO
        {
            public int BookingId { get; set; }
            public int CustomerId { get; set; }
            public int VehicleId { get; set; }
            public int BookingStatusId { get; set; }
            public BookingVehicleInfo BookingVehicleInfor { get; set; }

            public class BookingVehicleInfo
            {
                public DateTime RentalStartDate { get; set; }
                public DateTime RentalEndDate { get; set; }
                public string PickupLocation { get; set; }
                public string DropOfLocation { get; set; }
                public int InitialRentalDays { get; set; }
                public int InitialTotalAmount { get; set; }
                public string InitialCheckNotes { get; set; }
                public VehicleInfo VehicleInfo { get; set; }
                public BookingStatusDTO.BookingStatusInfo BookingStatusInfo { get; set; }
            }

        }
      
        // Booking Information From Customer 
        public class BookingInfoFromCustomer
        {
            public DateTime? RentalStartDate { get; set; }
            public DateTime? RentalEndDate { get; set; }
            public string? PickupLocation { get; set; }
            public string? DropOfLocation { get; set; }

            public BookingInfoFromCustomer()
            {

            }
            public BookingInfoFromCustomer(DateTime? rentalstartdate,DateTime? rentalenddate,string? picuplocation, string? dropofflocation)
            {
                RentalStartDate = rentalstartdate;
                RentalEndDate = rentalenddate;
                PickupLocation = picuplocation;
                DropOfLocation = dropofflocation;
            }
        }
        public class CustomerViewBookingInfo
        {
            public int? InitialRentalDays { get; set; }
            public int? InitialTotalAmount { get; set; }
            public CustomerViewVehicleDTO VehicleInfo { get; set; }
            public BookingStatusDTO.BookingStatusInfo BookingStatusInfo { get; set; }
            public BookingInfoFromCustomer BookingInfoFromCustomer { get; set; }

            public CustomerViewBookingInfo() 
            {
                this.VehicleInfo = new CustomerViewVehicleDTO();    
                this.BookingInfoFromCustomer = new BookingInfoFromCustomer(); 
                this.BookingStatusInfo = new BookingStatusDTO.BookingStatusInfo();
            }
        }
        public class UserViewBookingInfo
        {
            public int? BookingId { get; set; }
            public int? CustomerId { get; set; }
            public int? VehicleId { get; set; }
            public int? BookingStatusId { get; set; }
            public string? InitialCheckNotes { get; set; }
        }
        public class GettingBookingInfoDTO
        {
            public CustomerViewBookingInfo CustomerViewBookingInfo { get; set; }
            public UserViewBookingInfo UserViewBookingInfo { get; set; }

            public GettingBookingInfoDTO(CustomerViewBookingInfo customerViewBookingInfo, UserViewBookingInfo userViewBookingInfo)
            {
                CustomerViewBookingInfo = customerViewBookingInfo;
                UserViewBookingInfo = userViewBookingInfo;
            }
            public GettingBookingInfoDTO()
            {
                CustomerViewBookingInfo = new CustomerViewBookingInfo();
                UserViewBookingInfo = new UserViewBookingInfo();
            }
        }
        public class MiniInfoOfBookingDTO
        {
            public int BookingStatusId { get; set; }
            public int VehicleId { get; set; }

            public SendingInfo SendingMiniBookingInfo { get; set; }

            public MiniInfoOfBookingDTO()
            {
                SendingMiniBookingInfo = new SendingInfo();
            }
            public class SendingInfo
            {
                public int BookingId { get; set; }
                public BookingStatusDTO.BookingStatusInfo BookingStatus { get; set; }
                public string VehicleName { get; set; }
                public SendingInfo()
                {
                    BookingStatus = new BookingStatusDTO.BookingStatusInfo();
                    VehicleName = "";
                }
                public SendingInfo(BookingStatusDTO.BookingStatusInfo bookingStatus, string vehiclename)
                {
                    BookingStatus = bookingStatus;
                    VehicleName = vehiclename;
                }
            }
          
        }
        public class AddingBookingInfoDTO
        {
            public UserViewBookingInfo UserViewBookingInfo { get; set; }
            public BookingInfoFromCustomer BookingInfoFromCustomer { get; set; }

            public AddingBookingInfoDTO(UserViewBookingInfo userViewBookingInfo, BookingInfoFromCustomer bookingInfoFromCustomer)
            {
                UserViewBookingInfo = userViewBookingInfo;
                BookingInfoFromCustomer = bookingInfoFromCustomer;
            }

            public AddingBookingInfoDTO()
            {
                UserViewBookingInfo = new UserViewBookingInfo();
                BookingInfoFromCustomer =new BookingInfoFromCustomer();
            }

        }

        public class CardDTO
        {
            public int CardId { get; set; }
            public string? BankName { get; set; }
            public string? CardType { get; set; }
            public string? CardHolderName { get; set; }
            public string? CardNumber { get; set; }
            public string? CardExpirationDate { get; set; }
            public int CustomerId { get; set; }
        }

        public class PaymentMethodDTO
        {
            public int? PaymentMethodId { get; set; }
            public PaymentMethodInfo paymentMethodInfo { get; set; }
            public class PaymentMethodInfo
            {
                public string? PaymentMethod { get; set; }

                public PaymentMethodInfo(string paymentMethod)
                {
                    PaymentMethod = paymentMethod;
                }
                public PaymentMethodInfo()
                {
                    PaymentMethod = null;
                }
            }
            public PaymentMethodDTO(int paymentmethodid , PaymentMethodInfo paymentmethodinfo)
            {
                PaymentMethodId = paymentmethodid;
                paymentMethodInfo = paymentmethodinfo;
            }
            public PaymentMethodDTO()
            {
                paymentMethodInfo = new PaymentMethodInfo();
                PaymentMethodId = null;
            }
        }
        public class PaymentStatusDTO
        {
            public int? PaymentStatusId { get; set; }
            public PaymentStatusInfo paymentStatusInfo { get; set; }

            public PaymentStatusDTO(int paymentstatusid, PaymentStatusInfo paymentstatusinfo)
            {
                PaymentStatusId = paymentstatusid;
                paymentStatusInfo = paymentstatusinfo;
            }
            public PaymentStatusDTO()
            {
                paymentStatusInfo = new PaymentStatusInfo();
                PaymentStatusId = null;

            }
            public class PaymentStatusInfo
            {
                public string PaymentStatus { get; set; }

                public PaymentStatusInfo(string paymentstatus)
                {
                    PaymentStatus = paymentstatus;
                }
                public PaymentStatusInfo()
                {
                    PaymentStatus = "";
                }
            }
        }
        public class TransactionTypeDTO
        {
            public int? TransactionTypeId { get; set; }
            public TransactionTypeInfo transactionTypeInfo { get; set; }
            public TransactionTypeDTO(int transactionTypeId, TransactionTypeInfo transactionTypeInfo)
            {
                TransactionTypeId = transactionTypeId;
                this.transactionTypeInfo = transactionTypeInfo;
            }
            public TransactionTypeDTO()
            {
                transactionTypeInfo = new TransactionTypeInfo();
                TransactionTypeId = null;
            }
            public class TransactionTypeInfo
            {
                public string TransactionType { get; set; }

                public TransactionTypeInfo(string transactionType)
                {
                    TransactionType = transactionType;
                }
                public TransactionTypeInfo()
                {
                    TransactionType = "";
                }
            }
        }

        public class RentalTransactionDTO
        {
            public int? TransactionId { get; set; }

            public int? TransactionTypeId { get; set; }
            public int? BookingId { get; set; }
            public decimal? Amount { get; set; }
            public DateTime? TransactionDate { get; set; }
            public int? PaymentMethodId { get; set; }
            public int? PaymentStatusId { get; set; }

        }


        public class VehicleReturnLog
        {
            public int? ReturnId { get; set; }
            public int? BookingId { get; set; }
            public DateTime? ActualReturnDate { get; set; }
            public int? ActualRentalDays { get; set; }
            public string? FinalCheckNotes { get; set; }
            public decimal? ActualTotalAmount { get; set; }
        }

    }
}

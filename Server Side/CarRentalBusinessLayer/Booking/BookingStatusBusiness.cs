using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;
namespace CarRentalBusinessLayer.Booking
{
    public class BookingStatusBusiness : IBookingStatusBusiness
    {
        private readonly IBookingStatusData _BookinsStatusData; 
        public enum enMode { AddNewMode = 1, UpdateMode = 2}

        public enMode Mode;
        public DTO.BookingStatusDTO BookingStatusInfo { get; set; }
        public BookingStatusBusiness(IBookingStatusData bookinsStatusData)
        {
            _BookinsStatusData = bookinsStatusData;
            BookingStatusInfo = new DTO.BookingStatusDTO();
        }
        public void Initialize(DTO.BookingStatusDTO bookingStatusInfo, enMode mode = enMode.AddNewMode)
        {
            BookingStatusInfo = bookingStatusInfo;
            Mode = mode;
        }

        private bool _AddNewVehicleStatus()
        {
            BookingStatusInfo.BookingStatusId = _BookinsStatusData.AddNewBookingStatus(BookingStatusInfo);
            return BookingStatusInfo.BookingStatusId.HasValue;
        }

        private bool _UpdateVehicleStatus()
        {
            return _BookinsStatusData.UpdateBookingStatus(BookingStatusInfo);
        }
        public bool DeleteVehicleStatus(int bookinstatusid)
        {
            return _BookinsStatusData.DeleteBookingStatus(bookinstatusid);
        }

        public DTO.BookingStatusDTO GetBookingStatusBy(int? VehicleStatusId, string StatusName)
        {
            return _BookinsStatusData.GetBookingStatusBy(VehicleStatusId, StatusName);
        }
        public List<DTO.BookingStatusDTO> GetAllBookingStatus()
        {
            return _BookinsStatusData.GetAllBookingStatuses();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewVehicleStatus())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateVehicleStatus())
                    {
                        return true;
                    }
                    else { return false; }
                default:
                    return false;
            }
        }
    }
}

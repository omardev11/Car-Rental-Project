using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalBusinessLayer.Vehicle
{
    public class LocationBusiness : BusinessLayerInterfaces.ILocationBusiness
    {
        private readonly DataLayerInterfaces.ILocationData _LocationData;

        public enum enMode { AddNewMode = 1, UpdateMode = 2, DeleteMode = 3 }

        public enMode Mode;
        public DTO.LocationDTO LocationInfo { get; set; }

        public LocationBusiness(DataLayerInterfaces.ILocationData locationData)
        {
            _LocationData = locationData;
            LocationInfo = new DTO.LocationDTO();
        }

        public void Initialize(DTO.LocationDTO Location, enMode mode = enMode.AddNewMode)
        {
            LocationInfo = Location;
            Mode = mode;
        }

        private bool _AddNewLocation()
        {
            LocationInfo.LocationId = _LocationData.AddNewLocation(LocationInfo);
            return LocationInfo.LocationId != -1;
        }

        private bool _UpdateLocation()
        {
            return _LocationData.UpdateLocation(LocationInfo);
        }

        private bool _DeleteLocation()
        {
            return _LocationData.DeleteLocation(LocationInfo.LocationId);
        }

        public DTO.LocationDTO GetLocationInfoById(int LocationId)
        {
            return _LocationData.GetLocationById(LocationId);
        }
        public List<DTO.LocationDTO> GetAllLocationsByLocationNameOrAdress(string LocationName,string Adress)
        {
            return _LocationData.GetAllLocationsByLocationNameOrAdress(LocationName, Adress);
        }

        public List<DTO.LocationDTO> GetAllLocations()
        {
            return _LocationData.GetAllLocations();
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewLocation())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateLocation())
                    {
                        return true;
                    }
                    else { return false; }
                case enMode.DeleteMode:
                    if (_DeleteLocation())
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

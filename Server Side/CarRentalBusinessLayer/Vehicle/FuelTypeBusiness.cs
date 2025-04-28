using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRentalDataLayer.Vehicle;

namespace CarRentalBusinessLayer.Vehicle
{
    public class FuelTypeBusiness : BusinessLayerInterfaces.IFuelTypeBusiness
    {
        private readonly DataLayerInterfaces.IFuelTypeData _FuelTypeData;

        public enum enMode { AddNewMode = 1, UpdateMode = 2, DeleteMode = 3 }

        public enMode Mode;
        public DTO.FuelTypeDTO FuelTypeInfo { get; set; }

        public FuelTypeBusiness(DataLayerInterfaces.IFuelTypeData fuelTypeData)
        {
            _FuelTypeData = fuelTypeData;
            FuelTypeInfo = new DTO.FuelTypeDTO();
        }

        public void Initialize(DTO.FuelTypeDTO FuelType, enMode mode = enMode.AddNewMode)
        {
            FuelTypeInfo = FuelType;
            Mode = mode;
        }

        private bool _AddNewFuelType()
        {
            FuelTypeInfo.FuelTypeId = _FuelTypeData.AddNewFuelType(FuelTypeInfo);
            return FuelTypeInfo.FuelTypeId != -1;
        }

        private bool _UpdateFuelType()
        {
            return _FuelTypeData.UpdateFuelType(FuelTypeInfo);
        }

        private bool _DeleteFuelType()
        {
            return _FuelTypeData.DeleteFuelType(FuelTypeInfo.FuelTypeId);
        }

        public DTO.FuelTypeDTO GetFuelTypeInfoBy(int? FuelTypeId, string FuelTypeName)
        {
            return _FuelTypeData.GetFuelTypeBy(FuelTypeId, FuelTypeName);
        }
        public List<DTO.FuelTypeDTO> GetAllFuelTypeInfo()
        {
            return _FuelTypeData.GetAllFuelType();
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    if (_AddNewFuelType())
                    {
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else { return false; }
                case enMode.UpdateMode:
                    if (_UpdateFuelType())
                    {
                        return true;
                    }
                    else { return false; }
                case enMode.DeleteMode:
                    if (_DeleteFuelType())
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

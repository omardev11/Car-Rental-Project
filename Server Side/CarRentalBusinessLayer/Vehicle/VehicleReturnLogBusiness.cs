using CarRentalDataLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalBusinessLayer.Vehicle
{
    public class VehicleReturnLogBusiness : BusinessLayerInterfaces.IVehicleReturnLogBusiness
    {
        private readonly DataLayerInterfaces.IVehicleReturnLogData _vehicleReturnLogData;

        public enum enMode { AddNewMode = 1, UpdateMode = 2 }
        public enMode Mode { get; set; }
        public DTO.VehicleReturnLog VehicleReturnLogInfo { get; set; }

        public VehicleReturnLogBusiness(DataLayerInterfaces.IVehicleReturnLogData vehicleReturnLogData)
        {
            _vehicleReturnLogData = vehicleReturnLogData;
            VehicleReturnLogInfo = new DTO.VehicleReturnLog();
        }

        public void Initialize(DTO.VehicleReturnLog vehicleReturnLog, enMode mode = enMode.AddNewMode)
        {
            VehicleReturnLogInfo = vehicleReturnLog;
            Mode = mode;
        }

        private bool _AddNewVehicleReturnLog()
        {
            VehicleReturnLogInfo.ReturnId = _vehicleReturnLogData.AddNewVehicleReturnLog(VehicleReturnLogInfo);
            return VehicleReturnLogInfo.ReturnId.HasValue;
        }

        private bool _UpdateVehicleReturnLog()
        {
            return _vehicleReturnLogData.UpdateVehicleReturnLog(VehicleReturnLogInfo);
        }

      
        public List<DTO.VehicleReturnLog> GetAllVehicleReturnLogsBy(DTO.VehicleReturnLog vehiclereturninfo)
        {
            return _vehicleReturnLogData.GetAllVehicleReturnLogsBy(vehiclereturninfo);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewMode:
                    return _AddNewVehicleReturnLog();
                case enMode.UpdateMode:
                    return _UpdateVehicleReturnLog();
                default:
                    return false;
            }
        }
    }
}

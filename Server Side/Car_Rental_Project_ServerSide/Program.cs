using CarRentalBusinessLayer;
using CarRentalBusinessLayer.Booking;
using CarRentalBusinessLayer.CardsAndPayments;
using CarRentalBusinessLayer.Vehicle;
using CarRentalDataLayer;
using CarRentalDataLayer.Booking;
using CarRentalDataLayer.CardsAndPayments;
using CarRentalDataLayer.Settings;
using CarRentalDataLayer.Vehicle;
using static CarRentalBusinessLayer.BusinessLayerInterfaces;
using static CarRentalDataLayer.Settings.DataLayerInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register configuration as a singleton so it can be injected into classes
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Initialize your DataAccessSetting with configuration
DataAccessSetting.Initialize(builder.Configuration);



// Register your data layer class
builder.Services.AddScoped<DataAccessSetting>();

// Register services in the DI container
builder.Services.AddScoped<ICustomerBusiness, CustomerBusiness>();
builder.Services.AddScoped<ICustomerData, CustomerData>();
builder.Services.AddScoped<IPeopleData,PeopleData>();
builder.Services.AddScoped<ILicenseBusiness, LicenseBusiness>();
builder.Services.AddScoped<ILicenseData, LicenseData>();
builder.Services.AddScoped<IVehicleStatusBusiness,VehicleStatusBusiness>();
builder.Services.AddScoped<IVehicleStatusData, VehicleStatusData>();
builder.Services.AddScoped<IVehicleCategoryBusiness,VehicleCategoryBusiness>();
builder.Services.AddScoped<IVehicleCategoryData, VehicleCategoryData>();
builder.Services.AddScoped<IFuelTypeBusiness, FuelTypeBusiness>();
builder.Services.AddScoped<IFuelTypeData, FuelTypeData>();
builder.Services.AddScoped<ILocationBusiness, LocationBusiness>();
builder.Services.AddScoped<ILocationData, VehicleLocationData>();
builder.Services.AddScoped<IVehicleBusiness, VehicleBusiness>();
builder.Services.AddScoped<IVehicleData, VehicleData>();
builder.Services.AddScoped<IBookingStatusBusiness , BookingStatusBusiness>();
builder.Services.AddScoped<IBookingStatusData, BookingStatusData>();
builder.Services.AddScoped<IBookingVehicleBusiness, BookingVehicleBusiness>();
builder.Services.AddScoped<IBookingVehicleData , BookingVehicleData>();
builder.Services.AddScoped<ICardBusiness, CardBusiness>();
builder.Services.AddScoped<ICardData , CardData>();
builder.Services.AddScoped<IPaymentMethodBusiness, PaymentMethodBusiness>();
builder.Services.AddScoped<IPaymentMethodData, PaymentMethodData>();
builder.Services.AddScoped<IPaymentStatusBusiness, PaymentStatusBusiness>();
builder.Services.AddScoped<IPaymentStatusData, PaymentStatusData>();
builder.Services.AddScoped<ITransactionTypeBusiness, TransactionTypeBusiness>();
builder.Services.AddScoped<ITransactionTypeData, TransactionTypeData>();
builder.Services.AddScoped<IRentalTransactionBusiness, RentalTransactionBusiness>();
builder.Services.AddScoped<IRentalTransactionData, RentalTransactionData>();
builder.Services.AddScoped<IVehicleReturnLogBusiness, VehicleReturnLogBusiness>();
builder.Services.AddScoped<IVehicleReturnLogData, VehicleReturnLogData>();
builder.Services.AddScoped<IUserBusiness , UserBusiness>();
builder.Services.AddScoped<IUserData , UserData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

	interface IDal
	{
		 bool addStation(Station station);
		bool addDrone(Drone drone);
		bool addCustomer(Customer customer);
		int addParsel(Parcel parcel);
		void addDroneCarge(DroneCarge droneCarge);
		bool AssignPackageToDrone(int parcelId, int droneId);
		bool PackageCollectionByDrone(int parcelId);
		bool DeliveryPackageToCustomer(int parcelId);
		bool ReleaseDroneCarge(int droneId);
		bool addingCargeSlotsToStation(int stationId);
		bool reductionCargeSlotsToStation(int stationId);
		Station? getStation(int Id);
		Drone? getDrone(int Id);
		Customer? getCustomer(int Id);
		Parcel? getParcel(int Id);
		IEnumerable<Station> DisplaysIistOfStations(Predicate<Station> p = null);
		IEnumerable<Drone> DisplaysTheListOfDrons(Predicate<Drone> p = null);
		IEnumerable<Customer> DisplaysIistOfCustomers(Predicate<Customer> p = null);
		IEnumerable<Parcel> DisplaysIistOfparcels(Predicate<Parcel> p = null);
		double[] PowerConsumptionRate();
		bool removeDrone(int id);
		bool removeStation(int id);
		bool removeCustomer(int id);
		bool removeParcel(int id);

	}
}
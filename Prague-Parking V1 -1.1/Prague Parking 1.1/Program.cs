using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Prague_Parking_1._1
{
    // Main program class
    class Program
    {
        // Array to store parking spots. Each index represents a parking spot.
        public static string[] ParkingList = new string[100];

        static void Main()
        {
            // Entry point of the program, calls the MainMenu function
            MainMenu();
        }

        // Displays the main menu to the user
        static void MainMenu()
        {
            Console.WriteLine("Hello and welcome to Prague Parking, what would you like to do? Type the number of your menu choice" +
                "\n" +
                "\n 1. Availability and overview" +
                "\n" +
                "\n 2. Park vehicle" +
                "\n" +
                "\n 3. Check out vehicle" +
                "\n" +
                "\n 4. Search for and Move vehicle" +
                "\n" +
                "\n 5. Optimize the motorcycles in the parking lot" +
                "\n");
            Console.Write("Number: ");
            string menuChoice = Console.ReadLine(); // Get user input for menu selection
            int a;
            bool choice = int.TryParse(menuChoice, out a); // Validate the input as an integer

            // Check if the input is within the range of valid menu options
            if (a >= 1 && a <= 5)
            {
                switch (a)
                {
                    case 1: Available(); break; // Check parking availability
                    case 2: ParkVehicle(); break; // Park a vehicle
                    case 3: CheckOut(); break; // Check out a parked vehicle
                    case 4: MoveVehicle(); break; // Move a vehicle to a new spot
                    case 5: OptimizeSpace(); break; // Optimize parking for motorcycles
                }
            }
            else
            {
                // Handle invalid input
                Console.Clear();
                Console.WriteLine("Invalid input, try again");
                MainMenu();
            }
        }

        // Displays parking availability and an overview of parking spots
        static void Available()
        {
            Console.Clear();
            Console.WriteLine("1. Availability and overview");

            // Get the count of available parking spaces
            int count = Availability();

            // Display the count of available spaces
            Console.WriteLine("\n\n\nThere are {0} parking spaces available", count);

            Console.WriteLine("\n\n\nHere are all the parking spots:");

            // Display all parking spots and their statuses
            AllSpaces();

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }

        // Counts the number of available (empty) parking spots
        static int Availability()
        {
            int counter = 0; // Initialize counter
            foreach (string space in ParkingList)
            {
                if (space == null)
                {
                    counter++; // Increment counter for each empty spot
                }
            }
            return counter;
        }

        // Displays all parking spots and their current status
        static void AllSpaces()
        {
            int i = 0; // Spot index
            string[] vehicle = new string[200]; // Temporary storage for vehicle info

            foreach (string element in ParkingList)
            {
                i++;
                Console.WriteLine(" _____________");
                if (element == null)
                {
                    // Display empty spot
                    Console.WriteLine("|" + i + "\n|_____________");
                }
                else if (element.Contains("mc") && element.Contains(", "))
                {
                    // Display spot with two motorcycles
                    vehicle[i] = " " + element.Replace('!', ' ');
                    string[] twoVehicle = vehicle[i].Split(", ");
                    Console.WriteLine("|" + i + twoVehicle[0] + "\n|  " + twoVehicle[1] + "\n|_____________");
                }
                else
                {
                    // Display spot with a single vehicle
                    vehicle[i] = " " + element.Replace('!', ' ');
                    Console.WriteLine("|" + i + vehicle[i] + "\n|_____________");
                }
            }
        }

        // Handles parking a new vehicle
        static void ParkVehicle()
        {
            Console.Clear();
            Console.Write("2. Park vehicle \n \nPlease enter vehicle type, car / mc: ");
            string park = Console.ReadLine(); // Get vehicle type from user
            string carPark;
            string mcPark;

            // Check if the user wants to park a car
            if (park == "car")
            {
                carPark = ParkCar(); // Call function to park a car
                Console.WriteLine(carPark);
            }
            else if (park == "mc")
            {
                mcPark = ParkMc(); // Call function to park a motorcycle
                Console.WriteLine(mcPark);
            }
            // Allow user to test parking functionality by filling all spots
            else if (park == "all")
            {
                for (int i = 0; i < 100; i++)
                {
                    int spot = i + 1;
                    string spotReceipt = "mc!A" + spot;
                    SpotAllocation(spot, spotReceipt);
                }
            }
            else
            {
                // Restart the park vehicle process if input is invalid
                ParkVehicle();
            }

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }

        // Helper function to park a car
        static string ParkCar()
        {
            string carReg = ""; // Stores the car's registration number
            DateTime now = DateTime.Now; // Get current time
            int empty = EmptySpace(); // Find the next available parking spot
            string spotReceipt = ""; // Parking receipt information
            string notCorrect = "\nPlease enter a valid registration number";
            Console.Clear();
            Console.Write("\nPlease enter the car's registration number in one string: ");
            carReg = Console.ReadLine().ToUpper(); // Get car registration number

            string checker = CheckReg(carReg); // Check if the registration number is already in use

            if (checker == "Ok")
            {
                if (carReg.Length <= 10) // Validate length of registration number
                {
                    if (!carReg.Contains(" ")) // Ensure no spaces in the registration number
                    {
                        string receipt = "\nYou have parked a car \nwith the registration number: " + carReg + "\nat: " + now + "\nin spot: " + empty;
                        spotReceipt = "car!" + carReg; // Format parking information
                        SpotAllocation(empty, spotReceipt); // Assign the car to the parking spot
                        return receipt;
                    }
                    else
                    {
                        return notCorrect; // Invalid input
                    }
                }
                else
                {
                    return notCorrect; // Invalid input
                }
            }
            else
            {
                Console.WriteLine(checker); // Show error if registration is not valid
                return notCorrect;
            }
        }
        // Finds the next available parking spot
        static int EmptySpace()
        {
            int counter = 0; // Keeps track of the parking spot index
            int emptySpot = 0; // Stores the first empty parking spot
            foreach (string element in ParkingList)
            {
                counter++;
                if (element == null) // Check if the spot is empty
                {
                    emptySpot = counter;
                    return emptySpot; // Return the index of the empty spot
                }
                else if (counter == 100) // If all spots are full
                {
                    Console.WriteLine("There are no available spots");
                    Console.WriteLine("\n\n\nPress any key to return to the main menu");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu(); // Return to main menu
                }
            }
            return emptySpot; // Default return value if no empty spots are found
        }

        // Allocates a vehicle to a specific parking spot
        static void SpotAllocation(int spot, string vehicle)
        {
            spot = spot - 1; // Adjust for zero-based index
            ParkingList[spot] = vehicle; // Assign the vehicle to the spot
        }

        // Checks if the registration number is already in use
        static string CheckReg(string search)
        {
            string clone = "\nThere is already a vehicle parked with this registration number, please try again";
            string noClone = "Ok";

            foreach (string parkingSpot in ParkingList)
            {
                if (parkingSpot != null)
                {
                    string[] vehicles = parkingSpot.Split(", "); // Split for multiple vehicles in a spot

                    foreach (var splits in vehicles)
                    {
                        string[] atoms = splits.Split("!"); // Split type and registration number
                        if (atoms[1] == search)
                        {
                            return clone; // If the registration matches, return an error message
                        }
                    }
                }
            }
            return noClone; // Return "Ok" if the registration number is not in use
        }

        // Parks a motorcycle in the parking lot
        static string ParkMc()
        {
            string mcReg1 = ""; // Registration number for the first motorcycle
            string mcReg2 = ""; // Registration number for the second motorcycle (if any)
            DateTime now = DateTime.Now; // Current timestamp
            int empty = EmptySpace(); // Get the next empty parking spot
            string receipt = ""; // Parking receipt
            string spotReceipt = ""; // Parking spot information

            Console.Write("\nIs it one or two motorcycles you would like to park?: ");
            string mcSum = Console.ReadLine(); // Number of motorcycles

            if (mcSum == "one" || mcSum == "1")
            {
                Console.Write("\nPlease enter the motorcycle's registration number in one string: ");
                mcReg1 = Console.ReadLine().ToUpper();
                string checker = CheckReg(mcReg1);

                if (checker == "Ok")
                {
                    if (mcReg1.Length <= 10 && !mcReg1.Contains(" ")) // Validate registration
                    {
                        receipt = "\nYou have parked a motorcycle \nwith the registration number: " + mcReg1 + "\nat: " + now + "\nin spot: " + empty;
                        spotReceipt = "mc!" + mcReg1;
                        SpotAllocation(empty, spotReceipt);
                    }
                    else
                    {
                        return "\nPlease enter a valid registration number";
                    }
                }
                else
                {
                    Console.WriteLine("The registration number is already parked! Check menu option 1 for an overview.");
                }
            }
            else if (mcSum == "two" || mcSum == "2")
            {
                Console.Write("\nPlease enter the first motorcycle's registration number in one string: ");
                mcReg1 = Console.ReadLine().ToUpper();
                Console.Write("\nPlease enter the second motorcycle's registration number in one string: ");
                mcReg2 = Console.ReadLine().ToUpper();

                if (mcReg1 == mcReg2)
                {
                    Console.WriteLine("\nEach vehicle needs its own registration number. Please try again.");
                }
                else
                {
                    string checker = CheckReg(mcReg1);
                    string doubleChecker = CheckReg(mcReg2);

                    if (checker == "Ok" && doubleChecker == "Ok")
                    {
                        if (mcReg1.Length <= 10 && mcReg2.Length <= 10 && !mcReg1.Contains(" ") && !mcReg2.Contains(" "))
                        {
                            receipt = "\nYou have parked two motorcycles \nwith the registration numbers: " + mcReg1 + " and " + mcReg2 + "\nat: " + now + "\nin spot: " + empty;
                            spotReceipt = "mc!" + mcReg1 + ", mc!" + mcReg2;
                            SpotAllocation(empty, spotReceipt);
                        }
                        else
                        {
                            return "\nPlease enter valid registration numbers";
                        }
                    }
                    else
                    {
                        Console.WriteLine("One or both registration numbers are already parked! Check menu option 1 for an overview.");
                    }
                }
            }
            else
            {
                ParkVehicle(); // Restart the park vehicle process for invalid input
            }

            return receipt; // Return the parking receipt
        }

        // Handles checking out a vehicle
        static void CheckOut()
        {
            Console.Clear();
            Console.Write("3. Check out vehicle \n\nPlease enter the registration number of the vehicle you would like to check out: ");
            string regNr = Console.ReadLine().ToUpper(); // Get registration number
            string answer = RemoveVehicle(regNr); // Call function to remove vehicle
            Console.WriteLine(answer);

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }

        // Removes a vehicle from the parking lot based on registration number
        static string RemoveVehicle(string search)
        {
            string isThere = "\nThe vehicle has been checked out!";
            string notThere = "\nThere is no vehicle parked with this registration number, please try again";

            foreach (string parkingSpot in ParkingList)
            {
                if (parkingSpot != null)
                {
                    string[] vehicles = parkingSpot.Split(", ");

                    foreach (var splits in vehicles)
                    {
                        string[] atoms = splits.Split("!");

                        if (atoms[1] == search)
                        {
                            int nr = Array.IndexOf(ParkingList, parkingSpot);

                            if (vehicles.Length == 2)
                            {
                                if (vehicles[0].Contains(atoms[1]))
                                {
                                    ParkingList[nr] = vehicles[1]; // Keep the second motorcycle if present
                                }
                                else
                                {
                                    ParkingList[nr] = vehicles[0]; // Keep the first motorcycle if present
                                }
                            }
                            else
                            {
                                ParkingList[nr] = null; // Clear the parking spot
                            }

                            return isThere;
                        }
                    }
                }
            }
            return notThere;
        }

        // Search for a vehicle and allows the user to move it to a new spot
        static void MoveVehicle()
        {
            Console.Clear();
            Console.Write("4. Search for and Move vehicle \n\nPlease enter the registration number of the vehicle that you would like to search for and move: ");
            string move = Console.ReadLine().ToUpper(); // Get registration number
            int search = CheckForVehicle(move); // Find the vehicle's current spot

            if (search > 0)
            {
                Console.WriteLine("This vehicle is parked in spot {0}", search);
            }
            else
            {
                Console.WriteLine("There is no vehicle parked with that registration number");
                Console.WriteLine("\n\n\nPress any key to return to the main menu");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }

            Console.WriteLine("\n(If you don't want to move the vehicle, just type 'no')");
            Console.Write("\n\nWhich spot would you like to move it to?: ");
            string spotSuggestion = Console.ReadLine();

            if (spotSuggestion.ToLower() == "no")
            {
                Console.WriteLine("\n\n\nOk! No changes have been made. Press any key to return to the main menu");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }

            int newSpot;
            bool isNumber = int.TryParse(spotSuggestion, out newSpot);

            if (isNumber && newSpot >= 1 && newSpot <= 100)
            {
                string answer = ChangeVehicleSpot(move, newSpot);
                Console.WriteLine(answer);
            }
            else
            {
                Console.WriteLine("\nSpot input is out of range or invalid.");
            }

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }

        // Helper function to find a vehicle by registration number
        static int CheckForVehicle(string search)
        {
            int currentSpot = 0;

            foreach (string parkingSpot in ParkingList)
            {
                if (parkingSpot != null)
                {
                    string[] vehicles = parkingSpot.Split(", ");

                    foreach (var splits in vehicles)
                    {
                        string[] atoms = splits.Split("!");

                        if (atoms[1] == search)
                        {
                            currentSpot = Array.IndexOf(ParkingList, parkingSpot) + 1;
                            return currentSpot;
                        }
                    }
                }
            }
            return 0; // Return 0 if the vehicle is not found
        }

        // Change the parking spot of a vehicle
        static string ChangeVehicleSpot(string searchVehicle, int spotSuggestion)
        {
            int spotFinder = spotSuggestion - 1; // Adjust for zero-based index
            string notAvailable = "\n\nThis spot is not available! Please start over.";
            string available = "\n\nThe change has been made! Your vehicle is now in the new spot.";
            string error = "\n\nThe vehicle type or registration number is incorrect. Start over.";

            foreach (string parkingSpot in ParkingList)
            {
                if (parkingSpot != null)
                {
                    string[] vehicles = parkingSpot.Split(", ");
                    foreach (var splits in vehicles)
                    {
                        string[] atoms = splits.Split("!");

                        if (atoms[1] == searchVehicle)
                        {
                            int currentSpot = Array.IndexOf(ParkingList, parkingSpot);

                            if (ParkingList[spotFinder] == null)
                            {
                                // Move the vehicle to the new spot
                                ParkingList[spotFinder] = splits;
                                ParkingList[currentSpot] = null;
                                return available;
                            }
                            else
                            {
                                return notAvailable;
                            }
                        }
                    }
                }
            }
            return error;
        }

        // Optimizes parking for motorcycles
        static void OptimizeSpace()
        {
            Console.Clear();
            Console.Write("5. Optimize the motorcycles in the parking lot.\n\nWould you like to combine single-parked motorcycles? (yes/no): ");
            string optimize = Console.ReadLine();

            if (optimize.ToLower() == "yes")
            {
                for (int i = 0; i < ParkingList.Length; i++)
                {
                    if (ParkingList[i] != null && ParkingList[i].Contains("mc") && !ParkingList[i].Contains(", "))
                    {
                        for (int j = i + 1; j < ParkingList.Length; j++)
                        {
                            if (ParkingList[j] != null && ParkingList[j].Contains("mc") && !ParkingList[j].Contains(", "))
                            {
                                ParkingList[i] += ", " + ParkingList[j];
                                ParkingList[j] = null;
                                break;
                            }
                        }
                    }
                }
                Console.WriteLine("Optimization complete.");
            }
            else
            {
                Console.WriteLine("No changes made.");
            }

            Console.WriteLine("\n\nPress any key to return to the main menu.");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
    }
}

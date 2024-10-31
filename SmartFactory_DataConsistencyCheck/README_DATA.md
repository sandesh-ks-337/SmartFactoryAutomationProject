#SmartFactory Data Consistency Integration

## Description
The SmartFactory Data Consistency Project is designed to verify that data remains consistent between IT and OT systems in a smart factory setup. It ensures that changes made via the User Interface (UI) are correctly reflected in backend data and that data synchronization between systems is reliable.

## Folder Structure

SmartFactory_API_Integration
├── Interfaces
│   ├── IOrderDataSource.cs
│   └── IProductDataSource.cs
├── Models
│   ├── Order.cs
│   └── Product.cs
└── Tests
       └── DataConsistencyTests.cs

## Requirements
.NET Core SDK
Visual Studio
NUnit Framework

## Setup Instructions
1. Clone the Repository: Clone this repository to your local machine.
    git clone https://github.com/sandesh-ks-337/SmartFactoryAutomationProject.git
    Navigate to the Project Directory: Move to the SmartFactory_DataConsistencyCheck directory.

    Install Dependencies: Ensure all dependencies are available. If necessary, restore dependencies using:
    dotnet restore

2. Build the Project:
    dotnet build

3. Running the Tests
   To execute the data consistency tests, use the following command in the terminal:
   dotnet test

## Test Cases

VerifyConcurrentUpdatesOnOrder: Checks if concurrent order updates reflect accurately in backend data.
VerifyOrderTotalPriceCalculation: Ensures that the total order price is correctly calculated and consistent across systems.
VerifyProductSynchronization: Validates that product updates through the UI are accurately synchronized with backend data.
VerifyDataAfterDeletion: Confirms that deletions performed via one system are correctly updated and reflected in the other.

## Expected Outputs
All tests passing: Data consistency is successfully maintained across IT and OT systems.

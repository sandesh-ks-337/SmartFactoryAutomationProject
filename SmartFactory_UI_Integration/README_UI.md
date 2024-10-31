# SmartFactory API Integration

## Description
This test suite uses Robot Framework with SeleniumLibrary to validate the key UI functionalities of the SmartFactory Dashboard, specifically focusing on the dashboard display and configuration settings. The tests simulate user actions, such as navigating the dashboard, refreshing data, and saving configuration settings.

## Prerequisites
   -Python (v3.x) installed
   -Robot Framework installed
        pip install robotframework
   -SeleniumLibrary and Browser Driver installed for Chrome
        pip install robotframework-seleniumlibrary
   -Download the Chrome driver and ensure it is in your system PATH or in the project directory.

## Folder Structure

  -SmartFactory_UI_Integration
├── Tests
│   └── ui_tests.robot
│   
└──  UI
      ├── config.html
      └── Dashboard.html

## Test Cases
Validate Dashboard Page Loads - Checks if the dashboard page loads with key elements visible.
Check Inventory Refresh Button - Confirms that the refresh functionality works correctly on the dashboard.
Validate Configuration Page Loads - Verifies that the configuration page loads with required elements.
Test Save Config Button - Tests the functionality of saving configurations.
Check Sync Interval Value After Saving - Ensures that configuration changes are saved and reflected correctly.


## Running the Tests
   -Clone the Respository:
      git clone https://github.com/sandesh-ks-337/SmartFactoryAutomationProject.git
   -Go to UI directory (Tests Folder)
   -Open a terminal in the project directory.
   -Run the tests using:
        robot ui_tests.robot
   -Expected Results
       Each test validates specific elements and functions on the SmartFactory UI pages, with results output in the terminal. The suite also generates an HTML report for review, displaying pass/fail status and details for each test case.
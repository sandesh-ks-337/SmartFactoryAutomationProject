*** Settings ***
Library    SeleniumLibrary
Library    RequestsLibrary

Suite Setup    Open Browser    ${URL_DASHBOARD}    chrome
Suite Teardown    Close Browser

*** Variables ***
${URL_DASHBOARD}    file:///C:/Users/Sunil Bangalore/Desktop/SmmartFactoryProject_2/SmartFactoryAutomationProject/SmartFactory_UI_Integration/UI/dashboard.html
${URL_CONFIG}       file:///C:/Users/Sunil Bangalore/Desktop/SmmartFactoryProject_2/SmartFactoryAutomationProject/SmartFactory_UI_Integration/UI/config.html

*** Test Cases ***

Validate Dashboard Page Loads Correctly
    Go To    ${URL_DASHBOARD}
    Title Should Be    Dashboard
    Page Should Contain    Dashboard
    Page Should Contain Element    id=inventoryStatus
    Page Should Contain Element    id=recentOrders
    Page Should Contain Element    id=status

Check Real-Time Inventory Refresh
    Go To    ${URL_DASHBOARD}
    ${initial_inventory}=    Get Text    id=inventoryStatus
    Click Button    id=refreshButton
    Sleep    1
    Handle Alert
    ${updated_inventory}=    Get Text    id=inventoryStatus
    Should Not Be Equal    ${initial_inventory}    ${updated_inventory}
    Should Match Regexp    ${updated_inventory}    Current Inventory: \\d+ items

Validate Recent Orders Section
    Go To    ${URL_DASHBOARD}
    Page Should Contain    Recent Orders
    Page Should Contain    No recent orders.

Validate System Status Display
    Go To    ${URL_DASHBOARD}
    Page Should Contain    System Status
    ${status}=    Get Text    id=status
    Should Be Equal    ${status}    Online

Validate Configuration Page Loads Correctly
    Go To    ${URL_CONFIG}
    Title Should Be    Configuration Settings
    Page Should Contain    Configuration Settings
    Page Should Contain Element    id=syncInterval
    Page Should Contain Element    id=authParam
    Page Should Contain Element    id=saveConfigButton

Test Sync Interval and Authentication Parameter Input
    Go To    ${URL_CONFIG}
    Input Text    id=syncInterval    30
    Input Text    id=authParam       secret_auth_key
    Click Button    id=saveConfigButton
    Handle Alert
    Go To    ${URL_CONFIG}
    ${sync_value}=    Get Value    id=syncInterval
    ${auth_value}=    Get Value    id=authParam
    Should Be Equal    ${sync_value}    30
    Should Be Equal    ${auth_value}    secret_auth_key
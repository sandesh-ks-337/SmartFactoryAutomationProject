*** Settings ***
Library    SeleniumLibrary
Library    RequestsLibrary
Library    

*** Variables ***
${URL_DASHBOARD}    file:///C:/Users/Meghana/Desktop/SmartFactoryProject/SmartFactoryAutomationProject/SmartFactory_UI_Integration/UI/dashboard.html
${URL_CONFIG}   file:///C:/Users/Meghana/Desktop/SmartFactoryProject/SmartFactoryAutomationProject/SmartFactory_UI_Integration/UI/config.html

*** Test Cases ***
Validate Configuration Page Loads
    Open Browser    ${URL_CONFIG}    chrome
    Title Should Be    Configuration Settings
    Page Should Contain    Configuration Settings

Test Save Config Button
    [Setup]    Open Browser    ${URL_CONFIG}    chrome
    Input Text    id=syncInterval    30
    Click Button    id=saveConfigButton
    Sleep    1
    Handle Alert 

Validate Dashboard Page Loads
    Open Browser    ${URL_DASHBOARD}    chrome
    Title Should Be    Dashboard
    Page Should Contain    Dashboard

Check Inventory Refresh Button
    [Setup]    Open Browser    ${URL_DASHBOARD}    chrome
    Click Button    id=refreshButton
    Sleep    1
    Handle Alert
    Page Should Contain    Current Inventory:

Check Sync Interval Value After Saving
    [Setup]    Open Browser    ${URL_CONFIG}    chrome
    Input Text    id=syncInterval    30
    Click Button    id=saveConfigButton
    Handle Alert
    Sleep    2
    ${sync_value}=    Get Value    id=syncInterval
    Should Be Equal    ${sync_value}    30
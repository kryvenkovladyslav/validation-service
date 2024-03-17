# ectd-validation-service
eCTD Validation is a process of validation XML-message accroding to eCTD requirements against DTD or XML-Schema.

# Validation process 
There are some created strategies for validating against XML-Schema or DTD. Each of that strategy has its own validation setting provider for creating specific rules for reading the document.

# Azure Blob Storage 
The storage is needed for storing eCTD strucutre on Azure, so you can provide a full path to XML-Document in order to start validation process.

# Validation 
Scenario: you have to provide a full path to XML-Document that will be validated against DTD:
![image](https://github.com/kryvenkovladyslav/ectd-validation-service/assets/58219605/0088b56a-ff53-49f2-9a31-4a3ffc67e2f4)
![image](https://github.com/kryvenkovladyslav/ectd-validation-service/assets/58219605/30f403c2-2c50-45f0-86a8-09b40c675169)
![image](https://github.com/kryvenkovladyslav/ectd-validation-service/assets/58219605/4105ed9e-d10b-4fc4-86c5-31aff53c0ab1)
![image](https://github.com/kryvenkovladyslav/ectd-validation-service/assets/58219605/81db90cd-4d29-43f5-a91b-5efc850d1fa8)

# Docker support
The repository always contains all required file for running the application in a Docker container.
